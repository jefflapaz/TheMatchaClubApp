using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class DashboardView
    {
        // ── Hourly Sales Bar Chart ────────────────────────────────
        private void PnlHourlySales_Paint(object? sender, PaintEventArgs e)
        {
            if (_hourlySalesData == null || _hourlySalesData.Count == 0) return;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var pnl = (Control)sender!;
            int left = 50, top = 40, right = pnl.Width - 16, bottom = pnl.Height - 26;
            int chartW = right - left, chartH = bottom - top;
            if (chartW < 20 || chartH < 20) return;

            var active = _hourlySalesData.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToList();
            int minH = active.Count > 0 ? Math.Max(0, active.Min() - 1) : 6;
            int maxH = active.Count > 0 ? Math.Min(23, active.Max() + 1) : 22;
            int hourCount = maxH - minH + 1;
            if (hourCount <= 0) return;
            decimal maxVal = _hourlySalesData.Values.Max();
            if (maxVal == 0) maxVal = 100;

            using var gridPen = new Pen(BorderCard, 1);
            for (int i = 0; i <= 4; i++)
            {
                int y = bottom - (int)(chartH * i / 4.0);
                g.DrawLine(gridPen, left, y, right, y);
                using var f = new Font("Segoe UI", 7F);
                g.DrawString($"₱{maxVal * i / 4:#,##0}", f, Brushes.Gray, 2, y - 6);
            }

            float barW = Math.Min(28, (float)chartW / hourCount * 0.7f);
            float gapF = (float)chartW / hourCount;
            for (int h = minH; h <= maxH; h++)
            {
                decimal val = _hourlySalesData.ContainsKey(h) ? _hourlySalesData[h] : 0;
                float barH = maxVal > 0 ? (float)((double)val / (double)maxVal * chartH) : 0;
                float x = left + (h - minH) * gapF + (gapF - barW) / 2;
                float y = bottom - barH;
                if (barH > 2)
                {
                    using var brush = new LinearGradientBrush(new RectangleF(x, y, barW, Math.Max(barH, 1)), Green, ColorTranslator.FromHtml("#86CD77"), 90F);
                    g.FillRoundedRectangle(brush, x, y, barW, barH, 4);
                }
                using var lf = new Font("Segoe UI", 6.5F);
                string label = h > 12 ? $"{h - 12}p" : h == 12 ? "12p" : h == 0 ? "12a" : $"{h}a";
                var sz = g.MeasureString(label, lf);
                g.DrawString(label, lf, Brushes.Gray, x + barW / 2 - sz.Width / 2, bottom + 2);
            }
        }

        // ── Top Products with Progress Bars ───────────────────────
        private void PnlTopProducts_Paint(object? sender, PaintEventArgs e)
        {
            if (_topProducts == null || _topProducts.Count == 0) return;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var pnl = (Control)sender!;
            int maxUnits = _topProducts.Max(p => p.Units);
            if (maxUnits == 0) maxUnits = 1;
            int y = 38, rowH = Math.Min(28, (pnl.Height - 44) / Math.Max(_topProducts.Count, 1));

            using var nameFont = new Font("Segoe UI", 8.5F);
            using var valFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            using var rankFont = new Font("Segoe UI", 8F, FontStyle.Bold);

            for (int i = 0; i < _topProducts.Count; i++)
            {
                var p = _topProducts[i];
                int barMaxW = Math.Max(50, pnl.Width - 260);

                // Rank
                using var rankBrush = new SolidBrush(i < 3 ? Green : TextMuted);
                g.DrawString($"#{i + 1}", rankFont, rankBrush, 16, y + 2);

                // Name
                g.DrawString(p.Name, nameFont, new SolidBrush(TextPrimary), 42, y + 2);

                // Progress bar
                int barX = pnl.Width - 200, barW = 100;
                float pct = (float)p.Units / maxUnits;
                using var barBg = new SolidBrush(BorderCard);
                using var barFg = new SolidBrush(Color.FromArgb(180, Green));
                var bgRect = new Rectangle(barX, y + 4, barW, 14);
                using var bgPath = CreateRoundRectPath(bgRect, 4);
                g.FillPath(barBg, bgPath);
                if (pct > 0)
                {
                    var fgRect = new Rectangle(barX, y + 4, Math.Max(8, (int)(barW * pct)), 14);
                    using var fgPath = CreateRoundRectPath(fgRect, 4);
                    g.FillPath(barFg, fgPath);
                }

                // Units & Revenue
                string info = $"{p.Units} sold  ₱{p.Revenue:#,##0}";
                g.DrawString(info, valFont, new SolidBrush(TextSecondary), pnl.Width - 90, y + 2);
                y += rowH;
            }
        }

        // ── Recent Transactions ───────────────────────────────────
        private void PnlRecentTx_Paint(object? sender, PaintEventArgs e)
        {
            if (_recentOrders == null || _recentOrders.Count == 0)
            {
                var g2 = e.Graphics; g2.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                using var f = new Font("Segoe UI", 9F);
                g2.DrawString("No recent transactions.", f, new SolidBrush(TextMuted), 16, 44);
                return;
            }
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var pnl = (Control)sender!;

            // Header row
            int y = 36;
            using var hdrFont = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            using var hdrBrush = new SolidBrush(TextMuted);
            g.DrawString("ORDER ID", hdrFont, hdrBrush, 16, y);
            g.DrawString("CUSTOMER", hdrFont, hdrBrush, pnl.Width * 0.25f, y);
            g.DrawString("AMOUNT", hdrFont, hdrBrush, pnl.Width * 0.55f, y);
            g.DrawString("TIME", hdrFont, hdrBrush, pnl.Width * 0.75f, y);
            y += 20;

            using var rowFont = new Font("Segoe UI", 8.5F);
            using var boldFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            int rowH = Math.Min(26, (pnl.Height - 60) / Math.Max(_recentOrders.Count, 1));

            for (int i = 0; i < _recentOrders.Count; i++)
            {
                var o = _recentOrders[i];
                if (i % 2 == 1)
                {
                    using var altBrush = new SolidBrush(Color.FromArgb(6, 0, 0, 0));
                    g.FillRectangle(altBrush, 8, y - 2, pnl.Width - 16, rowH);
                }
                g.DrawString(o.OrderId.Length > 16 ? o.OrderId[^8..] : o.OrderId, rowFont, new SolidBrush(TextBody), 16, y);
                g.DrawString(string.IsNullOrEmpty(o.CustomerName) ? "Walk-in" : o.CustomerName, rowFont, new SolidBrush(TextBody), pnl.Width * 0.25f, y);
                g.DrawString($"₱{o.Total:#,##0.00}", boldFont, new SolidBrush(Green), pnl.Width * 0.55f, y);
                g.DrawString(o.Timestamp.ToString("h:mm tt"), rowFont, new SolidBrush(TextSecondary), pnl.Width * 0.75f, y);
                y += rowH;
            }
        }

        // ── Session Status Panel ──────────────────────────────────
        private void PnlSessionStatus_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var pnl = (Control)sender!;

            var session = Program.SessionService.GetActiveSession();
            using var keyFont = new Font("Segoe UI", 8.5F);
            using var valFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            using var keyBrush = new SolidBrush(TextSecondary);

            int y = 42, rowH = 44, padL = 20;

            // Status indicator
            if (session != null)
            {
                using var dotBrush = new SolidBrush(Green);
                g.FillEllipse(dotBrush, pnl.Width - 36, 14, 10, 10);
            }
            else
            {
                using var dotBrush = new SolidBrush(TextMuted);
                g.FillEllipse(dotBrush, pnl.Width - 36, 14, 10, 10);
            }

            if (session == null)
            {
                g.DrawString("No active session", valFont, new SolidBrush(TextMuted), padL, y + 20);
                return;
            }

            // Separator line below title
            using var sepPen = new Pen(BorderCard, 1);
            g.DrawLine(sepPen, padL, y - 4, pnl.Width - padL, y - 4);

            DrawSessionRow(g, "Opened At", session.OpenedAt.ToString("h:mm tt"), padL, ref y, rowH, keyFont, valFont, keyBrush, TextPrimary);
            DrawSessionRow(g, "Duration", _sessionDurationText ?? "—", padL, ref y, rowH, keyFont, valFont, keyBrush, TextPrimary);
            DrawSessionRow(g, "Cashier", session.OpenedBy, padL, ref y, rowH, keyFont, valFont, keyBrush, TextPrimary);

            g.DrawLine(sepPen, padL, y - 4, pnl.Width - padL, y - 4);

            DrawSessionRow(g, "Starting Cash", $"₱{session.StartingCash:#,##0.00}", padL, ref y, rowH, keyFont, valFont, keyBrush, Green);
            decimal expectedCash = session.StartingCash + _todaySalesTotal;
            DrawSessionRow(g, "Expected Cash", $"₱{expectedCash:#,##0.00}", padL, ref y, rowH, keyFont, valFont, keyBrush, TextPrimary);

            if (session.ActualCash > 0)
            {
                DrawSessionRow(g, "Actual Cash", $"₱{session.ActualCash:#,##0.00}", padL, ref y, rowH, keyFont, valFont, keyBrush, TextPrimary);
                decimal diff = session.ActualCash - expectedCash;
                var diffColor = diff >= 0 ? Green : Rose;
                string diffText = diff >= 0 ? $"+₱{diff:#,##0.00}" : $"-₱{Math.Abs(diff):#,##0.00}";
                DrawSessionRow(g, "Over/Short", diffText, padL, ref y, rowH, keyFont, valFont, keyBrush, diffColor);
            }
        }

        private void DrawSessionRow(Graphics g, string key, string value, int padL, ref int y, int rowH, Font keyFont, Font valFont, Brush keyBrush, Color valColor)
        {
            g.DrawString(key, keyFont, keyBrush, padL, y);
            using var vb = new SolidBrush(valColor);
            g.DrawString(value, valFont, vb, padL, y + 16);
            y += rowH;
        }
    }
}
