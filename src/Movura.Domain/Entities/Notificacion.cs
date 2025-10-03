using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Domain.Entities;

[Table("Notificaciones")]
public class Notificacion
{
    [Key]
    [Column("id_notificacion")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int? UserId { get; set; }

    [Required]
    [Column("canal")]
    [StringLength(50)]
    public string Canal { get; set; } = string.Empty;

    [Required]
    [Column("mensaje")]
    [StringLength(1000)]
    public string Mensaje { get; set; } = string.Empty;

    [Required]
    [Column("fecha_envio")]
    public DateTime FechaEnvio { get; set; }

    [Required]
    [Column("estatus_envio")]
    [StringLength(50)]
    public string EstatusEnvio { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}