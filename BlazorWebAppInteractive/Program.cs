using BlazorWebAppInteractive.Backend.Data;
using BlazorWebAppInteractive.Backend.Data.Models;
using BlazorWebAppInteractive.Backend.IServices;
using BlazorWebAppInteractive.Backend.Services;
using BlazorWebAppInteractive.Frontend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using MudBlazor.Translations;

namespace BlazorWebAppInteractive
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.ClearAfterNavigation = true;
                config.SnackbarConfiguration.BackgroundBlurred = false;
            });
            builder.Services.AddMudTranslations();
            // Add services to the container.
            builder.Services.AddRazorComponents(options =>
            {
                options.DetailedErrors = true;
            })
                .AddInteractiveServerComponents(options =>
                {
                    options.DetailedErrors = true;
                })
                .AddCircuitOptions(options =>
                {
                    options.DetailedErrors = true;
                });

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddSingleton<ThemeService>();
            builder.Services.AddSingleton<ProfilePictureService>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            })
            .AddCookie();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;
                options.LoginPath = "/login"; // Redirects to /login without any ReturnUrl parameter
                options.AccessDeniedPath = "/accessdenied"; // Redirects to /accessdenied without any ReturnUrl
                options.Events.OnRedirectToLogin = context =>
               {
                   context.Response.Redirect("/login");
                   return Task.CompletedTask;
               };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.Redirect("/accessdenied");
                    return Task.CompletedTask;
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            SetRoles(app.Services.GetRequiredService<IServiceProvider>().CreateScope().ServiceProvider).Wait();
            app.Run();
        }

        private static async Task SetRoles(IServiceProvider ServiceProvider)
        {
            var context = ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleStore = new RoleStore<IdentityRole>(context);

            if (!roleStore.Roles.Any(x => x.Name == "role1"))
            {
                await roleStore.CreateAsync(new IdentityRole
                {
                    Name = "role1",
                    NormalizedName = "role1".Normalize().ToUpper()
                });
            }

            if (!roleStore.Roles.Any(x => x.Name == "role2"))
            {

                await roleStore.CreateAsync(new IdentityRole
                {
                    Name = "role2",
                    NormalizedName = "role2".Normalize().ToUpper()
                });
            }

        }
    }
}
//Admin!abc123
//Admin!abc1234