using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    /// <summary>
    /// One-time setup form that creates the first administrator account.
    /// Shown only when no users exist in the Identity database.
    /// Uses ASP.NET Identity's built-in UserManager for secure password hashing.
    /// </summary>
    public partial class AdminSetupForm : Form
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Set to true when account creation succeeds, so Program.cs knows
        /// to proceed to the LoginForm.
        /// </summary>
        public bool AccountCreated { get; private set; } = false;

        public AdminSetupForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InitializeComponent();
            DoubleBuffered = true;
            InitializeDesign();
        }

        /// <summary>
        /// Handles the Create Account button click.
        /// Validates all fields, then uses UserManager to create the admin user
        /// with Identity's built-in secure password hashing.
        /// </summary>
        private async void BtnCreateAccount_Click(object? sender, EventArgs e)
        {
            // ── Clear previous errors ───────────────────────────────
            lblError.Visible = false;
            lblError.Text = "";

            // ── Field validation ────────────────────────────────────
            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ShowError("Full Name is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Username is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowError("Email is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Password is required.");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Password and Confirm Password do not match.");
                return;
            }

            // ── Disable button to prevent double-click ──────────────
            btnCreateAccount.Enabled = false;
            btnCreateAccount.Text = "Creating Account...";

            try
            {
                // Resolve Identity services from DI
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // ── Check uniqueness ────────────────────────────────
                var existingByUsername = await userManager.FindByNameAsync(username);
                if (existingByUsername != null)
                {
                    ShowError("Username is already taken.");
                    return;
                }

                var existingByEmail = await userManager.FindByEmailAsync(email);
                if (existingByEmail != null)
                {
                    ShowError("Email is already in use.");
                    return;
                }

                // ── Create "Admin" role if it doesn't exist ─────────
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (!roleResult.Succeeded)
                    {
                        ShowError("Failed to create Admin role: " +
                            string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                        return;
                    }
                }

                // ── Create the admin user ───────────────────────────
                // Password is hashed automatically by Identity's UserManager
                var user = new ApplicationUser
                {
                    FullName = fullName,
                    UserName = username,
                    Email = email,
                    DateCreated = DateTime.UtcNow
                };

                var createResult = await userManager.CreateAsync(user, password);

                if (!createResult.Succeeded)
                {
                    // Show Identity validation errors (e.g. password too weak)
                    string errors = string.Join("\n", createResult.Errors.Select(e => e.Description));
                    ShowError(errors);
                    return;
                }

                // ── Assign Admin role ───────────────────────────────
                await userManager.AddToRoleAsync(user, "Admin");

                // ── Success! ────────────────────────────────────────
                AccountCreated = true;

                MessageBox.Show(
                    $"Admin account \"{username}\" created successfully!\n\nYou can now sign in.",
                    "Setup Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                // Re-enable button in case of error
                if (!AccountCreated)
                {
                    btnCreateAccount.Enabled = true;
                    btnCreateAccount.Text = "Create Admin Account & Continue";
                }
            }
        }

        /// <summary>
        /// Displays an error message in the error label.
        /// </summary>
        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
