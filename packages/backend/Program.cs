using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using Serilog.Events;
using YaleAccess.Models.Options;
using YaleAccess.Services;
using YaleAccess.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Create the bootstraper logger
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

try
{
    // Add services to the container.
    builder.Services.AddControllers();

    // Configure the options
    builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection(AuthenticationOptions.Authentication));
    builder.Services.Configure<CodesOptions>(builder.Configuration.GetSection(CodesOptions.Codes));
    builder.Services.Configure<DevicesOptions>(builder.Configuration.GetSection(DevicesOptions.Devices));
    builder.Services.Configure<ZWaveOptions>(builder.Configuration.GetSection(ZWaveOptions.ZWave));

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Get a copy of the configuration
    IConfiguration configuration = builder.Configuration;
    string logLocation = configuration["LogLocation"] ?? "Log.txt";

    // Setup the application logger
    Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Error)
                .WriteTo.File(logLocation, rollingInterval: RollingInterval.Day)
                .CreateLogger();

    // Configure the DI services
    // If the environment is development and the mock option is set to true, use the mock service
    if (builder.Environment.IsDevelopment() && configuration.GetValue<bool>("UseMockDevelopmentMode"))
    {
        builder.Services.AddSingleton<MockYaleData>();
        builder.Services.AddScoped<IYaleAccessor, MockYaleAccessor>();
    }
    else
    {
        builder.Services.AddScoped<IYaleAccessor, YaleAccessor>();
    }
    

    // Setup CORS
    string[] corsAllowedOrigins = (configuration["CorsAllowedOrigins"] ?? "http://localhost:3000").Split(",") ;
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(corsAllowedOrigins)
                .AllowCredentials()
                .WithHeaders("Content-Type", "Authorization");
        });
    });

    // Setup cookie authentication scheme
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            // Set HTTP only to true to prevent XSS attacks
            options.Cookie.HttpOnly = true;

            // Set secure policy to always to prevent sending cookies over HTTP for production use, for development set to None
            if (builder.Environment.IsDevelopment())
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            else
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            
            // Ensure that the API will return a 401 Unauthorized instead of redirecting to the login page
            options.AccessDeniedPath = string.Empty;
            options.LoginPath = string.Empty;
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });

    // Setup logging flow
    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure the forwarded headers middleware
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    // Ignore host aborted exceptions caused by build checks
    if (ex is not HostAbortedException)
    {
        Log.Fatal(ex, "Host terminated unexpectedly");
        throw;
    }
}
finally
{
    Log.CloseAndFlush();
}