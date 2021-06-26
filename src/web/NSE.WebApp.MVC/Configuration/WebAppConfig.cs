using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC.Configuration
{
    //Vou declarar os métodos de extenção por ser um método de extensão eu uso o static
    public static class WebAppConfig
    {
        public static void AddMvcConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            //veja que este(Configure) esta representando o arquivo AppSettings do MVC
            services.Configure<AppSettings>(configuration);
        }

        public static void UseMvcConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Vou verificar o ambiente de desenvolvimento, para usar o (UseDeveloperExceptionPage)
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            ////Caso não use => sou direcionado a ambiente de Produção ou Stage 
            //else
            //{
            //    //ele está enviando os nosso erros para a nossa rota (("/error/500"))
            //    //porque 500 porque são erro de servidor
            //    //Resumindo este pega todos que NÃO foi tratado. Como os que não foi tratado eu 
            //    //não quais são eu trato de forma generica como erro de servidor (500)
            //    app.UseExceptionHandler("/erro/500");
            //    //agora se eu souber o erro que foi, eu vou tratar de uma outra forma.
            //    //este para metro {0} é o erro do statusCode que eu contruir = namespace NSE.WebApp.MVC.Services
            //    //Resumindo este pega todos que FOI tratado
            //    app.UseStatusCodePagesWithRedirects("/erro/{0}");
            //    app.UseHsts();
            //}

            //ele está enviando os nosso erros para a nossa rota (("/error/500"))
            //porque 500 porque são erro de servidor
            //Resumindo este pega todos que NÃO foi tratado. Como os que não foi tratado eu 
            //não quais são eu trato de forma generica como erro de servidor (500)
            app.UseExceptionHandler("/erro/500");
            //agora se eu souber o erro que foi, eu vou tratar de uma outra forma.
            //este para metro {0} é o erro do statusCode que eu contruir = namespace NSE.WebApp.MVC.Services
            //Resumindo este pega todos que FOI tratado
            app.UseStatusCodePagesWithRedirects("/erro/{0}");
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Eu trouce da (ClassIdentityConfig) do MVC
            app.UseIdentityConfiguration();

            // Estou registrando o meu Middleware no sistem  e Com o Middleware todo o resquest passará por ele 
            // Estou centralizando os erros do sitema no tratamento do meu Middleware = Isto significa que não preciso 
            // espelhar try catch no meu sistema.
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
