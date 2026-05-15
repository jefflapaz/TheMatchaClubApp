using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace TheMatchaClubApp.Forms
{
    public partial class CartItemRow
    {
        private static readonly Color TextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color TextSecondary = ColorTranslator.FromHtml("#374151");
        private static readonly Color TextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color BorderColor = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color Green = ColorTranslator.FromHtml("#52B743");
        private static readonly Color Red = ColorTranslator.FromHtml("#EF4444");

        private void InitializeDesign()
        {
            this.BackColor = Color.White;
            this.Margin = new Padding(0, 0, 0, 4);

            pnlContainer.BackColor = Color.White;
            pnlContainer.FillColor = Color.White;
            pnlContainer.BorderThickness = 0;
            pnlContainer.ShadowDecoration.Enabled = false;

            // ── Quantity Controls ──
            StyleQtyButton(btnMinus, "\u2212"); // Minus
            btnMinus.HoverState.FillColor = ColorTranslator.FromHtml("#FEE2E2");
            btnMinus.HoverState.ForeColor = Red;

            StyleQtyButton(btnPlus, "+");
            btnPlus.HoverState.FillColor = ColorTranslator.FromHtml("#D1FAE5");
            btnPlus.HoverState.ForeColor = Green;

            txtQty.BackColor = Color.White;
            txtQty.FillColor = Color.White;
            txtQty.BorderThickness = 0;
            txtQty.Font = new Font("Segoe UI Semibold", 10F);
            txtQty.ForeColor = TextPrimary;
            txtQty.FocusedState.BorderColor = Color.Transparent;
            txtQty.FocusedState.FillColor = Color.White;
            txtQty.ShadowDecoration.Enabled = false;

            // ── Item Info ──
            lblName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblName.ForeColor = TextSecondary;
            lblName.BackColor = Color.Transparent;
            lblName.AutoEllipsis = true;

            lblPrice.Font = new Font("Segoe UI", 7.5F);
            lblPrice.ForeColor = TextMuted;
            lblPrice.BackColor = Color.Transparent;

            // ── Total ──
            lblTotal.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblTotal.ForeColor = TextPrimary;
            lblTotal.BackColor = Color.Transparent;

            // ── Remove Button ──
            btnRemove.FillColor = Color.Transparent;
            btnRemove.ForeColor = TextMuted;
            btnRemove.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnRemove.BorderRadius = 15;
            btnRemove.BorderThickness = 0;
            btnRemove.ShadowDecoration.Enabled = false;
            btnRemove.HoverState.FillColor = ColorTranslator.FromHtml("#FEE2E2");
            btnRemove.HoverState.ForeColor = Red;
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.PressedColor = Color.FromArgb(40, Red);

            // Responsive alignment helper
            this.Resize += (s, e) => LayoutControls();
            LayoutControls();
        }

        private void StyleQtyButton(Guna2Button btn, string text)
        {
            btn.Text = text;
            btn.Size = new Size(28, 28);
            btn.BorderRadius = 8;
            btn.FillColor = ColorTranslator.FromHtml("#F9FAFB");
            btn.ForeColor = TextSecondary;
            btn.BorderColor = BorderColor;
            btn.BorderThickness = 1;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.ShadowDecoration.Enabled = false;
            btn.Animated = false; // Disable animation to prevent rendering glitches
        }

        private void LayoutControls()
        {
            int w = this.Width;
            if (w < 100) return;

            int totalW = 80;
            int removeW = 32;
            int qtyAreaW = 115;
            int infoW = w - qtyAreaW - totalW - removeW - 16;

            lblName.Width = Math.Max(40, infoW);
            lblPrice.Width = Math.Max(40, infoW);
            
            btnRemove.Left = w - totalW - removeW - 4;
            lblTotal.Left = w - totalW - 4;
            lblTotal.Width = totalW;
        }
    }
}
