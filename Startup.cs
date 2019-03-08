using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Autofac;
using webapp.Services;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using webapp.Abstractions;
using webapp.Data;
using webapp.Models;
using System.Reflection;
using System.IO;

namespace webapp
{
    public class Startup
    {
        private readonly IConfiguration _config = null;
        public Startup(IConfiguration configuration)
        {
            this._config = configuration;
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<JwtHelper>().As<IJwtHelper>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<Encryption>().As<IEncryption>();
            builder.RegisterGeneric(typeof(MongoContext<>)).As(typeof(IMongoContext<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(GeneralRepository<>)).As(typeof(IGeneralRepository<>)).InstancePerDependency();

        }


        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DbSettings>(options =>
            {
                options.ConnectionString
                    = _config.GetSection("MongoConnection:ConnectionString").Value;
                options.Database
                    = _config.GetSection("MongoConnection:Database").Value;
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Issuer",
                    ValidAudience = "Audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["ak:superSecretKey"]))
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });
            services.AddAutoMapper();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = ".Net Core API",
                    Description = ".Net Core Backend",
                    Contact = new Contact
                    {
                        Name = "olegtar83@gmail.com",
                        Email = string.Empty,
                        Url = "https://twitter.com/olegtar83"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://localhost/"
                    }               
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.InjectStylesheet("/swagger/ui/custom.css");
                c.RoutePrefix = string.Empty;
            });
            app.UseCors("EnableCORS");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
