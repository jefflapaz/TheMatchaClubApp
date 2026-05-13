using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace TheMatchaClubApp.Forms
{
    public partial class AdminSetupForm
    {
        // ── Drag-to-move state ──────────────────────────────────────
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        /// <summary>
        /// Applies the same visual language as LoginForm:
        ///   • #FAFAFA background, white card, #52B743 green accent
        ///   • Segoe UI fonts, 8px input radius, 16px card radius
        ///   • Title bar drag-to-move, custom painted logo
        /// </summary>
        private void InitializeDesign()
        {
            // ── Form Setup ──────────────────────────────────────────
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = ColorTranslator.FromHtml("#FAFAFA");
            this.StartPosition = FormStartPosition.CenterScreen;

            // ── Title Bar ───────────────────────────────────────────
            pnlTitleBar.BackColor = ColorTranslator.FromHtml("#FAFAFA");
            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            pnlTitleBar.MouseMove += TitleBar_MouseMove;
            pnlTitleBar.MouseUp += TitleBar_MouseUp;

            // Close & Minimize Buttons (same style as LoginForm)
            btnClose.FillColor = Color.Transparent;
            btnClose.IconColor = Color.Gray;
            btnClose.HoverState.FillColor = Color.LightGray;
            btnClose.HoverState.IconColor = Color.Black;

            btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            btnMinimize.FillColor = Color.Transparent;
            btnMinimize.IconColor = Color.Gray;
            btnMinimize.HoverState.FillColor = Color.LightGray;
            btnMinimize.HoverState.IconColor = Color.Black;

            // ── Card Panel ──────────────────────────────────────────
            pnlCard.BackColor = Color.Transparent;
            pnlCard.FillColor = Color.White;
            pnlCard.BorderRadius = 16;
            pnlCard.ShadowDecoration.Enabled = false;

            // ── Badge ───────────────────────────────────────────────
            pnlBadge.BackColor = Color.Transparent;
            pnlBadge.FillColor = Color.White;
            pnlBadge.BorderRadius = 12;
            pnlBadge.BorderThickness = 1;
            pnlBadge.BorderColor = ColorTranslator.FromHtml("#E5E7EB");

            lblBadge.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblBadge.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblBadge.TextAlign = ContentAlignment.MiddleCenter;

            // ── Logo ────────────────────────────────────────────────
            picLogo.Paint += PicLogo_Paint;

            // ── Title & Subtitle ────────────────────────────────────
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = ColorTranslator.FromHtml("#111827");
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblSubtitle.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // ── Field Labels (same style as LoginForm) ──────────────
            StyleFieldLabel(lblFullName);
            StyleFieldLabel(lblUsername);
            StyleFieldLabel(lblEmail);
            StyleFieldLabel(lblPassword);
            StyleFieldLabel(lblConfirmPassword);

            // ── Text Inputs (same style as LoginForm) ───────────────
            StyleTextInput(txtFullName);
            StyleTextInput(txtUsername);
            StyleTextInput(txtEmail);
            StyleTextInput(txtPassword);
            StyleTextInput(txtConfirmPassword);

            // Password masking
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

            // ── Error Label ─────────────────────────────────────────
            lblError.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblError.ForeColor = Color.FromArgb(220, 38, 38); // red-600
            lblError.TextAlign = ContentAlignment.MiddleLeft;

            // ── Create Account Button (matches Sign In button) ──────
            btnCreateAccount.FillColor = ColorTranslator.FromHtml("#52B743");
            btnCreateAccount.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnCreateAccount.PressedColor = ColorTranslator.FromHtml("#3D8F32");
            btnCreateAccount.BorderRadius = 8;
            btnCreateAccount.ForeColor = Color.White;
            btnCreateAccount.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btnCreateAccount.Click += BtnCreateAccount_Click;
        }

        // ── Helper: style a field label ─────────────────────────────
        private void StyleFieldLabel(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lbl.ForeColor = ColorTranslator.FromHtml("#6B7280");
        }

        // ── Helper: style a text input ──────────────────────────────
        private void StyleTextInput(Guna2TextBox txt)
        {
            txt.BackColor = Color.Transparent;
            txt.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            txt.BorderRadius = 8;
            txt.ForeColor = ColorTranslator.FromHtml("#374151");
            txt.Font = new Font("Segoe UI", 9F);
            txt.TextOffset = new Point(5, 0);
        }

        // ── Logo painting (identical to LoginForm) ──────────────────
        private void PicLogo_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Drop shadow
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
            {
                e.Graphics.FillEllipse(shadowBrush, new Rectangle(1, 1, 50, 50));
            }

            // Green circle
            using (SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml("#52B743")))
            {
                e.Graphics.FillEllipse(brush, new Rectangle(0, 0, 50, 50));
            }

            // Leaf icon
            string leaf = "♣";
            using (Font font = new Font("Segoe UI", 24F))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                SizeF textSize = e.Graphics.MeasureString(leaf, font);
                e.Graphics.DrawString(leaf, font, textBrush, (52 - textSize.Width) / 2, (52 - textSize.Height) / 2 + 2);
            }
        }

        // ── Title bar drag support ──────────────────────────────────
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

        // ── Center card on resize ───────────────────────────────────
        private void AdminSetupForm_Resize(object? sender, EventArgs e)
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
