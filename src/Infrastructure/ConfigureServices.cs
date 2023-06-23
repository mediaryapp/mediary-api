using Infrastructure.Common;
using Infrastructure.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add Database connections
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("mediary-api-inmemory-db"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();
        
        
        // Add services
        services.AddTransient<IDateTime, DateTimeService>();


        // Add auth options
        // TODO: Difference between AddIdentity and AddIdentityCore and AddDefaultIdentity
        services
            // .AddIdentity<ApplicationUser, IdentityRole>()
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders(); // TODO: wtf

        // TODO: Wtf is identity service (to turn it on also add app.UseIdentityServer(); in Program.cs)
        // services.AddIdentityServer()
        //     .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        
        // TODO: Add options
        // services.Configure<IdentityOptions>(options =>
        // {
        //     //Password settings        
        //     options.Password.RequireDigit = true;
        //     options.Password.RequiredLength = 6;
        //     options.Password.RequireNonAlphanumeric = false; // will we?
        //     options.Password.RequireUppercase = true;
        //     options.Password.RequireLowercase = true;
        //     options.Password.RequiredUniqueChars = 1;
        //
        //     //Lockout settings
        //     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //     options.Lockout.MaxFailedAccessAttempts = 5;
        //     options.Lockout.AllowedForNewUsers = true;
        //     
        //     // User settings
        //     options.User.RequireUniqueEmail = true;
        //     options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
        // });
        
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IIdentityService, IdentityService>();
        
        // TODO: MAY BE NOT NEEDED
        // var cookiePolicyOptions = new CookiePolicyOptions
        // {
        //     MinimumSameSitePolicy = SameSiteMode.Strict,
        // };

        // TODO: Remove JWT
        // services.AddAuthentication();
        //     .AddIdentityServerJwt();

        // TODO: Cookie? WTF is IdentityCookie vs Cookie + what happens when I remove "CookieAuthenticationDefaults.AuthenticationScheme"
        // NOTE: Identity cookie auth variant below
        // services.AddAuthentication()
        //     .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
        //     {
        //         o.LoginPath = "/api/account";
        //         o.AccessDeniedPath = string.Empty;
        //         o.Events.OnRedirectToLogin = context =>
        //         {
        //             context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //             return Task.FromResult<object>(null);
        //         };
        //         o.Events.OnRedirectToAccessDenied = context =>
        //         {
        //             context.Response.StatusCode = StatusCodes.Status403Forbidden;
        //             return Task.FromResult<object>(null);
        //         };
        //         o.Events.OnRedirectToReturnUrl = context =>
        //         {
        //             context.Response.StatusCode = StatusCodes.Status403Forbidden;
        //             return Task.FromResult<object>(null);
        //         };
        //         // o.Cookie.HttpOnly = true;
        //         o.Cookie.Name = "testCookie";
        //         // o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //         // o.SlidingExpiration = true;
        //     });

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
        }).AddIdentityCookies(cookiesBuilder =>
        {
            cookiesBuilder.ApplicationCookie?.Configure(options =>
            {
                // TODO: Cookie options
                options.Cookie.Name = "testCookie";
                
                // Rewrite default redirects
                options.LoginPath = string.Empty;
                options.AccessDeniedPath = string.Empty;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;                
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;                
                };
            });
        });

        // TODO: I don't know wtf is this
        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}