using EnglishCentralManagement.Areas.Admin.Services;
using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Models.Enum;
using EnglishCentralManagement.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace EnglishCentralManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region QuestPDF License
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            #endregion

            #region DI
            //builder.Services.AddScoped<SessionAuthorizeFilter>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IStaffService, StaffService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IClassService, ClassService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IReceiptService, ReceiptService>();
            builder.Services.AddScoped<IExpenseService, ExpenseService>();
            builder.Services.AddScoped<IExcelService, ExcelService>();
            builder.Services.AddScoped<IClassSessionService, ClassSessionService>();
            builder.Services.AddScoped<ISettingService, SettingService>();
            builder.Services.AddScoped<IEmailService, GmailService>();
            builder.Services.AddScoped<EmailTemplateService>();
            builder.Services.AddSingleton<CloudflareR2Service>();

            #endregion
            //Rate Limiting
            builder.Services.AddRateLimiter(options =>
            {
                //Rateliming chống spam từ 1 user 
                // muốn chặn DDOS thì dùng Cloudfare hoặc Azure DDoS Protection + API Management
                options.RejectionStatusCode = 429;
                options.AddPolicy("RegisterPolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 3,
                            Window = TimeSpan.FromMinutes(60),
                            QueueLimit = 0
                        }
                    )
                );
            });
            //Authentication + Authorization
            builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Admin/Login";           // URL login
                options.AccessDeniedPath = "/Admin/access-denied";
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
                #region Authorize Controller
                options.AddPolicy("DashboardPolicy", policy =>
                    policy.RequireRole(
                        nameof(RoleType.Admin),
                        nameof(RoleType.Manager)
                    ));

                options.AddPolicy("ManageUsersPolicy", policy =>
                    policy.RequireRole(
                        nameof(RoleType.Admin),
                        nameof(RoleType.Manager),
                        nameof(RoleType.HR)
                    ));

                options.AddPolicy("ManageClassPolicy", policy =>
                    policy.RequireRole(
                        nameof(RoleType.Admin),
                        nameof(RoleType.Manager),
                        nameof(RoleType.Coordinator)
                    ));

                options.AddPolicy("TeacherPolicy", policy =>
                    policy.RequireRole(
                        nameof(RoleType.Teacher),
                        nameof(RoleType.Manager),
                        nameof(RoleType.Admin)
                    ));
                options.AddPolicy("ProfilePolicy", policy =>
                    policy.RequireRole(
                        nameof(RoleType.Teacher),
                        nameof(RoleType.Manager),
                        nameof(RoleType.Coordinator),
                        nameof(RoleType.HR),
                        nameof(RoleType.Accountant),
                        nameof(RoleType.Admin)
                    ));
                #endregion
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

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseRateLimiter();
            app.UseAuthorization();
            #region Admin Route
            app.MapControllerRoute(
                name: "admin-login",
                pattern: "admin/login",
                defaults: new { area = "Admin", controller = "Login", action = "Login" }
            );
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
