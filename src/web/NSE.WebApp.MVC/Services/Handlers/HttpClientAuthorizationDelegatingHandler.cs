using NSE.WebApp.MVC.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading;
//using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services.Handlers
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        //vou injetar o meu usuario para que eu possa manipular o request e obter o context
        private readonly IUser _user;

        public HttpClientAuthorizationDelegatingHandler(IUser user)
        {
            _user = user;
        }
    
        //vou dar um (override) e reescrever o método (SendAsync)
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //a minha variavel authorizationHeader = vai receber a autorização atraves do meu contexto (_user.ObterHttpContext())
            var authorizationHeader =  _user.ObterHttpContext().Request.Headers["Authorization"];

            //se o meu retorno for diferente de nulo ou vazio significa que temos a autorização
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                //então eu vou adicionar dentro deste (Headers) a chave  (Authorization)
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }
            var token = _user.ObterUserToken();

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            return  base.SendAsync(request, cancellationToken);
        }
    }
}
