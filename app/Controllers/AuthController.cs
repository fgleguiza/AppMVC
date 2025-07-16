using app.Context;
using app.Models;
using app.Service;
using Microsoft.AspNetCore.Mvc;


namespace app.Controllers
{
    //hola
    //unsefiusenfisunfiuse
    //uwandiuawnidu
    public class AuthController : Controller
    {

        private readonly ILogger<AuthController> _logger;
        private readonly DataBaseContext _context;
        private readonly EmailService _emailService;

        public AuthController(ILogger<AuthController> logger, DataBaseContext context , EmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;

        }

       
        private IActionResult ViewAuth(string viewName, object model = null)
        {
            ViewBag.Layout = "_AuthLayout";
            return View(viewName, model);
        }

        [HttpGet]
        public async Task<IActionResult> sendEmail()
        {
            await _emailService.SendEmailAsync(
                "fgleguiza2001@gmail.com",
                "holaaaa!",
                "<h1>¡Correo enviado correctamente!</h1>"
            );

            return Content("OK");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            _logger.LogInformation("Formulario de recuperacion contrasenia.");
            return ViewAuth("ForgotPassword");
        }

        //[HttpGet]
        //public async Task<IActionResult> SendVerificationCode(UsuarioEmailCode usuario)
        //{
       
        //    if (!ModelState.IsValid)
        //        return ViewAuth("SendVerificationCode", usuario);

        //    var usuarioBuscado = _context.Usuario.FirstOrDefault(
        //       u => u.Email == usuario.Email
        //    );



        //    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
        //    if (usuario == null)
        //    {
        //        ModelState.AddModelError("Email", "El correo no está registrado.");
        //        return View(model);
        //    }

        //    var codigoValido = await _context.EmailCodes
        //        .Where(c => c.UsuarioId == usuario.Id && c.Codigo == model.Codigo)
        //        .OrderByDescending(c => c.Expiracion)
        //        .FirstOrDefaultAsync();

        //    if (codigoValido == null || codigoValido.Expiracion < DateTime.UtcNow)
        //    {
        //        ModelState.AddModelError("Codigo", "Código inválido o expirado.");
        //        return View(model);
        //    }

        //    // Éxito → redirigir al cambio de contraseña
        //    return RedirectToAction("CambiarContrasena", new { email = model.Email });


        //    _logger.LogInformation("Formulario de codigo de verificacion contrasenia.");
        //    return ViewAuth("SendVerificationCode");
        //}



        [HttpPost]
        public async Task<IActionResult> GetVerificationCode(Usuario usuario)
        {

            if (!EmailYaRegistrado(usuario.Email))
            {
                TempData["CuentaExistente"] = "Usuaro inexitente";
                return ViewAuth("ForgotPassword", usuario);
            }


            var usuarioBuscado = _context.Usuario.FirstOrDefault(
               u => u.Email == usuario.Email
            );

            string codigo = new Random().Next(100000, 999999).ToString();

            var nuevoCodigo = new EmailCode
            {
                Codigo = codigo,
                Expiracion = DateTime.UtcNow.AddMinutes(10),
                UsuarioId = usuarioBuscado.Id
            };

            _context.Add(nuevoCodigo);
            await _context.SaveChangesAsync();

            // Enviar por correo
            string body = $"Tu código de verificación es: <strong>{codigo}</strong><br>Expira en 10 minutos.";
            await _emailService.SendEmailAsync(usuarioBuscado.Email, "Código de verificación", body);

            return ViewAuth("SendVerificationCode", usuario);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
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

            if (usuario.Email == "facuAdmin@gmail.com" && usuario.Contrasena == "123456")
            {
                HttpContext.Session.SetInt32("id", 0); // o un valor reservado para admin
                HttpContext.Session.SetString("nombreCompleto", "Facundo");
                HttpContext.Session.SetString("email", usuario.Email);
                _logger.LogInformation("Usuario administrador logeado correctamente");
                return RedirectToAction("Index", "Admin");
            }


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

            _logger.LogInformation("Usuario logeado correctamente : {Email}", usuario.Email);
            // 🔐 Guardar en sesión
            HttpContext.Session.SetInt32("id", usuarioBuscado.Id);
            HttpContext.Session.SetString("nombreCompleto", usuarioBuscado.NombreCompleto);
            HttpContext.Session.SetString("email", usuarioBuscado.Email);

            _logger.LogInformation("Usuario en seesion correctamente : {Email}", usuario.Email);

            return RedirectToAction("Index", "Home");
        }
    }
}
