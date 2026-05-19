using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TheMatchaClubDomain.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class LoginForm
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void InitializeDesign()
        {
            // Form Setup
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = ColorTranslator.FromHtml("#FAFAFA");
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title Bar
            pnlTitleBar.BackColor = ColorTranslator.FromHtml("#FAFAFA");
            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            pnlTitleBar.MouseMove += TitleBar_MouseMove;
            pnlTitleBar.MouseUp += TitleBar_MouseUp;

            // Close, Maximize & Minimize Buttons
            btnClose.FillColor = Color.Transparent;
            btnClose.IconColor = Color.Gray;
            btnClose.HoverState.FillColor = Color.LightGray;
            btnClose.HoverState.IconColor = Color.Black;

            btnMaximize.FillColor = Color.Transparent;
            btnMaximize.IconColor = Color.Gray;
            btnMaximize.HoverState.FillColor = Color.LightGray;
            btnMaximize.HoverState.IconColor = Color.Black;

            btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            btnMinimize.FillColor = Color.Transparent;
            btnMinimize.IconColor = Color.Gray;
            btnMinimize.HoverState.FillColor = Color.LightGray;
            btnMinimize.HoverState.IconColor = Color.Black;

            // Card Panel
            pnlCard.BackColor = Color.Transparent;
            pnlCard.FillColor = Color.White;
            pnlCard.BorderRadius = 16;
            pnlCard.ShadowDecoration.Enabled = false;

            // Badge
            pnlBadge.BackColor = Color.Transparent;
            pnlBadge.FillColor = Color.White;
            pnlBadge.BorderRadius = 12;
            pnlBadge.BorderThickness = 1;
            pnlBadge.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            
            lblBadge.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblBadge.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblBadge.TextAlign = ContentAlignment.MiddleCenter;

            // Logo
            picLogo.Paint += PicLogo_Paint;

            // Title & Subtitle
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = ColorTranslator.FromHtml("#111827");
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblSubtitle.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // Email Field — no more hardcoded defaults
            lblEmail.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblEmail.ForeColor = ColorTranslator.FromHtml("#6B7280");
            
            txtEmail.BackColor = Color.Transparent;
            txtEmail.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            txtEmail.BorderRadius = 8;
            txtEmail.ForeColor = ColorTranslator.FromHtml("#374151");
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.DefaultText = "";
            txtEmail.PlaceholderText = "Enter your email or username";
            txtEmail.TextOffset = new Point(5, 0);

            // Password Field — no more hardcoded defaults
            lblPassword.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblPassword.ForeColor = ColorTranslator.FromHtml("#6B7280");

            lblForgotPassword.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblForgotPassword.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblForgotPassword.Cursor = Cursors.Hand;
            lblForgotPassword.TextAlign = ContentAlignment.MiddleRight;
            lblForgotPassword.Click += LblForgotPassword_Click;

            txtPassword.BackColor = Color.Transparent;
            txtPassword.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            txtPassword.BorderRadius = 8;
            txtPassword.ForeColor = ColorTranslator.FromHtml("#374151");
            txtPassword.Font = new Font("Segoe UI", 9F);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.DefaultText = "";
            txtPassword.PlaceholderText = "Enter your password";
            txtPassword.TextOffset = new Point(5, 0);

            // Sign In Button
            btnSignIn.FillColor = ColorTranslator.FromHtml("#52B743");
            btnSignIn.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnSignIn.PressedColor = ColorTranslator.FromHtml("#3D8F32");
            btnSignIn.BorderRadius = 8;
            btnSignIn.ForeColor = Color.White;
            btnSignIn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnSignIn.Click += BtnSignIn_Click;

            // Show/Hide Password Toggle
            var lblShowPassword = new Label
            {
                Text = "Show",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#9CA3AF"),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                AutoSize = true
            };
            
            // Wait for layout to properly position it inside the text box
            lblShowPassword.Location = new Point(txtPassword.Location.X + txtPassword.Width - 60, txtPassword.Location.Y + 22);
            
            lblShowPassword.Click += (s, e) =>
            {
                if (txtPassword.UseSystemPasswordChar)
                {
                    txtPassword.UseSystemPasswordChar = false;
                    lblShowPassword.Text = "Hide";
                }
                else
                {
                    txtPassword.UseSystemPasswordChar = true;
                    lblShowPassword.Text = "Show";
                }
            };
            
            pnlCard.Controls.Add(lblShowPassword);
            lblShowPassword.BringToFront();
        }

        private void PicLogo_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Draw drop shadow
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
            {
                e.Graphics.FillEllipse(shadowBrush, new Rectangle(1, 1, 50, 50));
            }
            
            // Draw circle
            using (SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml("#52B743")))
            {
                e.Graphics.FillEllipse(brush, new Rectangle(0, 0, 50, 50));
            }

            // Draw leaf icon
            string leaf = "♣"; // Unicode leaf approximation
            using (Font font = new Font("Segoe UI", 24F))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                SizeF textSize = e.Graphics.MeasureString(leaf, font);
                e.Graphics.DrawString(leaf, font, textBrush, (52 - textSize.Width) / 2, (52 - textSize.Height) / 2 + 2);
            }
        }

        private void TitleBar_MouseDown(object? sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void TitleBar_MouseMove(object? sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void TitleBar_MouseUp(object? sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void LblForgotPassword_Click(object? sender, EventArgs e)
        {
            using var dialog = new ForgotPasswordDialog(_serviceProvider);
            dialog.ShowDialog(this);
        }

        /// <summary>
        /// Authenticates the user against the Identity database.
        /// Supports login by email OR username.
        /// Uses Identity's built-in password verification (no custom hashing).
        /// </summary>
        private async void BtnSignIn_Click(object? sender, EventArgs e)
        {
            // ── Validate empty fields ───────────────────────────────
            string loginInput = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(loginInput))
            {
                MessageBox.Show("Please enter your email or username.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your password.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // ── Disable button during auth ──────────────────────────
            btnSignIn.Enabled = false;
            btnSignIn.Text = "Signing In...";

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // ── Try to find user by email first, then by username ─
                ApplicationUser? user = null;

                // Check if input looks like an email
                if (loginInput.Contains('@'))
                {
                    user = await userManager.FindByEmailAsync(loginInput);
                }

                // If not found by email, try username
                if (user == null)
                {
                    user = await userManager.FindByNameAsync(loginInput);
                }

                // If still not found, also try email for non-@ inputs
                if (user == null)
                {
                    user = await userManager.FindByEmailAsync(loginInput);
                }

                if (user == null)
                {
                    MessageBox.Show("Invalid email/username or password.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ── Verify password using Identity's built-in hashing ─
                var passwordValid = await userManager.CheckPasswordAsync(user, password);

                if (!passwordValid)
                {
                    MessageBox.Show("Invalid email/username or password.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ── Login successful! ───────────────────────────────
                // Store logged-in user info for the session
                Program.CurrentUser = user;

                var shell = new MainShell();
                shell.FormClosed += (_, __) =>
                {
                    // Clear user on logout and return to login
                    Program.CurrentUser = null;
                    txtPassword.Text = "";
                    this.Show();
                };
                this.Hide();
                shell.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSignIn.Enabled = true;
                btnSignIn.Text = "Sign In to Terminal";
            }
        }

        private void LoginForm_Resize(object? sender, EventArgs e)
        {
            if (pnlCard != null)
            {
                pnlCard.Location = new Point(
                    (this.ClientSize.Width - pnlCard.Width) / 2,
                    (this.ClientSize.Height - pnlCard.Height) / 2 - 10
                );
            }
        }
    }
}
