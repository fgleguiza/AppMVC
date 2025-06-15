using app.Context;
using app.Models;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Diagnostics;

namespace app.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly DataBaseContext _context;




        public AuthController(ILogger<AuthController> logger, DataBaseContext context)
        {
            _logger = logger;
            _context = context;

        }

        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Mostrando formulario de Registro.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Id,NombreCompleto,Email,Contrasena,Telefono,FechaNacimiento")] Usuario usuario)
        {
            _logger.LogInformation("Intento de registro Registro.");
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "Registro exitoso";
                return View("Register");
            }
            return View(usuario);
        }

        private IActionResult ViewWithLayout(string viewName, object model = null)
        {
            ViewBag.Layout = "_AuthLayout";
            return View(viewName, model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Mostrando formulario de login.");
            return ViewWithLayout("Login");
        }


        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            _logger.LogInformation("Intento de login para el usuario : {email}", email);

            var tablaUsuario = _context.Usuario;
            var usuarioBuscado = tablaUsuario.FirstOrDefault(u => u.Email == email && u.Contrasena == password);

            if (usuarioBuscado == null)
            {
                _logger.LogWarning("Login fallido  Credenciales incorrectas. {email}", email);
                ModelState.AddModelError(string.Empty, "Credenciales incorrectas");
                return View("Login");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
