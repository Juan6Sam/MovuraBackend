using System.ComponentModel.DataAnnotations;

namespace Movura.Api.Data.Entities;

public class Transaccion
{
    [Key]
    public int Id { get; set; }

    public int? PagoId { get; set; }
    public Pago? Pago { get; set; }

    public string? ParkingId { get; set; }
    public Parking? Parking { get; set; }
    
    public int? AccesoId { get; set; }
    public Acceso? Acceso { get; set; }

    public int? QRId { get; set; }
    public CodigoQR? CodigoQR { get; set; }

    public int StatusId { get; set; }
    public Status? Status { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }
    
    public int? TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    
    public int? ComercioId { get; set; }
    public Comercio? Comercio { get; set; }

    public decimal Amount { get; set; }
    
    public decimal DiscountAmount { get; set; }
    
    public string PaymentMethod { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
}
