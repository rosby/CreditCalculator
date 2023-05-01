using CreditCalculator.Domain;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});
builder.Services.AddTransient<CreditCalculatorService>();
var app = builder.Build();
app.MapControllers();


await app.RunAsync();