using Microsoft.AspNetCore.Mvc;
using PaginationApp.Services.Parts;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using PaginationApp.Infraestucture;
using Microsoft.Extensions.DependencyInjection;
using PaginationApp.Services.ElasticSearch;
using PaginationApp.Services.Parts.Contracts;


Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Registro de servicios de la aplicación
builder.Services.AddControllers();

var elasticUrl = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL");
var elasticUsername = Environment.GetEnvironmentVariable("ELASTICSEARCH_USERNAME");
var elasticPassword = Environment.GetEnvironmentVariable("ELASTICSEARCH_PASSWORD");

// Registrar ElasticConnection como Singleton
builder.Services.AddSingleton<ElasticConnection>(provider =>
{
    return new ElasticConnection(elasticUrl, elasticUsername, elasticPassword);
});

builder.Services.AddScoped<IPartMapper, PartMapper>();

builder.Services.AddScoped<ElasticSearchService>();
builder.Services.AddScoped<PartService>();
builder.Services.AddScoped<IPartSearchService, ElasticPartSearchService>();

var app = builder.Build();

await InitializeElasticSearch(app);

app.MapControllers();

await app.RunAsync();

async Task InitializeElasticSearch(WebApplication app)
{

    var partService = app.Services.GetRequiredService<PartService>();
    
   
}
