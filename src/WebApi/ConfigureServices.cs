using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ZymLabs.NSwag.FluentValidation;

namespace WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        
        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();
        
        services.AddControllers();
        
        // Add swagger 
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "mediary api",
            });
        });
        
        services.AddScoped<FluentValidationSchemaProcessor>(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        // TODO: OPEN ID
        // services.AddOpenApiDocument((configure, serviceProvider) =>
        // {
        //     var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();
        //
        //     // Add the fluent validations schema processor
        //     configure.SchemaProcessors.Add(fluentValidationSchemaProcessor);
        //
        //     configure.Title = "mediary API";
        //     configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
        //     {
        //         Type = OpenApiSecuritySchemeType.ApiKey,
        //         Name = "Authorization",
        //         In = OpenApiSecurityApiKeyLocation.Header,
        //         Description = "Type into the textbox: Bearer {your JWT token}."
        //     });
        //
        //     configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        // });

        return services;
    }
}