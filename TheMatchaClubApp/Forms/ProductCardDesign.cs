using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class ProductCard
    {
        private static readonly Color Green = ColorTranslator.FromHtml("#52B743");
        private static readonly Color BorderColor = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color TextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color TextMuted = ColorTranslator.FromHtml("#9CA3AF");

        private System.Windows.Forms.Timer _hoverTimer = new System.Windows.Forms.Timer { Interval = 16 };
        private float _shadowTarget = 6f, _shadowCurrent = 6f;

        private void InitializeDesign()
        {
            this.BackColor = Color.Transparent;
            this.Cursor = Cursors.Hand;

            // Card panel
            pnlCard.BackColor = Color.Transparent;
            pnlCard.FillColor = Color.White;
            pnlCard.BorderRadius = 12;
            pnlCard.BorderColor = BorderColor;
            pnlCard.BorderThickness = 1;
            pnlCard.ShadowDecoration.Enabled = true;
            pnlCard.ShadowDecoration.Color = Color.FromArgb(6, 0, 0, 0);
            pnlCard.ShadowDecoration.Depth = 6;

            // Image
            picImage.BackColor = Color.FromArgb(249, 250, 251);
            picImage.SizeMode = PictureBoxSizeMode.Zoom;
            picImage.Paint += PicImage_Paint;

            // Price badge
            pnlPriceBadge.BackColor = Color.Transparent;
            pnlPriceBadge.FillColor = Green;
            pnlPriceBadge.BorderRadius = 6;
            pnlPriceBadge.BorderThickness = 0;
            pnlPriceBadge.ShadowDecoration.Enabled = false;
            pnlPriceBadge.BringToFront();

            lblPrice.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblPrice.ForeColor = Color.White;
            lblPrice.BackColor = Color.Transparent;
            lblPrice.TextAlign = ContentAlignment.MiddleCenter;

            // Product name
            lblName.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblName.ForeColor = TextPrimary;
            lblName.BackColor = Color.Transparent;

            // Category
            lblCategory.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblCategory.ForeColor = TextMuted;
            lblCategory.BackColor = Color.Transparent;

            // Click propagation
            pnlCard.Click += (s, e) => OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            picImage.Click += (s, e) => OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            lblName.Click += (s, e) => OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            lblCategory.Click += (s, e) => OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            lblPrice.Click += (s, e) => OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));

            // Hover animation
            _hoverTimer.Tick += HoverTimer_Tick;

            pnlCard.MouseEnter += (s, e) => { _shadowTarget = 14f; _hoverTimer.Start(); };
            pnlCard.MouseLeave += (s, e) => { _shadowTarget = 6f; _hoverTimer.Start(); };
            picImage.MouseEnter += (s, e) => { _shadowTarget = 14f; _hoverTimer.Start(); };
            picImage.MouseLeave += (s, e) => { _shadowTarget = 6f; _hoverTimer.Start(); };
            lblName.MouseEnter += (s, e) => { _shadowTarget = 14f; _hoverTimer.Start(); };
            lblName.MouseLeave += (s, e) => { _shadowTarget = 6f; _hoverTimer.Start(); };
            this.MouseEnter += (s, e) => { _shadowTarget = 14f; _hoverTimer.Start(); };
            this.MouseLeave += (s, e) => { _shadowTarget = 6f; _hoverTimer.Start(); };

            // Cursor for children
            pnlCard.Cursor = Cursors.Hand;
            picImage.Cursor = Cursors.Hand;
            lblName.Cursor = Cursors.Hand;
            lblCategory.Cursor = Cursors.Hand;
        }

        private void HoverTimer_Tick(object? s, EventArgs e)
        {
            _shadowCurrent += (_shadowTarget - _shadowCurrent) * 0.25f;
            pnlCard.ShadowDecoration.Depth = (int)_shadowCurrent;
            if (Math.Abs(_shadowTarget - _shadowCurrent) < 0.5f)
                _hoverTimer.Stop();
        }

        private void PicImage_Paint(object? sender, PaintEventArgs e)
        {
            if (picImage.Image != null) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Draw placeholder icon (image icon)
            using var brush = new SolidBrush(Color.FromArgb(180, 156, 163, 175));
            using var font = new Font("Segoe UI Symbol", 20F);
            string icon = "\u2603"; // placeholder
            var sz = g.MeasureString(icon, font);
            float cx = (picImage.Width - sz.Width) / 2;
            float cy = (picImage.Height - sz.Height) / 2;

            // Draw a simple image icon shape
            int ix = picImage.Width / 2 - 20;
            int iy = picImage.Height / 2 - 20;
            using var pen = new Pen(Color.FromArgb(120, 156, 163, 175), 2f);
            g.DrawRectangle(pen, ix, iy, 40, 40);
            // Mountain shape inside
            g.DrawLine(pen, ix, iy + 30, ix + 15, iy + 15);
            g.DrawLine(pen, ix + 15, iy + 15, ix + 25, iy + 25);
            g.DrawLine(pen, ix + 25, iy + 25, ix + 35, iy + 10);
            g.DrawLine(pen, ix + 35, iy + 10, ix + 40, iy + 30);
            // Sun circle
            g.DrawEllipse(pen, ix + 28, iy + 5, 8, 8);
        }
    }
}
