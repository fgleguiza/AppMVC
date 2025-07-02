using System.ComponentModel.DataAnnotations;

namespace app.Models
{
        public class Hamburguesa
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }

            // Imagen en binario
            public byte[]? Imagen { get; set; }

            [MaxLength(100)]
            public string? ImagenMimeType { get; set; }

        // Relación muchos a muchos con Ingrediente
        public ICollection<HamburguesaIngrediente> HamburguesaIngredientes { get; set; }
        }
}
