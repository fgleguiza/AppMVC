using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models
{
    public class ItemPedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        [Required]
        public string TipoProducto { get; set; } // Ej: "Hamburguesa" o "Bebida"

        [Required]
        public int ProductoId { get; set; } // Id de Hamburguesa o Bebida

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        // Total calculado
        [NotMapped]
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}
