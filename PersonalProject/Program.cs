using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalProject.Data;
using PersonalProject.Models;
using PersonalProject.Models.Interfaces;
using PersonalProject.Models.Services;

namespace PersonalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000") // Replace with your React app's URL
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            string connString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connString));
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddTransient<IUser, IdentityUserService>();
            builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            builder.Services.AddTransient<IProduct, ProductService>();
            builder.Services.AddTransient<ICategory, CategoryService>();
            builder.Services.AddTransient<IProductsCategory, ProductsCategoryService>();
            builder.Services.AddTransient<ICart, CartService>();
            builder.Services.AddTransient<IProductsCart, ProductsCartService>();
            builder.Services.AddTransient<IEmailSender, EmailSenderService>();
            builder.Services.AddHttpClient<CardService>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "PersonalProject",
                    Version = "v1",
                });
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.Cookie.Name = "AuthCookieECommerce"; // Replace with your preferred name
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set the expiration time
                  options.SlidingExpiration = true; // Extend the expiration time on each request
                                                    // Other options...
              });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.LoginPath = "/Main/Login/";
            });
            var app = builder.Build();

            app.UseSwagger(options => {
                options.RouteTemplate = "/api/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/api/v1/swagger.json", "PersonalProject");
                options.RoutePrefix = "docs";
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add CORS middleware here
            app.UseCors("AllowAll");
            
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Main}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
