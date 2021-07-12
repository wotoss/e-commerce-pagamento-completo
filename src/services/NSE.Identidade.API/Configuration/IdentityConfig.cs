using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;
using NSE.WebAPI.Core.Identidade;
using System.Text;

namespace NSE.Identidade.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {

            //Configurações ou suporte a base de dados
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //Fim - Base de dados.

            //Inicio - Configurações ou suporte ao Identity
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>() //regras sobre perfil de usuarios
                .AddErrorDescriber<IdentityMensagensPortugues>()//Inicializando as mensagen de erro em portugues. => através do meu configureServices
                .AddEntityFrameworkStores<ApplicationDbContext>() //pega informações da fonte de dados <ApplicationDbContext>
                .AddDefaultTokenProviders(); //não tem haver com JWT <=> AddDefaultTokenProviders é diferente são token ou criptografia que te reconhece como dono da conta, em um possivel reset de conta

            services.AddJwtConfiguration(configuration);

            return services;
        }
    }
}
