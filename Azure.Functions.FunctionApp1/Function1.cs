using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Azure.Functions.FunctionApp1
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("Function1 HTTP trigger -> iniciando el proceso de la petición.");

                string nombre = req.Query["nombre"];

                string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                nombre = nombre ?? data?.nombre;

                string mensaje = string.IsNullOrEmpty(nombre)
                    ? "Función HTTP ejecutada correctamente. Introduzca un nombre en la cadena de consulta o en el cuerpo de la solicitud para obtener una respuesta personalizada."
                    : $"Hola, {nombre}. Función HTTP ejecutada correctamente.";

                _logger.LogInformation("Function1 HTTP trigger -> proceso finalizado.");

                return new OkObjectResult(mensaje);
            }
            catch (Exception e)
            {
                return new ConflictObjectResult(e.Message);
            }
        }
    }
}
