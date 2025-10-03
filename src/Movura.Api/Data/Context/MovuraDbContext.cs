using Microsoft.EntityFrameworkCore;
using Movura.Domain.Entities;

namespace Movura.Api.Data.Context;

public class MovuraDbContext : DbContext
{
    public MovuraDbContext(DbContextOptions<MovuraDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Status> Estatus { get; set; }
    public DbSet<Marca> Marcas { get; set; }
    public DbSet<Contrasena> Contrasenas { get; set; }
    public DbSet<Comercio> Comercios { get; set; }
    public DbSet<ComercioEmail> ComercioEmails { get; set; }
    public DbSet<Acceso> Accesos { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Exceso> Excesos { get; set; }
    public DbSet<CodigoQR> CodigosQR { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Transaccion> Transacciones { get; set; }
    public DbSet<Tarjeta> Tarjetas { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Notificacion> Notificaciones { get; set; }
    public DbSet<Parking> Parkings { get; set; }
    public DbSet<ParkingConfig> ParkingConfigs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // --- RELACIONES FUNDAMENTALES FALTANTES ---

        modelBuilder.Entity<Comercio>()
            .HasOne(c => c.Parking)
            .WithMany(p => p.Comercios)
            .HasForeignKey(c => c.ParkingId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Parking)
            .WithMany(p => p.Tickets)
            .HasForeignKey(t => t.ParkingId);

        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.Parking)
            .WithMany(p => p.Transacciones)
            .HasForeignKey(t => t.ParkingId);
            
        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.Comercio)
            .WithMany()
            .HasForeignKey(t => t.ComercioId);

        // --- CONFIGURACIONES EXISTENTES ---

        // User
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Status)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.StatusId);

        // Comercio
        modelBuilder.Entity<Comercio>()
            .HasOne(c => c.Status)
            .WithMany(s => s.Comercios)
            .HasForeignKey(c => c.StatusId);

        modelBuilder.Entity<Comercio>()
            .Property(c => c.Valor)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<ComercioEmail>()
            .HasOne(ce => ce.Comercio)
            .WithMany(c => c.Emails)
            .HasForeignKey(ce => ce.ComercioId);

        // Acceso y relacionados
        modelBuilder.Entity<Acceso>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accesos)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<Acceso>()
            .HasOne(a => a.Status)
            .WithMany(s => s.Accesos)
            .HasForeignKey(a => a.StatusId);

        modelBuilder.Entity<Exceso>()
            .HasOne(e => e.Acceso)
            .WithMany(a => a.Excesos)
            .HasForeignKey(e => e.AccesoId);

        // Ticket y relacionados
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Acceso)
            .WithMany(a => a.Tickets)
            .HasForeignKey(t => t.AccesoId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Status)
            .WithMany(s => s.Tickets)
            .HasForeignKey(t => t.StatusId);

        // QR
        modelBuilder.Entity<CodigoQR>()
            .HasOne(qr => qr.User)
            .WithMany(u => u.CodigosQR)
            .HasForeignKey(qr => qr.UserId);

        modelBuilder.Entity<CodigoQR>()
            .HasOne(qr => qr.Comercio)
            .WithMany(c => c.CodigosQR)
            .HasForeignKey(qr => qr.ComercioId);

        modelBuilder.Entity<CodigoQR>()
            .HasOne(qr => qr.Acceso)
            .WithMany(a => a.CodigosQR)
            .HasForeignKey(qr => qr.AccesoId);

        modelBuilder.Entity<CodigoQR>()
            .HasOne(qr => qr.Status)
            .WithMany(s => s.CodigosQR)
            .HasForeignKey(qr => qr.StatusId);

        // Pago y Transacción
        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Ticket)
            .WithMany(t => t.Pagos)
            .HasForeignKey(p => p.TicketId);

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.User)
            .WithMany(u => u.Pagos)
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Acceso)
            .WithMany(a => a.Pagos)
            .HasForeignKey(p => p.AccesoId);

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Status)
            .WithMany(s => s.Pagos)
            .HasForeignKey(p => p.StatusId);

        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.Pago)
            .WithMany(p => p.Transacciones)
            .HasForeignKey(t => t.PagoId);

        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.Acceso)
            .WithMany(a => a.Transacciones)
            .HasForeignKey(t => t.AccesoId);

        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.CodigoQR)
            .WithMany(qr => qr.Transacciones)
            .HasForeignKey(t => t.QRId);

        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.Status)
            .WithMany(s => s.Transacciones)
            .HasForeignKey(t => t.StatusId);

        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transacciones)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Transaccion>()
            .Property(t => t.Amount)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Transaccion>()
            .Property(t => t.DiscountAmount)
            .HasColumnType("decimal(18, 2)");

        // Parking
        modelBuilder.Entity<Parking>()
            .HasOne(p => p.Config)
            .WithOne(pc => pc.Parking)
            .HasForeignKey<ParkingConfig>(pc => pc.ParkingId);

        modelBuilder.Entity<ParkingConfig>()
            .Property(pc => pc.CostoFraccion)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<ParkingConfig>()
            .Property(pc => pc.CostoHora)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<ParkingConfig>()
            .Property(pc => pc.TarifaBase)
            .HasColumnType("decimal(18, 2)");
            
        // Tarjeta
        modelBuilder.Entity<Tarjeta>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tarjetas)
            .HasForeignKey(t => t.UserId);

        // Log y Notificación
        modelBuilder.Entity<Log>()
            .HasOne(l => l.User)
            .WithMany(u => u.Logs)
            .HasForeignKey(l => l.UserId);

        modelBuilder.Entity<Notificacion>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notificaciones)
            .HasForeignKey(n => n.UserId);
        
        // RefreshToken
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);

        // Índices
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Acceso>()
            .HasIndex(a => a.HoraEntrada);

        modelBuilder.Entity<Transaccion>()
            .HasIndex(t => t.CreatedAt);

        modelBuilder.Entity<CodigoQR>()
            .HasIndex(qr => qr.Codigo)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.FechaEmision);

        modelBuilder.Entity<Pago>()
            .HasIndex(p => p.FechaPago);
    }
}
