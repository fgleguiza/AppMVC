using app.Context;
using app.Middlewares;
using app.Service;
using Microsoft.EntityFrameworkCore;

namespace app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<EmailService>();

            //conexion db global
            builder.Services.AddDbContext<DataBaseContext>(
                options => options.UseSqlServer(
                    builder.Configuration["ConnectionString:BurgerAppDB"]
                 )
            );


            //configuaracion
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseMiddleware<AutenticacionMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();




            //rutas de Auth
            app.MapControllerRoute(
                name: "login",
                pattern: "login",
                defaults: new { controller = "Auth", action = "Login" });

            app.MapControllerRoute(
                name: "register",
                pattern: "register",
                defaults: new { controller = "Auth", action = "Register" });

            app.MapControllerRoute(
                name: "logout",
                pattern: "salir",
                defaults: new { controller = "Auth", action = "Logout" });

            app.MapControllerRoute(
                name: "forgotPassword",
                pattern: "forgotPassword",
                defaults: new { controller = "Auth", action = "ForgotPassword" });


            //rutas de home
            app.MapControllerRoute(
                name: "home",
                pattern: "home",
                defaults: new { controller = "Home", action = "Index" });

            app.MapControllerRoute(
               name: "Hamburguesas",
               pattern: "Hamburguesas",
               defaults: new { controller = "Home", action = "MenuHamburguesas" });

            app.MapControllerRoute(
               name: "Bebidas",
               pattern: "Bebidas",
               defaults: new { controller = "Home", action = "MenuBebidas" });

            app.MapControllerRoute(
              name: "carrito",
              pattern: "carrito",
              defaults: new { controller = "Home", action = "Carrito" });

            app.MapControllerRoute(
             name: "hacerpedido",
             pattern: "hacerpedido",
             defaults: new { controller = "Home", action = "HacerPedido" });


            app.MapControllerRoute(
               name: "mispedidos",
               pattern: "mispedidos",
               defaults: new { controller = "Pedido", action = "MisPedidos" });


            app.MapControllerRoute(
                name: "confirmar",
                pattern: "confirmar",
                defaults: new { controller = "Pedido", action = "Confirmar" });


            app.MapControllerRoute(
                name: "comprobante",
                pattern: "comprobante/{numero?}",
                defaults: new { controller = "Pedido", action = "Comprobante" });


            app.MapControllerRoute(
                name: "mispedidos",
                pattern: "mispedidos",
                defaults: new { controller = "Pedido", action = "MisPedidos" });


            app.MapControllerRoute(
               name: "admin",
               pattern: "admin",
               defaults: new { controller = "Admin", action = "Admin" });




            // ruta default
            app.MapDefaultControllerRoute();

            app.Run(); 

        }
    }
}
