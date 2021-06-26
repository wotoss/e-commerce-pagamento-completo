
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace NSE.WebApp.MVC.Extensions
{
    //Esta (Interface IUser) => representa o nosso usuario. 
    //Desta forma vou facilitar meios de acessar dados do nosso (usuario).
    public interface IUser
    {
        //Temos varias forma de obter o informações do usuarios.
        //vou ter o nome do usuario aqui diretamente.
        string Name { get; } 

        //Posso obter o Id do usuario
        Guid ObterUserId();
        //Posso obter o email
        string ObterUserEmail();
        //Posso obter o token o JsonWebToken
        string ObterUserToken();
        //Saber se ele está autenticado.
        bool EstaAutenticado();
        //Se ele está dentro de uma role (regra) especifica. 
        bool PossuiRole(string role);
        //Obter a coleção de clamis
        IEnumerable<Claim> ObterClaims();
        //Obter o HttpContex.;
        HttpContext ObterHttpContext();
    }

    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;
        //Esta injeção de dependencia (IHttpContextAccessor) => O nome já diz ele acessa Contexto da sua requisição http
        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;


        public Guid ObterUserId()
        {
            //Se estiver Autenticado ele retona um Guid = > Caso esteja não esteja ele retorna um Guid.Vazio
            return EstaAutenticado() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string ObterUserEmail()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public string ObterUserToken()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserToken() : "";
        }

        public bool EstaAutenticado()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool PossuiRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> ObterClaims()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public HttpContext ObterHttpContext()
        {
            return _accessor.HttpContext;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst("sub");
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }
            var claim = principal.FindFirst("email");
            return claim?.Value;
        }

        public static string GetUserToken(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst("JWT");
            return claim?.Value;
        }
    }
}
