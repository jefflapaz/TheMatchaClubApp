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
        // ── Brand palette ──────────────────────────────────────────────
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

        private void InitializeDesign()
        {
            // ── UserControl itself ─────────────────────────────────────
            this.BackColor = BgColor;
            this.Dock = DockStyle.Fill;

            // ════════════════════════════════════════════════════════════
            //  TOP HEADER BAR
            // ════════════════════════════════════════════════════════════
            pnlTopHeader.BackColor = CardBg;
            pnlTopHeader.Paint += PnlTopHeader_Paint;  // draws the bottom border line

            // Chevron
            lblChevron.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            lblChevron.ForeColor = TextMuted;
            lblChevron.TextAlign = ContentAlignment.MiddleLeft;
            lblChevron.BackColor = Color.Transparent;

            // View name
            lblViewName.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblViewName.ForeColor = TextPrimary;
            lblViewName.TextAlign = ContentAlignment.MiddleLeft;
            lblViewName.BackColor = Color.Transparent;

            // ── Search bar ─────────────────────────────────────────────
            txtSearch.BorderRadius = 20;
            txtSearch.BorderColor = BorderLight;
            txtSearch.FillColor = SearchBg;
            txtSearch.BackColor = Color.Transparent;
            txtSearch.ForeColor = TextBody;
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.PlaceholderForeColor = TextMuted;
            txtSearch.TextOffset = new Point(4, 0);
            txtSearch.Paint += TxtSearch_Paint;  // Draws "Ctrl K" hint on the right side

            // ── Store status pill ──────────────────────────────────────
            pnlStoreStatus.BackColor = Color.Transparent;
            pnlStoreStatus.FillColor = GreenBg;
            pnlStoreStatus.BorderColor = GreenBorder;
            pnlStoreStatus.BorderThickness = 1;
            pnlStoreStatus.BorderRadius = 11;
            pnlStoreStatus.Paint += PnlStoreStatus_Paint; // green dot

            lblStoreStatus.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblStoreStatus.ForeColor = Green;
            lblStoreStatus.BackColor = Color.Transparent;
            lblStoreStatus.TextAlign = ContentAlignment.MiddleCenter;

            // ── Date label ─────────────────────────────────────────────
            lblDate.Font = new Font("Segoe UI", 9F);
            lblDate.ForeColor = TextBody;
            lblDate.TextAlign = ContentAlignment.MiddleLeft;
            lblDate.BackColor = Color.Transparent;
            lblDate.Text = "📅 " + DateTime.Now.ToString("M/d/yyyy");

            // ── Notification bell ──────────────────────────────────────
            btnNotification.FillColor = Color.Transparent;
            btnNotification.ForeColor = TextBody;
            btnNotification.BorderThickness = 0;
            btnNotification.Font = new Font("Segoe UI", 12F);
            btnNotification.HoverState.FillColor = BorderCard;
            btnNotification.Paint += BtnNotification_Paint;  // red badge dot

            // ── New Order button ───────────────────────────────────────
            btnNewOrder.FillColor = Green;
            btnNewOrder.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnNewOrder.PressedColor = ColorTranslator.FromHtml("#3D8F32");
            btnNewOrder.BorderRadius = 8;
            btnNewOrder.ForeColor = Color.White;
            btnNewOrder.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            // ── Avatar ─────────────────────────────────────────────────
            picAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
            picAvatar.BackColor = Color.Transparent;
            // Generate a placeholder avatar via OnPaint
            picAvatar.Paint += PicAvatar_Paint;

            // ════════════════════════════════════════════════════════════
            //  KPI STAT CARDS — shared styling helper
            // ════════════════════════════════════════════════════════════
            SetupStatCard(pnlCard1, pnlCard1Icon, lblCard1Title, lblCard1Value);
            SetupStatCard(pnlCard2, pnlCard2Icon, lblCard2Title, lblCard2Value);
            SetupStatCard(pnlCard3, pnlCard3Icon, lblCard3Title, lblCard3Value);
            SetupStatCard(pnlCard4, pnlCard4Icon, lblCard4Title, lblCard4Value);

            // Card-specific badge styling
            SetupBadge(pnlCard1Badge, lblCard1Badge, "↑ +14.2%");
            SetupBadge(pnlCard2Badge, lblCard2Badge, "↑ +8.1%");

            // Icon paint handlers
            pnlCard1Icon.Paint += Card1Icon_Paint;  // dollar
            pnlCard2Icon.Paint += Card2Icon_Paint;  // shopping bag
            pnlCard3Icon.Paint += Card3Icon_Paint;  // trending up
            pnlCard4Icon.Paint += Card4Icon_Paint;  // wallet

            // ════════════════════════════════════════════════════════════
            //  CHART PLACEHOLDER
            // ════════════════════════════════════════════════════════════
            pnlChart.BackColor = Color.Transparent;
            pnlChart.FillColor = CardBg;
            pnlChart.BorderRadius = 16;
            pnlChart.BorderColor = BorderCard;
            pnlChart.BorderThickness = 1;
            pnlChart.ShadowDecoration.Enabled = false;
            pnlChart.Paint += PnlChart_Paint;  // bar chart icon

            lblChartMessage.Font = new Font("Segoe UI", 9F);
            lblChartMessage.ForeColor = TextSecondary;
            lblChartMessage.TextAlign = ContentAlignment.MiddleCenter;
            lblChartMessage.BackColor = Color.Transparent;
            // Push text down to leave room for the painted icon above center
            lblChartMessage.Padding = new Padding(40, 40, 40, 0);

            // ════════════════════════════════════════════════════════════
            //  RESIZE WIRING
            // ════════════════════════════════════════════════════════════
            this.Resize += DashboardView_Resize;

            // Fire initial layout
            ResizeCards();
            RepositionHeader();
        }

        // ────────────────────────────────────────────────────────────────
        //  HELPER: shared stat card styling
        // ────────────────────────────────────────────────────────────────
        private void SetupStatCard(Guna2Panel card, Panel iconPanel, Label titleLbl, Label valueLbl)
        {
            card.BackColor = Color.Transparent;
            card.FillColor = CardBg;
            card.BorderRadius = 16;
            card.BorderColor = BorderCard;
            card.BorderThickness = 1;

            card.ShadowDecoration.Enabled = true;
            card.ShadowDecoration.Depth = 8;
            card.ShadowDecoration.Color = Color.FromArgb(8, 0, 0, 0);

            iconPanel.BackColor = Color.Transparent;

            titleLbl.AutoSize = true;
            titleLbl.Font = new Font("Segoe UI", 8F);
            titleLbl.ForeColor = TextSecondary;
            titleLbl.BackColor = Color.Transparent;

            valueLbl.AutoSize = true;
            valueLbl.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            valueLbl.ForeColor = TextPrimary;
            valueLbl.BackColor = Color.Transparent;
        }

        private void SetupBadge(Guna2Panel badgePanel, Label badgeLabel, string text)
        {
            badgePanel.BackColor = Color.Transparent;
            badgePanel.FillColor = GreenBg;
            badgePanel.BorderRadius = 9;
            badgePanel.BorderColor = GreenBorder;
            badgePanel.BorderThickness = 0;
            badgePanel.ShadowDecoration.Enabled = false;

            badgeLabel.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            badgeLabel.ForeColor = Green;
            badgeLabel.BackColor = Color.Transparent;
            badgeLabel.Text = text;
            badgeLabel.TextAlign = ContentAlignment.MiddleCenter;
        }

        // ────────────────────────────────────────────────────────────────
        //  RESIZE LOGIC
        // ────────────────────────────────────────────────────────────────
        private void DashboardView_Resize(object? sender, EventArgs e)
        {
            ResizeCards();
            RepositionHeader();
        }

        private void ResizeCards()
        {
            int margin = 24;
            int gap = 16;
            int availableWidth = this.ClientSize.Width - (margin * 2) - (gap * 3);
            int cardWidth = Math.Max(160, availableWidth / 4);
            int cardHeight = 135; // Increased from 110 to fit the 16pt font + padding
            int topOffset = 88; // below the 64px header + 24px margin

            pnlCard1.Location = new Point(margin, topOffset);
            pnlCard1.Size = new Size(cardWidth, cardHeight);

            pnlCard2.Location = new Point(margin + cardWidth + gap, topOffset);
            pnlCard2.Size = new Size(cardWidth, cardHeight);

            pnlCard3.Location = new Point(margin + (cardWidth + gap) * 2, topOffset);
            pnlCard3.Size = new Size(cardWidth, cardHeight);

            pnlCard4.Location = new Point(margin + (cardWidth + gap) * 3, topOffset);
            pnlCard4.Size = new Size(cardWidth, cardHeight);

            // Reposition badge to top-right of each card
            RepositionBadge(pnlCard1, pnlCard1Badge);
            RepositionBadge(pnlCard2, pnlCard2Badge);

            // Chart panel fills remaining space
            int chartTop = topOffset + cardHeight + margin;
            int chartHeight = Math.Max(120, this.ClientSize.Height - chartTop - margin);
            pnlChart.Location = new Point(margin, chartTop);
            pnlChart.Size = new Size(this.ClientSize.Width - margin * 2, chartHeight);
        }

        private void RepositionBadge(Guna2Panel card, Guna2Panel badge)
        {
            badge.Location = new Point(card.Width - badge.Width - 16, 16);
        }

        private void RepositionHeader()
        {
            int w = this.ClientSize.Width;

            // Right-side controls: avatar, new order, notification, date, store pill
            int rightEdge = w - 16;

            // Avatar
            rightEdge -= 32;
            picAvatar.Location = new Point(rightEdge, 16);

            // New Order button
            rightEdge -= (100 + 8);
            btnNewOrder.Location = new Point(rightEdge, 16);

            // Notification bell
            rightEdge -= (32 + 8);
            btnNotification.Location = new Point(rightEdge, 16);

            // Date
            rightEdge -= (100 + 8);
            lblDate.Location = new Point(rightEdge, 20);

            // Store status pill
            rightEdge -= (100 + 8);
            pnlStoreStatus.Location = new Point(rightEdge, 21);

            // Search bar — centered, but constrained so it doesn't overlap
            int searchX = (w - txtSearch.Width) / 2;
            int minSearchX = lblViewName.Right + 16;
            
            if (searchX + txtSearch.Width + 16 > pnlStoreStatus.Left && pnlStoreStatus.Left > 0)
            {
                // Push it to the left if it overlaps the right controls
                searchX = pnlStoreStatus.Left - txtSearch.Width - 16;
            }

            // Ensure it doesn't overlap the left title
            if (searchX < minSearchX) 
            {
                searchX = minSearchX;
            }

            txtSearch.Location = new Point(searchX, 16);
        }

        // ────────────────────────────────────────────────────────────────
        //  PAINT HANDLERS
        // ────────────────────────────────────────────────────────────────

        /// <summary>Draws a 1px bottom border on the header.</summary>
        private void PnlTopHeader_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(BorderLight, 1);
            int y = pnlTopHeader.Height - 1;
            e.Graphics.DrawLine(pen, 0, y, pnlTopHeader.Width, y);
        }

        /// <summary>Draws "Ctrl K" shortcut boxes on the right side of the search bar.</summary>
        private void TxtSearch_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int boxH = 18, boxY = (txtSearch.Height - boxH) / 2;

            using var bgBrush = new SolidBrush(BorderCard);
            using var borderPen = new Pen(BorderLight, 1);
            using var font = new Font("Segoe UI", 7F, FontStyle.Bold);
            using var textBrush = new SolidBrush(TextMuted);

            // "Ctrl" box
            var ctrlSize = g.MeasureString("Ctrl", font);
            int ctrlW = (int)ctrlSize.Width + 8;
            int ctrlX = txtSearch.Width - ctrlW - 32 - 8;
            var ctrlRect = new Rectangle(ctrlX, boxY, ctrlW, boxH);
            DrawRoundRect(g, ctrlRect, 4, bgBrush, borderPen);
            g.DrawString("Ctrl", font, textBrush, ctrlX + 4, boxY + 2);

            // "K" box
            var kSize = g.MeasureString("K", font);
            int kW = (int)kSize.Width + 8;
            int kX = ctrlX + ctrlW + 4;
            var kRect = new Rectangle(kX, boxY, kW, boxH);
            DrawRoundRect(g, kRect, 4, bgBrush, borderPen);
            g.DrawString("K", font, textBrush, kX + 4, boxY + 2);
        }

        /// <summary>Draws the green dot on the store status pill.</summary>
        private void PnlStoreStatus_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(Green);
            e.Graphics.FillEllipse(brush, new Rectangle(8, 8, 6, 6));
        }

        /// <summary>Red notification badge on the bell button.</summary>
        private void BtnNotification_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(Color.FromArgb(239, 68, 68)); // red-500
            e.Graphics.FillEllipse(brush, new Rectangle(btnNotification.Width - 12, 2, 8, 8));
        }

        /// <summary>Draws a placeholder avatar circle with initials.</summary>
        private void PicAvatar_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Background circle
            using var bgBrush = new SolidBrush(ColorTranslator.FromHtml("#E0E7FF"));
            g.FillEllipse(bgBrush, 0, 0, 31, 31);

            // Initials
            using var font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            using var textBrush = new SolidBrush(ColorTranslator.FromHtml("#4F46E5"));
            var size = g.MeasureString("A", font);
            g.DrawString("A", font, textBrush, (32 - size.Width) / 2, (32 - size.Height) / 2);
        }

        // ── Card icon painters ─────────────────────────────────────────

        /// <summary>Card 1: Dollar sign icon with green background circle.</summary>
        private void Card1Icon_Paint(object? sender, PaintEventArgs e)
        {
            PaintIconCircle(e.Graphics, GreenBg, "$", Green);
        }

        /// <summary>Card 2: Shopping bag icon.</summary>
        private void Card2Icon_Paint(object? sender, PaintEventArgs e)
        {
            PaintIconCircle(e.Graphics, ColorTranslator.FromHtml("#EFF6FF"), "🛒", ColorTranslator.FromHtml("#3B82F6"));
        }

        /// <summary>Card 3: Trending up icon.</summary>
        private void Card3Icon_Paint(object? sender, PaintEventArgs e)
        {
            PaintIconCircle(e.Graphics, ColorTranslator.FromHtml("#FFF7ED"), "📈", ColorTranslator.FromHtml("#F59E0B"));
        }

        /// <summary>Card 4: Wallet icon.</summary>
        private void Card4Icon_Paint(object? sender, PaintEventArgs e)
        {
            PaintIconCircle(e.Graphics, ColorTranslator.FromHtml("#F5F3FF"), "💰", ColorTranslator.FromHtml("#8B5CF6"));
        }

        private void PaintIconCircle(Graphics g, Color bgColor, string symbol, Color fgColor)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Background circle
            using var bgBrush = new SolidBrush(bgColor);
            g.FillEllipse(bgBrush, 0, 0, 39, 39);

            // Symbol
            bool isEmoji = symbol.Length > 1;
            using var font = isEmoji ? new Font("Segoe UI Emoji", 14F) : new Font("Segoe UI", 16F, FontStyle.Bold);
            using var textBrush = new SolidBrush(fgColor);
            var size = g.MeasureString(symbol, font);
            g.DrawString(symbol, font, textBrush, (40 - size.Width) / 2, (40 - size.Height) / 2);
        }

        /// <summary>Draws a bar chart icon centered in the chart panel.</summary>
        private void PnlChart_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int iconSize = 48;
            int cx = (pnlChart.Width - iconSize) / 2;
            int cy = (pnlChart.Height - iconSize) / 2 - 20; // shift up so text sits below

            using var barBrush = new SolidBrush(Color.FromArgb(50, TextMuted));
            using var barAccent = new SolidBrush(Color.FromArgb(100, Green));

            // Draw 4 bars of varying height
            int barW = 8, gap = 4;
            int baseY = cy + iconSize;
            int[] heights = { 20, 32, 24, 40 };
            Brush[] brushes = { barBrush, barAccent, barBrush, barAccent };
            int startX = cx + (iconSize - (barW * 4 + gap * 3)) / 2;

            for (int i = 0; i < 4; i++)
            {
                int x = startX + i * (barW + gap);
                int h = heights[i];
                var rect = new Rectangle(x, baseY - h, barW, h);
                using var path = CreateRoundRectPath(rect, 3);
                g.FillPath(brushes[i], path);
            }
        }

        // ────────────────────────────────────────────────────────────────
        //  DRAWING HELPERS
        // ────────────────────────────────────────────────────────────────
        private void DrawRoundRect(Graphics g, Rectangle rect, int radius, Brush fill, Pen border)
        {
            using var path = CreateRoundRectPath(rect, radius);
            g.FillPath(fill, path);
            g.DrawPath(border, path);
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
