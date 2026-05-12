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

        private Guna2Button[] _categoryButtons = Array.Empty<Guna2Button>();

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

            string[] cats = { "All", "Matcha", "Tea", "Snacks", "Seasonal" };
            _categoryButtons = new Guna2Button[cats.Length];
            for (int i = 0; i < cats.Length; i++)
            {
                var btn = new Guna2Button
                {
                    Text = cats[i],
                    Tag = cats[i],
                    Size = new Size(80, 32),
                    Margin = new Padding(4, 0, 4, 0),
                    BorderRadius = 20,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    BorderThickness = 1,
                    Cursor = Cursors.Hand
                };
                btn.Click += CategoryFilter_Click;
                _categoryButtons[i] = btn;
                flpCategories.Controls.Add(btn);
            }
            UpdateCategoryPills();

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

            btnEatIn.FillColor = Color.White;
            btnEatIn.ForeColor = TextPrimary;
            btnEatIn.BorderColor = BorderLight;
            btnEatIn.BorderRadius = 8;
            btnEatIn.BorderThickness = 1;
            btnEatIn.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);

            // Cart items
            pnlCartItems.BackColor = CardBg;

            // Cart totals
            pnlCartTotals.BackColor = CardBg;
            pnlCartTotals.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#F3F4F6"), 1);
                e.Graphics.DrawLine(pen, 16, 0, pnlCartTotals.Width - 16, 0);
                // Separator before total
                e.Graphics.DrawLine(pen, 16, 52, pnlCartTotals.Width - 16, 52);
            };

            lblSubtotal.Font = new Font("Segoe UI", 9F);
            lblSubtotal.ForeColor = TextSecondary;
            lblSubtotal.BackColor = Color.Transparent;
            lblSubtotalValue.Font = new Font("Segoe UI", 9F);
            lblSubtotalValue.ForeColor = TextSecondary;
            lblSubtotalValue.BackColor = Color.Transparent;

            lblTax.Font = new Font("Segoe UI", 9F);
            lblTax.ForeColor = TextSecondary;
            lblTax.BackColor = Color.Transparent;
            lblTaxValue.Font = new Font("Segoe UI", 9F);
            lblTaxValue.ForeColor = TextSecondary;
            lblTaxValue.BackColor = Color.Transparent;

            lblTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotal.ForeColor = TextPrimary;
            lblTotal.BackColor = Color.Transparent;
            lblTotalValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTotalValue.ForeColor = Green;
            lblTotalValue.BackColor = Color.Transparent;

            // Print / Email buttons
            StyleActionButton(btnPrint);
            StyleActionButton(btnEmail);

            // Complete sale button
            btnCompleteSale.BorderRadius = 12;
            btnCompleteSale.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            btnCompleteSale.ForeColor = ColorTranslator.FromHtml("#D1D5DB");
            btnCompleteSale.HoverState.FillColor = GreenHover;
            btnCompleteSale.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCompleteSale.BorderThickness = 0;
            btnCompleteSale.Enabled = false;

            lblCashNote.Font = new Font("Segoe UI", 8F);
            lblCashNote.ForeColor = TextMuted;
            lblCashNote.BackColor = Color.Transparent;

            // Initialize cart UI
            RefreshCartUI();
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
