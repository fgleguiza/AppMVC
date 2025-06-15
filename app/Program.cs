using Microsoft.EntityFrameworkCore;
using app.Context;
namespace app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DataBaseContext>(
                options => options.UseSqlServer(
                    builder.Configuration["ConnectionString:BurgerAppDB"]
                 )
            );


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

     

            app.UseAuthorization();

            // Tus rutas personalizadas (la más específica primero)
            app.MapControllerRoute(
                name: "login",
                pattern: "Login",
                defaults: new { controller = "Auth", action = "Login" });

            app.MapControllerRoute(
                name: "register",
                pattern: "Register",
                defaults: new { controller = "Auth", action = "Register" });

            app.MapControllerRoute(
                name: "home",
                pattern: "home",
                defaults: new { controller = "Home", action = "Index" });

            // La ruta genérica "default" SIEMPRE debe ir al final
            // de todas tus rutas específicas, porque es un "catch-all".
            // Si la pones antes, podría coincidir antes que tus rutas específicas.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}"); // Valores por defecto para si no se especifica nada.

            app.Run(); 

        }
    }
}
