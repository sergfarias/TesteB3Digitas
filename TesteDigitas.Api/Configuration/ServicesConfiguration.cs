using Microsoft.OpenApi.Models;
namespace TesteDigitas.Api.Configuration
{
    public static class ServicesConfiguration
    {
        /// <summary>
        /// Configures the swagger documentation
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerServices(this IServiceCollection services)
        {
            //Swagger
            var ver = typeof(Startup).Assembly.GetName()?.Version?.ToString();
            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                     new Microsoft.OpenApi.Models.OpenApiInfo
                     {
                         Title = "API Ambulatorio",
                         Version = "v1",
                         Description = "Build: " + ver,
                         Contact = new Microsoft.OpenApi.Models.OpenApiContact
                         {
                             Name = "Api",
                         }
                     });

                //Determine base path for the application.
                var basePath = AppContext.BaseDirectory;
                var assemblyName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
                var fileName = System.IO.Path.GetFileName(assemblyName + ".xml");

                //Set the comments path for the swagger json and ui.
                //c.IncludeXmlComments(System.IO.Path.Combine(basePath, fileName));

                // Add security definitions
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, Array.Empty<string>() }
                });

            });
        }
    }
}
