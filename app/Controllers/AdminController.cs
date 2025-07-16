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
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly DataBaseContext _context;
        private readonly EmailService _emailService;

       

        public AdminController(ILogger<AdminController> logger, DataBaseContext context, EmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;

        }

        public IActionResult ViewAdmin(string viewName, object model = null)
        {
            ViewBag.Layout = "_AdminLayout";
            return View(viewName, model);
        }


        public IActionResult Admin()
        {
            var nombre = HttpContext.Session.GetString("nombreCompleto");
            ViewBag.Nombre = nombre;

            _logger.LogInformation("Mostrando la vista de inicio para el usuario: {Nombre}", nombre);
            return ViewAdmin("Admin");
        }

        [HttpGet]
        public IActionResult ListaPedidos()
        {
            var pedidos = _context.Pedido
                .Include(p => p.Usuario)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            return ViewAdmin("ListaPedidos", pedidos);
        }

        [HttpPost]
        public IActionResult ActualizarEstado([FromBody] EstadoViewModel data)
        {
            var pedido = _context.Pedido.FirstOrDefault(p => p.Id == data.Id);
            if (pedido == null)
            {
                return NotFound();
            }

            pedido.Estado = data.Estado;
            _context.SaveChanges();

            return Ok();
        }


    }

}
