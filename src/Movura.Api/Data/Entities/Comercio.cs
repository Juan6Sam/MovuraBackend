using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("Comercios")]
public class Comercio
{
    [Key]
    [Column("id_comercio")]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Column("direccion")]
    [StringLength(300)]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    [Column("tipo_cortesia")]
    [StringLength(20)]
    public string Tipo { get; set; } = string.Empty; // "monto" o "tiempo"

    [Required]
    [Column("valor", TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }

    [Required]
    [Column("id_estatus")]
    public int StatusId { get; set; }

    [Required]
    [Column("fecha_alta")]
    public DateTime FechaAlta { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status? Status { get; set; }

    public virtual ICollection<ComercioEmail> Emails { get; set; } = new List<ComercioEmail>();
    public virtual ICollection<CodigoQR> CodigosQR { get; set; } = new List<CodigoQR>();
}