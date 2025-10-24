using ApiMatematicas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMatematicas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<SistemaRacha> Rachas { get; set; }

        public DbSet<ReinicioContrasenaToken> ReinicioContraseñas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Forzar minúsculas en tablas y columnas (Postgres friendly)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName()!.ToLower());

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName()!.ToLower());
                }
            }

            // Relación 1:N entre Usuario y Rachas
            modelBuilder.Entity<SistemaRacha>()
                .HasOne(r => r.usuario)
                .WithMany(u => u.Rachas)
                .HasForeignKey(r => r.usuarioId);

            // Relación 1:N entre Usuario y ReinicioContraseñas
            modelBuilder.Entity<ReinicioContrasenaToken>()
                .HasOne(r => r.User)
                .WithMany(u => u.ReinicioContraseñas)
                .HasForeignKey(r => r.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
