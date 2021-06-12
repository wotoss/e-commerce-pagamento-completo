using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        //Neste (RegisterServices) eu vou Registrar as minhas dependencias
        public static void RegisterServices(this IServiceCollection services)
        {
            //estou adicionando um serviço http e (NÂO um scoped - singleton -transient) - "Mas poderia"
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
        }
    }
}
