using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("Accesos")]
public class Acceso
{
    [Key]
    [Column("id_acceso")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int? UserId { get; set; }

    [Required]
    [Column("hora_entrada")]
    public DateTime HoraEntrada { get; set; }

    [Column("hora_salida")]
    public DateTime? HoraSalida { get; set; }

    [Required]
    [Column("id_estatus")]
    public int StatusId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status? Status { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public virtual ICollection<Exceso> Excesos { get; set; } = new List<Exceso>();
    public virtual ICollection<CodigoQR> CodigosQR { get; set; } = new List<CodigoQR>();
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public virtual ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}