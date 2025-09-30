using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("Transacciones")]
public class Transaccion
{
    [Key]
    [Column("id_transaccion")]
    public int Id { get; set; }

    [Column("id_pago")]
    public int? PagoId { get; set; }

    [Column("id_acceso")]
    public int? AccesoId { get; set; }

    [Column("id_qr")]
    public int? QRId { get; set; }

    [Required]
    [Column("tipo")]
    [StringLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    [Column("fecha_transaccion")]
    public DateTime FechaTransaccion { get; set; }

    [Required]
    [Column("monto", TypeName = "decimal(18,2)")]
    public decimal Monto { get; set; }

    [Required]
    [Column("id_estatus")]
    public int StatusId { get; set; }

    [Required]
    [Column("id_usuario")]
    public int UserId { get; set; }

    [ForeignKey(nameof(PagoId))]
    public virtual Pago? Pago { get; set; }

    [ForeignKey(nameof(AccesoId))]
    public virtual Acceso? Acceso { get; set; }

    [ForeignKey(nameof(QRId))]
    public virtual CodigoQR? CodigoQR { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status? Status { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}