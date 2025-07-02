using System.ComponentModel.DataAnnotations;

namespace app.ViewModels
{
    public class ItemPedidoViewModel
    {
        public string TipoProducto { get; set; } // "Hamburguesa" o "Bebida"
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
