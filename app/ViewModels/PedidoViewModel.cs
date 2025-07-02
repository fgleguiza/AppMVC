using System.ComponentModel.DataAnnotations;

namespace app.ViewModels
{
    public class PedidoViewModel
    {
        public int UsuarioId { get; set; }
        public string FormaPago { get; set; }
        public string DireccionEntrega { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? Observaciones { get; set; }
        public List<ItemPedidoViewModel> Items { get; set; }
    }

}
