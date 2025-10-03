using System.ComponentModel.DataAnnotations;

namespace Movura.Api.Data.Entities;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; } 
    public User? User { get; set; }

    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
    public DateTime? Revoked { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
}
