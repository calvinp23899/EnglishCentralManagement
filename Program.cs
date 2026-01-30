using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Areas.Admin.Services;
using EnglishCentralManagement.Data;
using Microsoft.EntityFrameworkCore;
using EnglishCentralManagement.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region DI
            //builder.Services.AddScoped<SessionAuthorizeFilter>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IStaffService, StaffService>();
            #endregion

            //Authentication + Authorization
            builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Admin/Login/Index";           // URL login
                options.AccessDeniedPath = "/Admin/Login/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("NotUser", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                    !context.User.IsInRole(RoleType.User.ToString()));
                });
            });

            // Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
            });
            builder.Services.AddDbContext<EnglishCentreDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<EnglishCentreDbContext>();
                await DbInitializer.SeedAsync(db);
            }
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
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            #region Admin Route
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}"
            );
            #endregion
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
