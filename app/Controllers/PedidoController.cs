using app.Context;
using app.Models;
using app.Service;
using app.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace app.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ILogger<PedidoController> _logger;
        private readonly DataBaseContext _context;
        private readonly EmailService _emailService;


        public PedidoController(ILogger<PedidoController> logger, DataBaseContext context, EmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;

        }


        [HttpGet]
        public IActionResult Comprobante(string numero)
        {
            ViewBag.NumeroPedido = numero;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Confirmar([FromBody] PedidoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Volver a mostrar el formulario con errores
            }

            var usuario = await _context.Usuario.FindAsync(model.UsuarioId);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Generar número de pedido
            string numeroPedido = "PED-" + DateTime.Now.ToString("yyMMdd") + "-" + Guid.NewGuid().ToString("N").Substring(0, 2).ToUpper();

            // Crear objeto Pedido
            var pedido = new Pedido
            {
                NumeroPedido = numeroPedido,
                UsuarioId = model.UsuarioId,
                FormaPago = model.FormaPago,
                DireccionEntrega = model.DireccionEntrega,
                TelefonoContacto = model.TelefonoContacto,
                Observaciones = model.Observaciones,
                FechaPedido = DateTime.Now,
                Items = new List<ItemPedido>()
            };

            // Agregar ítems (ejemplo: hamburguesas y bebidas del carrito)
            foreach (var item in model.Items)
            {
                pedido.Items.Add(new ItemPedido
                {
                    TipoProducto = item.TipoProducto,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario
                });
            }

            _context.Pedido.Add(pedido);
            await _context.SaveChangesAsync();

                                    // Enviar email de comprobante
                                    string body = $@"
                        <table width='100%' cellpadding='0' cellspacing='0' border='0' style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
                            <tr>
                                <td align='center'>
                                    <table width='600' cellpadding='0' cellspacing='0' border='0' style='background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                                        <tr>
                                            <td style='background-color: #ff6b35; color: white; padding: 20px; text-align: center; font-size: 24px; font-weight: bold;'>
                                                🍔 ¡Gracias por tu pedido, {usuario.NombreCompleto}!
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='padding: 20px; font-size: 16px; color: #333;'>
                                                <p>Hemos recibido tu pedido y lo estamos procesando.</p>

                                                <table cellpadding='0' cellspacing='0' border='0' width='100%' style='margin-top: 10px;'>
                                                    <tr>
                                                        <td style='padding: 8px; font-weight: bold;'>Número de pedido:</td>
                                                        <td style='padding: 8px;'>{numeroPedido}</td>
                                                    </tr>
                                                    <tr style='background-color: #f2f2f2;'>
                                                        <td style='padding: 8px; font-weight: bold;'>Fecha:</td>
                                                        <td style='padding: 8px;'>{pedido.FechaPedido}</td>
                                                    </tr>
                                                    <tr>
                                                        <td style='padding: 8px; font-weight: bold;'>Forma de pago:</td>
                                                        <td style='padding: 8px;'>{pedido.FormaPago}</td>
                                                    </tr>
                                                    <tr style='background-color: #f2f2f2;'>
                                                        <td style='padding: 8px; font-weight: bold;'>Dirección de entrega:</td>
                                                        <td style='padding: 8px;'>{pedido.DireccionEntrega}</td>
                                                    </tr>
                                                </table>

                                                <p style='margin-top: 20px;'>Te avisaremos cuando tu pedido esté en camino. ¡Disfrutá de tu BurgerApp! 🍟</p>

                                                <p style='font-size: 14px; color: #777; margin-top: 30px;'>Si no realizaste este pedido, por favor ignora este mensaje o contáctanos.</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='background-color: #ff6b35; color: white; padding: 10px; text-align: center; font-size: 14px;'>
                                                © 2025 BurgerApp
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        ";

            await _emailService.SendEmailAsync(usuario.Email, "Confirmación de Pedido", body);

            return Json(new { success = true, numeroPedido = pedido.NumeroPedido });
        }




        public IActionResult MisPedidos()
        {
            var usuarioId = HttpContext.Session.GetInt32("id");

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Auth"); // o algún mensaje de error
            }

            var pedidos = _context.Pedido
            .Where(p => p.UsuarioId == usuarioId)
            .Include(p => p.Items)
            .OrderByDescending(p => p.FechaPedido)
            .ToList();

  
            _logger.LogInformation("Mostrar pedidos: {pedidos}", pedidos);

            return View(pedidos);
        }
    }


}
