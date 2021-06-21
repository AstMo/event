using System;
using System.IO;
using System.Text;
using PartyMaker.Application.Exception;
using PartyMaker.Application.Middleware;
using PartyMaker.Application.Service;
using PartyMaker.Configuration.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;

namespace PartyMaker.Application
{
    public class Startup
    {
        public Startup(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            Environment.SetEnvironmentVariable("LOGBASEDIR", env.ContentRootPath);

            var builder = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json", true, true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                   .AddJsonFile($"configs{Path.DirectorySeparatorChar}authentication.{env.EnvironmentName}.json", true)
                   .AddJsonFile($"configs{Path.DirectorySeparatorChar}queues.{env.EnvironmentName}.json", true)
                   .AddJsonFile($"configs{Path.DirectorySeparatorChar}imageStore.{env.EnvironmentName}.json", true)
                   .AddJsonFile($"configs{Path.DirectorySeparatorChar}serilog.{env.EnvironmentName}.json", true)
                   .AddJsonFile($"configs{Path.DirectorySeparatorChar}database.{env.EnvironmentName}.json", true)
                   .AddJsonFile($"configs{Path.DirectorySeparatorChar}webappconf.{env.EnvironmentName}.json", true)
                   .AddEnvironmentVariables();

            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            var appSettingsSection = Configuration.GetSection("Authentication");
            services.Configure<AuthenticationSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AuthenticationSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                    });
            });

            services.AddRouting();

            // Swagger
            services.AddOpenApiDocument(configure =>
            {
                configure.SchemaNameGenerator = new CustomSchemaNameGenerator();
                configure.TypeNameGenerator = new CustomTypeNameGenerator();
            });

            services.AddSingleton<IConfiguration>(x => this.Configuration);

            var serviceProvider = AppIocConfigure.ConfigureServices(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/home/error");
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x =>
            {
                x.MapControllerRoute(name: "default", "{controller=Home}/{action=Index}");
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMiddleware<AppMiddlewareException>();
            app.UseMiddleware<LoggerMiddleware>();
            try
            {
                AppIocConfigure.Configure(app);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
