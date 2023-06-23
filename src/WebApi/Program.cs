using Infrastructure;
using Infrastructure.Persistence;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices();

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

    // Use swagger
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "mediary api v1");
        options.DocumentTitle = "mediary swagger";
        options.RoutePrefix = string.Empty;
    });
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// TODO: Remove cors
app.UseCors("CorsAllowAll");


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
// TODO: Identity server
// app.UseIdentityServer();
app.UseAuthorization();

// TODO: I don't know
app.UseCookiePolicy();

app.MapControllers();


app.Run();