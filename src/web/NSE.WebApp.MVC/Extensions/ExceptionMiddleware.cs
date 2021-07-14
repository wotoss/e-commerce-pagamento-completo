

using Microsoft.AspNetCore.Http;
using Refit;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{ 
    //Istou Criando um Middleware mesmo
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //O método () é o que vai tratar o que você irá fazer dentro do Middleware
        public async Task InvokeAsync(HttpContext httpContext)
        {
            //Nest momento estou fazendo um try porque eu sei que podemos ter uma (Exceptio) 
            //e eu quero tratar esta (Exception) desde que ela seja uma (CustomHttpRequestException) que é a Exception que eu construi.
            try
            {
                await _next(httpContext);
            }
            catch (CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            //usando refit nesta parte do catch
           catch (ValidationApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);

            }
        }

        //Este é o teste com Refit
        private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
           
            if (statusCode == HttpStatusCode.Unauthorized)
            {
               
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");

                //Interressante => eu coloco o return neste caso porque o (Redirect = Faz o dircionamento mas NÃO INTERROMPE O PROCESSO)
                //desta forma ele pode ler a linha de baixo ou o proximo comando com o return não. "será interrompido o processo".
                return;
            }
            //agora chegando até aqui caso não seja 401 que esta sendo tratado acima.
            // Já aqui estou dizendo que  Response.StatusCode será ou vai receber (=) o StatusCode da minha (Exception).
            context.Response.StatusCode = (int)statusCode;





            //private static void HandleRequestExceptionAsync(HttpContext context, CustomHttpRequestException httpRequestException)
            //{
            //    //Se a minha (httpRequestException.StatusCode) for igual == (HttpStatusCode.Unauthorized "que seria o erro 401 = NÃO AUTORIZADO")
            //    //Eu já sei o que eu tenho que fazer (esta pessoa não conseguiu acessar porque ele NÂO é conhecido então) eu direciono para a tela de login.
            //    if (httpRequestException.StatusCode == HttpStatusCode.Unauthorized)
            //    {
            //        //context é o meu HttpContext => Response vemo como minha resposta = e o Redirect faz o redirecionamento para o ("/login")
            //        //aqui eu passo um parametro na minha rota(?ReturnUrl={context.Request.Path}")). 
            //        //quando vocÊ fizer o login. já pode ir para pagina logado que seria o caminho do (Path)
            //        context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");

            //        //Interressante => eu coloco o return neste caso porque o (Redirect = Faz o dircionamento mas NÃO INTERROMPE O PROCESSO)
            //        //desta forma ele pode ler a linha de baixo ou o proximo comando com o return não. "será interrompido o processo".
            //        return;
            //    }
            //    //agora chegando até aqui caso não seja 401 que esta sendo tratado acima.
            //    // Já aqui estou dizendo que  Response.StatusCode será ou vai receber (=) o StatusCode da minha (Exception).
            //    context.Response.StatusCode = (int)httpRequestException.StatusCode;
        }
    }
}
