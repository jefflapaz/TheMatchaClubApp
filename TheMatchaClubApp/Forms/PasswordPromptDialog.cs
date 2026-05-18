using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    public class PasswordPromptDialog : Form
    {
        private Guna2TextBox txtPassword;
        private Guna2Button btnConfirm;
        private Guna2Button btnCancel;
        private Label lblError;

        public bool IsAuthenticated { get; private set; } = false;

        public PasswordPromptDialog(string promptMessage = "Please enter your password to continue.")
        {
            InitializeDesign(promptMessage);
        }

        private void InitializeDesign(string promptMessage)
        {
            this.Text = "Security Verification";
            this.Size = new Size(360, 240);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            var lblPrompt = new Label
            {
                Text = promptMessage,
                Location = new Point(24, 24),
                Size = new Size(300, 40),
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Black
            };

            txtPassword = new Guna2TextBox
            {
                Location = new Point(24, 76),
                Size = new Size(296, 36),
                PasswordChar = '•',
                PlaceholderText = "Password",
                BorderRadius = 6,
                Font = new Font("Segoe UI", 9F)
            };
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnConfirm_Click(s, e); };

            lblError = new Label
            {
                Text = "Incorrect password.",
                Location = new Point(24, 116),
                Size = new Size(296, 20),
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Red,
                Visible = false
            };

            btnCancel = new Guna2Button
            {
                Text = "Cancel",
                Location = new Point(116, 146),
                Size = new Size(96, 36),
                FillColor = ColorTranslator.FromHtml("#F3F4F6"),
                ForeColor = Color.Black,
                BorderRadius = 6,
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, e) => this.Close();

            btnConfirm = new Guna2Button
            {
                Text = "Confirm",
                Location = new Point(224, 146),
                Size = new Size(96, 36),
                FillColor = ColorTranslator.FromHtml("#52B743"),
                ForeColor = Color.White,
                BorderRadius = 6,
                Cursor = Cursors.Hand
            };
            btnConfirm.Click += BtnConfirm_Click;

            this.Controls.AddRange(new Control[] { lblPrompt, txtPassword, lblError, btnCancel, btnConfirm });
        }

        private async void BtnConfirm_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text)) return;

            lblError.Visible = false;
            btnConfirm.Enabled = false;

            if (Program.CurrentUser == null)
            {
                lblError.Text = "No active user session.";
                lblError.Visible = true;
                btnConfirm.Enabled = true;
                return;
            }

            using var scope = Program.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            var user = await userManager.FindByIdAsync(Program.CurrentUser.Id);
            if (user != null && await userManager.CheckPasswordAsync(user, txtPassword.Text))
            {
                IsAuthenticated = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblError.Text = "Incorrect password.";
                lblError.Visible = true;
                txtPassword.SelectAll();
                txtPassword.Focus();
                btnConfirm.Enabled = true;
            }
        }
    }
}
