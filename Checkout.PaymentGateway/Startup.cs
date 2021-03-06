using Checkout.PaymentGateway.Middleware;
using Checkout.PaymentGateway.Services;
using Checkout.PaymentGateway.StartupConfiguration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Checkout.PaymentGateway
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
                });

            BsonSerializer.RegisterSerializationProvider(new DateTimeOffsetSerializationProvider());
            services.AddApplication(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
            });

            services
                .AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });

            services.AddSwagger(Environment.EnvironmentName);

            services
                .AddMvcCore()
                .AddMetricsCore();

            services.AddServiceClients(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseSwagger(Environment.EnvironmentName);

            app.UseRouting();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
