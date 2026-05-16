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
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var pnl = (Control)sender!;

            if (_hourlySalesData == null || _hourlySalesData.Count == 0 || _hourlySalesData.Values.All(v => v == 0))
            {
                // Empty state for chart
                DrawPanelEmptyState(g, pnl, "📊", "No sales data yet");
                return;
            }

            int left = 50, top = 46, right = pnl.Width - 16, bottom = pnl.Height - 30;
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
                using var f = new Font("Segoe UI", 7.5F);
                g.DrawString($"₱{maxVal * i / 4:#,##0}", f, Brushes.Gray, 2, y - 7);
            }

            float barW = Math.Min(32, (float)chartW / hourCount * 0.7f);
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
                    g.FillRoundedRectangle(brush, x, y, barW, barH, 5);
                }
                using var lf = new Font("Segoe UI", 7F);
                string label = h > 12 ? $"{h - 12}p" : h == 12 ? "12p" : h == 0 ? "12a" : $"{h}a";
                var sz = g.MeasureString(label, lf);
                g.DrawString(label, lf, Brushes.Gray, x + barW / 2 - sz.Width / 2, bottom + 4);
            }
        }

        // ── Top Products with Progress Bars ───────────────────────
        private void PnlTopProducts_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var pnl = (Control)sender!;

            if (_topProducts == null || _topProducts.Count == 0)
            {
                DrawPanelEmptyState(g, pnl, "🏆", "No products sold yet");
                return;
            }

            int maxUnits = _topProducts.Max(p => p.Units);
            if (maxUnits == 0) maxUnits = 1;
            int y = 40, rowH = Math.Max(30, Math.Min(34, (pnl.Height - 48) / Math.Max(_topProducts.Count, 1)));

            using var nameFont = new Font("Segoe UI", 9F);
            using var valFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            using var rankFont = new Font("Segoe UI", 8F, FontStyle.Bold);

            for (int i = 0; i < _topProducts.Count; i++)
            {
                var p = _topProducts[i];
                int barMaxW = Math.Max(50, pnl.Width - 280);

                // Rank badge with background
                var rankBg = i < 3 ? GreenBg : ColorTranslator.FromHtml("#F9FAFB");
                var rankFg = i < 3 ? Green : TextMuted;
                using var rankBgBrush = new SolidBrush(rankBg);
                g.FillEllipse(rankBgBrush, 14, y, 22, 22);
                using var rankBrush = new SolidBrush(rankFg);
                var rankStr = $"{i + 1}";
                var rankSz = g.MeasureString(rankStr, rankFont);
                g.DrawString(rankStr, rankFont, rankBrush, 14 + (22 - rankSz.Width) / 2, y + (22 - rankSz.Height) / 2);

                // Name
                g.DrawString(p.Name, nameFont, new SolidBrush(TextPrimary), 44, y + 2);

                // Progress bar
                int barX = pnl.Width - 220, barW = 110;
                float pct = (float)p.Units / maxUnits;
                using var barBg = new SolidBrush(BorderCard);
                using var barFg = new SolidBrush(Color.FromArgb(180, Green));
                var bgRect = new Rectangle(barX, y + 3, barW, 16);
                using var bgPath = CreateRoundRectPath(bgRect, 5);
                g.FillPath(barBg, bgPath);
                if (pct > 0)
                {
                    var fgRect = new Rectangle(barX, y + 3, Math.Max(8, (int)(barW * pct)), 16);
                    using var fgPath = CreateRoundRectPath(fgRect, 5);
                    g.FillPath(barFg, fgPath);
                }

                // Units & Revenue
                string info = $"{p.Units} sold  ₱{p.Revenue:#,##0}";
                var infoSz = g.MeasureString(info, valFont);
                g.DrawString(info, valFont, new SolidBrush(TextSecondary), pnl.Width - 16 - infoSz.Width, y + 3);
                y += rowH;
            }
        }

        // ── Recent Transactions ───────────────────────────────────
        private void PnlRecentTx_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var pnl = (Control)sender!;

            if (_recentOrders == null || _recentOrders.Count == 0)
            {
                DrawPanelEmptyState(g, pnl, "🧾", "No recent transactions");
                return;
            }

            // Header row
            int y = 38;
            using var hdrFont = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            using var hdrBrush = new SolidBrush(TextMuted);
            g.DrawString("ORDER ID", hdrFont, hdrBrush, 16, y);
            g.DrawString("CUSTOMER", hdrFont, hdrBrush, pnl.Width * 0.25f, y);
            g.DrawString("AMOUNT", hdrFont, hdrBrush, pnl.Width * 0.55f, y);
            g.DrawString("TIME", hdrFont, hdrBrush, pnl.Width * 0.78f, y);
            y += 18;

            // Header separator
            using var sepPen = new Pen(BorderLight, 1) { DashStyle = DashStyle.Dash };
            g.DrawLine(sepPen, 16, y, pnl.Width - 16, y);
            y += 6;

            using var rowFont = new Font("Segoe UI", 8.5F);
            using var boldFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            int rowH = Math.Max(28, Math.Min(32, (pnl.Height - 68) / Math.Max(_recentOrders.Count, 1)));

            for (int i = 0; i < _recentOrders.Count; i++)
            {
                var o = _recentOrders[i];

                // Alternating row background
                if (i % 2 == 1)
                {
                    using var altBrush = new SolidBrush(Color.FromArgb(10, 0, 0, 0));
                    g.FillRectangle(altBrush, 8, y - 2, pnl.Width - 16, rowH);
                }

                // Hover indicator (row tracking done in DashboardView.cs)
                if (_hoveredTxRow == i)
                {
                    using var hoverBrush = new SolidBrush(Color.FromArgb(20, Green));
                    g.FillRectangle(hoverBrush, 8, y - 2, pnl.Width - 16, rowH);
                }

                g.DrawString(o.OrderId.Length > 16 ? o.OrderId[^8..] : o.OrderId, rowFont, new SolidBrush(TextBody), 16, y);
                g.DrawString(string.IsNullOrEmpty(o.CustomerName) ? "Walk-in" : o.CustomerName, rowFont, new SolidBrush(TextBody), pnl.Width * 0.25f, y);
                g.DrawString($"₱{o.Total:#,##0.00}", boldFont, new SolidBrush(Green), pnl.Width * 0.55f, y);
                g.DrawString(o.Timestamp.ToString("h:mm tt"), rowFont, new SolidBrush(TextSecondary), pnl.Width * 0.78f, y);
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
            using var sectionFont = new Font("Segoe UI", 7F, FontStyle.Bold);
            using var keyFont = new Font("Segoe UI", 8F);
            using var valFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var keyBrush = new SolidBrush(TextSecondary);
            using var sectionBrush = new SolidBrush(TextMuted);
            using var sepPen = new Pen(BorderCard, 1);

            int y = 42, rowH = 38, padL = 20;

            // Status indicator dot
            using var dotBrush = new SolidBrush(session != null ? Green : TextMuted);
            g.FillEllipse(dotBrush, pnl.Width - 36, 14, 10, 10);

            if (session == null)
            {
                // Centered no-session state
                using var emptyIcon = new Font("Segoe UI", 28F);
                using var emptyMsg = new Font("Segoe UI", 10F);
                using var emptyHint = new Font("Segoe UI", 8.5F);
                string icon = "⏸";
                string msg = "No Active Session";
                string hint = "Open a session to start tracking";
                var iconSz = g.MeasureString(icon, emptyIcon);
                var msgSz = g.MeasureString(msg, emptyMsg);
                var hintSz = g.MeasureString(hint, emptyHint);
                int cy = pnl.Height / 2 - 40;
                g.DrawString(icon, emptyIcon, new SolidBrush(TextMuted), (pnl.Width - iconSz.Width) / 2, cy);
                g.DrawString(msg, emptyMsg, new SolidBrush(TextSecondary), (pnl.Width - msgSz.Width) / 2, cy + 48);
                g.DrawString(hint, emptyHint, new SolidBrush(TextMuted), (pnl.Width - hintSz.Width) / 2, cy + 72);
                return;
            }

            // ── SESSION INFO ──
            g.DrawLine(sepPen, padL, y - 4, pnl.Width - padL, y - 4);
            g.DrawString("SESSION INFO", sectionFont, sectionBrush, padL, y);
            y += 18;
            DrawSessionRow(g, "Opened At", session.OpenedAt.ToString("h:mm tt"), padL, ref y, rowH, keyFont, valFont, keyBrush, TextPrimary);
            DrawSessionRow(g, "Duration", _sessionDurationText ?? "—", padL, ref y, rowH, keyFont, valFont, keyBrush, TextPrimary);

            // ── OPERATOR ──
            y += 4;
            g.DrawLine(sepPen, padL, y - 4, pnl.Width - padL, y - 4);
            g.DrawString("OPERATOR", sectionFont, sectionBrush, padL, y);
            y += 18;
            DrawSessionRow(g, "Cashier", session.OpenedBy, padL, ref y, rowH, keyFont, valFont, keyBrush, TextPrimary);

            // ── CASH INFO ──
            y += 4;
            g.DrawLine(sepPen, padL, y - 4, pnl.Width - padL, y - 4);
            g.DrawString("CASH INFO", sectionFont, sectionBrush, padL, y);
            y += 18;
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

        // ── Shared Panel Empty State ─────────────────────────────
        private void DrawPanelEmptyState(Graphics g, Control pnl, string icon, string message)
        {
            using var iconFont = new Font("Segoe UI", 20F);
            using var msgFont = new Font("Segoe UI", 9F);
            var iconSz = g.MeasureString(icon, iconFont);
            var msgSz = g.MeasureString(message, msgFont);
            int cy = pnl.Height / 2 - 20;
            g.DrawString(icon, iconFont, new SolidBrush(TextMuted), (pnl.Width - iconSz.Width) / 2, cy);
            g.DrawString(message, msgFont, new SolidBrush(TextMuted), (pnl.Width - msgSz.Width) / 2, cy + 32);
        }
    }
}
