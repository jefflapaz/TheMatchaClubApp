using System.Drawing;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class SessionDetailForm
    {
        private void InitializeDesign()
        {
            // ── Header styling ───────────────────────
            pnlHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
                e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            btnClose.Location = new Point(pnlHeader.Width - 56, 20);
            lblStatusBadge.Location = new Point(pnlHeader.Width - 148, 24);

            pnlHeader.Resize += (s, e) =>
            {
                btnClose.Location = new Point(pnlHeader.Width - 56, 20);
                lblStatusBadge.Location = new Point(pnlHeader.Width - 148, 24);
            };

            // ── Tab bar bottom border ────────────────
            pnlTabBar.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
                e.Graphics.DrawLine(pen, 0, pnlTabBar.Height - 1, pnlTabBar.Width, pnlTabBar.Height - 1);
            };

            // ── Cash Reconciliation card styling ─────
            pnlCashRecon.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
                using var path = RoundedRect(0, 0, pnlCashRecon.Width - 1, pnlCashRecon.Height - 1, 10);
                g.DrawPath(pen, path);

                // Separator line above Over/Short
                using var sepPen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
                g.DrawLine(sepPen, 16, 112, pnlCashRecon.Width - 16, 112);
            };

            // ── Insights panel card styling ──────────
            pnlInsightsRow.Paint += (s2, e2) =>
            {
                // Draw after the custom paint handler
            };
            StyleCardBorder(pnlInsightsRow);
            StyleCardBorder(pnlHourlyChart);
            StyleCardBorder(pnlCategoryBreakdown);

            // ── Footer styling ───────────────────────
            pnlFooter.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
                e.Graphics.DrawLine(pen, 0, 0, pnlFooter.Width, 0);
            };

            btnCloseBottom.Location = new Point(pnlFooter.Width - 122, 10);
            pnlFooter.Resize += (s, e) =>
            {
                btnCloseBottom.Location = new Point(pnlFooter.Width - 122, 10);
            };

            // ── DataGridView styling ─────────────────
            StyleGrid(dgvTransactions);
            StyleGrid(dgvProducts);

            // ── Responsive layout ────────────────────
            pnlOverviewTab.Resize += (s, e) => LayoutOverview();
            LayoutOverview();
        }

        private void LayoutOverview()
        {
            int w = pnlOverviewTab.ClientSize.Width;
            int margin = 32;
            int usable = w - margin * 2;

            flpKpiCards.Location = new Point(margin, 16);
            flpKpiCards.Size = new Size(usable, 100);

            int halfW = (usable - 16) / 2;
            pnlCashRecon.Location = new Point(margin, 124);
            pnlCashRecon.Size = new Size(halfW, 160);

            pnlInsightsRow.Location = new Point(margin + halfW + 16, 124);
            pnlInsightsRow.Size = new Size(halfW, 160);

            pnlHourlyChart.Location = new Point(margin, 300);
            pnlHourlyChart.Size = new Size(usable, 200);

            // Update cash value label positions
            int rightX = halfW - 100;
            lblStartingCashValue.Location = new Point(rightX, lblStartingCashValue.Location.Y);
            lblExpectedCashValue.Location = new Point(rightX, lblExpectedCashValue.Location.Y);
            lblActualCashValue.Location = new Point(rightX, lblActualCashValue.Location.Y);
            lblOverShortValue.Location = new Point(rightX, lblOverShortValue.Location.Y);
        }

        private void StyleCardBorder(Panel pnl)
        {
            pnl.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
                using var path = RoundedRect(0, 0, pnl.Width - 1, pnl.Height - 1, 10);
                g.DrawPath(pen, path);
            };
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ColorTranslator.FromHtml("#F9FAFB"),
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                SelectionBackColor = ColorTranslator.FromHtml("#F9FAFB"),
                SelectionForeColor = ColorTranslator.FromHtml("#6B7280"),
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml("#374151"),
                Font = new Font("Segoe UI", 9F),
                SelectionBackColor = ColorTranslator.FromHtml("#EBF5E7"),
                SelectionForeColor = ColorTranslator.FromHtml("#374151"),
                Padding = new Padding(8, 0, 0, 0)
            };
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(int x, int y, int w, int h, int r)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 180, 90);
            path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
            path.AddArc(x + w - r * 2, y + h - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(x, y + h - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
