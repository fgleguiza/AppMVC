using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string NumeroPedido { get; set; }

        [Required]
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        [Required]
        public string FormaPago { get; set; }

        [Required]
        public string DireccionEntrega { get; set; }

        public string? TelefonoContacto { get; set; }

        public string? Observaciones { get; set; }

        public string? Estado { get; set; }

        // Relación con Usuario
        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Relación con los ítems pedidos
        public ICollection<ItemPedido> Items { get; set; } = new List<ItemPedido>();
    }

}
