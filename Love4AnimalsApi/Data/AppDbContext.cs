using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Data;

/// <summary>
/// DbContext de Entity Framework Core para Love4Animals
/// Mapea las entidades C# a tablas de PostgreSQL
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // DbSets: Cada una representa una tabla en la base de datos
    public DbSet<User> Users => Set<User>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Donation> Donations => Set<Donation>();

    /// <summary>
    /// Configuración del modelo y convenciones de EF Core
    /// Define relaciones, restricciones y precisiones
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ═══════════════════════════════════════════════════════════════
        // Configuración de Usuario
        // ═══════════════════════════════════════════════════════════════
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).ValueGeneratedOnAdd(); // Auto-incremento
            e.Property(u => u.Name).HasMaxLength(100).IsRequired();
            e.Property(u => u.Email).HasMaxLength(255).IsRequired();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.PasswordHash)
                .HasColumnName("Password")
                .HasMaxLength(255)
                .IsRequired();
            e.Property(u => u.Rol).HasMaxLength(50).IsRequired();

            // Un usuario crea muchas campañas
            e.HasMany(u => u.Campaigns)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Un usuario publica muchos posts
            e.HasMany(u => u.Posts)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Un usuario escribe muchos comentarios
            e.HasMany(u => u.Comments)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Un usuario realiza muchas donaciones
            e.HasMany(u => u.Donations)
                .WithOne(d => d.Usuario)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ═══════════════════════════════════════════════════════════════
        // Configuración de Campaña
        // ═══════════════════════════════════════════════════════════════
        modelBuilder.Entity<Campaign>(e =>
        {
            e.HasKey(c => c.IdCampania);
            e.Property(c => c.IdCampania).ValueGeneratedOnAdd();
            e.Property(c => c.Titulo).HasMaxLength(200).IsRequired();
            e.Property(c => c.Descripcion).HasMaxLength(2000).IsRequired();
            e.Property(c => c.MetaMonto).HasPrecision(18, 2); // Decimal con 2 decimales
            e.Property(c => c.MontoRecaudado).HasPrecision(18, 2);
            e.Property(c => c.Estado).HasMaxLength(50).IsRequired();
            e.Property(c => c.FechaInicio).HasColumnType("timestamp with time zone");
            e.Property(c => c.FechaFin).HasColumnType("timestamp with time zone");

            // Relación con Usuario (FK)
            e.HasOne(c => c.Usuario)
                .WithMany(u => u.Campaigns)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Una campaña tiene muchos posts
            e.HasMany(c => c.Posts)
                .WithOne(p => p.Campania)
                .HasForeignKey(p => p.IdCampania)
                .OnDelete(DeleteBehavior.SetNull);

            // Una campaña recibe muchas donaciones
            e.HasMany(c => c.Donations)
                .WithOne(d => d.Campania)
                .HasForeignKey(d => d.IdCampania)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ═══════════════════════════════════════════════════════════════
        // Configuración de Post
        // ═══════════════════════════════════════════════════════════════
        modelBuilder.Entity<Post>(e =>
        {
            e.HasKey(p => p.IdPost);
            e.Property(p => p.IdPost).ValueGeneratedOnAdd();
            e.Property(p => p.Titulo).HasMaxLength(300).IsRequired();
            e.Property(p => p.Descripcion).HasMaxLength(5000).IsRequired();
            e.Property(p => p.FotoUrl).HasMaxLength(1000);
            e.Property(p => p.Fecha).HasColumnType("timestamp with time zone");

            // Relación con Usuario (FK)
            e.HasOne(p => p.Usuario)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Campaña (FK opcional)
            e.HasOne(p => p.Campania)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.IdCampania)
                .OnDelete(DeleteBehavior.SetNull);

            // Un post tiene muchos comentarios
            e.HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.IdPost)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ═══════════════════════════════════════════════════════════════
        // Configuración de Comentario
        // ═══════════════════════════════════════════════════════════════
        modelBuilder.Entity<Comment>(e =>
        {
            e.HasKey(c => c.IdComment);
            e.Property(c => c.IdComment).ValueGeneratedOnAdd();
            e.Property(c => c.Texto).HasMaxLength(2000).IsRequired();
            e.Property(c => c.Fecha).HasColumnType("timestamp with time zone");

            // Relación con Usuario (FK)
            e.HasOne(c => c.Usuario)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Post (FK)
            e.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.IdPost)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ═══════════════════════════════════════════════════════════════
        // Configuración de Donación
        // ═══════════════════════════════════════════════════════════════
        modelBuilder.Entity<Donation>(e =>
        {
            e.HasKey(d => d.IdDonation);
            e.Property(d => d.IdDonation).ValueGeneratedOnAdd();
            e.Property(d => d.Monto).HasPrecision(18, 2).IsRequired();
            e.Property(d => d.MetodoPago).HasMaxLength(100).IsRequired();
            e.Property(d => d.Comprobante).HasMaxLength(50000); // Para base64 de imágenes/PDFs
            e.Property(d => d.Fecha).HasColumnType("timestamp with time zone");

            // Relación con Usuario (FK)
            e.HasOne(d => d.Usuario)
                .WithMany(u => u.Donations)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Campaña (FK)
            e.HasOne(d => d.Campania)
                .WithMany(c => c.Donations)
                .HasForeignKey(d => d.IdCampania)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
