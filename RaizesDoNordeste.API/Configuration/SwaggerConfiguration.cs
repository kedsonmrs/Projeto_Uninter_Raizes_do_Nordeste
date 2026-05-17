using Microsoft.OpenApi.Models;

namespace RaizesDoNordeste.API.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Raízes do Nordeste API",
                Version = "v1",
                Description = "API da rede de lanchonetes Raízes do Nordeste. " +
                              "Suporta múltiplos canais: App, Totem, Balcão, Pickup e Web."
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "<NAO PRECISA POR BEARER> Insira o token JWT. Exemplo: eyJhbGci..."
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
                    []
                }
            });
          
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    public static WebApplication UseSwaggerService(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Raízes do Nordeste API v1");
            c.RoutePrefix = "swagger"; 
        });

        return app;
    }
}