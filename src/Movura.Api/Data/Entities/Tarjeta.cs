using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("Tarjetas")]
public class Tarjeta
{
    [Key]
    [Column("id_tarjeta")]
    public int Id { get; set; }

    [Required]
    [Column("id_usuario")]
    public int UserId { get; set; }

    [Required]
    [Column("numero_enmascarado")]
    [StringLength(20)]
    public string NumeroEnmascarado { get; set; } = string.Empty;

    [Required]
    [Column("expiracion")]
    [StringLength(7)]
    public string Expiracion { get; set; } = string.Empty;

    [Required]
    [Column("token_seguro")]
    [StringLength(200)]
    public string TokenSeguro { get; set; } = string.Empty;

    [Required]
    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; }

    [Required]
    [Column("marca")]
    [StringLength(50)]
    public string Marca { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}