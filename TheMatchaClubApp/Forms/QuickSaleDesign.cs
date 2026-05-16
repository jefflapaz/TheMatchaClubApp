using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace TheMatchaClubApp.Forms
{
    public partial class QuickSaleView
    {
        private static readonly Color BgColor = ColorTranslator.FromHtml("#FAFAFA");
        private static readonly Color CardBg = Color.White;
        private static readonly Color BorderLight = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color TextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color TextSecondary = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color TextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color Green = ColorTranslator.FromHtml("#52B743");
        private static readonly Color GreenHover = ColorTranslator.FromHtml("#86CD77");

        private System.Collections.Generic.List<Guna2Button> _categoryButtons = new();

        private void InitializeDesign()
        {
            this.BackColor = BgColor;
            this.Dock = DockStyle.Fill;

            // ── Top Header ──
            pnlTopHeader.BackColor = CardBg;
            pnlTopHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderLight, 1);
                e.Graphics.DrawLine(pen, 0, pnlTopHeader.Height - 1, pnlTopHeader.Width, pnlTopHeader.Height - 1);
            };

            lblChevron.Font = new Font("Segoe UI", 8F);
            lblChevron.ForeColor = TextMuted;
            lblChevron.BackColor = Color.Transparent;

            lblViewName.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblViewName.ForeColor = TextPrimary;
            lblViewName.BackColor = Color.Transparent;

            txtSearch.BorderRadius = 8;
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.BorderColor = BorderLight;
            txtSearch.FocusedState.BorderColor = Green;
            txtSearch.IconLeft = null; // No icon for simplicity unless we have one
            txtSearch.TextOffset = new Point(4, 0);

            btnAlert.FillColor = Color.Transparent;
            btnAlert.ForeColor = TextSecondary;
            btnAlert.BorderThickness = 0;
            btnAlert.Font = new Font("Segoe UI", 12F);
            btnAlert.HoverState.FillColor = ColorTranslator.FromHtml("#F3F4F6");

            // ── Category Row ──
            pnlCategoryRow.BackColor = CardBg;
            pnlCategoryRow.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderLight, 1);
                e.Graphics.DrawLine(pen, 0, pnlCategoryRow.Height - 1, pnlCategoryRow.Width, pnlCategoryRow.Height - 1);
            };

            // Category arrow buttons
            StyleCategoryArrow(btnCatLeft);
            StyleCategoryArrow(btnCatRight);

            // Scroll container
            pnlCategoryScroll.BackColor = CardBg;
            flpCategories.BackColor = CardBg;

            // Category buttons are populated dynamically in QuickSaleView.cs -> PopulateCategories()

            // ── Product Grid ──
            pnlProductGrid.BackColor = Color.Transparent;
            pnlProductGrid.FillColor = BgColor;
            pnlProductGrid.BorderThickness = 0;
            pnlProductGrid.ShadowDecoration.Enabled = false;
            flpProducts.BackColor = BgColor;

            // ── Cart Sidebar ──
            pnlCartSidebar.BackColor = Color.Transparent;
            pnlCartSidebar.FillColor = CardBg;
            pnlCartSidebar.BorderThickness = 0;
            pnlCartSidebar.ShadowDecoration.Enabled = false;
            pnlCartSidebar.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderLight, 1);
                e.Graphics.DrawLine(pen, 0, 0, 0, pnlCartSidebar.Height);
            };

            // Cart header
            pnlCartHeader.BackColor = CardBg;
            pnlCartHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderLight, 1);
                e.Graphics.DrawLine(pen, 0, pnlCartHeader.Height - 1, pnlCartHeader.Width, pnlCartHeader.Height - 1);
            };

            lblCurrentOrder.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCurrentOrder.ForeColor = TextPrimary;
            lblCurrentOrder.BackColor = Color.Transparent;

            lblOrderMeta.Font = new Font("Segoe UI", 8F);
            lblOrderMeta.ForeColor = TextSecondary;
            lblOrderMeta.BackColor = Color.Transparent;

            // Cart items
            pnlCartItems.BackColor = CardBg;
            pnlCartItems.Padding = new Padding(8, 4, 8, 4);

            // Cart totals
            pnlCartTotals.BackColor = CardBg;
            pnlCartTotals.Padding = new Padding(16, 12, 16, 12);
            pnlCartTotals.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderLight, 1);
                // Top separator
                e.Graphics.DrawLine(pen, 16, 0, pnlCartTotals.Width - 16, 0);
                // Separator before total
                e.Graphics.DrawLine(pen, 16, 44, pnlCartTotals.Width - 16, 44);
            };

            lblSubtotal.Font = new Font("Segoe UI", 9F);
            lblSubtotal.ForeColor = TextSecondary;
            lblSubtotalValue.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblSubtotalValue.ForeColor = TextPrimary;

            lblTotal.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblTotal.ForeColor = TextPrimary;
            lblTotalValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotalValue.ForeColor = Green;

            // Complete sale button
            btnCompleteSale.BorderRadius = 10;
            btnCompleteSale.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            btnCompleteSale.ForeColor = ColorTranslator.FromHtml("#D1D5DB");
            btnCompleteSale.HoverState.FillColor = Green;
            btnCompleteSale.HoverState.ForeColor = Color.White;
            btnCompleteSale.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCompleteSale.ShadowDecoration.Enabled = true;
            btnCompleteSale.ShadowDecoration.Color = Color.FromArgb(20, 0, 0, 0);
            btnCompleteSale.ShadowDecoration.Depth = 10;

            lblCashNote.Font = new Font("Segoe UI Semibold", 8F);
            lblCashNote.ForeColor = TextMuted;

            // Clear Cart Button styling
            btnClearCart.FillColor = Color.Transparent;
            btnClearCart.ForeColor = ColorTranslator.FromHtml("#EF4444");
            btnClearCart.BorderThickness = 0;
            btnClearCart.Font = new Font("Segoe UI Semibold", 9F);
            btnClearCart.HoverState.FillColor = Color.FromArgb(20, 239, 68, 68); // Very light red
            btnClearCart.Cursor = Cursors.Hand;

            // Initialize cart UI
            RefreshCartUI();

            // ── Session Overlay Styling ──
            pnlSessionOverlay.FillColor = Color.FromArgb(210, 255, 255, 255);
            pnlSessionOverlay.BorderThickness = 0;
            
            lblSessionWarning.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblSessionWarning.ForeColor = TextPrimary;
            lblSessionWarning.BackColor = Color.Transparent;
            
            btnQuickOpenSession.FillColor = Green;
            btnQuickOpenSession.ForeColor = Color.White;
            btnQuickOpenSession.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnQuickOpenSession.HoverState.FillColor = GreenHover;
            btnQuickOpenSession.ShadowDecoration.Enabled = true;
            btnQuickOpenSession.ShadowDecoration.Color = Color.FromArgb(30, 0, 0, 0);
            btnQuickOpenSession.Cursor = Cursors.Hand;
        }

        private void StyleActionButton(Guna2Button btn)
        {
            btn.FillColor = Color.White;
            btn.ForeColor = TextSecondary;
            btn.BorderColor = BorderLight;
            btn.BorderRadius = 8;
            btn.BorderThickness = 1;
            btn.Font = new Font("Segoe UI", 9F);
            btn.HoverState.FillColor = ColorTranslator.FromHtml("#F9FAFB");
        }

        private void StyleCategoryArrow(Guna2Button btn)
        {
            btn.FillColor = CardBg;
            btn.ForeColor = TextSecondary;
            btn.BorderThickness = 0;
            btn.BorderRadius = 0;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.HoverState.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            btn.Cursor = Cursors.Hand;
        }

        private void UpdateCategoryPills()
        {
            foreach (var btn in _categoryButtons)
            {
                bool active = (btn.Tag?.ToString() ?? "All") == _activeCategory;
                if (active)
                {
                    btn.FillColor = Green;
                    btn.ForeColor = Color.White;
                    btn.BorderColor = Green;
                }
                else
                {
                    btn.FillColor = Color.White;
                    btn.ForeColor = TextSecondary;
                    btn.BorderColor = BorderLight;
                }
            }
        }
    }
}
