using System.ComponentModel.DataAnnotations;

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
}

public class ParkingConfig
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public int TotalSpaces { get; set; }
    
    [Required]
    public decimal TarifaBase { get; set; }
    
    [Required]
    public decimal CostoHora { get; set; }
    
    [Required]
    public int FraccionMin { get; set; }
    
    [Required]
    public decimal CostoFraccion { get; set; }
    
    [Required]
    public int GraciaMin { get; set; }
    
    [Required]
    public string HoraCorte { get; set; } = "00:00";
    
    public string ParkingId { get; set; } = null!;
    public Parking Parking { get; set; } = null!;
}
