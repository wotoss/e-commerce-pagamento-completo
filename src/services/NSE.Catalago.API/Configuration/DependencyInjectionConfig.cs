using Microsoft.Extensions.DependencyInjection;
using NSE.Catalago.API.Data;
using NSE.Catalago.API.Data.Repository;
using NSE.Catalago.API.Models;


namespace NSE.Catalago.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<CatalogoContext>();
        }
    }
}
