using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

public class ParkingConfig
{
    [Key]
    public int Id { get; set; }

    public string ParkingId { get; set; } = string.Empty;
    public Parking? Parking { get; set; }

    public int TotalSpaces { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TarifaBase { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostoHora { get; set; }
    
    // Propiedad que faltaba, ahora agregada.
    public int FraccionMin { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostoFraccion { get; set; }

    // Propiedad renombrada para coincidir con el servicio.
    public int GraciaMin { get; set; }
    
    // Propiedad que faltaba, ahora agregada.
    public string HoraCorte { get; set; } = "00:00";
}
