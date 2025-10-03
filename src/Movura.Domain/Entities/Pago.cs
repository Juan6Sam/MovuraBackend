using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Domain.Entities;

[Table("Pagos")]
public class Pago
{
    [Key]
    [Column("id_pago")]
    public int Id { get; set; }

    [Required]
    [Column("id_ticket")]
    public int TicketId { get; set; }

    [Column("id_usuario")]
    public int? UserId { get; set; }

    [Column("id_acceso")]
    public int? AccesoId { get; set; }

    [Required]
    [Column("monto", TypeName = "decimal(18,2)")]
    public decimal Monto { get; set; }

    [Required]
    [Column("metodo_pago")]
    [StringLength(50)]
    public string MetodoPago { get; set; } = string.Empty;

    [Required]
    [Column("fecha_pago")]
    public DateTime FechaPago { get; set; }

    [Required]
    [Column("id_estatus")]
    public int StatusId { get; set; }

    [ForeignKey(nameof(TicketId))]
    public virtual Ticket? Ticket { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    [ForeignKey(nameof(AccesoId))]
    public virtual Acceso? Acceso { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status? Status { get; set; }

    public virtual ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}