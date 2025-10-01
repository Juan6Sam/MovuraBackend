using System;
using System.Collections.Generic;

namespace Movura.Api.Models.Dto
{
    public class ParkingListItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Grupo { get; set; }
        public string AdminNombre { get; set; }
        public string AdminCorreo { get; set; }
        public DateTime AltaISO { get; set; }
        public string Estatus { get; set; }
    }

    public class ParkingDetail
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public object Config { get; set; }
        public object Comercios { get; set; }
    }

    public class ParkingConfigRequest
    {
        public decimal TarifaBase { get; set; }
        public decimal CostoHora { get; set; }
        public int FraccionMin { get; set; }
        public decimal CostoFraccion { get; set; }
        public int GraciaMin { get; set; }
        public string HoraCorte { get; set; }
    }

    public class ParkingConfigResult
    {
        public bool Success { get; set; }
        public object Config { get; set; }
        public string Message { get; set; }
    }
}
