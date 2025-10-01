using System.Collections.Generic;

namespace Movura.Api.Models.Dto
{
    public class ComercioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; } // monto|tiempo
        public decimal Valor { get; set; }
        public List<string> Usuarios { get; set; }
        public string Estatus { get; set; }
    }

    public class ComercioCreateRequest
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public List<string> Usuarios { get; set; }
        public string Estatus { get; set; }
    }

    public class ComerciosBulkRequest
    {
        public List<ComercioCreateRequest> Comercios { get; set; }
    }

    public class ComerciosBulkResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ComercioDto> Comercios { get; set; }
    }
}
