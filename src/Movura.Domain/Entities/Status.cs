using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movura.Domain.Entities;

[Table("Estatus")]
public class Status
{
    [Key]
    [Column("id_estatus")]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Column("modulo")]
    [StringLength(50)]
    public string Modulo { get; set; } = string.Empty;

    [Column("descripcion")]
    [StringLength(200)]
    public string? Descripcion { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<Comercio> Comercios { get; set; } = new List<Comercio>();
    public virtual ICollection<Acceso> Accesos { get; set; } = new List<Acceso>();
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public virtual ICollection<CodigoQR> CodigosQR { get; set; } = new List<CodigoQR>();
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public virtual ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}