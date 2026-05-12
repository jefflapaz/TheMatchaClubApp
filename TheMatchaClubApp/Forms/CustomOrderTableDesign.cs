using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class CustomOrderTable
    {
        private static readonly Color TblHeaderBg = ColorTranslator.FromHtml("#F9FAFB");
        private static readonly Color TblBorder = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color TblTextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color TblTextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color TblTextBody = ColorTranslator.FromHtml("#374151");
        private static readonly Color TblTextGray = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color TblGreen = ColorTranslator.FromHtml("#52B743");
        private static readonly Color TblSelectedBg = ColorTranslator.FromHtml("#F2FAEF");
        private static readonly Color TblHoverBg = ColorTranslator.FromHtml("#F9FAFB");
        private static readonly Color TblOrangePill = ColorTranslator.FromHtml("#F59E0B");

        private int _hoverRow = -1;

        private void InitializeDesign()
        {
            this.BackColor = Color.White;
            this.Cursor = Cursors.Hand;

            this.MouseMove += OnTableMouseMove;
            this.MouseLeave += (s, e) => { _hoverRow = -1; Invalidate(); };
        }

        private void OnTableMouseMove(object? sender, MouseEventArgs e)
        {
            int headerH = 40;
            int rowH = 56;
            int newHover = -1;
            if (e.Y >= headerH)
                newHover = (e.Y - headerH) / rowH;
            if (newHover != _hoverRow)
            {
                _hoverRow = newHover;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int w = this.Width;
            int headerH = 40;
            int rowH = 56;

            // Column widths (proportional)
            int[] colW = { 100, 120, 140, 180, 80, 80 };
            string[] headers = { "ORDER NO.", "DATE/TIME", "CUSTOMER", "ITEMS SUMMARY", "TYPE", "TOTAL" };

            // Adjust last column to fill
            int usedW = 0;
            for (int i = 0; i < colW.Length - 1; i++) usedW += colW[i];
            colW[^1] = Math.Max(80, w - usedW);

            // ── Header ──
            using var headerBrush = new SolidBrush(TblHeaderBg);
            g.FillRectangle(headerBrush, 0, 0, w, headerH);
            using var borderPen = new Pen(TblBorder, 1);
            g.DrawLine(borderPen, 0, headerH - 1, w, headerH - 1);

            using var headerFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            using var headerTextBrush = new SolidBrush(TblTextMuted);

            int hx = 16;
            for (int i = 0; i < headers.Length; i++)
            {
                g.DrawString(headers[i], headerFont, headerTextBrush, hx, (headerH - 16) / 2);
                hx += colW[i];
            }

            // ── Data Rows ──
            using var rowFont = new Font("Segoe UI", 9F);
            using var rowFontBold = new Font("Segoe UI", 9F, FontStyle.Bold);
            using var textPrimBrush = new SolidBrush(TblTextPrimary);
            using var textBodyBrush = new SolidBrush(TblTextBody);
            using var textGrayBrush = new SolidBrush(TblTextGray);
            using var greenBrush = new SolidBrush(TblGreen);
            using var selectedBrush = new SolidBrush(TblSelectedBg);
            using var hoverBrush = new SolidBrush(TblHoverBg);

            for (int r = 0; r < _orders.Count; r++)
            {
                var order = _orders[r];
                int ry = headerH + r * rowH;

                // Row background
                if (r == _selectedIndex)
                {
                    g.FillRectangle(selectedBrush, 0, ry, w, rowH);
                    // Left green border for selected
                    using var greenPen = new Pen(TblGreen, 4);
                    g.DrawLine(greenPen, 2, ry, 2, ry + rowH);
                }
                else if (r == _hoverRow)
                {
                    g.FillRectangle(hoverBrush, 0, ry, w, rowH);
                }

                // Bottom border
                g.DrawLine(borderPen, 0, ry + rowH - 1, w, ry + rowH - 1);

                int rx = 16;
                int textY = ry + (rowH - 18) / 2;

                // ORDER NO.
                g.DrawString(order.OrderNo, rowFontBold, textPrimBrush, rx, textY);
                rx += colW[0];

                // DATE/TIME
                g.DrawString($"{order.Date}\n{order.Time}", rowFont, textGrayBrush, rx, ry + 8);
                rx += colW[1];

                // CUSTOMER
                g.DrawString(order.Customer, rowFont, textBodyBrush, rx, textY);
                rx += colW[2];

                // ITEMS SUMMARY
                string summary = order.ItemsSummary.Length > 30 ? order.ItemsSummary[..30] + "..." : order.ItemsSummary;
                g.DrawString(summary, rowFont, textGrayBrush, rx, textY);
                rx += colW[3];

                // TYPE badge
                bool isDineIn = order.Type == "Dine-in";
                Color pillColor = isDineIn ? TblGreen : TblOrangePill;
                Color pillBg = isDineIn ? ColorTranslator.FromHtml("#F2FAEF") : ColorTranslator.FromHtml("#FFF7ED");
                using var pillBgBrush = new SolidBrush(pillBg);
                using var pillTextBrush = new SolidBrush(pillColor);
                using var pillFont = new Font("Segoe UI", 7F, FontStyle.Bold);

                var pillRect = new Rectangle(rx, textY - 2, 64, 22);
                using var pillPath = CreateRoundedRectPath(pillRect, 6);
                g.FillPath(pillBgBrush, pillPath);
                var pillSz = g.MeasureString(order.Type, pillFont);
                g.DrawString(order.Type, pillFont, pillTextBrush,
                    rx + (64 - pillSz.Width) / 2, textY);
                rx += colW[4];

                // TOTAL
                g.DrawString(order.Total.ToString("C2"), rowFontBold, greenBrush, rx, textY);
            }
        }

        private static GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
