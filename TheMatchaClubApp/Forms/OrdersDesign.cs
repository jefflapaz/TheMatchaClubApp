using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class OrdersView
    {
        private static readonly Color OBgColor = ColorTranslator.FromHtml("#FAFAFA");
        private static readonly Color OCardBg = Color.White;
        private static readonly Color OBorderLight = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color OTextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color OTextSecondary = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color OTextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color OGreen = ColorTranslator.FromHtml("#52B743");
        private static readonly Color OGreenBg = ColorTranslator.FromHtml("#F2FAEF");

        private void InitializeDesign()
        {
            this.BackColor = OBgColor;
            this.Dock = DockStyle.Fill;

            // Top header
            pnlTopHeader.BackColor = OCardBg;
            pnlTopHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(OBorderLight, 1);
                e.Graphics.DrawLine(pen, 0, pnlTopHeader.Height - 1, pnlTopHeader.Width, pnlTopHeader.Height - 1);
            };
            lblChevron.Font = new Font("Segoe UI", 8F);
            lblChevron.ForeColor = OTextMuted;
            lblChevron.BackColor = Color.Transparent;
            lblViewName.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblViewName.ForeColor = OTextPrimary;
            lblViewName.BackColor = Color.Transparent;

            btnNewOrder.FillColor = OGreen;
            btnNewOrder.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnNewOrder.ForeColor = Color.White;
            btnNewOrder.BorderRadius = 8;
            btnNewOrder.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnNewOrder.BorderThickness = 0;

            // Filter bar
            pnlFilterBar.BackColor = OCardBg;
            pnlFilterBar.Paint += (s, e) =>
            {
                using var pen = new Pen(OBorderLight, 1);
                e.Graphics.DrawLine(pen, 0, pnlFilterBar.Height - 1, pnlFilterBar.Width, pnlFilterBar.Height - 1);
            };

            txtSearch.BorderRadius = 8;
            txtSearch.BorderColor = OBorderLight;
            txtSearch.FillColor = Color.FromArgb(249, 250, 251);
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.PlaceholderForeColor = OTextMuted;

            StyleFilterPill(btnFilterAll, true);
            StyleFilterPill(btnFilterDineIn, false);
            StyleFilterPill(btnFilterTakeaway, false);

            lblDateDisplay.Font = new Font("Segoe UI", 8F);
            lblDateDisplay.ForeColor = OTextSecondary;
            lblDateDisplay.BackColor = Color.Transparent;

            btnExport.FillColor = OCardBg;
            btnExport.ForeColor = OTextSecondary;
            btnExport.BorderColor = OBorderLight;
            btnExport.BorderRadius = 8;
            btnExport.BorderThickness = 1;
            btnExport.Font = new Font("Segoe UI", 8F);

            // Pagination
            pnlPagination.BackColor = OCardBg;
            pnlPagination.Paint += (s, e) =>
            {
                using var pen = new Pen(OBorderLight, 1);
                e.Graphics.DrawLine(pen, 0, 0, pnlPagination.Width, 0);
            };
            lblPaginationInfo.Font = new Font("Segoe UI", 9F);
            lblPaginationInfo.ForeColor = OTextSecondary;
            lblPaginationInfo.BackColor = Color.Transparent;

            // dgvOrders Styling
            dgvOrders.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvOrders.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvOrders.ThemeStyle.RowsStyle.ForeColor = OTextSecondary;
            dgvOrders.ThemeStyle.RowsStyle.SelectionBackColor = OGreenBg;
            dgvOrders.ThemeStyle.RowsStyle.SelectionForeColor = OTextPrimary;
            dgvOrders.ThemeStyle.HeaderStyle.BackColor = OBgColor;
            dgvOrders.ThemeStyle.HeaderStyle.ForeColor = OTextMuted;
            dgvOrders.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dgvOrders.GridColor = OBorderLight;
            dgvOrders.RowTemplate.Height = 52;
            dgvOrders.BorderStyle = BorderStyle.None;
            dgvOrders.BackgroundColor = OCardBg;

            // Receipt sidebar
            pnlReceiptSidebar.BackColor = Color.Transparent;
            pnlReceiptSidebar.FillColor = OCardBg;
            pnlReceiptSidebar.BorderThickness = 0;
            pnlReceiptSidebar.ShadowDecoration.Enabled = false;
            pnlReceiptSidebar.Paint += (s, e) =>
            {
                using var pen = new Pen(OBorderLight, 1);
                e.Graphics.DrawLine(pen, 0, 0, 0, pnlReceiptSidebar.Height);
            };

            // Receipt header
            pnlReceiptHeader.BackColor = OCardBg;
            pnlReceiptHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#F3F4F6"), 1);
                e.Graphics.DrawLine(pen, 0, pnlReceiptHeader.Height - 1, pnlReceiptHeader.Width, pnlReceiptHeader.Height - 1);
            };
            lblReceiptTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblReceiptTitle.ForeColor = OGreen;
            lblReceiptTitle.BackColor = Color.Transparent;

            btnCloseReceipt.FillColor = Color.Transparent;
            btnCloseReceipt.ForeColor = OTextMuted;
            btnCloseReceipt.BorderThickness = 0;
            btnCloseReceipt.Font = new Font("Segoe UI", 10F);

            // Receipt body
            pnlReceiptBody.BackColor = OCardBg;
            pnlReceiptBody.Paint += PnlReceiptBody_Paint;

            lblStoreName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStoreName.ForeColor = OTextPrimary;
            lblStoreName.BackColor = Color.Transparent;
            lblStoreName.TextAlign = ContentAlignment.MiddleCenter;

            lblStoreAddress.Font = new Font("Segoe UI", 8F);
            lblStoreAddress.ForeColor = OTextSecondary;
            lblStoreAddress.BackColor = Color.Transparent;
            lblStoreAddress.TextAlign = ContentAlignment.MiddleCenter;

            StyleReceiptLabel(lblReceiptOrderIdLabel);
            StyleReceiptValue(lblReceiptOrderId);
            StyleReceiptLabel(lblReceiptDateLabel);
            StyleReceiptValue(lblReceiptDate);
            StyleReceiptLabel(lblReceiptCustomerLabel);
            StyleReceiptValue(lblReceiptCustomer);

            lblReceiptItems.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblReceiptItems.ForeColor = OTextPrimary;
            lblReceiptItems.BackColor = Color.Transparent;

            StyleReceiptLabel(lblReceiptSubtotalLabel);
            StyleReceiptValue(lblReceiptSubtotal);
            StyleReceiptLabel(lblReceiptTaxLabel);
            StyleReceiptValue(lblReceiptTax);

            lblReceiptTotalLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblReceiptTotalLabel.ForeColor = OTextPrimary;
            lblReceiptTotalLabel.BackColor = Color.Transparent;
            lblReceiptTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblReceiptTotal.ForeColor = OGreen;
            lblReceiptTotal.BackColor = Color.Transparent;

            lblPaidVia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPaidVia.ForeColor = OTextSecondary;
            lblPaidVia.BackColor = Color.FromArgb(249, 250, 251);

            lblThankYou.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblThankYou.ForeColor = OTextMuted;
            lblThankYou.BackColor = Color.Transparent;

            // Reprint / Email buttons
            btnReprint.FillColor = OGreen;
            btnReprint.ForeColor = Color.White;
            btnReprint.BorderRadius = 8;
            btnReprint.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnReprint.BorderThickness = 0;

            btnEmailReceipt.FillColor = OCardBg;
            btnEmailReceipt.ForeColor = OTextSecondary;
            btnEmailReceipt.BorderColor = OBorderLight;
            btnEmailReceipt.BorderRadius = 8;
            btnEmailReceipt.BorderThickness = 1;
            btnEmailReceipt.Font = new Font("Segoe UI", 9F);
        }

        private void StyleFilterPill(Guna.UI2.WinForms.Guna2Button btn, bool active)
        {
            btn.BorderRadius = 8;
            btn.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btn.BorderThickness = active ? 0 : 1;
            btn.FillColor = active ? Color.FromArgb(243, 244, 246) : OCardBg;
            btn.ForeColor = active ? OTextPrimary : OTextSecondary;
            btn.BorderColor = OBorderLight;
        }

        private void StyleReceiptLabel(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 8F);
            lbl.ForeColor = OTextSecondary;
            lbl.BackColor = Color.Transparent;
        }

        private void StyleReceiptValue(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 8F);
            lbl.ForeColor = OTextPrimary;
            lbl.BackColor = Color.Transparent;
        }

        private void PnlReceiptBody_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Store logo circle
            using var logoBrush = new SolidBrush(OGreenBg);
            g.FillEllipse(logoBrush, 138, 20, 44, 44); // Centered (320 width / 2 = 160 - 22)
            using var leafFont = new Font("Segoe UI", 16F);
            using var greenBrush = new SolidBrush(OGreen);
            g.DrawString("\U0001F375", leafFont, greenBrush, 145, 26);

            // Dashed separators
            using var dashPen = new Pen(ColorTranslator.FromHtml("#D1D5DB"), 1);
            dashPen.DashStyle = DashStyle.Dash;
            
            // Above Order ID
            g.DrawLine(dashPen, 16, lblReceiptOrderIdLabel.Top - 10, 304, lblReceiptOrderIdLabel.Top - 10);
            
            // Below Station / Customer
            int headerBottom = lblReceiptCustomerLabel.Bottom + 10;
            g.DrawLine(dashPen, 16, headerBottom, 304, headerBottom);

            // Solid line above Subtotal
            using var solidPen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
            g.DrawLine(solidPen, 16, lblReceiptSubtotalLabel.Top - 10, 304, lblReceiptSubtotalLabel.Top - 10);
        }
    }
}
