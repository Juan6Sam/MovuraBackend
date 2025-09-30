using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("Contrasenas")]
public class Contrasena
{
    [Key]
    [Column("id_contrasena")]
    public int Id { get; set; }

    [Required]
    [Column("id_usuario")]
    public int UserId { get; set; }

    [Required]
    [Column("hash")]
    [StringLength(512)]
    public string Hash { get; set; } = string.Empty;

    [Required]
    [Column("salt")]
    [StringLength(128)]
    public string Salt { get; set; } = string.Empty;

    [Required]
    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; }

    [Required]
    [Column("activa")]
    public bool Activa { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}