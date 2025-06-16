using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace app.Middlewares
{
    public class AutenticacionMiddleware
    {
        private readonly RequestDelegate _next;

        public AutenticacionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();

            // Rutas que no requieren sesión
            var rutasPermitidas = new[] {
                "/login",
                "/register",
                "/salir",
                "/css",
                "/js",
                "/images",
                "/favicon.ico"
            };

            bool rutaEsPublica = rutasPermitidas.Any(p => path.StartsWith(p));

            if (!rutaEsPublica)
            {
                var usuarioLogeado = context.Session.GetInt32("id");

                if (usuarioLogeado == null)
                {
                    context.Response.Redirect("login");
                    return;
                }
            }

            await _next(context);
        }
    }
}