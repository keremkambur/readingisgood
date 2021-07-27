using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ReadingIsGood.Api.Controllers;
using ReadingIsGood.Api.Extensions;
using ReadingIsGood.Api.Middleware;
using ReadingIsGood.Api.Middleware.ApiExceptionHandler;
using ReadingIsGood.Api.Middleware.JwtCheckToRefreshATokenMiddleware;
using ReadingIsGood.BusinessLayer;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.Options;
using ReadingIsGood.BusinessLayer.Services;
using ReadingIsGood.DataLayer;
using ReadingIsGood.DataLayer.Mappings.Base;
using ReadingIsGood.EntityLayer.Enum;
using ReadingIsGood.Utils;
using ReadingIsGood.Utils.Jwt;

namespace ReadingIsGood.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddOptions();
            services.Configure<SqlOptions>(Configuration.GetSection(nameof(SqlOptions)));
            services.Configure<AuthenticationServiceOptions>(Configuration.GetSection(nameof(AuthenticationServiceOptions)));

            var jwtValidationOptions = Configuration.GetSection(nameof(JwtValidationOptions)).Get<JwtValidationOptions>();

            services.Configure<JwtValidationOptions>(Configuration.GetSection(nameof(JwtValidationOptions)));
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(RSA.Create()), SecurityAlgorithms.RsaSha256);
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtValidationOptions.Issuer;
                options.SigningCredentials = signingCredentials;

                var config = Configuration.GetSection(nameof(JwtValidForKindOptions)).Get<JwtValidForKindOptions>();

                options.AccessTokenValidFor.Clear();
                options.AccessTokenValidFor.Add(nameof(JwtValidForKind.Customer), TimeSpan.FromMinutes(config.AccessToken.Customer));
                options.AccessTokenValidFor.Add(nameof(JwtValidForKind.Admin), TimeSpan.FromMinutes(config.AccessToken.Admin));

                options.RefreshTokenValidFor.Clear();
                options.RefreshTokenValidFor.Add(nameof(JwtValidForKind.Customer), TimeSpan.FromMinutes(config.RefreshToken.Customer));
                options.RefreshTokenValidFor.Add(nameof(JwtValidForKind.Admin), TimeSpan.FromMinutes(config.RefreshToken.Admin));
            });

            services
                .AddMvcCore()
                .AddAuthorization()
                .AddFormatterMappings();

            services.AddTransient<SqlDbContext>();
            services.AddScoped<IEntityMapper, EntityMapper>();
            services.AddTransient<IBusinessObject, BusinessObject>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidIssuer = jwtValidationOptions.Issuer,
                    IssuerSigningKey = signingCredentials.Key,
                    ValidAudience = nameof(Audience.Auth),
                    ClockSkew = TimeSpan.Zero,
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage(); 
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseMiddleware<ApiExceptionHandlerMiddleware>();
        }
    }
}