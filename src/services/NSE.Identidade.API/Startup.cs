using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;

namespace NSE.Identidade.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Configurações ou suporte a base de dados
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //Fim - Base de dados.

            //Inicio - Configurações ou suporte ao Identity
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>() //regras sobre perfil de usuarios
                .AddEntityFrameworkStores<ApplicationDbContext>() //pega informações da fonte de dados <ApplicationDbContext>
                .AddDefaultTokenProviders(); //não tem haver com JWT <=> AddDefaultTokenProviders é diferente são token ou criptografia que te reconhece como dono da conta, em um possivel reset de conta


            //Inicio - AppSettings e JWT

            //Importantissimo => Vou popular a minha classe => NSE.Identidade.API.Extensions (class AppSettings).
            //Com as informações que esta dentro da minha pasta => Extension/AppSettings.cs

            //Vá até o arquivo de configuração é pegue o nó AppSettings
            var appSettingsSection = Configuration.GetSection("AppSettings");
            //aqui eu já peço para que os dados ("AppSettings") => preencha a classe => Configure<AppSettings>
            services.Configure<AppSettings>(appSettingsSection);

            //Neste momento eu vou obter esta classe atraves do Get<AppSettings> "já com a configuração que passei acima"
            var appSettings = appSettingsSection.Get<AppSettings>();
            // a minha chave key vai ser transformada em um Encoding sequencia de Bytes no formato ASCII 
            //Esta chave será passada na linha 82 => SymmetricSecurityKey
            var Key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //Fim - AppSettings e JWT



            //Inicio - Vamos fazer a configuração do Jason Web Token (JWT)
            services.AddAuthentication( options =>
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
            
            //Fim - Identity
            services.AddControllers();

            //Configuração da (documentação do meu Swagger) para testagem.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NerdStore Enterprise Identity API",
                    Description = "Esta API faz parte do curso ASP.NET Core Enterprise Applications.",
                    Contact = new OpenApiContact() { Name = "Woto Santana", Email = "wotoss10@gmail.com"},
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/license/MIT")}
                });
            });
        }

        //Configure => é a configuração de uso 
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            //confguração (visual ou interface) UI do Swagger
            app.UseSwaggerUI(c => 
             {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization(); //Sitema faz a Autorização

            app.UseAuthentication(); //Sistema faz a Autenticação

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
