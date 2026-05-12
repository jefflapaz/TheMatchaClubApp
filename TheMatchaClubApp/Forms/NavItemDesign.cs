using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class NavItem
    {
        private static readonly Color NormalFg = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color ActiveFg = ColorTranslator.FromHtml("#52B743");
        private static readonly Color HoverBg = ColorTranslator.FromHtml("#F3F4F6");
        private static readonly Color ActiveBorder = ColorTranslator.FromHtml("#E5E7EB");

        private void InitializeDesign()
        {
            pnlContainer.BackColor = Color.Transparent;
            pnlContainer.FillColor = Color.Transparent;
            pnlContainer.BorderRadius = 8;
            pnlContainer.BorderThickness = 0;
            pnlContainer.ShadowDecoration.Enabled = false;

            pnlIcon.BackColor = Color.Transparent;
            pnlIcon.Paint += PnlIcon_Paint;

            lblText.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblText.ForeColor = NormalFg;
            lblText.BackColor = Color.Transparent;
            lblText.Cursor = Cursors.Hand;

            // Click wiring — whole surface clickable
            pnlContainer.Click += (s, e) => RaiseClick();
            lblText.Click += (s, e) => RaiseClick();
            pnlIcon.Click += (s, e) => RaiseClick();
            this.Click += (s, e) => RaiseClick();

            pnlContainer.Cursor = Cursors.Hand;
            pnlIcon.Cursor = Cursors.Hand;

            // Hover
            pnlContainer.MouseEnter += OnHoverEnter;
            lblText.MouseEnter += OnHoverEnter;
            pnlIcon.MouseEnter += OnHoverEnter;
            pnlContainer.MouseLeave += OnHoverLeave;
            lblText.MouseLeave += OnHoverLeave;
            pnlIcon.MouseLeave += OnHoverLeave;

            ApplyState();
        }

        private void OnHoverEnter(object? s, EventArgs e)
        {
            if (!_isActive)
                pnlContainer.FillColor = HoverBg;
        }

        private void OnHoverLeave(object? s, EventArgs e)
        {
            if (!_isActive)
                pnlContainer.FillColor = Color.Transparent;
        }

        internal void ApplyState()
        {
            if (_isActive)
            {
                pnlContainer.FillColor = Color.White;
                pnlContainer.BorderColor = ActiveBorder;
                pnlContainer.BorderThickness = 1;
                pnlContainer.ShadowDecoration.Enabled = true;
                pnlContainer.ShadowDecoration.Depth = 4;
                pnlContainer.ShadowDecoration.Color = Color.FromArgb(12, 0, 0, 0);
                lblText.ForeColor = ActiveFg;
            }
            else
            {
                pnlContainer.FillColor = Color.Transparent;
                pnlContainer.BorderThickness = 0;
                pnlContainer.ShadowDecoration.Enabled = false;
                lblText.ForeColor = NormalFg;
            }
            pnlIcon.Invalidate();
        }

        private void PnlIcon_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            Color iconColor = _isActive ? ActiveFg : NormalFg;
            using var pen = new Pen(iconColor, 1.6f);
            using var brush = new SolidBrush(iconColor);

            switch (_iconKey)
            {
                case "dashboard": DrawDashboardIcon(g, pen, brush); break;
                case "quicksale": DrawCartIcon(g, pen, brush); break;
                case "orders": DrawClockIcon(g, pen, brush); break;
                case "items": DrawBoxIcon(g, pen, brush); break;
                case "customers": DrawPersonIcon(g, pen, brush); break;
                case "reports": DrawChartIcon(g, pen, brush); break;
                case "settings": DrawGearIcon(g, pen, brush); break;
                case "logout": DrawLogoutIcon(g, pen, brush); break;
            }
        }

        private void DrawDashboardIcon(Graphics g, Pen p, SolidBrush b)
        {
            g.FillRectangle(b, 2, 2, 7, 7);
            g.FillRectangle(b, 11, 2, 7, 7);
            g.FillRectangle(b, 2, 11, 7, 7);
            g.FillRectangle(b, 11, 11, 7, 7);
        }

        private void DrawCartIcon(Graphics g, Pen p, SolidBrush b)
        {
            g.DrawLine(p, 1, 3, 4, 3);
            g.DrawLine(p, 4, 3, 5, 12);
            g.DrawLine(p, 5, 12, 16, 12);
            g.DrawLine(p, 16, 12, 18, 5);
            g.DrawLine(p, 18, 5, 6, 5);
            g.FillEllipse(b, 6, 15, 4, 4);
            g.FillEllipse(b, 13, 15, 4, 4);
        }

        private void DrawClockIcon(Graphics g, Pen p, SolidBrush b)
        {
            g.DrawEllipse(p, 2, 2, 16, 16);
            g.DrawLine(p, 10, 6, 10, 10);
            g.DrawLine(p, 10, 10, 14, 12);
        }

        private void DrawBoxIcon(Graphics g, Pen p, SolidBrush b)
        {
            g.DrawRectangle(p, 2, 4, 16, 14);
            g.DrawLine(p, 2, 4, 10, 1);
            g.DrawLine(p, 10, 1, 18, 4);
            g.DrawLine(p, 10, 1, 10, 10);
            g.DrawLine(p, 2, 4, 10, 7);
            g.DrawLine(p, 18, 4, 10, 7);
        }

        private void DrawPersonIcon(Graphics g, Pen p, SolidBrush b)
        {
            g.DrawEllipse(p, 6, 1, 8, 8);
            g.DrawArc(p, 2, 12, 16, 10, 180, 180);
        }

        private void DrawChartIcon(Graphics g, Pen p, SolidBrush b)
        {
            g.FillRectangle(b, 2, 12, 4, 6);
            g.FillRectangle(b, 8, 6, 4, 12);
            g.FillRectangle(b, 14, 2, 4, 16);
        }

        private void DrawGearIcon(Graphics g, Pen p, SolidBrush b)
        {
            g.DrawEllipse(p, 5, 5, 10, 10);
            float cx = 10, cy = 10;
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f * (float)(Math.PI / 180.0);
                float x1 = cx + 6f * (float)Math.Cos(angle);
                float y1 = cy + 6f * (float)Math.Sin(angle);
                float x2 = cx + 9f * (float)Math.Cos(angle);
                float y2 = cy + 9f * (float)Math.Sin(angle);
                g.DrawLine(p, x1, y1, x2, y2);
            }
        }

        private void DrawLogoutIcon(Graphics g, Pen p, SolidBrush b)
        {
            g.DrawLine(p, 10, 4, 10, 16);
            g.DrawArc(p, 3, 2, 14, 16, 200, 140);
        }
    }
}
