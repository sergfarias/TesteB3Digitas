using TesteDigitas.Api.Configuration;
using TesteDigitas.Application.Services;
using TesteDigitas.Application.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;
using TesteDigitas.Application.Services.Price;
using TesteDigitas.Application.Services.MongoDb;
using TesteDigitas.Application.Services.BitStamp;

namespace TesteDigitas.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IOrderBookService, OrderBookService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IMongoDbService, MongoDbService>();
            services.AddScoped<IPriceService, PriceService>();

            services.AddControllers(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });

            // cache in memory
            services.AddMemoryCache();
            services.AddSwaggerServices();

            CronJobSettings.Runtime = Convert.ToInt32(Configuration.GetSection("CronJobSettings:Runtime").Value);
            //Connection.ConnectionString = Configuration.GetSection("Connection:ConnectionString").Value;
            services.AddScoped<CronJobService>();
            var serviceProvider = services.BuildServiceProvider();
            var task = serviceProvider.GetService<CronJobService>().StartAsync();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //exception handler with Azure Service Bus
            //app.ConfigureExceptionHandler(new AzureServiceBus(Configuration));
            app.UseRouting();
            //enables swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API DevOps v1");
            });
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseWebSockets();
        }
    }
}
