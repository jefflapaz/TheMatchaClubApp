using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TheMatchaClubApp.Core;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    public class ForgotPasswordDialog : Form
    {
        private readonly IServiceProvider _serviceProvider;

        // State 1: Identify
        private Guna2Panel pnlIdentify;
        private Guna2TextBox txtUsername;
        private Guna2Button btnSendOtp;
        private Label lblIdentifyError;

        // State 2: Verify
        private Guna2Panel pnlVerify;
        private Guna2TextBox txtOtp;
        private Guna2Button btnVerify;
        private Label lblVerifyError;

        // State 3: Reset
        private Guna2Panel pnlReset;
        private Guna2TextBox txtNewPassword;
        private Guna2TextBox txtConfirmPassword;
        private Guna2Button btnReset;
        private Label lblResetError;

        private Guna2Button btnClose;

        private string _generatedOtp = "";
        private ApplicationUser? _targetUser;

        public ForgotPasswordDialog(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeDesign();
            ShowPanel(pnlIdentify);
        }

        private void InitializeDesign()
        {
            this.Text = "Forgot Password";
            this.Size = new Size(400, 320);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            btnClose = new Guna2Button
            {
                Text = "Cancel",
                Location = new Point(24, 230),
                Size = new Size(100, 36),
                FillColor = ColorTranslator.FromHtml("#F3F4F6"),
                ForeColor = Color.Black,
                BorderRadius = 6,
                Cursor = Cursors.Hand
            };
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            InitializeIdentifyPanel();
            InitializeVerifyPanel();
            InitializeResetPanel();

            this.Controls.Add(pnlIdentify);
            this.Controls.Add(pnlVerify);
            this.Controls.Add(pnlReset);
        }

        private void InitializeIdentifyPanel()
        {
            pnlIdentify = new Guna2Panel { Location = new Point(0, 0), Size = new Size(400, 220), Visible = false };

            var lblTitle = new Label { Text = "Account Recovery", Location = new Point(24, 24), Size = new Size(300, 30), Font = new Font("Segoe UI Semibold", 12F), ForeColor = Color.Black };
            var lblSub = new Label { Text = "Enter your username or email to receive a recovery code.", Location = new Point(24, 54), Size = new Size(350, 40), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray };

            txtUsername = new Guna2TextBox { Location = new Point(24, 100), Size = new Size(336, 40), PlaceholderText = "Email or Username", BorderRadius = 6, Font = new Font("Segoe UI", 9F) };
            
            lblIdentifyError = new Label { Location = new Point(24, 145), Size = new Size(336, 30), Font = new Font("Segoe UI", 8F), ForeColor = Color.Red, Visible = false };

            btnSendOtp = new Guna2Button { Text = "Send Code", Location = new Point(260, 230), Size = new Size(100, 36), FillColor = ColorTranslator.FromHtml("#52B743"), ForeColor = Color.White, BorderRadius = 6, Cursor = Cursors.Hand };
            btnSendOtp.Click += BtnSendOtp_Click;
            this.Controls.Add(btnSendOtp);

            pnlIdentify.Controls.AddRange(new Control[] { lblTitle, lblSub, txtUsername, lblIdentifyError });
        }

        private void InitializeVerifyPanel()
        {
            pnlVerify = new Guna2Panel { Location = new Point(0, 0), Size = new Size(400, 220), Visible = false };

            var lblTitle = new Label { Text = "Verify Code", Location = new Point(24, 24), Size = new Size(300, 30), Font = new Font("Segoe UI Semibold", 12F), ForeColor = Color.Black };
            var lblSub = new Label { Text = "Enter the 6-digit code sent to your email address.", Location = new Point(24, 54), Size = new Size(350, 40), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray };

            txtOtp = new Guna2TextBox { Location = new Point(24, 100), Size = new Size(336, 40), PlaceholderText = "000000", BorderRadius = 6, Font = new Font("Segoe UI", 14F, FontStyle.Bold), TextAlign = HorizontalAlignment.Center, MaxLength = 6 };
            
            lblVerifyError = new Label { Location = new Point(24, 145), Size = new Size(336, 20), Font = new Font("Segoe UI", 8F), ForeColor = Color.Red, Visible = false };

            btnVerify = new Guna2Button { Text = "Verify", Location = new Point(260, 230), Size = new Size(100, 36), FillColor = ColorTranslator.FromHtml("#52B743"), ForeColor = Color.White, BorderRadius = 6, Cursor = Cursors.Hand, Visible = false };
            btnVerify.Click += BtnVerify_Click;
            this.Controls.Add(btnVerify);

            pnlVerify.Controls.AddRange(new Control[] { lblTitle, lblSub, txtOtp, lblVerifyError });
        }

        private void InitializeResetPanel()
        {
            pnlReset = new Guna2Panel { Location = new Point(0, 0), Size = new Size(400, 220), Visible = false };

            var lblTitle = new Label { Text = "Reset Password", Location = new Point(24, 24), Size = new Size(300, 30), Font = new Font("Segoe UI Semibold", 12F), ForeColor = Color.Black };
            var lblSub = new Label { Text = "Enter your new password below.", Location = new Point(24, 54), Size = new Size(350, 30), Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray };

            txtNewPassword = new Guna2TextBox { Location = new Point(24, 90), Size = new Size(336, 40), PlaceholderText = "New Password", BorderRadius = 6, PasswordChar = '•', Font = new Font("Segoe UI", 9F) };
            txtConfirmPassword = new Guna2TextBox { Location = new Point(24, 140), Size = new Size(336, 40), PlaceholderText = "Confirm Password", BorderRadius = 6, PasswordChar = '•', Font = new Font("Segoe UI", 9F) };
            
            lblResetError = new Label { Location = new Point(24, 185), Size = new Size(336, 20), Font = new Font("Segoe UI", 8F), ForeColor = Color.Red, Visible = false };

            btnReset = new Guna2Button { Text = "Update", Location = new Point(260, 230), Size = new Size(100, 36), FillColor = ColorTranslator.FromHtml("#52B743"), ForeColor = Color.White, BorderRadius = 6, Cursor = Cursors.Hand, Visible = false };
            btnReset.Click += BtnReset_Click;
            this.Controls.Add(btnReset);

            pnlReset.Controls.AddRange(new Control[] { lblTitle, lblSub, txtNewPassword, txtConfirmPassword, lblResetError });
        }

        private void ShowPanel(Guna2Panel panel)
        {
            pnlIdentify.Visible = false;
            pnlVerify.Visible = false;
            pnlReset.Visible = false;
            btnSendOtp.Visible = false;
            btnVerify.Visible = false;
            btnReset.Visible = false;

            panel.Visible = true;
            if (panel == pnlIdentify) btnSendOtp.Visible = true;
            else if (panel == pnlVerify) btnVerify.Visible = true;
            else if (panel == pnlReset) btnReset.Visible = true;
        }

        private async void BtnSendOtp_Click(object? sender, EventArgs e)
        {
            string input = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            lblIdentifyError.Visible = false;
            btnSendOtp.Enabled = false;
            btnSendOtp.Text = "Wait...";

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Look up user
                _targetUser = input.Contains("@") ? await userManager.FindByEmailAsync(input) : await userManager.FindByNameAsync(input);
                if (_targetUser == null && !input.Contains("@")) _targetUser = await userManager.FindByEmailAsync(input);

                if (_targetUser == null || string.IsNullOrEmpty(_targetUser.Email))
                {
                    lblIdentifyError.Text = "User not found or no email address associated.";
                    lblIdentifyError.Visible = true;
                    btnSendOtp.Enabled = true;
                    btnSendOtp.Text = "Send Code";
                    return;
                }

                // Check SMTP settings
                var settings = Program.DataService.Settings;
                if (string.IsNullOrEmpty(settings.SmtpServer) || string.IsNullOrEmpty(settings.SmtpPassword))
                {
                    lblIdentifyError.Text = "SMTP is not configured in Store Profile.\nPlease contact the administrator or reset manually.";
                    lblIdentifyError.Visible = true;
                    btnSendOtp.Enabled = true;
                    btnSendOtp.Text = "Send Code";
                    return;
                }

                // Generate OTP
                _generatedOtp = new Random().Next(100000, 999999).ToString();

                // Send Email
                await Task.Run(() =>
                {
                    using var client = new SmtpClient(settings.SmtpServer, settings.SmtpPort)
                    {
                        EnableSsl = true,
                        Timeout = 15000,
                        Credentials = new NetworkCredential(settings.Email, settings.SmtpPassword)
                    };
                    using var mail = new MailMessage
                    {
                        From = new MailAddress(settings.Email, settings.StoreName),
                        Subject = "Matcha Cafe POS - Password Recovery Code",
                        Body = $"<div style='font-family:sans-serif;padding:20px;'><h2 style='color:#52B743;'>Password Recovery</h2><p>Your one-time recovery code is: <strong style='font-size:24px;'>{_generatedOtp}</strong></p><p>If you did not request this, please ignore this email.</p></div>",
                        IsBodyHtml = true
                    };
                    mail.To.Add(_targetUser.Email);
                    client.Send(mail);
                });

                ShowPanel(pnlVerify);
                txtOtp.Focus();
            }
            catch (Exception ex)
            {
                lblIdentifyError.Text = $"Failed to send email: {ex.Message}";
                lblIdentifyError.Visible = true;
            }
            finally
            {
                btnSendOtp.Enabled = true;
                btnSendOtp.Text = "Send Code";
            }
        }

        private void BtnVerify_Click(object? sender, EventArgs e)
        {
            if (txtOtp.Text.Trim() == _generatedOtp && !string.IsNullOrEmpty(_generatedOtp))
            {
                ShowPanel(pnlReset);
                txtNewPassword.Focus();
            }
            else
            {
                lblVerifyError.Text = "Incorrect code. Please try again.";
                lblVerifyError.Visible = true;
                txtOtp.Clear();
                txtOtp.Focus();
            }
        }

        private async void BtnReset_Click(object? sender, EventArgs e)
        {
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                lblResetError.Text = "Passwords do not match.";
                lblResetError.Visible = true;
                return;
            }

            if (txtNewPassword.Text.Length < 6)
            {
                lblResetError.Text = "Password must be at least 6 characters long.";
                lblResetError.Visible = true;
                return;
            }

            btnReset.Enabled = false;
            btnReset.Text = "Saving...";
            lblResetError.Visible = false;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var user = await userManager.FindByIdAsync(_targetUser!.Id);
                
                // ASP.NET Core Identity trick to force reset without token
                var token = await userManager.GeneratePasswordResetTokenAsync(user!);
                var result = await userManager.ResetPasswordAsync(user!, token, txtNewPassword.Text);

                if (result.Succeeded)
                {
                    MessageBox.Show("Password updated successfully! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    lblResetError.Text = result.Errors.FirstOrDefault()?.Description ?? "Failed to reset password.";
                    lblResetError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblResetError.Text = "Error updating password.";
                lblResetError.Visible = true;
            }
            finally
            {
                btnReset.Enabled = true;
                btnReset.Text = "Update";
            }
        }
    }
}
