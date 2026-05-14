using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class ProductItemCard
    {
        private static readonly Color CardGreen = ColorTranslator.FromHtml("#52B743");
        private static readonly Color CardBorder = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color CardTextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color CardTextBody = ColorTranslator.FromHtml("#374151");
        private static readonly Color CardTextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color CardTextGray = ColorTranslator.FromHtml("#6B7280");

        private void InitializeDesign()
        {
            this.BackColor = Color.Transparent;

            // Card panel
            pnlCard.BackColor = Color.Transparent;
            pnlCard.FillColor = Color.White;
            pnlCard.BorderRadius = 8;
            pnlCard.BorderColor = CardBorder;
            pnlCard.BorderThickness = 1;
            pnlCard.ShadowDecoration.Enabled = false;

            // Image placeholder
            picImage.BackColor = Color.FromArgb(249, 250, 251);
            picImage.Paint += PicImage_Paint;

            // Product ID
            lblProductId.Font = new Font("Segoe UI", 8F);
            lblProductId.ForeColor = CardTextMuted;
            lblProductId.BackColor = Color.Transparent;

            // Price
            lblPrice.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPrice.ForeColor = CardTextPrimary;
            lblPrice.BackColor = Color.Transparent;

            // Name
            lblName.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblName.ForeColor = CardTextPrimary;
            lblName.BackColor = Color.Transparent;

            // Separator
            pnlSeparator.BackColor = CardBorder;

            // Edit button
            btnEdit.FillColor = Color.Transparent;
            btnEdit.ForeColor = CardTextPrimary;
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            btnEdit.BorderRadius = 4;
            btnEdit.BorderColor = CardBorder;
            btnEdit.BorderThickness = 0;
            btnEdit.HoverState.FillColor = Color.FromArgb(249, 250, 251);
            btnEdit.Click += (s, e) => EditClicked?.Invoke(this, EventArgs.Empty);

            // Delete button
            btnDelete.FillColor = Color.Transparent;
            btnDelete.ForeColor = CardTextMuted;
            btnDelete.Font = new Font("Segoe UI", 10F);
            btnDelete.BorderRadius = 4;
            btnDelete.BorderColor = CardBorder;
            btnDelete.BorderThickness = 0;
            btnDelete.HoverState.FillColor = Color.FromArgb(254, 242, 242);
            btnDelete.HoverState.ForeColor = ColorTranslator.FromHtml("#EF4444");
            btnDelete.Click += (s, e) => DeleteClicked?.Invoke(this, EventArgs.Empty);
        }

        private void PicImage_Paint(object? sender, PaintEventArgs e)
        {
            if (picImage.Image != null) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var pen = new Pen(Color.FromArgb(120, 156, 163, 175), 2f);
            int ix = picImage.Width / 2 - 20;
            int iy = picImage.Height / 2 - 20;
            g.DrawRectangle(pen, ix, iy, 40, 40);
            g.DrawLine(pen, ix, iy + 30, ix + 15, iy + 15);
            g.DrawLine(pen, ix + 15, iy + 15, ix + 25, iy + 25);
            g.DrawLine(pen, ix + 25, iy + 25, ix + 35, iy + 10);
            g.DrawLine(pen, ix + 35, iy + 10, ix + 40, iy + 30);
            g.DrawEllipse(pen, ix + 28, iy + 5, 8, 8);
        }
    }
}
