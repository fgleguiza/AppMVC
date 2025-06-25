
using Microsoft.EntityFrameworkCore;
using app.Models;

namespace app.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<EmailCode> EmailCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Insertamos un usuario admi nistrador
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    NombreCompleto = "Facuno Gerardo Leguiza",
                    Email = "fgleguiza2001@gmail.com",
                    Contrasena = "123456",
                    Telefono = "113834103",
                    FechaNacimiento = new DateTime(2001, 8, 6)
                }
            );
        }
    }
    
}
