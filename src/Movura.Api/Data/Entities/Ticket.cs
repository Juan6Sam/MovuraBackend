using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("Tickets")]
public class Ticket
{
    [Key]
    [Column("id_ticket")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int? UserId { get; set; }

    [Column("id_acceso")]
    public int? AccesoId { get; set; }

    [Required]
    [Column("fecha_emision")]
    public DateTime FechaEmision { get; set; }

    [Required]
    [Column("id_estatus")]
    public int StatusId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    [ForeignKey(nameof(AccesoId))]
    public virtual Acceso? Acceso { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status? Status { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}