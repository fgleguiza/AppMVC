using app.Context;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;

        public HomeController(ILogger<HomeController> logger, DataBaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> MenuHamburguesas()
        {
            var hamburguesas = await _context.Hamburguesa.ToListAsync();
            return View(hamburguesas);
        }

        public async Task<IActionResult> verhamburguesa(int id)
        {
            var hamburguesa = await _context.Hamburguesa
                .Include(h => h.HamburguesaIngredientes)
                    .ThenInclude(hi => hi.Ingrediente)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hamburguesa == null)
            {
                return NotFound();
            }

            return View(hamburguesa);
        }


        public async Task<IActionResult> MenuBebidas()
        {
            var bebidas = await _context.Bebida.ToListAsync();
            return View(bebidas);
        }

        public IActionResult Index()
        {
            var nombre = HttpContext.Session.GetString("nombreCompleto");
            ViewBag.Nombre = nombre;
       
            _logger.LogInformation("Mostrando la vista de inicio para el usuario: {Nombre}", nombre);

            return View();
        }

        public IActionResult Carrito()
        {
            return View();
        }


        public IActionResult HacerPedido()
        {
            var nombre = HttpContext.Session.GetString("nombreCompleto");
            var id = HttpContext.Session.GetInt32("id");
            ViewBag.UserId = id;
            ViewBag.Nombre = nombre;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

      

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
