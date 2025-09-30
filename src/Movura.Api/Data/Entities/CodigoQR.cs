using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("CodigosQR")]
public class CodigoQR
{
    [Key]
    [Column("id_qr")]
    public int Id { get; set; }

    [Required]
    [Column("codigo")]
    public Guid Codigo { get; set; }

    [Column("id_usuario")]
    public int? UserId { get; set; }

    [Column("id_comercio")]
    public int? ComercioId { get; set; }

    [Column("id_acceso")]
    public int? AccesoId { get; set; }

    [Required]
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; }

    [Required]
    [Column("fecha_expiracion")]
    public DateTime FechaExpiracion { get; set; }

    [Required]
    [Column("id_estatus")]
    public int StatusId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    [ForeignKey(nameof(ComercioId))]
    public virtual Comercio? Comercio { get; set; }

    [ForeignKey(nameof(AccesoId))]
    public virtual Acceso? Acceso { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status? Status { get; set; }

    public virtual ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}