using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;
using System;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        //Neste (RegisterServices) eu vou Registrar as minhas dependencias
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            //estou adicionando um serviço http e (NÂO um scoped - singleton -transient) - "Mas poderia"
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            //Caso eu use o Refit abaixo ai eu comento este. Apenas por minha decisão eu uso este no projeto
            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
                //estou dizendo que tipo de (politica || polly) vou adotar par o (erro http). Neste caso seria o (WaitAndRetryAsync)
                //.AddTransientHttpErrorPolicy(
                //que seria 3vezes. quanto tempo vamos esperar para chamar a proxima seria 600 milessegundos.
               // p => p.WaitAndRetryAsync(3, _=> TimeSpan.FromMilliseconds(600)));


            //montando o REFIT aqui ai não preciso usar a (Service)

            //services.AddHttpClient("Refit", options =>
            //{
            //    options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            //})
            //.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //.AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);


            //recomendação da microsoft que use (AddSingleton)
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //vou usar (AddScoped) que ai os dados do usuario(IUser) o qual estou tentando obter fica mais limitado ao request
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
