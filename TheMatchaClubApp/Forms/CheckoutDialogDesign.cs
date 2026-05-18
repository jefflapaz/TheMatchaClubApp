using System;
using System.Drawing;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class CheckoutDialogForm
    {
        private static readonly Color DlgBg = Color.White;
        private static readonly Color DlgGreen = ColorTranslator.FromHtml("#52B743");
        private static readonly Color DlgGreenHover = ColorTranslator.FromHtml("#46A037");
        private static readonly Color DlgBorder = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color DlgText = ColorTranslator.FromHtml("#111827");
        private static readonly Color DlgMuted = ColorTranslator.FromHtml("#6B7280");

        private void InitializeDesign()
        {
            this.BackColor = ColorTranslator.FromHtml("#F9FAFB");

            // Header
            pnlHeader.BackColor = Color.Transparent;

            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = DlgText;
            lblTitle.BackColor = Color.Transparent;

            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = DlgMuted;
            lblSubtitle.BackColor = Color.Transparent;

            btnClose.FillColor = Color.Transparent;
            btnClose.ForeColor = DlgMuted;
            btnClose.BorderThickness = 0;
            btnClose.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnClose.HoverState.FillColor = ColorTranslator.FromHtml("#FEE2E2");
            btnClose.HoverState.ForeColor = ColorTranslator.FromHtml("#EF4444");
            btnClose.Cursor = Cursors.Hand;
            btnClose.Text = "✕";
            btnClose.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            // Form border and shadow
            this.Paint += (s, e) =>
            {
                using var borderPen = new Pen(DlgBorder, 1);
                e.Graphics.DrawRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1);
                using var shadowPen = new Pen(Color.FromArgb(20, 0, 0, 0), 2);
                e.Graphics.DrawLine(shadowPen, 2, this.Height, this.Width, this.Height);
                e.Graphics.DrawLine(shadowPen, this.Width, 2, this.Width, this.Height);
            };

            // Drag-to-move via header
            Point _dragStart = Point.Empty;
            pnlHeader.MouseDown += (s, me) => { if (me.Button == MouseButtons.Left) _dragStart = me.Location; };
            pnlHeader.MouseMove += (s, me) =>
            {
                if (me.Button == MouseButtons.Left && _dragStart != Point.Empty)
                {
                    this.Left += me.X - _dragStart.X;
                    this.Top += me.Y - _dragStart.Y;
                }
            };
            pnlHeader.MouseUp += (s, me) => _dragStart = Point.Empty;
            lblTitle.MouseDown += (s, me) => { if (me.Button == MouseButtons.Left) _dragStart = me.Location; };
            lblTitle.MouseMove += (s, me) =>
            {
                if (me.Button == MouseButtons.Left && _dragStart != Point.Empty)
                {
                    this.Left += me.X - _dragStart.X;
                    this.Top += me.Y - _dragStart.Y;
                }
            };
            lblTitle.MouseUp += (s, me) => _dragStart = Point.Empty;

            // Card Panel Styling
            StyleCard(pnlOrderTypeCard);
            StyleCard(pnlCustomerCard);
            StyleCard(pnlPaymentCard);

            // Order Type Labels
            lblOrderTypeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblOrderTypeLabel.ForeColor = ColorTranslator.FromHtml("#4B5563");
            lblOrderTypeLabel.BackColor = Color.Transparent;

            // Dine-In / Take-Out buttons
            StyleTypeButton(btnDineIn, true);
            StyleTypeButton(btnTakeOut, false);
            btnDineIn.Click += (s, e) => { SetOrderType(true); };
            btnTakeOut.Click += (s, e) => { SetOrderType(false); };

            // Customer section
            lblCustomerLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCustomerLabel.ForeColor = ColorTranslator.FromHtml("#4B5563");
            lblCustomerLabel.BackColor = Color.Transparent;

            txtCustomerSearch.Font = new Font("Segoe UI", 11F);
            txtCustomerSearch.BorderRadius = 8;
            txtCustomerSearch.BorderColor = DlgBorder;
            txtCustomerSearch.FocusedState.BorderColor = DlgGreen;
            txtCustomerSearch.TextOffset = new Point(4, 0);

            pnlSuggestions.BackColor = Color.White;
            pnlSuggestions.BorderStyle = BorderStyle.None;
            pnlSuggestions.Paint += (s, e) =>
            {
                using var pen = new Pen(DlgBorder, 1);
                var rect = new Rectangle(0, 0, pnlSuggestions.Width - 1, pnlSuggestions.Height - 1);
                e.Graphics.DrawRectangle(pen, rect);
            };

            lstSuggestions.Font = new Font("Segoe UI", 10F);
            lstSuggestions.ForeColor = DlgText;
            lstSuggestions.BackColor = Color.White;

            lblNewCustomerLabel.Font = new Font("Segoe UI", 9F);
            lblNewCustomerLabel.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
            lblNewCustomerLabel.BackColor = Color.Transparent;

            StyleTextField(txtFirstName);
            StyleTextField(txtLastName);
            StyleTextField(txtPhone);
            StyleTextField(txtNewEmail);

            // Payment section
            lblPaymentLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPaymentLabel.ForeColor = ColorTranslator.FromHtml("#4B5563");
            lblPaymentLabel.BackColor = Color.Transparent;

            lblTotalLabel.Font = new Font("Segoe UI", 11F);
            lblTotalLabel.ForeColor = ColorTranslator.FromHtml("#4B5563");
            lblTotalLabel.BackColor = Color.Transparent;

            lblTotalValue.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTotalValue.ForeColor = DlgGreen;
            lblTotalValue.BackColor = Color.Transparent;

            lblCashLabel.Font = new Font("Segoe UI", 11F);
            lblCashLabel.ForeColor = ColorTranslator.FromHtml("#4B5563");
            lblCashLabel.BackColor = Color.Transparent;

            txtCash.Font = new Font("Segoe UI", 14F);
            txtCash.BorderRadius = 8;
            txtCash.BorderColor = DlgBorder;
            txtCash.FocusedState.BorderColor = DlgGreen;
            txtCash.TextAlign = HorizontalAlignment.Right;
            txtCash.ForeColor = ColorTranslator.FromHtml("#6B7280");

            lblChangeLabel.Font = new Font("Segoe UI", 11F);
            lblChangeLabel.ForeColor = ColorTranslator.FromHtml("#4B5563");
            lblChangeLabel.BackColor = Color.Transparent;

            lblChange.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblChange.ForeColor = ColorTranslator.FromHtml("#EF4444");
            lblChange.BackColor = Color.Transparent;

            lblValidation.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblValidation.ForeColor = ColorTranslator.FromHtml("#EF4444");
            lblValidation.BackColor = Color.Transparent;

            // Cancel button
            btnCancel.FillColor = Color.White;
            btnCancel.HoverState.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            btnCancel.ForeColor = ColorTranslator.FromHtml("#4B5563");
            btnCancel.BorderRadius = 10;
            btnCancel.Font = new Font("Segoe UI", 11F);
            btnCancel.BorderThickness = 1;
            btnCancel.BorderColor = DlgBorder;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            // Confirm button
            btnConfirm.FillColor = DlgGreen;
            btnConfirm.HoverState.FillColor = DlgGreenHover;
            btnConfirm.ForeColor = Color.White;
            btnConfirm.BorderRadius = 10;
            btnConfirm.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnConfirm.BorderThickness = 0;
            btnConfirm.DisabledState.FillColor = ColorTranslator.FromHtml("#D1D5DB");
            btnConfirm.DisabledState.ForeColor = Color.White;
            btnConfirm.DisabledState.BorderColor = Color.Transparent;
        }

        private void StyleCard(Guna.UI2.WinForms.Guna2Panel card)
        {
            card.BackColor = Color.Transparent;
            card.FillColor = Color.White;
            card.BorderRadius = 12;
            card.BorderThickness = 1;
            card.BorderColor = DlgBorder;
            card.ShadowDecoration.Enabled = true;
            card.ShadowDecoration.Shadow = new Padding(2, 2, 5, 5);
            card.ShadowDecoration.Color = Color.FromArgb(10, 0, 0, 0);
            card.ShadowDecoration.BorderRadius = 12;
        }

        private void StyleTextField(Guna.UI2.WinForms.Guna2TextBox txt)
        {
            txt.Font = new Font("Segoe UI", 10F);
            txt.BorderRadius = 8;
            txt.BorderColor = DlgBorder;
            txt.FocusedState.BorderColor = DlgGreen;
        }

        private void StyleTypeButton(Guna.UI2.WinForms.Guna2Button btn, bool active)
        {
            btn.BorderRadius = 8;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.BorderThickness = active ? 2 : 1;
            btn.BorderColor = active ? DlgGreen : DlgBorder;
            btn.FillColor = active ? ColorTranslator.FromHtml("#F2FAEF") : Color.White;
            btn.ForeColor = active ? DlgGreen : DlgText;
        }

        private void SetOrderType(bool dineIn)
        {
            _isDineIn = dineIn;
            StyleTypeButton(btnDineIn, dineIn);
            StyleTypeButton(btnTakeOut, !dineIn);
        }
    }
}
