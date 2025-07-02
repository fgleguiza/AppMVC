
using app.Models;
using BurgerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace app.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<EmailCode> EmailCodes { get; set; }
        public DbSet<Hamburguesa> Hamburguesa { get; set; }
        public DbSet<Ingrediente> Ingrediente { get; set; }
        public DbSet<Bebida> Bebida { get; set; }
        public DbSet <HamburguesaIngrediente> HamburguesaIngrediente { get; set; }

        public DbSet<Pedido> Pedido { get; set; }

        public DbSet<ItemPedido> ItemPedido { get; set; }

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



            // Clave compuesta para la tabla intermedia
            modelBuilder.Entity<HamburguesaIngrediente>()
                .HasKey(hi => new { hi.HamburguesaId, hi.IngredienteId });

            modelBuilder.Entity<HamburguesaIngrediente>()
                .HasOne(hi => hi.Hamburguesa)
                .WithMany(h => h.HamburguesaIngredientes)
                .HasForeignKey(hi => hi.HamburguesaId);

            modelBuilder.Entity<HamburguesaIngrediente>()
                .HasOne(hi => hi.Ingrediente)
                .WithMany(i => i.HamburguesaIngredientes)
                .HasForeignKey(hi => hi.IngredienteId);

            // Ingredientes
            modelBuilder.Entity<Ingrediente>().HasData(
                new Ingrediente { Id = 1, Nombre = "Carne" },
                new Ingrediente { Id = 2, Nombre = "Lechuga" },
                new Ingrediente { Id = 3, Nombre = "Queso" },
                new Ingrediente { Id = 4, Nombre = "Tomate" },
                new Ingrediente { Id = 5, Nombre = "Bacon" },
                new Ingrediente { Id = 6, Nombre = "Huevo" }
            );

            // Hamburguesas
            modelBuilder.Entity<Hamburguesa>().HasData(
                new Hamburguesa { Id = 1, Nombre = "Clásica", Precio = 2500 },
                new Hamburguesa { Id = 2, Nombre = "Bacon Lover", Precio = 2800 },
                new Hamburguesa { Id = 3, Nombre = "Veggie", Precio = 2400 }
            );

            // Relaciones Hamburguesa - Ingredientes
            modelBuilder.Entity<HamburguesaIngrediente>().HasData(
                // Clásica: carne, lechuga, tomate
                new HamburguesaIngrediente { HamburguesaId = 1, IngredienteId = 1 },
                new HamburguesaIngrediente { HamburguesaId = 1, IngredienteId = 2 },
                new HamburguesaIngrediente { HamburguesaId = 1, IngredienteId = 4 },

         
                // Bacon Lover: carne, bacon, queso, huevo
                new HamburguesaIngrediente { HamburguesaId = 2, IngredienteId = 1 },
                new HamburguesaIngrediente { HamburguesaId = 2, IngredienteId = 3 },
                new HamburguesaIngrediente { HamburguesaId = 2, IngredienteId = 5 },
                new HamburguesaIngrediente { HamburguesaId = 2, IngredienteId = 6 },

                // Veggie: lechuga, tomate, queso
                new HamburguesaIngrediente { HamburguesaId = 3, IngredienteId = 2 },
                new HamburguesaIngrediente { HamburguesaId = 3, IngredienteId = 3 },
                new HamburguesaIngrediente { HamburguesaId = 3, IngredienteId = 4 }
            );


            // Agregar datos semilla correctamente (esto debe ir por separado)
            modelBuilder.Entity<Bebida>().HasData(
                new Bebida { Id = 1, Nombre = "Coca Cola", Precio = 800 },
                new Bebida { Id = 2, Nombre = "Sprite", Precio = 800 },
                new Bebida { Id = 3, Nombre = "Agua Mineral", Precio = 600 }
            );
        }

    }

}
