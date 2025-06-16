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

        private IActionResult ViewAuth(string viewName, object model = null)
        {
            ViewBag.Layout = "_AuthLayout";
            return View(viewName, model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Mostrando formulario de Registro.");
            return ViewAuth("Register");
        }


        private bool EmailYaRegistrado(string email)
        {
            return _context.Usuario.Any(u => u.Email == email);
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [Bind(
                "Id," +
                "NombreCompleto," +
                "Email," +
                "Contrasena," +
                "Telefono," +
                "FechaNacimiento"
            )] Usuario usuario)
        {
            _logger.LogInformation("Intento de registro Registro.");

            if (EmailYaRegistrado(usuario.Email))
            {
                TempData["CuentaExistente"] = "Ese correo ya está registrado";
                return View(usuario);
            }

            var validarDatosEntrantes = ModelState.IsValid;

            if (validarDatosEntrantes)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Registro exitoso {usuario}", usuario);
                TempData["MensajeExito"] = "Registro exitoso";
                return ViewAuth("Login");
            }
            _logger.LogInformation("No se logro validar los datos.");
            return ViewAuth("Register", usuario);
        }

       
        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Mostrando formulario de login.");
            return ViewAuth("Login");
        }


        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            _logger.LogInformation("Intento de login para el usuario : {Email}", usuario.Email);


            if (!EmailYaRegistrado(usuario.Email))
            {
                TempData["CuentaExistente"] = "Usuaro inexitente";
                return ViewAuth("Login", usuario);
            }
            var usuarioBuscado = _context.Usuario.FirstOrDefault(
                u => u.Email == usuario.Email && u.Contrasena == usuario.Contrasena
             );


            if (usuarioBuscado == null)
            {
                TempData["CuentaExistente"] = "Contrasenia Incorrecta!";
                return ViewAuth("Login", usuario);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
