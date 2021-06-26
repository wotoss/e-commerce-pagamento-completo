using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    //Vamos criar os métodos que vão nos fornecer suporte á todas funcionalidade.
    //Lembrete => Uma (ActionResult) sempre precisa retornar algo.
    public class IdentidadeController : MainController
    {       
        private readonly IAutenticacaoService _autenticacaoService;

        //Vou injetar na minha controler este serviço de autenticação
        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {

            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            //se a modelState não for valida eu retorno a minha view com os dados da minha (model=>usuarioRegistro) = Assim informa o erro na tela
            if (!ModelState.IsValid) return View(usuarioRegistro);
            //Vamos se comunicar com a API - Para fazer o Registro

            var resposta = await _autenticacaoService.Registro(usuarioRegistro);

            //se der errado.
            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

            //Realizar o login na aplicação
            await RealizarLogin(resposta);

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            //este parametro de rota eu acabei de criar como null = string returnUrl = null
            //eu guardo ele dentro da ViewData que será usado dentro da minha (return View)
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login (UsuarioLogin usuarioLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(usuarioLogin);

            //Se comunicar com a API - Login
            var resposta = await _autenticacaoService.Login(usuarioLogin);

            //agora eu sei quando eu possuo erros.
            if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioLogin);

            //Realizar login na APP
            await RealizarLogin(resposta);

            //Se o  parametro (returnUrl) or nullo ou vazio eu dou um RedirectToAction("Index", "Home")
           if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            //Caso venha preenchido eu dou um LocalRedirect(returnUrl) = para esta url = (returnUrl)
            //Como eu fiz o login ele me retorna de onde eu vim.
            return LocalRedirect(returnUrl);


        }

        //No logout ele vai Limpar o Cookie.
        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            //para sair você vai fazer um (SignOut = sair ) ele vai zera o cookie e sair do sistema 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task RealizarLogin(UsuarioRespostaLogin resposta)
        {
            //vou receber o meu  (resposta.AccessToken) e quero ele formatado => ObterTokenFormatado => dentro do => var token
            var token = ObterTokenFormatado(resposta.AccessToken);

            //vou criar uma lista de claims => para que eu possa popular minha lista de claims
            var claims = new List<Claim>();
            //vou adicionar(Add)um claim vou chamar ela de (JWT) e vou guardar ou armazenar o meu token (resposta.AccessToken) dentro de uma (claims.add) no inicio da linha
            claims.Add(new Claim("JWT", resposta.AccessToken));
            //Detalhe=> eu faço um (AddRange) porque estou armazenando uma (Lista dentro de uma Lista)
            claims.AddRange(token.Claims);

            //Vou criar o ClaimsIdentity
            //Ele vai gerar os meus claims => dentro do cookie
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //Quanto tempo ele vai durar neste caso 60 minutos
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                //e digo que ele é persistente => Ele vai durar multiplos requeste (dentro dos 60 minutos).
                IsPersistent = true
            };


            //aqui dentro (SignInAsync()) vou passar um esquema que indica que tipo de autenticação estou usando (Cookie)
            await HttpContext.SignInAsync( //(SignInAsync =>seria => O usuario faz (O login)
            //Neste momento eu digo. você vai trabalhar com o (schema de autennticação Cookie)
                CookieAuthenticationDefaults.AuthenticationScheme,
            //Agora eu preciso passar a minha coleção (claims)
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }


        //Para este método eu preciso instalar o JsonWebToken => Microsoft.AspNetCore.Authentication.JwtBearer
        //Veja como é este método. Eu passo (string jwtToken) e ele me retorna (JwtSecurityToken)
        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            //Eu passo JwtSecurityTokenHandler() Leio o Token (jwtToken) e faço um Cast (JwtSecurityToken)
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

    }
}
