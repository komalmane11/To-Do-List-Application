using Microsoft.EntityFrameworkCore;
using To_Do_List_Application.Data;

namespace To_Do_List_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add DbContext for the database
            builder.Services.AddDbContext<ToDoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);  // Set session timeout
                options.Cookie.HttpOnly = true;  // Secure cookie settings
                options.Cookie.IsEssential = true;  // Essential for app's functionality
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Enable static files to serve web content like images, CSS, JS
            app.UseStaticFiles();

            // Enable session middleware to use session
            app.UseSession();  // This should come before routing or any request handling

            // Set up routing and controller actions
            app.UseRouting();
            app.UseAuthorization();

            // Define routing for controllers
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Run the application
            app.Run();
        }
    }
}
