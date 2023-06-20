using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

// TODO: REMOVE CORS!!!!
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsAllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
    });
});


// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddVersionedApiExplorer( options => options.GroupNameFormat = "'v'VVV" );
// builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
// builder.Services.AddSwaggerGen(options =>
// {
//     options.EnableAnnotations();
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    
    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
    }
    app.UseSwagger();
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
    // app.UseSwagger();
    // app.UseSwaggerUI(options =>
    // {
    //     var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>(); 
    //     foreach ( var description in provider.ApiVersionDescriptions )
    //     {
    //         options.SwaggerEndpoint(
    //             $"/swagger/{description.GroupName}/swagger.json",
    //             description.GroupName.ToUpperInvariant() 
    //         );
    //     }
    //     // serve from root
    //     options.RoutePrefix = string.Empty;
    // });
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// TODO: REMOVE CORS!!!!
app.UseCors("CorsAllowAll");


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();