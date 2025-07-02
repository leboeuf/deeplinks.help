using Deeplinks.Help.Examples.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<ExampleService>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllers();

app.Run();
