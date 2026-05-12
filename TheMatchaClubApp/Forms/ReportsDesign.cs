using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace TheMatchaClubApp.Forms
{
    public partial class ReportsView
    {
        private static readonly Color RBg = ColorTranslator.FromHtml("#FAFAFA");
        private static readonly Color RCard = Color.White;
        private static readonly Color RBorder = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color RTextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color RTextSecondary = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color RTextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color RGreen = ColorTranslator.FromHtml("#52B743");
        private static readonly Color RGreenBg = ColorTranslator.FromHtml("#F2FAEF");
        private static readonly Color RRedBg = ColorTranslator.FromHtml("#FEF2F2");
        private static readonly Color RRed = ColorTranslator.FromHtml("#EF4444");

        private void InitializeDesign()
        {
            this.BackColor = RBg;
            this.Dock = DockStyle.Fill;

            // Top section
            pnlTopSection.BackColor = RBg;
            lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTitle.ForeColor = RTextPrimary;
            lblTitle.BackColor = Color.Transparent;
            lblSubTitle.Font = new Font("Segoe UI", 9F);
            lblSubTitle.ForeColor = RTextSecondary;
            lblSubTitle.BackColor = Color.Transparent;

            // Filter tabs
            pnlFilterTabs.BackColor = Color.Transparent;
            StyleFilterBtn(btnToday, true);
            StyleFilterBtn(btnYesterday, false);
            StyleFilterBtn(btnThisWeek, false);
            StyleFilterBtn(btnCustomDate, false);

            btnExportCsv.FillColor = RCard;
            btnExportCsv.ForeColor = RTextSecondary;
            btnExportCsv.BorderColor = RBorder;
            btnExportCsv.BorderRadius = 8;
            btnExportCsv.BorderThickness = 1;
            btnExportCsv.Font = new Font("Segoe UI", 8F);

            // KPI cards
            pnlKpiRow.BackColor = RBg;
            CreateKpiCards();

            // Table card
            pnlTableCard.BackColor = Color.Transparent;
            pnlTableCard.FillColor = RCard;
            pnlTableCard.BorderRadius = 16;
            pnlTableCard.BorderColor = ColorTranslator.FromHtml("#F3F4F6");
            pnlTableCard.BorderThickness = 1;
            pnlTableCard.ShadowDecoration.Enabled = false;
            pnlTableCard.Padding = new Padding(0, 0, 0, 0);

            lblTableHeader.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblTableHeader.ForeColor = RTextPrimary;
            lblTableHeader.BackColor = Color.Transparent;

            lblViewAll.Font = new Font("Segoe UI", 9F);
            lblViewAll.ForeColor = RGreen;
            lblViewAll.BackColor = Color.Transparent;
            lblViewAll.Cursor = Cursors.Hand;

            pnlTableInner.BackColor = Color.Transparent;
            pnlTableInner.Paint += PnlTableInner_Paint;

            // ── Closeout sidebar ──
            pnlCloseoutSidebar.BackColor = Color.Transparent;
            pnlCloseoutSidebar.FillColor = RCard;
            pnlCloseoutSidebar.BorderThickness = 0;
            pnlCloseoutSidebar.ShadowDecoration.Enabled = false;
            pnlCloseoutSidebar.Paint += (s, e) =>
            {
                using var pen = new Pen(RBorder, 1);
                e.Graphics.DrawLine(pen, 0, 0, 0, pnlCloseoutSidebar.Height);
            };

            pnlCloseoutHeader.BackColor = RCard;
            pnlCloseoutHeader.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Orange dot
                using var dotBrush = new SolidBrush(ColorTranslator.FromHtml("#F59E0B"));
                g.FillEllipse(dotBrush, 16, 22, 8, 8);
                using var pen = new Pen(ColorTranslator.FromHtml("#F3F4F6"), 1);
                g.DrawLine(pen, 0, pnlCloseoutHeader.Height - 1, pnlCloseoutHeader.Width, pnlCloseoutHeader.Height - 1);
            };

            lblCloseoutTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCloseoutTitle.ForeColor = RTextPrimary;
            lblCloseoutTitle.BackColor = Color.Transparent;

            lblExpectedCash.Font = new Font("Segoe UI", 9F);
            lblExpectedCash.ForeColor = RTextSecondary;
            lblExpectedCash.BackColor = Color.Transparent;
            lblExpectedCashValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblExpectedCashValue.ForeColor = RTextPrimary;
            lblExpectedCashValue.BackColor = Color.Transparent;

            lblDrawerFund.Font = new Font("Segoe UI", 9F);
            lblDrawerFund.ForeColor = RTextSecondary;
            lblDrawerFund.BackColor = Color.Transparent;
            lblDrawerFundValue.Font = new Font("Segoe UI", 9F);
            lblDrawerFundValue.ForeColor = RTextPrimary;
            lblDrawerFundValue.BackColor = Color.Transparent;

            lblActualCashLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblActualCashLabel.ForeColor = RTextMuted;
            lblActualCashLabel.BackColor = Color.Transparent;

            txtActualCash.BorderRadius = 8;
            txtActualCash.BorderColor = RBorder;
            txtActualCash.FocusedState.BorderColor = RGreen;
            txtActualCash.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            txtActualCash.ForeColor = RTextPrimary;

            pnlInfoBox.BackColor = Color.Transparent;
            pnlInfoBox.FillColor = ColorTranslator.FromHtml("#EFF6FF");
            pnlInfoBox.BorderColor = ColorTranslator.FromHtml("#DBEAFE");
            pnlInfoBox.BorderRadius = 12;
            pnlInfoBox.BorderThickness = 1;
            pnlInfoBox.ShadowDecoration.Enabled = false;
            lblInfoText.Font = new Font("Segoe UI", 8F);
            lblInfoText.ForeColor = ColorTranslator.FromHtml("#2563EB");
            lblInfoText.BackColor = Color.Transparent;

            btnCloseDay.FillColor = ColorTranslator.FromHtml("#98D88A");
            btnCloseDay.HoverState.FillColor = ColorTranslator.FromHtml("#86CD77");
            btnCloseDay.ForeColor = Color.White;
            btnCloseDay.BorderRadius = 12;
            btnCloseDay.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCloseDay.BorderThickness = 0;

            // Nav cards
            StyleNavCard(pnlNavTaxes, lblNavTaxes);
            StyleNavCard(pnlNavPrevReports, lblNavPrevReports);
        }

        private void StyleFilterBtn(Guna2Button btn, bool active)
        {
            btn.BorderRadius = 8;
            btn.Font = new Font("Segoe UI", 8F, active ? FontStyle.Bold : FontStyle.Regular);
            btn.FillColor = active ? RCard : Color.Transparent;
            btn.ForeColor = active ? RTextPrimary : RTextSecondary;
            btn.BorderThickness = active ? 1 : 0;
            btn.BorderColor = RBorder;
        }

        private void StyleNavCard(Guna2Panel pnl, Label lbl)
        {
            pnl.BackColor = Color.Transparent;
            pnl.FillColor = RCard;
            pnl.BorderRadius = 12;
            pnl.BorderColor = RBorder;
            pnl.BorderThickness = 1;
            pnl.ShadowDecoration.Enabled = false;
            pnl.Cursor = Cursors.Hand;
            lbl.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lbl.ForeColor = RTextPrimary;
            lbl.BackColor = Color.Transparent;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void CreateKpiCards()
        {
            var kpis = new[]
            {
                ("Total Sales", "$3,429.50", "\u2191 12.5%", true),
                ("Total Orders", "142", "\u2191 8.2%", true),
                ("Avg Order Value", "$24.15", "\u2193 2.1%", false),
                ("Net Profit", "$1,842.10", "\u2191 14.3%", true)
            };

            foreach (var (title, value, badge, isUp) in kpis)
            {
                var card = new Guna2Panel
                {
                    Size = new Size(148, 80),
                    Margin = new Padding(4),
                    BackColor = Color.Transparent,
                    FillColor = RCard,
                    BorderRadius = 12,
                    BorderColor = ColorTranslator.FromHtml("#F3F4F6"),
                    BorderThickness = 1
                };
                card.ShadowDecoration.Enabled = false;

                var lblT = new Label
                {
                    Text = title,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = RTextSecondary,
                    BackColor = Color.Transparent,
                    Location = new Point(12, 10),
                    Size = new Size(120, 16)
                };

                var lblV = new Label
                {
                    Text = value,
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = RTextPrimary,
                    BackColor = Color.Transparent,
                    Location = new Point(12, 28),
                    Size = new Size(120, 24),
                    AutoSize = true
                };

                var badgePanel = new Guna2Panel
                {
                    Size = new Size(64, 18),
                    Location = new Point(12, 56),
                    BackColor = Color.Transparent,
                    FillColor = isUp ? RGreenBg : RRedBg,
                    BorderRadius = 9,
                    BorderThickness = 0
                };
                badgePanel.ShadowDecoration.Enabled = false;

                var lblB = new Label
                {
                    Text = badge,
                    Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                    ForeColor = isUp ? RGreen : RRed,
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                badgePanel.Controls.Add(lblB);

                card.Controls.Add(lblT);
                card.Controls.Add(lblV);
                card.Controls.Add(badgePanel);
                pnlKpiRow.Controls.Add(card);
            }
        }

        private void PnlTableInner_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int w = pnlTableInner.Width;
            int headerH = 36;
            int rowH = 44;

            string[] headers = { "PRODUCT NAME", "CATEGORY", "UNITS", "REVENUE", "EST. PROFIT" };
            int[] colW = { 200, 100, 70, 100, 100 };

            // Header
            using var hBg = new SolidBrush(ColorTranslator.FromHtml("#F9FAFB"));
            g.FillRectangle(hBg, 0, 0, w, headerH);
            using var bPen = new Pen(RBorder, 1);
            g.DrawLine(bPen, 0, headerH - 1, w, headerH - 1);

            using var hFont = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            using var hBrush = new SolidBrush(RTextMuted);
            int hx = 24;
            for (int i = 0; i < headers.Length; i++)
            {
                g.DrawString(headers[i], hFont, hBrush, hx, (headerH - 14) / 2);
                hx += colW[i];
            }

            // Rows
            var rows = new[]
            {
                ("Ceremonial Matcha Latte", "Drinks", "58", "$493.00", "$312.00"),
                ("Hojicha Roast Tea", "Drinks", "42", "$231.00", "$189.00"),
                ("Matcha Mochi Donut", "Pastry", "35", "$157.50", "$98.00"),
                ("Iced Strawberry Matcha", "Drinks", "31", "$217.00", "$145.00"),
                ("Matcha White Choc Cookie", "Pastry", "28", "$112.00", "$72.50")
            };

            using var rFont = new Font("Segoe UI", 9F);
            using var rFontBold = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            using var pillFont = new Font("Segoe UI", 7F);
            using var tPrim = new SolidBrush(RTextPrimary);
            using var tBody = new SolidBrush(ColorTranslator.FromHtml("#374151"));
            using var tGray = new SolidBrush(RTextSecondary);
            using var tGreen = new SolidBrush(RGreen);
            using var pillBg = new SolidBrush(ColorTranslator.FromHtml("#F3F4F6"));

            for (int r = 0; r < rows.Length; r++)
            {
                var (name, cat, units, rev, profit) = rows[r];
                int ry = headerH + r * rowH;
                int rx = 24;
                int ty = ry + (rowH - 18) / 2;

                g.DrawLine(bPen, 0, ry + rowH - 1, w, ry + rowH - 1);

                g.DrawString(name, rFontBold, tPrim, rx, ty);
                rx += colW[0];

                // Category pill
                var catSz = g.MeasureString(cat, pillFont);
                var pillRect = new Rectangle(rx, ty - 1, (int)catSz.Width + 12, 20);
                using var pillPath = CreateRoundedRectPath(pillRect, 6);
                g.FillPath(pillBg, pillPath);
                g.DrawString(cat, pillFont, tGray, rx + 6, ty + 1);
                rx += colW[1];

                g.DrawString(units, rFont, tBody, rx, ty);
                rx += colW[2];

                g.DrawString(rev, rFont, tBody, rx, ty);
                rx += colW[3];

                g.DrawString(profit, rFontBold, tGreen, rx, ty);
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
