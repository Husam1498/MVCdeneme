using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Entites;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());//automapper kalk�nd�r�yoruz
            builder.Services.AddDbContext<DatabaseContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                // opts.UseLazyLoadingProxies();
            });

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts =>
                {
                    opts.Cookie.Name = "Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.auth";
                    opts.ExpireTimeSpan = TimeSpan.FromDays(1);
                    opts.SlidingExpiration = true;//her giri� yapt���nda ExpireTimeSpan � tekrar ba�lat�r 
                    opts.LoginPath = "/Account/Login";
                    opts.LogoutPath = "/Account/Logout";
                    opts.AccessDeniedPath = "/Home/AccessDenied";

                });

            //API Ontroller
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,//sonsuz token oldu
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("AppSettings:Secret")))
                    };
                });

            builder.Services.AddScoped<IHasher,Hasher>();//IHasher interface i �a��rd���mda Hasher class�n� ver bana
            builder.Services.AddScoped<ITokenHelper, TokenHelper>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}