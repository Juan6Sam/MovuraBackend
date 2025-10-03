using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Movura.Domain.Entities;

public class Comercio
{
    [Key]
    public int Id { get; set; }

    // TIPO CORREGIDO: ParkingId ahora es un string para coincidir con Parking.Id
    [Required]
    public string ParkingId { get; set; }
    public Parking? Parking { get; set; }

    public int StatusId { get; set; }
    public Status? Status { get; set; }
    
    public string Nombre { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public decimal Valor { get; set; }
    
    public ICollection<ComercioEmail> Emails { get; set; } = new List<ComercioEmail>();
    
    public ICollection<CodigoQR> CodigosQR { get; set; } = new List<CodigoQR>();
}
