using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Movura.Api.Data.Entities;

public class Parking
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string Nombre { get; set; } = null!;
    
    [Required]
    public string Direccion { get; set; } = null!;
    
    public string? Grupo { get; set; }
    
    [Required]
    public string AdminNombre { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    public string AdminCorreo { get; set; } = null!;
    
    public DateTime AltaISO { get; set; } = DateTime.UtcNow;
    
    [Required]
    public string Estatus { get; set; } = "Activo";
    
    public ParkingConfig Config { get; set; } = null!;
    
    public ICollection<Comercio> Comercios { get; set; } = new List<Comercio>();

    // Colecciones que faltaban, ahora agregadas para resolver errores de DbContext.
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}
