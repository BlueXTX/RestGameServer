using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PaidGame.Server.Models;
using PaidGame.Server.Services;

namespace PaidGame.Server
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling =
                    ReferenceLoopHandling.Ignore);
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo {Title = "PaidGame.Server", Version = "v1"});
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            var authOptionsConfiguration = Configuration.GetSection("Auth");
            services.Configure<AuthOptions>(authOptionsConfiguration);
            services.AddDbContext<ApplicationContext>(ServiceLifetime.Transient);
            services.AddScoped<BoosterManager>();
            services.AddScoped<LeaguesManager>();
            services.AddScoped<AccountsManager>();
            services.AddScoped<TokensManager>();
            services.AddJwtAuth(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("v1/swagger.json", "PaidGame.Server v1"));
            }

            app.UseCors(x =>
                x.SetIsOriginAllowed(str => _ = true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseDeveloperExceptionPage();
        }
    }
}
