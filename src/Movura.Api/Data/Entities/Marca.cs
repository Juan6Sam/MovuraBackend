using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Api.Data.Entities;

[Table("Marcas")]
public class Marca
{
    [Key]
    [Column("id_marca")]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Column("descripcion")]
    [StringLength(200)]
    public string? Descripcion { get; set; }

    public virtual ICollection<Tarjeta> Tarjetas { get; set; } = new List<Tarjeta>();
}