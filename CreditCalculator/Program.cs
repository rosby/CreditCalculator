using CreditCalculator.Domain;
using CreditCalculator.Middlewares;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

#region Configuration
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

#endregion


#region Controller

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

#endregion


#region Swagger

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1.0", new OpenApiInfo
    {
        Title = "CreditCalculator"
    });
    
    options.UseAllOfToExtendReferenceSchemas();

    var pathToXmlDocs = Path.Combine(AppContext.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".xml");
    options.IncludeXmlComments(pathToXmlDocs, true);
});

#endregion


#region Services

builder.Services.AddTransient<CreditCalculatorService>();


#endregion

    
var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

if (app.Environment.IsDevelopment() || configuration.GetValue<bool>("enable_swagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Credit Calculator");
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });

    app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));
}

await app.RunAsync();