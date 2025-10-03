using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Domain.Entities;

[Table("Roles")]
public class Role
{
    [Key]
    [Column("id_rol")]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Column("descripcion")]
    [StringLength(250)]
    public string? Descripcion { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}