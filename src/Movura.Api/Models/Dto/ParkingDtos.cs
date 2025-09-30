namespace Movura.Api.Models.Dto;

public class ParkingDto
{
    public string Id { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public string? Grupo { get; set; }
    public string AdminNombre { get; set; } = null!;
    public string AdminCorreo { get; set; } = null!;
    public DateTime AltaISO { get; set; }
    public string Estatus { get; set; } = null!;
    public ParkingConfigDto Config { get; set; } = null!;
    public List<ComercioDto> Comercios { get; set; } = new();
}

public class ParkingConfigDto
{
    public decimal TarifaBase { get; set; }
    public decimal CostoHora { get; set; }
    public int FraccionMin { get; set; }
    public decimal CostoFraccion { get; set; }
    public int GraciaMin { get; set; }
    public string HoraCorte { get; set; } = null!;
}