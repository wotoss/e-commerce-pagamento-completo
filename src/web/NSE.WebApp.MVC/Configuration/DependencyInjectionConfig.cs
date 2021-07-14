using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using System;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        //Neste (RegisterServices) eu vou Registrar as minhas dependencias
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            //estou adicionando um serviço http e (NÂO um scoped - singleton -transient) - "Mas poderia"
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            //neste momento eu vou comentar porque vou utilizar o Refit
            //services.AddHttpClient<ICatalogoService, CatalogoService>()
            //    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();


            //montando o refit aqui ai não preciso usar a (Service)
            services.AddHttpClient("Refit", options =>
            {
                options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            })
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);


            //recomendação da microsoft que use (AddSingleton)
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //vou usar (AddScoped) que ai os dados do usuario(IUser) o qual estou tentando obter fica mais limitado ao request
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
