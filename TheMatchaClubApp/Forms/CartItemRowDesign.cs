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
            this.Margin = new Padding(0, 0, 0, 2);

            pnlContainer.BackColor = Color.White;
            pnlContainer.FillColor = Color.White;
            pnlContainer.BorderThickness = 0;
            pnlContainer.ShadowDecoration.Enabled = false;

            // ── Quantity Controls (white circles with dynamic icons) ──
            StyleQtyButton(btnMinus);
            btnMinus.Image = CreateMinusImage(TextSecondary);
            btnMinus.HoverState.Image = CreateMinusImage(Red);
            btnMinus.HoverState.FillColor = ColorTranslator.FromHtml("#FEE2E2");
            btnMinus.HoverState.BorderColor = Red;
            btnMinus.PressedColor = Color.FromArgb(40, Red);

            StyleQtyButton(btnPlus);
            btnPlus.Image = CreatePlusImage(TextSecondary);
            btnPlus.HoverState.Image = CreatePlusImage(Green);
            btnPlus.HoverState.FillColor = ColorTranslator.FromHtml("#D1FAE5");
            btnPlus.HoverState.BorderColor = Green;
            btnPlus.PressedColor = Color.FromArgb(40, Green);

            this.Disposed += (s, e) =>
            {
                btnMinus.Image?.Dispose();
                btnMinus.HoverState.Image?.Dispose();
                btnPlus.Image?.Dispose();
                btnPlus.HoverState.Image?.Dispose();
            };

            txtQty.BackColor = Color.White;
            txtQty.FillColor = Color.White;
            txtQty.BorderThickness = 0;
            txtQty.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            txtQty.ForeColor = TextPrimary;
            txtQty.FocusedState.BorderColor = Color.Transparent;
            txtQty.FocusedState.FillColor = Color.White;
            txtQty.ShadowDecoration.Enabled = false;

            // ── Item Info ──
            lblName.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblName.ForeColor = TextSecondary;
            lblName.BackColor = Color.Transparent;
            lblName.AutoEllipsis = true;

            lblPrice.Font = new Font("Segoe UI", 7.5F);
            lblPrice.ForeColor = TextMuted;
            lblPrice.BackColor = Color.Transparent;

            // ── Total ──
            lblTotal.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblTotal.ForeColor = Green;
            lblTotal.BackColor = Color.Transparent;

            // ── Remove Button ──
            btnRemove.FillColor = Color.Transparent;
            btnRemove.ForeColor = TextMuted;
            btnRemove.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRemove.BorderRadius = 14;
            btnRemove.BorderThickness = 0;
            btnRemove.ShadowDecoration.Enabled = false;
            btnRemove.HoverState.FillColor = ColorTranslator.FromHtml("#FEE2E2");
            btnRemove.HoverState.ForeColor = Red;
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.PressedColor = Color.FromArgb(40, Red);
            btnRemove.Text = "✕";

            // Responsive alignment
            this.Resize += (s, e) => LayoutControls();
            LayoutControls();
        }

        private void StyleQtyButton(Guna2Button btn)
        {
            btn.Text = "";
            btn.Size = new Size(28, 28);
            btn.BorderRadius = 14;
            btn.FillColor = Color.White;
            btn.BorderColor = BorderColor;
            btn.BorderThickness = 1;
            btn.Cursor = Cursors.Hand;
            btn.ShadowDecoration.Enabled = false;
            btn.Animated = false;
        }

        private void LayoutControls()
        {
            int w = this.Width;
            if (w < 100) return;

            int totalW = 80;
            int removeW = 30;
            int qtyAreaW = 120; // minus(30) + qty(48) + plus(30) + padding

            // Position and size quantity control elements
            btnMinus.Size = new Size(28, 28);
            btnMinus.Location = new Point(8, 12);

            txtQty.Size = new Size(40, 28);
            txtQty.Location = new Point(36, 12);

            btnPlus.Size = new Size(28, 28);
            btnPlus.Location = new Point(76, 12);

            // Info labels fill available space
            int infoX = qtyAreaW + 4;
            int infoW = Math.Max(40, w - qtyAreaW - totalW - removeW - 16);
            lblName.Location = new Point(infoX, 6);
            lblName.Width = infoW;
            lblPrice.Location = new Point(infoX, 28);
            lblPrice.Width = infoW;

            // Remove + Total anchored to right
            btnRemove.Location = new Point(w - totalW - removeW - 4, 11);
            btnRemove.Size = new Size(removeW, 30);
            lblTotal.Location = new Point(w - totalW, 0);
            lblTotal.Size = new Size(totalW, 52);
        }

        private static Image CreateMinusImage(Color color)
        {
            var bmp = new Bitmap(12, 12);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var pen = new Pen(color, 2f);
                g.DrawLine(pen, 2, 6, 10, 6);
            }
            return bmp;
        }

        private static Image CreatePlusImage(Color color)
        {
            var bmp = new Bitmap(12, 12);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var pen = new Pen(color, 2f);
                g.DrawLine(pen, 2, 6, 10, 6);
                g.DrawLine(pen, 6, 2, 6, 10);
            }
            return bmp;
        }
    }
}
