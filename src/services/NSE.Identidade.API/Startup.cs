
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NSE.Identidade.API.Configuration;


namespace NSE.Identidade.API
{
    public class Startup
    {
      
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(hostEnvironment.ContentRootPath) // (SetBasePath) => irá pegar o caminho da minha aplicação atraves do (ContentRootPath)
                 .AddJsonFile("appsettings.json", true, true)//logo irei decobrir o (appsettings.json) e adiciono ele a minha configuração
                 .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true) //este (hostEnvironment.EnvironmentName) é o meu ambiente appsettings.Desenvolvimento.json ou equivale => appsettings.production.json
                 .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>(); //A sua Startup pode usar o secrets ou não
            }
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentityConfiguration(Configuration);

            services.AddApiConfiguration();

            services.AddSwaggerConfiguration();

        }

        //Configure => é a configuração de uso 
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration(env);
        }
    }
}
