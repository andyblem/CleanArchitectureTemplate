using CleanArchitecture.Application;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Infrastructure.IdentityProvider;
using CleanArchitecture.Infrastructure.Persistance.Seeders;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Persistence.Seeders;
using CleanArchitecture.Infrastructure.Shared;
using CleanArchitecture.Presentation.Web.API.Extensions;
using CleanArchitecture.Presentation.Web.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


//Read Configuration from appSettings
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "Development" : string.Empty)}.json",
        optional: true,
        reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

//Initialize Logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();

// create builder
var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel from appsettings.json
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Configure(config.GetSection("Kestrel"));
});

// use Serilog instead of default .Net Logger
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// add services to DI
builder.Services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();

// add services to container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationLayer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentityProviderInfrastructure(builder, config);
builder.Services.AddPersistenceInfrastructure(config);
builder.Services.AddSharedInfrastructure(config);


// add claims as authorization policies
var allUniqueClaims = RolesSeeder.Claims;
builder.Services.AddAuthorization(options =>
{
    // for each claim, add it as a policy
    foreach (var claim in allUniqueClaims)
    {
        // get defined claims
        options.AddPolicy(claim.ClaimType,
            policy => policy.RequireClaim(claim.ClaimType, claim.ClaimValue).RequireAuthenticatedUser());

    };
});


builder.Services.AddSwaggerExtension();
builder.Services.AddControllers();
builder.Services.AddApiVersioningExtension();
builder.Services.AddHealthChecks();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .Build();
        });
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});


// build the web application
var app = builder.Build();

// configure the web application
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseErrorHandlingMiddleware();
app.UseCors("AllowOrigin");
app.UseHttpsRedirection();
app.UseRouting();
app.UseHttpLogging();
app.UseSwaggerExtension();
app.UseHealthChecks("/health");

app.MapControllers();

app.AddIdentityProviderInfrastructure();


// default reply
app.MapGet("/", () => "Hello, you have reached CleanArchitecture WebAPI!!!");


// seed data
await DatabaseSeeder.SeedAsync(app.Services.CreateScope(), config);

// start app
try
{
    await app.RunAsync();
    Log.Information("CleanArchitecture(WebAPI) started successfuly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "CleanArchitecture(WebAPI) terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
