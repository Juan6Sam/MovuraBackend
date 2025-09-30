using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("ComercioEmails")]
public class ComercioEmail
{
    [Key]
    [Column("id_rel")]
    public int Id { get; set; }

    [Required]
    [Column("id_comercio")]
    public int ComercioId { get; set; }

    [Required]
    [Column("correo")]
    [StringLength(250)]
    public string Email { get; set; } = string.Empty;

    [ForeignKey(nameof(ComercioId))]
    public virtual Comercio? Comercio { get; set; }
}