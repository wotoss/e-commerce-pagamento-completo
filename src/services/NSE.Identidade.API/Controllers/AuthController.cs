using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using static NSE.Identidade.API.Models.UserViewModels;

using NSE.Identidade.API.Extensions;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Collections.Generic;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Identidade.API.Controllers
{
    
    //Exemplo importante => para acessar esta controller ficara assim localhos/api/identidade/"O NOME DA AÇÂO ou METODO exemplo =>" nova-conta
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        //injeção de dependencia estas são classes do proprio Identity
        private readonly SignInManager<IdentityUser> _signInManager; //irá gerenciar o login =>  "SignInManager"
        private readonly UserManager<IdentityUser> _userManager; //irá gerenciar o Usuario =>  "UserManager"
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager, 
                              UserManager<IdentityUser> userManager,
                              IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {

            ///return new StatusCodeResult(500);

            if (!ModelState.IsValid) return CustomResponse();

            //vou criar uma instância de IdentityUser
            var user = new IdentityUser 
            {
                UserName = usuarioRegistro.Email, //aqui esta certo eu coloco o nome de usuario como email "é mais facil lembrar do email"
                Email = usuarioRegistro.Email,
                EmailConfirmed = true //a confirmação do email do usuario eu estou (setando como true).
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);

            //se tudo deu certo ele fica neste if => não vai para linha de baixo
            if (result.Succeeded)
            {
                //esta gerando do token com o => metodo (GerarJwt)
                return CustomResponse(await GerarJwt(usuarioRegistro.Email)); //este retorno Ok seria um (200)
            }
           
            //para cada erro encontrado na minha lista de resultados ou result eu vou..
            //addionar ele na minha coleção de erros neste caso atraves do (Description => o que seria a msg de erro).
            foreach (var error in result.Errors)
            {
                // eu só preciso da string. por isto coloco o (Description). Pois me retorna uma string.
                AdicionarErroProcessamento(error.Description);
            }
            return CustomResponse(); //caso de erro retorno um (CustomResponse) => requsição não execultada com sucesso
        }

        [HttpPost("autenticar")] //seria o login
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {

            //vamos validar se a Model State veio correta
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            //1- result ou usuario (tentando logar)
            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha,
                false, true);//persistente = false, se ele deve bolquear se a senha for invalida = true => no caso ficará 5 minutos bloqueado

            //2- se deu certo e vamos gerar o token atraves deste método (GerarJwt)
            if (result.Succeeded)
            {
                return CustomResponse(await GerarJwt(usuarioLogin.Email));
            }

            //3 -foi bloqueado por 5 tentativas invalidas. Este é um erro enviado pelo identity
            //mesmo que na sequencia acerte a senha ele ficará por 5 minutos bloqueado.
            //Lembrando caso vc queira desabilitar na configuração podemos. mas eu dixei habilitada => true
            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }
            //4 - Ou o erro foi por (Usuario e Senha incorretos)
            AdicionarErroProcessamento("Usuário ou Senha incorretos");
            return CustomResponse();
        }

        private async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            //aqui eu vou obter o usuario, através do email
            var user = await _userManager.FindByEmailAsync(email);
            //Agora vou gerar uma lista de claims
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await ObterClaimsUsuario(claims, user);

            var encodedToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, user, claims);

        }



            //Método Obter as Claims do Usuario
            private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                //neste momento vou adicionar mais claims a (var claims)
                //Agora vou adicionar em  uma (lista de claims) tudo que eu peguei do usuario (user) atraves das variaveis
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); //Jti me trás o Id do token não o Id do usuario.
                claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString())); //nbf é quando o token vai expirar
                claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)); //aqui é quando o roken foi emitido

                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim("role", userRole));
                }

                var identityClaims = new ClaimsIdentity();
                //agora o identityClaims esta recebendo todas as claims => claims do usuario, claims gerei do token, claims em formato role.
                identityClaims.AddClaims(claims);

                return identityClaims;
            }

            //Vamos Codificar o Token
            private string CodificarToken(ClaimsIdentity identityClaims)
            {
                //Agora vou gerar o manipulador do token que ira fazer a chave (key)
                var tokenHandler = new JwtSecurityTokenHandler();
                var Key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                //vamos fazer a criação do nosso (token), atraves do (tokenHandler)
                var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _appSettings.Emissor, //já esta configurado, vindo com valor 
                    Audience = _appSettings.ValidoEm,
                    Subject = identityClaims,//Subject Seria dados do usuario que é a coleção de identityClaims
                    Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras), //é importante o horario utc, ele irá colocar duas horas para frente no padrão UTC
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)//este é o tipo de algoritmo que vou usar para criptografar o meu token => (SecurityAlgorithms.HmacSha256Signature)
                });
                //Para eu obter o meu token
                //Este seria o token criptografado
                return tokenHandler.WriteToken(token);
            }
        
        //Vamos criar a resposta do token
        private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
                //Agora vamos popular a resposta para o método.
                return new UsuarioRespostaLogin
                {
                    AccessToken = encodedToken, //o (AccessToken) é o meu proprio token codificado (encodedToken)
                    ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                    UsuarioToken = new UsuarioToken //aqui temos os dados do usuario dentro do UsuarioToken
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                    }
                };
               
            }
            //Este é o padrão de data do JWT ele pega a data e transforma ToUnixEpochDate é uma data no padrão OFFSET
            private static long ToUnixEpochDate(DateTime date)
                => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

       }

    }

