using System;
using System.Collections.Generic;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SingleSignOn.Constants;
using SingleSignOn.DataAccess.Context;
using SingleSignOn.DataAccess.Repositories;
using WebApiBaseLibrary.Authorization.Configurators;
using WebApiBaseLibrary.Authorization.Constants;
using WebApiBaseLibrary.Authorization.Extensions;
using WebApiBaseLibrary.Authorization.Generators;
using WebApiBaseLibrary.Infrastructure.Configuration;
using WebApiBaseLibrary.Infrastructure.Generators;

namespace SingleSignOn
{
    public class Startup
    {
        private IServiceProvider _serviceProvider;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConnectionString = Configuration.GetConnectionString("SSO_DB_CONNECTION_STRING");

            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddDbContext<AccountContext>(options => options.UseNpgsql(databaseConnectionString!));

            services.AddScoped<IAccountRepository, AccountRepository>();

            var hashConfiguration = Configuration.GetSection("HashConfiguration").Get<HashConfiguration>();
            services.AddScoped<IHashGenerator, HashGenerator>(s => new HashGenerator(hashConfiguration));

            services.AddSingletonJwtConfiguration(Configuration);
            services.AddSingleton<IJwtConfigurator, JwtConfigurator>();
            services.AddConfiguredJwtBearer(() =>
            {
                var jwtConfigurator = _serviceProvider.GetService<IJwtConfigurator>();

                return jwtConfigurator?.ValidationParameters;
            });
            services.AddScoped<IJwtGenerator, JwtGenerator>();

            services.AddAuthorization();

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "Json Web Token for authorization. Write: 'Bearer {your token}'",
                    Name = HeaderNames.Authorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = Schemes.Bearer
                };
                options.AddSecurityDefinition(securityScheme.Scheme, securityScheme);

                var requirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = securityScheme.Scheme
                            },
                            Scheme = Schemes.OAuth,
                            Name = securityScheme.Scheme,
                            In = securityScheme.In
                        },
                        new List<string>()
                    }
                };

                options.AddSecurityRequirement(requirement);

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SingleSignOn", Version = "v1" });
            });

            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _serviceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SingleSignOn v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}