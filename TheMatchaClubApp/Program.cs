using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheMatchaClubApp.Core.Models;
using TheMatchaClubApp.Core.Services;

namespace TheMatchaClubApp
{
    /// <summary>
    /// Lightweight Identity-only DbContext used by the WinForms app for authentication.
    /// This avoids a circular project reference with TheMatchaClub.Infrastructure
    /// while still sharing the same database and ApplicationUser table.
    /// The Infrastructure project's MatchaClubDbContext handles business entities
    /// (Products, Orders, Customers, etc.) and migrations.
    /// </summary>
    internal class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
    }

    internal static class Program
    {
        // ── Existing data service (JSON-based for products, orders, etc.) ──
        public static JsonDataService DataService { get; } = new JsonDataService();

        // ── Session management service ───────────────────────────────────
        public static SessionService SessionService { get; } = new SessionService(DataService);

        // ── DI container for Identity & EF services ──────────────────────
        public static IServiceProvider Services { get; private set; } = null!;

        // ── Currently logged-in user (set after successful login) ─────────
        public static ApplicationUser? CurrentUser { get; set; }

        /// <summary>
        /// Centralized source of truth for the cashier identity.
        /// Prioritizes the editable setting, fallbacks to logged-in user's full name.
        /// </summary>
        public static string GetCurrentCashierName()
        {
            var s = DataService.Settings;
            if (!string.IsNullOrWhiteSpace(s.CashierName)) return s.CashierName;
            return CurrentUser?.FullName ?? "Admin";
        }

        /// <summary>
        /// Connection string for the local SQL Server database.
        /// Uses the same database as the Infrastructure layer.
        /// </summary>
        private const string ConnectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=TheMatchaClubDb;Trusted_Connection=True;";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ── Culture settings (existing) ─────────────────────────
            var culture = new CultureInfo("en-PH");
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            culture.DateTimeFormat.LongDatePattern = "dd MMMM yyyy";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;


            ApplicationConfiguration.Initialize();

            // ── Build DI container with Identity + EF ───────────────
            Services = ConfigureServices();

            // ── Ensure Identity tables exist ────────────────────────
            EnsureDatabase();

            // ── Startup flow: check if any users exist ──────────────
            // If no users → show AdminSetupForm (first-time setup)
            // If users exist → show LoginForm directly
            if (!HasAnyUsers())
            {
                // First-time launch — create admin account
                var setupForm = new Forms.AdminSetupForm(Services);
                Application.Run(setupForm);

                // If user closed the setup without creating an account, exit
                if (!setupForm.AccountCreated)
                    return;

                // Account created — now show login form
                Application.Run(new Forms.LoginForm(Services));
            }
            else
            {
                // Normal launch — go straight to login
                Application.Run(new Forms.LoginForm(Services));
            }
        }

        /// <summary>
        /// Configures the DI service provider with EF Core, Identity, and logging.
        /// Uses a lightweight AuthDbContext that shares the same database as the
        /// Infrastructure project's MatchaClubDbContext.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register the auth-only DbContext with SQL Server
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(ConnectionString));

            // Register ASP.NET Identity with our custom ApplicationUser
            // This sets up UserManager<ApplicationUser>, RoleManager<IdentityRole>,
            // and all supporting services (password hasher, validators, etc.)
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                // Password policy — relaxed for POS use; adjust as needed
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>();

            // Add minimal logging (required by Identity internals)
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Warning);
            });

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Ensures the Identity tables exist in the database.
        /// The business tables (Products, Orders, etc.) are managed separately
        /// by the Infrastructure project's migrations.
        /// </summary>
        private static void EnsureDatabase()
        {
            try
            {
                using var scope = Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                // EnsureCreated works because the Identity tables were already
                // created by the Infrastructure project's migration.
                // This is a safety net in case the app runs before migrations.
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to initialize database:\n\n{ex.Message}",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Checks if any user accounts exist in the Identity database.
        /// </summary>
        private static bool HasAnyUsers()
        {
            using var scope = Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            return userManager.Users.Any();
        }
    }
}