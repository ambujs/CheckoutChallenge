using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Checkout.AcquiringBank.Mock.StartupConfiguration
{
    public static class SwaggerConfiguration
    {
        private static readonly string AssemblyName = Assembly.GetAssembly(typeof(SwaggerConfiguration)).GetName().Name;
        private const string Version = "v1";
        private const string SwaggerRootUrl = "api";
        private static string GetTitle(string environmentName) => $"{AssemblyName} {Version} - ({environmentName})";

        public static void AddSwagger(this IServiceCollection services, string environmentName)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Version, new OpenApiInfo { Title = GetTitle(environmentName), Version = Version });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });

                c.EnableAnnotations();
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder applicationBuilder, string environmentName, string swaggerBaseUrl = "")
        {
            applicationBuilder.UseSwagger(options =>
            {
                if (!string.IsNullOrWhiteSpace(swaggerBaseUrl))
                {
                    options.PreSerializeFilters
                        .Add((swaggerDoc, httpRequest) => swaggerDoc.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer { Url = $"{httpRequest.Scheme}://{httpRequest.Host.Value}{swaggerBaseUrl}" }
                        });
                }
                options.RouteTemplate = $"{SwaggerRootUrl}/{{documentName}}/swagger.json";
            });

            applicationBuilder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/{SwaggerRootUrl}/{Version}/swagger.json", GetTitle(environmentName));
                options.RoutePrefix = SwaggerRootUrl;
            });

            return applicationBuilder;
        }
    }
}
