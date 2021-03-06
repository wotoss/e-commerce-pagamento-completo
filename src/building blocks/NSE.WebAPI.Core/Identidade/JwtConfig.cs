using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NSE.WebAPI.Core.Identidade
{
     public static class JwtConfig
    { 

        public static void AddJwtConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        //Inicio - AppSettings e JWT
           { 
            //Importantissimo => Vou popular a minha classe => NSE.Identidade.API.Extensions (class AppSettings).
            //Com as informações que esta dentro da minha pasta => Extension/AppSettings.cs

            //Vá até o arquivo de configuração é pegue o nó AppSettings
            var appSettingsSection = configuration.GetSection("AppSettings");
            //aqui eu já peço para que os dados ("AppSettings") => preencha a classe => Configure<AppSettings>
            services.Configure<AppSettings>(appSettingsSection);

            //Neste momento eu vou obter esta classe atraves do Get<AppSettings> "já com a configuração que passei acima"
            var appSettings = appSettingsSection.Get<AppSettings>();
            // a minha chave key vai ser transformada em um Encoding sequencia de Bytes no formato ASCII 
            //Esta chave será passada na linha 82 => SymmetricSecurityKey
            var Key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //Fim - AppSettings e JWT



            //Inicio - Vamos fazer a configuração do Jason Web Token (JWT)
            services.AddAuthentication(options =>
            {
                //estou dizendo que nos dois casos usarei o (AuthenticationScheme) como padrão. Poderia usar outro Provider omo cookie.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //Apartir daqui eu começo adicionar o JWT no meu serviço
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true; //terei acesso pelo https (true) por questão de segurança
                bearerOptions.SaveToken = true; //Token será guardado na instância assim que o login for realizado com sucesso
                bearerOptions.TokenValidationParameters = new TokenValidationParameters //Aqui eu vou montar os parametros de emissão do meu token.
                {
                    ValidateIssuerSigningKey = true, //vou validar o emissor com base na assinatura
                    IssuerSigningKey = new SymmetricSecurityKey(Key), //atraves (SymmetricSecurityKey) eu construo mainha chave ("x") de criptografia
                    ValidateIssuer = true, //validar o emissor true. Quero que o token seja valido somente nas API  que eu permitir e quiser.
                    ValidateAudience = true, //Para quais dominios este token é valido
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor //tambem farei a validação do emissor
                };
            });
            //Fim - JWT
        }
        public static void  UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
