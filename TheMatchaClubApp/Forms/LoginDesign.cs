using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

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
            
            lblBadge.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
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

            // Email Field
            lblEmail.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblEmail.ForeColor = ColorTranslator.FromHtml("#6B7280");
            
            txtEmail.BackColor = Color.Transparent;
            txtEmail.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            txtEmail.BorderRadius = 8;
            txtEmail.ForeColor = ColorTranslator.FromHtml("#374151");
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.DefaultText = "admin@matchacafe.pos";
            txtEmail.PlaceholderText = "";
            txtEmail.TextOffset = new Point(5, 0);
            // txtEmail.IconLeft = Properties.Resources.user_icon; // Uncomment and replace with actual PNG resource

            // Password Field
            lblPassword.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblPassword.ForeColor = ColorTranslator.FromHtml("#6B7280");

            lblForgotPassword.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblForgotPassword.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblForgotPassword.Cursor = Cursors.Hand;
            lblForgotPassword.TextAlign = ContentAlignment.MiddleRight;

            txtPassword.BackColor = Color.Transparent;
            txtPassword.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            txtPassword.BorderRadius = 8;
            txtPassword.ForeColor = ColorTranslator.FromHtml("#374151");
            txtPassword.Font = new Font("Segoe UI", 9F);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.DefaultText = "password123";
            txtPassword.PlaceholderText = "";
            txtPassword.TextOffset = new Point(5, 0);
            // txtPassword.IconLeft = Properties.Resources.lock_icon; // Uncomment and replace with actual PNG resource

            // Sign In Button
            btnSignIn.FillColor = ColorTranslator.FromHtml("#52B743");
            btnSignIn.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnSignIn.PressedColor = ColorTranslator.FromHtml("#3D8F32");
            btnSignIn.BorderRadius = 8;
            btnSignIn.ForeColor = Color.White;
            btnSignIn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnSignIn.Click += BtnSignIn_Click;

            // Divider
            pnlDividerLeft.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            pnlDividerRight.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            
            lblOr.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblOr.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
            lblOr.TextAlign = ContentAlignment.MiddleCenter;

            // Clock In Button
            btnClockIn.FillColor = Color.White;
            btnClockIn.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            btnClockIn.BorderThickness = 1;
            btnClockIn.BorderRadius = 8;
            btnClockIn.ForeColor = ColorTranslator.FromHtml("#374151");
            btnClockIn.Font = new Font("Segoe UI", 9F);
            btnClockIn.HoverState.FillColor = ColorTranslator.FromHtml("#F9FAFB");

            // First Time Setup
            pnlFirstTimeSetup.BackColor = Color.Transparent;
            pnlFirstTimeSetup.FillColor = ColorTranslator.FromHtml("#F2FAEF");
            pnlFirstTimeSetup.BorderColor = ColorTranslator.FromHtml("#E2F3DD");
            pnlFirstTimeSetup.BorderThickness = 1;
            pnlFirstTimeSetup.BorderRadius = 12;
            pnlFirstTimeSetup.Paint += PnlFirstTimeSetup_Paint;

            lblSetupTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSetupTitle.ForeColor = ColorTranslator.FromHtml("#111827");
            
            lblSetupDesc.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblSetupDesc.ForeColor = ColorTranslator.FromHtml("#6B7280");

            lnkSetup.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lnkSetup.LinkColor = ColorTranslator.FromHtml("#52B743");
            lnkSetup.ActiveLinkColor = ColorTranslator.FromHtml("#46A037");
            lnkSetup.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkSetup.Cursor = Cursors.Hand;
            lnkSetup.Click += LnkSetup_Click;


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

        private void PnlFirstTimeSetup_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Draw info icon
            using (SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml("#52B743")))
            {
                e.Graphics.FillEllipse(brush, new Rectangle(12, 12, 18, 18));
            }

            using (Font font = new Font("Segoe UI", 10F, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                e.Graphics.DrawString("i", font, textBrush, 16, 11);
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

        private void BtnSignIn_Click(object? sender, EventArgs e)
        {
            var shell = new MainShell();
            shell.FormClosed += (_, __) =>
            {
                this.Show(); // return to login when shell is closed
            };
            this.Hide();
            shell.Show();
        }

        private void LnkSetup_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Navigate to SetupWizardForm", "First Time Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
