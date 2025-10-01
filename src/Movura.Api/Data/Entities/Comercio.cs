using System.ComponentModel.DataAnnotations;

namespace Movura.Api.Data.Entities;

public class Comercio
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string ParkingId { get; set; } = string.Empty;
    public Parking? Parking { get; set; }

    public int StatusId { get; set; }
    public Status? Status { get; set; }
    
    public string Nombre { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty; // "monto" o "tiempo"

    public decimal Valor { get; set; }
    
    public ICollection<ComercioEmail> Emails { get; set; } = new List<ComercioEmail>();
    
    public ICollection<User> Usuarios { get; set; } = new List<User>();
    
    public ICollection<CodigoQR> CodigosQR { get; set; } = new List<CodigoQR>();
}
