namespace Movura.Api.Models.Dto;

public class ComercioDto
{
    public int Id { get; set; }
    public string? ParkingId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // "monto" o "tiempo"
    public decimal Valor { get; set; }
    public List<UserDto> Usuarios { get; set; } = new();
}
