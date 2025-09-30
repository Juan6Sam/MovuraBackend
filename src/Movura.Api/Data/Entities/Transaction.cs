using System.ComponentModel.DataAnnotations;

namespace Movura.Api.Data.Entities;

public class Transaction
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    public DateTime EntradaISO { get; set; }
    
    [Required]
    public DateTime SalidaISO { get; set; }
    
    [Required]
    public int Minutes { get; set; }
    
    [Required]
    public string Status { get; set; } = null!;
    
    [Required]
    public decimal Monto { get; set; }
    
    [Required]
    public decimal Excedente { get; set; }
    
    [Required]
    public decimal Total { get; set; }
    
    public string? OperatorId { get; set; }
    
    public string? PaymentMethod { get; set; }
    
    public string? Notes { get; set; }
    
    public string TicketId { get; set; } = null!;
    public Ticket Ticket { get; set; } = null!;
    
    public string ParkingId { get; set; } = null!;
    public Parking Parking { get; set; } = null!;
}