using app.Context;
using app.Middlewares;
using app.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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


            


            // ruta default
            app.MapDefaultControllerRoute();

            app.Run(); 

        }
    }
}
