using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("Excesos")]
public class Exceso
{
    [Key]
    [Column("id_exceso")]
    public int Id { get; set; }

    [Required]
    [Column("id_acceso")]
    public int AccesoId { get; set; }

    [Required]
    [Column("hora_inicio")]
    public DateTime HoraInicio { get; set; }

    [Required]
    [Column("hora_fin")]
    public DateTime HoraFin { get; set; }

    [Required]
    [Column("monto", TypeName = "decimal(18,2)")]
    public decimal Monto { get; set; }

    [ForeignKey(nameof(AccesoId))]
    public virtual Acceso? Acceso { get; set; }
}