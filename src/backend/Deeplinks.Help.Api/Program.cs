using Deeplinks.Help.Api.Infrastructure.Configuration;
using Deeplinks.Help.Api.Infrastructure.Constants;
using Deeplinks.Help.Api.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient(HttpClients.ChecksHttpClient)
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        AllowAutoRedirect = false,
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        UseCookies = false,
        MaxConnectionsPerServer = 64
    });

var allowedCorsOrigin = builder.Configuration.GetValue<string>("AllowedCorsOrigin")!;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCorsFromTrustedDomain", policy =>
    {
        policy.WithOrigins(allowedCorsOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<CacheConfiguration>(builder.Configuration.GetSection("Cache"));
builder.Services.AddSingleton<CheckService>();

var app = builder.Build();

app.UseCors("AllowCorsFromTrustedDomain");

app.UseAuthorization();

app.MapControllers();

app.Run();
