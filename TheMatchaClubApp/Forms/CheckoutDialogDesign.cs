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
            this.BackColor = DlgBg;

            // Header
            pnlHeader.BackColor = DlgBg;
            pnlHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(DlgBorder, 1);
                e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = DlgText;
            lblTitle.BackColor = Color.Transparent;

            btnClose.FillColor = Color.Transparent;
            btnClose.ForeColor = DlgMuted;
            btnClose.BorderThickness = 0;
            btnClose.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnClose.HoverState.FillColor = ColorTranslator.FromHtml("#FEE2E2");
            btnClose.HoverState.ForeColor = ColorTranslator.FromHtml("#EF4444");
            btnClose.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            // Order Type Labels
            lblOrderTypeLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblOrderTypeLabel.ForeColor = DlgMuted;
            lblOrderTypeLabel.BackColor = Color.Transparent;

            // Dine-In / Take-Out buttons
            StyleTypeButton(btnDineIn, true);
            StyleTypeButton(btnTakeOut, false);

            btnDineIn.Click += (s, e) => { SetOrderType(true); };
            btnTakeOut.Click += (s, e) => { SetOrderType(false); };

            // Customer section
            lblCustomerLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblCustomerLabel.ForeColor = DlgMuted;
            lblCustomerLabel.BackColor = Color.Transparent;

            cboCustomer.Font = new Font("Segoe UI", 10F);
            cboCustomer.BorderRadius = 8;
            cboCustomer.BorderColor = DlgBorder;
            cboCustomer.FocusedState.BorderColor = DlgGreen;

            lblNewNameLabel.Font = new Font("Segoe UI", 8F);
            lblNewNameLabel.ForeColor = DlgMuted;
            lblNewNameLabel.BackColor = Color.Transparent;

            txtNewName.Font = new Font("Segoe UI", 10F);
            txtNewName.BorderRadius = 8;
            txtNewName.BorderColor = DlgBorder;
            txtNewName.FocusedState.BorderColor = DlgGreen;

            txtNewEmail.Font = new Font("Segoe UI", 10F);
            txtNewEmail.BorderRadius = 8;
            txtNewEmail.BorderColor = DlgBorder;
            txtNewEmail.FocusedState.BorderColor = DlgGreen;

            // Confirm button
            btnConfirm.FillColor = DlgGreen;
            btnConfirm.HoverState.FillColor = DlgGreenHover;
            btnConfirm.ForeColor = Color.White;
            btnConfirm.BorderRadius = 10;
            btnConfirm.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnConfirm.BorderThickness = 0;
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
