using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Movura.Api.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Habilitar el buffer para poder leer el cuerpo de la petición varias veces
            context.Request.EnableBuffering();

            var requestBody = await GetRequestBody(context.Request);

            _logger.LogInformation(
                "\n--- INICIO PETICIÓN ---\n" +
                "Petición: {Method} {Path}\n" +
                "Cuerpo (JSON):\n{RequestBody}\n" +
                "--- FIN PETICIÓN ---",
                context.Request.Method,
                context.Request.Path,
                requestBody);

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var responseBodyContent = await GetResponseBody(context.Response);

                _logger.LogInformation(
                    "\n--- INICIO RESPUESTA ---\n" +
                    "Respuesta para {Method} {Path}: {StatusCode}\n" +
                    "Cuerpo (JSON):\n{ResponseBody}\n" +
                    "--- FIN RESPUESTA ---",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    responseBodyContent);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            request.Body.Position = 0;
            using (var reader = new StreamReader(request.Body, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return FormatJson(body);
            }
        }

        private async Task<string> GetResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(response.Body, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                response.Body.Seek(0, SeekOrigin.Begin);
                return FormatJson(body);
            }
        }

        private string FormatJson(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
            {
                return "<vacío>";
            }

            try
            {
                // Intenta deserializar y luego serializar con formato "pretty-print"
                using (var jDoc = System.Text.Json.JsonDocument.Parse(jsonStr))
                {
                    return System.Text.Json.JsonSerializer.Serialize(jDoc.RootElement, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                }
            }
            catch (System.Text.Json.JsonException)
            {
                // Si no es un JSON válido, devuélvelo tal cual.
                return jsonStr;
            }
        }
    }
}
