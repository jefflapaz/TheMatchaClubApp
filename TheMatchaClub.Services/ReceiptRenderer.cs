using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using TheMatchaClubDomain.Models;

namespace TheMatchaClub.Services
{
    /// <summary>
    /// Centralized receipt renderer. Every receipt output in the system
    /// (virtual preview, print, PDF bitmap, Settings preview) calls this.
    /// </summary>
    public static class ReceiptRenderer
    {
        // ── Brand colors ──────────────────────────────────────────
        private static readonly Color Green = ColorTranslator.FromHtml("#52B743");
        private static readonly Color TextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color TextSecondary = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color TextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color BorderColor = ColorTranslator.FromHtml("#D1D5DB");

        /// <summary>
        /// Renders a complete receipt onto the given Graphics surface.
        /// Pass null for <paramref name="order"/> to render a sample preview.
        /// Returns the total rendered height.
        /// </summary>
        public static float Render(Graphics g, Rectangle bounds, Order? order, StoreSettings settings, string currentCashierName)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.White);

            float x = 20, y = 20;
            float w = bounds.Width - 40;
            float cx = bounds.Width / 2f;

            // Fonts
            using var titleFont = new Font("Segoe UI", 13F, FontStyle.Bold);
            using var headerFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            using var bodyFont = new Font("Segoe UI", 9F);
            using var smallFont = new Font("Segoe UI", 8F);
            using var smallBold = new Font("Segoe UI", 8F, FontStyle.Bold);
            using var totalFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var footerFont = new Font("Segoe UI", 8.5F, FontStyle.Italic);

            // Brushes
            using var brush = new SolidBrush(TextPrimary);
            using var grayBrush = new SolidBrush(TextSecondary);
            using var greenBrush = new SolidBrush(Green);
            using var mutedBrush = new SolidBrush(TextMuted);
            using var dashPen = new Pen(BorderColor, 1) { DashStyle = DashStyle.Dash };

            var sf = new StringFormat { Alignment = StringAlignment.Center };

            // ── Store Logo Icon ──
            g.DrawString("🍵", new Font("Segoe UI", 16F), greenBrush, cx, y, sf);
            y += 28;

            // ── Store Name ──
            g.DrawString(settings.StoreName, titleFont, greenBrush, cx, y, sf);
            y += 22;

            // ── Location (dynamic) ──
            string location = GetDisplayLocation(settings);
            g.DrawString(location, smallFont, grayBrush, cx, y, sf);
            y += 16;

            // ── Contact ──
            g.DrawString($"{settings.Phone}  •  {settings.Email}", smallFont, grayBrush, cx, y, sf);
            y += 22;

            // ═══ Separator ═══
            g.DrawLine(dashPen, x, y, x + w, y);
            y += 10;

            // ── Order metadata ──
            bool isSample = order == null;
            string orderId = isSample ? "#MC-SAMPLE" : order!.OrderId;
            string dateStr = isSample ? DateTime.Now.ToString("dd MMM yyyy HH:mm") : order!.Timestamp.ToString("dd MMM yyyy HH:mm");
            string customer = isSample ? "Walk-In" : (string.IsNullOrEmpty(order!.CustomerName) ? "Walk-In" : order.CustomerName);
            string cashier = isSample ? currentCashierName : (order!.CashierName ?? currentCashierName);
            string orderType = isSample ? "Dine-In" : (order!.OrderType ?? "Dine-In");
            string paymentMethod = isSample ? "Cash" : (order!.PaymentMethod ?? "Cash");

            DrawPair(g, "Order ID", orderId, smallFont, headerFont, grayBrush, brush, x, w, ref y);
            DrawPair(g, "Date", dateStr, smallFont, bodyFont, grayBrush, brush, x, w, ref y);

            if (settings.ReceiptShowCustomerName)
                DrawPair(g, "Customer", customer, smallFont, bodyFont, grayBrush, brush, x, w, ref y);

            if (settings.ReceiptShowCashierName)
                DrawPair(g, "Cashier", cashier, smallFont, bodyFont, grayBrush, mutedBrush, x, w, ref y);

            if (settings.ReceiptShowOrderType)
                DrawPair(g, "Type", orderType, smallFont, bodyFont, grayBrush, brush, x, w, ref y);

            if (settings.ReceiptShowSessionNumber)
            {
                string sessionId = isSample ? "#S-001" : (order!.SessionId?.ToString() ?? "—");
                if (sessionId.Length > 10) sessionId = sessionId[^8..];
                DrawPair(g, "Session", sessionId, smallFont, bodyFont, grayBrush, mutedBrush, x, w, ref y);
            }

            y += 6;

            // ═══ Separator ═══
            g.DrawLine(dashPen, x, y, x + w, y);
            y += 10;

            // ── Items Header ──
            g.DrawString("ITEM", smallBold, mutedBrush, x, y);
            g.DrawString("QTY", smallBold, mutedBrush, x + w - 100, y);
            g.DrawString("TOTAL", smallBold, mutedBrush, x + w - 44, y);
            y += 18;

            if (isSample)
            {
                // Sample items for preview
                DrawItem(g, "Matcha Latte", 1, 180m, bodyFont, brush, x, w, ref y);
                DrawItem(g, "Hojicha Latte", 2, 340m, bodyFont, brush, x, w, ref y);
                DrawItem(g, "Green Tea Mochi", 1, 120m, bodyFont, brush, x, w, ref y);
            }
            else
            {
                foreach (var item in order!.Items)
                {
                    DrawItem(g, item.ProductName, item.Quantity, item.LineTotal, bodyFont, brush, x, w, ref y);
                }
            }

            y += 6;

            // ═══ Totals Separator ═══
            g.DrawLine(dashPen, x, y, x + w, y);
            y += 10;

            decimal subtotal = isSample ? 640m : order!.Subtotal;
            decimal total = isSample ? 640m : order!.Total;

            DrawPair(g, "Subtotal", Fmt(subtotal), smallFont, bodyFont, grayBrush, brush, x, w, ref y);

            // ── Grand Total (green bar) ──
            using var totalBg = new SolidBrush(Green);
            g.FillRectangle(totalBg, x, y, w, 28);
            g.DrawString("TOTAL", totalFont, Brushes.White, x + 8, y + 4);
            var totalStr = Fmt(total);
            var totalSz = g.MeasureString(totalStr, totalFont);
            g.DrawString(totalStr, totalFont, Brushes.White, x + w - totalSz.Width - 4, y + 4);
            y += 36;

            // ── Cash Tendered / Change ──
            decimal cashTendered = isSample ? 700m : order!.CashTendered;
            decimal changeGiven = isSample ? 60m : order!.ChangeGiven;

            if (cashTendered > 0)
            {
                DrawPair(g, "Cash Tendered", Fmt(cashTendered), smallFont, bodyFont, grayBrush, brush, x, w, ref y);
                DrawPair(g, "Change", Fmt(changeGiven), smallFont, headerFont, grayBrush, greenBrush, x, w, ref y);
            }

            y += 6;

            // ── Payment info ──
            g.DrawString($"Paid via {paymentMethod}", smallBold, grayBrush, cx, y, sf);
            y += 16;
            if (settings.ReceiptShowCashierName)
            {
                g.DrawString($"Served by {cashier}", smallFont, mutedBrush, cx, y, sf);
                y += 20;
            }
            else
            {
                y += 4;
            }

            // ═══ Footer Separator ═══
            g.DrawLine(dashPen, x, y, x + w, y);
            y += 10;

            // ── Footer message ──
            string footer = settings.ReceiptFooterMessage;
            if (string.IsNullOrWhiteSpace(footer)) footer = "Thank you for your purchase!";
            g.DrawString(footer, footerFont, mutedBrush, cx, y, sf);
            y += 20;

            return y;
        }

        /// <summary>
        /// Gets the display location, preferring CurrentOperatingLocation over Address.
        /// </summary>
        public static string GetDisplayLocation(StoreSettings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.CurrentOperatingLocation))
                return settings.CurrentOperatingLocation;
            return settings.Address;
        }

        private static void DrawPair(Graphics g, string label, string value, Font labelFont, Font valueFont, Brush labelBrush, Brush valueBrush, float x, float w, ref float y)
        {
            g.DrawString(label, labelFont, labelBrush, x, y);
            var sz = g.MeasureString(value, valueFont);
            g.DrawString(value, valueFont, valueBrush, x + w - sz.Width, y);
            y += 18;
        }

        private static void DrawItem(Graphics g, string name, int qty, decimal total, Font font, Brush brush, float x, float w, ref float y)
        {
            g.DrawString(name, font, brush, x, y);
            g.DrawString(qty.ToString(), font, brush, x + w - 95, y);
            var ts = g.MeasureString(Fmt(total), font);
            g.DrawString(Fmt(total), font, brush, x + w - ts.Width, y);
            y += 20;
        }

        private static string Fmt(decimal v) => $"₱{v:#,##0.00}";
    }
}
