using System.IO;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProyectoFinalCoderHouse.Models;

namespace ProyectoFinalCoderHouse.Data;

public partial class SistemaGestionContext : DbContext
{
    public SistemaGestionContext()
    {
    }

    public SistemaGestionContext(DbContextOptions<SistemaGestionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductoVendido> ProductoVendidos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Venta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("connectionDB"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");

            entity.Property(e => e.Costo).HasColumnType("money");
            entity.Property(e => e.Descripciones)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.PrecioVenta).HasColumnType("money");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Usuario");
        });

        modelBuilder.Entity<ProductoVendido>(entity =>
        {
            entity.ToTable("ProductoVendido");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductoVendidos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductoVendido_Producto");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.ProductoVendidos)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductoVendido_Venta");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Apellido)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Mail)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .IsRequired()
                .IsUnicode(false);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.Property(e => e.Comentarios).IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Venta_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
