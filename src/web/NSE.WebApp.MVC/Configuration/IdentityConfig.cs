using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Configuration
{
    //Importante => vou definir os métodos de extenção. Por isto definir a classe como static
    public static class IdentityConfig
    {
        public static void AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                 {
                     //Caso der um erro 401 || 403 ele trata desta forma.
                     //Quando o usuario não estiver logado e eu quiser encaminhar ele para uma area da minha aplicaçõa Direciono para (/login)
                     options.LoginPath = "/login"; 
                     //Ele que navegar para uma area da aplicação onde ele não tem acesso.(Direciono para (/acesso-negado))
                     options.AccessDeniedPath = "/acesso-negado";

                 });
        }
        public static void UseIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
