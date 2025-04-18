using Microsoft.AspNetCore.Mvc;
using PaginationApp.Services.Parts;
using DotNetEnv;
using PaginationApp.Services.ElasticSearch;
using PaginationApp.Services.Parts.Contracts;
using PaginationApp.Core.Exceptions;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Registro de servicios de la aplicación
builder.Services.AddControllers();

var elasticUrl = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL");
var elasticUsername = Environment.GetEnvironmentVariable("ELASTICSEARCH_USERNAME");
var elasticPassword = Environment.GetEnvironmentVariable("ELASTICSEARCH_PASSWORD");

// Validar las variables de entorno antes de registrar el servicio
if (string.IsNullOrWhiteSpace(elasticUrl))
    throw new ConfigurationException("ELASTICSEARCH_URL no está configurado");
if (string.IsNullOrWhiteSpace(elasticUsername))
    throw new ConfigurationException("ELASTICSEARCH_USERNAME no está configurado");
if (string.IsNullOrWhiteSpace(elasticPassword))
    throw new ConfigurationException("ELASTICSEARCH_PASSWORD no está configurado");

// Registrar ElasticConnection como Singleton con parámetros validados
builder.Services.AddSingleton<ElasticConnection>(_ => 
    new ElasticConnection(elasticUrl, elasticUsername, elasticPassword));

builder.Services.AddScoped<IPartMapper, PartMapper>();
builder.Services.AddScoped<ElasticSearchService>();
builder.Services.AddScoped<PartService>();
builder.Services.AddScoped<IPartSearchService, ElasticPartSearchService>();

var app = builder.Build();

// Inicialización sincrónica (elimina warning CS1998)
InitializeElasticSearch(app);

app.MapControllers();

await app.RunAsync();

// Método modificado para ser sincrónico
void InitializeElasticSearch(WebApplication app)
{
   
    var partService = app.Services.GetRequiredService<PartService>();
    
}