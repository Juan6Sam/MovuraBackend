using System.ComponentModel.DataAnnotations;

namespace Movura.Api.Data.Entities;

public class Ticket
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public int? AccesoId { get; set; }
    public Acceso? Acceso { get; set; }

    public string? ParkingId { get; set; }
    public Parking? Parking { get; set; }

    public int StatusId { get; set; }
    public Status? Status { get; set; }

    public string Plate { get; set; } = string.Empty;
    
    public DateTime EntryTime { get; set; }
    
    public DateTime? ExitTime { get; set; }
    
    public DateTime FechaEmision { get; set; }
    
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
