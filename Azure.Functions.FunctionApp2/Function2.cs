using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Azure.Functions.FunctionApp2
{
    public class Function2
    {
        private readonly ILogger<Function2> _logger;

        public Function2(ILogger<Function2> logger)
        {
            _logger = logger;
        }

        [Function("Function2")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("Function2 HTTP trigger -> iniciando el proceso de la petición.");

                string nombre = req.Query["nombre"];

                string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                nombre = nombre ?? data?.nombre;

                string mensaje = string.IsNullOrEmpty(nombre)
                    ? "Función HTTP ejecutada correctamente. Introduzca un nombre en la cadena de consulta o en el cuerpo de la solicitud para obtener una respuesta personalizada."
                    : $"Hola, {nombre}. Función HTTP ejecutada correctamente.";

                _logger.LogInformation("Function2 HTTP trigger -> proceso finalizado.");

                string storageConnection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                if (!string.IsNullOrEmpty(storageConnection))
                {
                    var storageClient = new TableServiceClient(storageConnection);
                    var table = storageClient.GetTableClient("operaciones");

                    var registro = new Register()
                    {
                        PartitionKey = "https",
                        RowKey = Guid.NewGuid().ToString(),
                        Nombre = nombre,
                        Mensaje = mensaje
                    };

                    table.AddEntity(registro);
                }
                else _logger.LogInformation("No se puede conectar con Storage.Table, falta cadena de conexión.");

                return new OkObjectResult(mensaje);
            }
            catch (Exception e)
            {
                return new ConflictObjectResult(e.Message);
            }
        }
    }

    public class Register : ITableEntity
    {
        public string Nombre { get; set; }
        public string Mensaje { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

    }
}
