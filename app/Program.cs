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


            app.MapControllerRoute(
                name: "login",
                pattern: "login",
                defaults: new { controller = "Home", action = "Login" });

            app.MapControllerRoute(
                name: "login",
                pattern: "{controller=Home}/{action=Login}");

            app.Run();

        }
    }
}
