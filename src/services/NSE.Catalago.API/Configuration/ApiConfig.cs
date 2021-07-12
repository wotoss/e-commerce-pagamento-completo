using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Catalago.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalago.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogoContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            //Crei uma regra que se chama Total, porque estou dando acesso total 
            //seria qualquer origem (AllowAnyOrigin) qualquer método (AllowAnyMethod) qualquer header (AllowAnyHeader)
            services.AddCors(options =>
            {
                //Isto significa que estou deixando minha API aberta para quem quiser consumi-lá
                options.AddPolicy("Total",
                    builder =>
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        //Parte da configuração
        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Direcionamento Https
            app.UseHttpsRedirection();
            app.UseRouting(); //Roteamento
            app.UseCors("Total"); //Uso do Cors que foi criado os acessos "TOTAL"

            //Aqui temos os mapeamentos dos nossos endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}