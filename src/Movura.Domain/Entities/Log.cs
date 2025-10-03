using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Domain.Entities;

[Table("Logs")]
public class Log
{
    [Key]
    [Column("id_log")]
    public int Id { get; set; }

    [Column("id_usuario")]
    public int? UserId { get; set; }

    [Required]
    [Column("accion")]
    [StringLength(200)]
    public string Accion { get; set; } = string.Empty;

    [Required]
    [Column("descripcion")]
    [StringLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [Column("fecha")]
    public DateTime Fecha { get; set; }

    [Required]
    [Column("ip_address")]
    [StringLength(50)]
    public string IpAddress { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}