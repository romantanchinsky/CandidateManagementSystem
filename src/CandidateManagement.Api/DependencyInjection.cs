using System.Reflection;
using CandidateManagement.Api.Exceptions;
using CandidateManagement.Infrastructure.Data;
using Microsoft.OpenApi.Models;

namespace CandidateManagement.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new()
            {
                Title = "HR Recruitment API",
                Version = "v1",
                Description = "API Cервис для HR по процессу отбора кандидатов"
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
            
            // Включение использования аннотаций
            c.EnableAnnotations();
            // Add JWT support in Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
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
                    Array.Empty<string>()
                }
            });
        });

        //services.AddOpenApi();
        services.AddProblemDetails(configure =>
            configure.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            });
        services.AddExceptionHandler<DomainExceptionHandler>();
        services.AddExceptionHandler<GloblaExceptionHandler>();
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireClaim("userRole", "Admin"));
        });
        services.AddControllers();
        return services;
    }
}