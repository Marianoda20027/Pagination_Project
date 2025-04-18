using Elasticsearch.Net;
using System;

public class ElasticConnection
{
    private readonly ElasticLowLevelClient _client;

    public ElasticConnection(string cloudUrl, string username, string password)
    {
        if (string.IsNullOrEmpty(cloudUrl))
            throw new ArgumentException("URL de Elasticsearch no puede ser nula o vacía.");
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            throw new ArgumentException("El usuario y la contraseña no pueden ser nulos o vacíos.");

        var pool = new SingleNodeConnectionPool(new Uri(cloudUrl));
        var connectionSettings = new ConnectionConfiguration(pool)
            .BasicAuthentication(username, password);  // Usar autenticación básica

        _client = new ElasticLowLevelClient(connectionSettings);
    }

    public ElasticLowLevelClient Client => _client;
}
