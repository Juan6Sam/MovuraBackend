using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Movura.Api.Data.Entities;

[Table("Usuarios")]
public class User
{
    [Key]
    [Column("id_usuario")]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Column("apellido")]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [Column("correo")]
    [StringLength(250)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("telefono")]
    [StringLength(50)]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [Column("id_rol")]
    public int RoleId { get; set; }

    [Required]
    [Column("id_estatus")]
    public int StatusId { get; set; }

    [Required]
    [Column("first_login")]
    public bool FirstLogin { get; set; }

    [Required]
    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status? Status { get; set; }

    public virtual ICollection<Contrasena> Contrasenas { get; set; } = new List<Contrasena>();
    public virtual ICollection<Acceso> Accesos { get; set; } = new List<Acceso>();
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public virtual ICollection<CodigoQR> CodigosQR { get; set; } = new List<CodigoQR>();
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public virtual ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    public virtual ICollection<Tarjeta> Tarjetas { get; set; } = new List<Tarjeta>();
    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

}
