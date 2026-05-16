using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace TheMatchaClubApp.Forms
{
    public partial class DashboardView
    {
        // ── Brand palette ──────────────────────────────────────────
        private static readonly Color BgColor = ColorTranslator.FromHtml("#FAFAFA");
        private static readonly Color CardBg = Color.White;
        private static readonly Color BorderLight = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color BorderCard = ColorTranslator.FromHtml("#F3F4F6");
        private static readonly Color TextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color TextSecondary = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color TextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color TextBody = ColorTranslator.FromHtml("#374151");
        private static readonly Color Green = ColorTranslator.FromHtml("#52B743");
        private static readonly Color GreenBg = ColorTranslator.FromHtml("#F2FAEF");
        private static readonly Color GreenBorder = ColorTranslator.FromHtml("#E2F3DD");
        private static readonly Color SearchBg = ColorTranslator.FromHtml("#F9FAFB");
        private static readonly Color BlueBg = ColorTranslator.FromHtml("#EFF6FF");
        private static readonly Color Blue = ColorTranslator.FromHtml("#3B82F6");
        private static readonly Color OrangeBg = ColorTranslator.FromHtml("#FFF7ED");
        private static readonly Color Orange = ColorTranslator.FromHtml("#F59E0B");
        private static readonly Color PurpleBg = ColorTranslator.FromHtml("#F5F3FF");
        private static readonly Color Purple = ColorTranslator.FromHtml("#8B5CF6");
        private static readonly Color CyanBg = ColorTranslator.FromHtml("#ECFEFF");
        private static readonly Color Cyan = ColorTranslator.FromHtml("#06B6D4");
        private static readonly Color RoseBg = ColorTranslator.FromHtml("#FFF1F2");
        private static readonly Color Rose = ColorTranslator.FromHtml("#F43F5E");
        private static readonly Color AmberBg = ColorTranslator.FromHtml("#FFFBEB");
        private static readonly Color Amber = ColorTranslator.FromHtml("#D97706");
        private static readonly Color TealBg = ColorTranslator.FromHtml("#F0FDFA");
        private static readonly Color Teal = ColorTranslator.FromHtml("#14B8A6");

        // Card icon config: (bgColor, fgColor, symbol)
        private static readonly (Color bg, Color fg, string sym)[] CardIcons = {
            (GreenBg, Green, "$"), (BlueBg, Blue, "🛒"), (OrangeBg, Orange, "📈"), (PurpleBg, Purple, "💰"),
            (CyanBg, Cyan, "📦"), (RoseBg, Rose, "⭐"), (AmberBg, Amber, "⏱"), (TealBg, Teal, "📊")
        };

        private void InitializeDesign()
        {
            this.BackColor = BgColor;
            this.Dock = DockStyle.Fill;

            // ── Header ────────────────────────────────────────────
            pnlTopHeader.BackColor = CardBg;
            pnlTopHeader.Paint += (s, e) => { using var p = new Pen(BorderLight); e.Graphics.DrawLine(p, 0, pnlTopHeader.Height - 1, pnlTopHeader.Width, pnlTopHeader.Height - 1); };
            lblChevron.Font = new Font("Segoe UI", 8F); lblChevron.ForeColor = TextMuted; lblChevron.BackColor = Color.Transparent;
            lblViewName.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold); lblViewName.ForeColor = TextPrimary; lblViewName.BackColor = Color.Transparent;
            pnlStoreStatus.BackColor = Color.Transparent; pnlStoreStatus.FillColor = GreenBg; pnlStoreStatus.BorderColor = GreenBorder;
            pnlStoreStatus.BorderThickness = 1; pnlStoreStatus.BorderRadius = 11;
            pnlStoreStatus.Paint += (s, e) => { 
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; 
                using var b = new SolidBrush(lblStoreStatus.ForeColor); 
                e.Graphics.FillEllipse(b, 8, 8, 6, 6); 
            };
            lblStoreStatus.Font = new Font("Segoe UI", 7F, FontStyle.Bold); lblStoreStatus.ForeColor = Green; lblStoreStatus.BackColor = Color.Transparent;
            lblDate.Font = new Font("Segoe UI", 9F); lblDate.ForeColor = TextBody; lblDate.BackColor = Color.Transparent;
            lblDate.Text = "📅 " + DateTime.Now.ToString("M/d/yyyy");
            btnNotification.FillColor = Color.Transparent; btnNotification.ForeColor = TextBody; btnNotification.BorderThickness = 0;
            btnNotification.Font = new Font("Segoe UI", 12F); btnNotification.HoverState.FillColor = BorderCard;
            picAvatar.SizeMode = PictureBoxSizeMode.StretchImage; picAvatar.BackColor = Color.Transparent;
            picAvatar.Paint += PicAvatar_Paint;

            // ── Quick Actions (PRIMARY → SECONDARY → TERTIARY) ────
            pnlQuickActions.BackColor = Color.Transparent;
            StyleActionBtn(btnQuickNewSale, Green, true);
            btnQuickNewSale.Size = new Size(140, 32);
            btnQuickNewSale.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            StyleActionBtn(btnQuickOpenSession, Green, false);
            StyleActionBtn(btnQuickCloseSession, Orange, false);
            StyleActionBtn(btnQuickReports, Purple, false);
            // Tertiary: muted fill, subtle text
            btnQuickAddProduct.BorderRadius = 8;
            btnQuickAddProduct.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnQuickAddProduct.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            btnQuickAddProduct.ForeColor = TextSecondary;
            btnQuickAddProduct.BorderThickness = 0;
            btnQuickAddProduct.HoverState.FillColor = ColorTranslator.FromHtml("#E5E7EB");

            // ── KPI Cards ─────────────────────────────────────────
            var cards = GetAllCards();
            for (int i = 0; i < cards.Length; i++)
            {
                var (card, title, value) = cards[i];
                StyleCard(card, title, value);
                int idx = i;
                card.Paint += (s, e) => PaintCardIcon(e.Graphics, idx);
            }

            // ── Analytics Panels ──────────────────────────────────
            StyleAnalyticsPanel(pnlHourlySales, lblHourlySalesTitle);
            StyleAnalyticsPanel(pnlTopProducts, lblTopProductsTitle);
            StyleAnalyticsPanel(pnlRecentTx, lblRecentTxTitle);
            StyleAnalyticsPanel(pnlSessionStatus, lblSessionStatusTitle);

            pnlHourlySales.Paint += PnlHourlySales_Paint;
            pnlTopProducts.Paint += PnlTopProducts_Paint;
            pnlRecentTx.Paint += PnlRecentTx_Paint;
            pnlSessionStatus.Paint += PnlSessionStatus_Paint;

            // ── Empty State ───────────────────────────────────────
            pnlEmptyState.BackColor = Color.Transparent; pnlEmptyState.FillColor = CardBg;
            pnlEmptyState.BorderRadius = 16; pnlEmptyState.BorderColor = BorderCard; pnlEmptyState.BorderThickness = 1;
            pnlEmptyState.ShadowDecoration.Enabled = false;
            lblEmptyIcon.Font = new Font("Segoe UI", 42F); lblEmptyIcon.ForeColor = TextMuted; lblEmptyIcon.BackColor = Color.Transparent;
            lblEmptyMessage.Font = new Font("Segoe UI", 12F); lblEmptyMessage.ForeColor = TextSecondary; lblEmptyMessage.BackColor = Color.Transparent;
            btnEmptyAction.FillColor = Green; btnEmptyAction.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnEmptyAction.BorderRadius = 12; btnEmptyAction.ForeColor = Color.White;
            btnEmptyAction.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnEmptyAction.Size = new Size(180, 44);

            // ── Resize ────────────────────────────────────────────
            this.Resize += (s, e) => ResizeLayout();
            ResizeLayout();
        }

        private (Guna2Panel card, Label title, Label value)[] GetAllCards()
        {
            return new[] {
                (pnlCard1, lblCard1Title, lblCard1Value), (pnlCard2, lblCard2Title, lblCard2Value),
                (pnlCard3, lblCard3Title, lblCard3Value), (pnlCard4, lblCard4Title, lblCard4Value),
                (pnlCard5, lblCard5Title, lblCard5Value), (pnlCard6, lblCard6Title, lblCard6Value),
                (pnlCard7, lblCard7Title, lblCard7Value), (pnlCard8, lblCard8Title, lblCard8Value)
            };
        }

        private void StyleCard(Guna2Panel card, Label title, Label value)
        {
            card.BackColor = Color.Transparent; card.FillColor = CardBg; card.BorderRadius = 14;
            card.BorderColor = BorderCard; card.BorderThickness = 1;
            card.ShadowDecoration.Enabled = true; card.ShadowDecoration.Depth = 6;
            card.ShadowDecoration.Color = Color.FromArgb(8, 0, 0, 0);
            title.Font = new Font("Segoe UI", 7.5F); title.ForeColor = TextMuted; title.BackColor = Color.Transparent;
            value.Font = new Font("Segoe UI", 18F, FontStyle.Bold); value.ForeColor = TextPrimary; value.BackColor = Color.Transparent;
        }

        private void StyleActionBtn(Guna2Button btn, Color accent, bool filled)
        {
            btn.BorderRadius = 8; btn.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btn.BorderThickness = filled ? 0 : 1; btn.BorderColor = accent;
            btn.FillColor = filled ? accent : Color.Transparent;
            btn.ForeColor = filled ? Color.White : accent;
            btn.HoverState.FillColor = filled ? Color.FromArgb(220, accent) : Color.FromArgb(15, accent);
        }

        private void StyleAnalyticsPanel(Guna2Panel pnl, Label title)
        {
            pnl.BackColor = Color.Transparent; pnl.FillColor = CardBg; pnl.BorderRadius = 14;
            pnl.BorderColor = BorderCard; pnl.BorderThickness = 1;
            pnl.ShadowDecoration.Enabled = true; pnl.ShadowDecoration.Depth = 4;
            pnl.ShadowDecoration.Color = Color.FromArgb(6, 0, 0, 0);
            title.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            title.ForeColor = TextBody; title.BackColor = Color.Transparent;
        }

        // ── Responsive Layout ─────────────────────────────────────
        private void ResizeLayout()
        {
            int m = 24, gap = 12;
            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;
            if (w < 100 || h < 100) return;

            RepositionHeader();

            // Quick actions row
            int qaY = 72;
            pnlQuickActions.Location = new Point(m, qaY);
            pnlQuickActions.Size = new Size(w - m * 2, 36);
            LayoutActionButtons();

            // KPI cards: 2 rows of 4
            int kpiY = qaY + 46;
            int cardW = Math.Max(160, (w - m * 2 - gap * 3) / 4);
            int cardH = 76;
            var cards = GetAllCards();
            for (int i = 0; i < 8; i++)
            {
                int row = i / 4, col = i % 4;
                cards[i].card.Location = new Point(m + col * (cardW + gap), kpiY + row * (cardH + gap));
                cards[i].card.Size = new Size(cardW, cardH);
                cards[i].title.Location = new Point(52, 14);
                cards[i].value.Location = new Point(52, 34);
                cards[i].value.MaximumSize = new Size(cardW - 64, 32);
            }

            // Analytics area
            int analyticsY = kpiY + cardH * 2 + gap * 2 + 10;
            int analyticsH = Math.Max(200, h - analyticsY - 8);
            int rightW = Math.Max(260, Math.Min(340, (int)(w * 0.30)));
            int leftW = w - m * 2 - gap - rightW;

            // Left column panels
            int leftX = m;
            int chartH = Math.Max(120, (int)(analyticsH * 0.42));
            int topProdH = Math.Max(100, (int)(analyticsH * 0.29));
            int recentH = Math.Max(100, analyticsH - chartH - topProdH - gap * 2);

            pnlHourlySales.Location = new Point(leftX, analyticsY);
            pnlHourlySales.Size = new Size(leftW, chartH);
            pnlTopProducts.Location = new Point(leftX, analyticsY + chartH + gap);
            pnlTopProducts.Size = new Size(leftW, topProdH);
            pnlRecentTx.Location = new Point(leftX, analyticsY + chartH + topProdH + gap * 2);
            pnlRecentTx.Size = new Size(leftW, recentH);

            // Right column
            int rightX = leftX + leftW + gap;
            pnlSessionStatus.Location = new Point(rightX, analyticsY);
            pnlSessionStatus.Size = new Size(rightW, analyticsH);

            // Empty state overlay
            pnlEmptyState.Location = new Point(m, analyticsY);
            pnlEmptyState.Size = new Size(w - m * 2, analyticsH);
            CenterEmptyState();

            // Invalidate painted panels
            pnlHourlySales.Invalidate();
            pnlTopProducts.Invalidate();
            pnlRecentTx.Invalidate();
            pnlSessionStatus.Invalidate();
        }

        private void LayoutActionButtons()
        {
            int x = 0, gap = 8;
            foreach (Control c in pnlQuickActions.Controls)
            {
                if (c is Guna2Button btn) { btn.Location = new Point(x, 0); btn.Height = 32; x += btn.Width + gap; }
            }
        }

        private void CenterEmptyState()
        {
            int cw = pnlEmptyState.Width, ch = pnlEmptyState.Height;
            lblEmptyIcon.Location = new Point((cw - lblEmptyIcon.Width) / 2, ch / 2 - 80);
            lblEmptyMessage.Location = new Point((cw - lblEmptyMessage.Width) / 2, ch / 2 - 10);
            btnEmptyAction.Location = new Point((cw - btnEmptyAction.Width) / 2, ch / 2 + 40);
        }

        private void RepositionHeader()
        {
            int w = this.ClientSize.Width;
            int rightEdge = w - 16;
            rightEdge -= 32; picAvatar.Location = new Point(rightEdge, 16);
            rightEdge -= 40; btnNotification.Location = new Point(rightEdge, 16);
            rightEdge -= 108; lblDate.Location = new Point(rightEdge, 20);
            rightEdge -= 108; pnlStoreStatus.Location = new Point(rightEdge, 21);
        }

        private void PaintCardIcon(Graphics g, int index)
        {
            if (index < 0 || index >= CardIcons.Length) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var (bg, fg, sym) = CardIcons[index];
            using var bgBrush = new SolidBrush(bg);
            g.FillEllipse(bgBrush, 8, 16, 34, 34);
            bool isEmoji = sym.Length > 1;
            using var font = isEmoji ? new Font("Segoe UI Emoji", 11F) : new Font("Segoe UI", 13F, FontStyle.Bold);
            using var textBrush = new SolidBrush(fg);
            var sz = g.MeasureString(sym, font);
            g.DrawString(sym, font, textBrush, 8 + (34 - sz.Width) / 2, 16 + (34 - sz.Height) / 2);
        }

        private void PicAvatar_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            using var bg = new SolidBrush(ColorTranslator.FromHtml("#E0E7FF"));
            g.FillEllipse(bg, 0, 0, 31, 31);
            using var f = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            using var tb = new SolidBrush(ColorTranslator.FromHtml("#4F46E5"));
            string initials = Program.CurrentUser?.FullName?.Substring(0, 1).ToUpper() ?? "A";
            var sz = g.MeasureString(initials, f);
            g.DrawString(initials, f, tb, (32 - sz.Width) / 2, (32 - sz.Height) / 2);
        }

        private static GraphicsPath CreateRoundRectPath(Rectangle rect, int radius)
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
