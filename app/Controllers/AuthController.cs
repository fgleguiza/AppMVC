using System.Diagnostics;
using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }


        public IActionResult TemplateLogin()
        {
            _logger.LogInformation("Mostrando formulario de login.");
            return View();
        }


        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            _logger.LogInformation("Intento de login para el usuario: {Username}", username);

            if (!(username == "admin@admin.com" && password == "1234"))
            {
                _logger.LogWarning("Login fallido para el usuario: {Username}. Credenciales incorrectas.", username);

                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");

                return View("TemplateLogin");
            }

            _logger.LogInformation("Login exitoso para el usuario: {Username}", username);
            return RedirectToAction("Index", "Home");
        }
    }
}
