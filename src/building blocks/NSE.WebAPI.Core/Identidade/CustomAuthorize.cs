using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NSE.WebAPI.Core.Identidade
{
    public class CustomAuthorize
    {
        //Metodo de Autorização (ValidarClaimsUsuario) no contexto da requisição e com base no (Nome da claim e no valor da Claim)
        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
            //1º eu valido se o usuario esta (Autenticado => IsAuthenticated)
            return context.User.Identity.IsAuthenticated &&
                // 2º eu verifico ele possui uma claim com base na (claimNome que estou passando && e esta claim contem (Value.Contains) o valor que estou esperando)
                context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }
    }
    //Ele é um atributo (ClaimsAuthorizeAttribute) que vai decorar um método e herda : TypeFilterAttribute
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        //agora eu passo o nome da Claim e os valor da claim
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }

    //ele vai implementar a interface (IAuthorizationFilter)
    public class RequisitoClaimFilter : IAuthorizationFilter
    {
     //Onde eu recebo uma (Claim) no construtor por injeção de dependencia, então posso usa-lá
        private readonly Claim _claim;

        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        //Já com este método (OnAuthorization) eu vou fazer a validação. Lembrando que este método vem da implementação da interface.
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Se ele não estiver autenticado não tem o que validar (erro 401 Não sei quem você é). 
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            //Eu sei quem ele é mas ele não pode acessar o recurso pois não tem a claim que eu estou pedindo (erro 403)
            if (!CustomAuthorize.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
