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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación 1:N entre Usuario y Rachas
            modelBuilder.Entity<SistemaRacha>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Rachas)
                .HasForeignKey(r => r.UsuarioId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
