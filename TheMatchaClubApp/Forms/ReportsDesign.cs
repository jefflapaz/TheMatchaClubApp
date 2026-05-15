using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
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

        private static readonly Color[] ChartColors = {
            ColorTranslator.FromHtml("#52B743"), ColorTranslator.FromHtml("#3B82F6"),
            ColorTranslator.FromHtml("#F59E0B"), ColorTranslator.FromHtml("#EF4444"),
            ColorTranslator.FromHtml("#8B5CF6"), ColorTranslator.FromHtml("#EC4899"),
            ColorTranslator.FromHtml("#06B6D4"), ColorTranslator.FromHtml("#84CC16")
        };

        private void InitializeDesign()
        {
            this.BackColor = RBg;
            this.Dock = DockStyle.Fill;

            // Tab bar
            pnlTabBar.BackColor = RCard;
            StyleTabBtn(btnTabOverview, true);
            StyleTabBtn(btnTabSales, false);
            StyleTabBtn(btnTabHistory, false);

            // Session header
            pnlSessionHeader.BackColor = RBg;
            lblTitle.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            lblTitle.ForeColor = RTextPrimary;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblSelectedSession.Font = new Font("Segoe UI", 9F);
            lblSelectedSession.ForeColor = RTextSecondary;
            lblSelectedSession.BackColor = Color.Transparent;
            lblSelectedSession.TextAlign = ContentAlignment.MiddleLeft;

            btnSessionCalendar.BorderRadius = 8;
            btnSessionCalendar.FillColor = RGreenBg;
            btnSessionCalendar.ForeColor = RGreen;
            btnSessionCalendar.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnSessionCalendar.BorderThickness = 1;
            btnSessionCalendar.BorderColor = RBorder;

            // Export buttons
            pnlExportButtons.BackColor = Color.Transparent;
            StyleExportBtn(btnExportCsv);
            StyleExportBtn(btnExportPdf);

            // KPI row
            pnlKpiRow.BackColor = Color.Transparent;

            // Charts
            StyleChartPanel(pnlDoughnutChart, lblDoughnutTitle);
            StyleChartPanel(pnlBarChart, lblBarChartTitle);
            pnlChartsRow.BackColor = Color.Transparent;
            pnlDoughnutChart.Paint += PnlDoughnutChart_Paint;
            pnlBarChart.Paint += PnlBarChart_Paint;

            // Insights row
            pnlInsightsRow.BackColor = Color.Transparent;

            // Top items
            StyleCardPanel(pnlTableCard);
            lblTableHeader.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblTableHeader.ForeColor = RTextPrimary;
            lblTableHeader.BackColor = Color.Transparent;
            StyleDgv(dgvTopItems);

            // Recent tx
            StyleCardPanel(pnlRecentTx);
            lblRecentTxTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblRecentTxTitle.ForeColor = RTextPrimary;
            lblRecentTxTitle.BackColor = Color.Transparent;
            StyleDgv(dgvRecentTx);

            // Sales Summary page
            pnlSalesHeader.BackColor = RBg;
            lblSalesTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblSalesTitle.ForeColor = RTextPrimary;
            lblSalesTitle.BackColor = Color.Transparent;
            lblSalesTitle.TextAlign = ContentAlignment.MiddleLeft;
            txtSalesSearch.BorderRadius = 8;
            txtSalesSearch.BorderColor = RBorder;
            txtSalesSearch.FocusedState.BorderColor = RGreen;
            txtSalesSearch.Font = new Font("Segoe UI", 9F);
            StyleDgv(dgvAllSales);

            // History page
            pnlHistoryCharts.BackColor = Color.Transparent;
            StyleChartPanel(pnlRevenueChart, lblRevenueChartTitle);
            StyleChartPanel(pnlTxChart, lblTxChartTitle);
            pnlRevenueChart.Paint += PnlRevenueChart_Paint;
            pnlTxChart.Paint += PnlTxChart_Paint;
            StyleCardPanel(pnlHistoryTableCard);
            lblHistoryTableTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblHistoryTableTitle.ForeColor = RTextPrimary;
            lblHistoryTableTitle.BackColor = Color.Transparent;
            StyleDgv(dgvSessionHistory);

            // Sidebar
            pnlCloseoutSidebar.FillColor = RCard;
            pnlCloseoutSidebar.BorderColor = RBorder;
            pnlCloseoutSidebar.BorderThickness = 1;
            pnlCloseoutSidebar.ShadowDecoration.Enabled = false;
            pnlCloseoutHeader.BackColor = RGreenBg;
            lblCloseoutTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblCloseoutTitle.ForeColor = RGreen;
            lblCloseoutTitle.BackColor = Color.Transparent;

            foreach (var lbl in new[] { lblExpectedCash, lblDrawerFund, lblTxCountLabel, lblBestSellerLabel, lblOverShortLabel })
            { lbl.Font = new Font("Segoe UI", 8.5F); lbl.ForeColor = RTextSecondary; lbl.BackColor = Color.Transparent; }
            foreach (var lbl in new[] { lblExpectedCashValue, lblDrawerFundValue, lblTxCountValue, lblBestSellerValue, lblOverShortValue })
            { lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lbl.ForeColor = RTextPrimary; lbl.BackColor = Color.Transparent; }

            lblActualCashLabel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblActualCashLabel.ForeColor = RTextMuted;
            lblActualCashLabel.BackColor = Color.Transparent;
            txtActualCash.BorderRadius = 8;
            txtActualCash.BorderColor = RBorder;
            txtActualCash.FocusedState.BorderColor = RGreen;
            txtActualCash.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            txtActualCash.ForeColor = RTextPrimary;

            pnlInfoBox.FillColor = ColorTranslator.FromHtml("#FFF9E6");
            pnlInfoBox.BorderRadius = 8;
            pnlInfoBox.BorderThickness = 0;
            pnlInfoBox.ShadowDecoration.Enabled = false;
            lblInfoText.Font = new Font("Segoe UI", 7.5F);
            lblInfoText.ForeColor = ColorTranslator.FromHtml("#B45309");
            lblInfoText.BackColor = Color.Transparent;

            btnCloseDay.FillColor = RGreen;
            btnCloseDay.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnCloseDay.ForeColor = Color.White;
            btnCloseDay.BorderRadius = 8;
            btnCloseDay.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnCloseDay.BorderThickness = 0;

            btnPrintReport.FillColor = Color.White;
            btnPrintReport.HoverState.FillColor = ColorTranslator.FromHtml("#F9FAFB");
            btnPrintReport.ForeColor = RGreen;
            btnPrintReport.BorderColor = RGreen;
            btnPrintReport.BorderThickness = 1;
            btnPrintReport.BorderRadius = 8;
            btnPrintReport.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            btnOpenStore.FillColor = ColorTranslator.FromHtml("#3B82F6");
            btnOpenStore.HoverState.FillColor = ColorTranslator.FromHtml("#2563EB");
            btnOpenStore.ForeColor = Color.White;
            btnOpenStore.BorderRadius = 10;
            btnOpenStore.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnOpenStore.BorderThickness = 0;

            lblSessionStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSessionStatus.ForeColor = RTextMuted;
            lblSessionStatus.BackColor = Color.Transparent;
            lblSessionTime.Font = new Font("Segoe UI", 7.5F);
            lblSessionTime.ForeColor = RTextSecondary;
            lblSessionTime.BackColor = Color.Transparent;
        }

        private void StyleTabBtn(Guna2Button btn, bool active)
        {
            btn.BorderRadius = 8;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.BorderThickness = active ? 2 : 0;
            btn.BorderColor = RGreen;
            btn.FillColor = active ? RGreenBg : Color.Transparent;
            btn.ForeColor = active ? RGreen : RTextSecondary;
        }

        private void StyleExportBtn(Guna2Button btn)
        {
            btn.BorderRadius = 8;
            btn.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            btn.ForeColor = RTextSecondary;
            btn.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btn.BorderThickness = 0;
        }

        private void StyleChartPanel(Guna2Panel pnl, Label lbl)
        {
            pnl.FillColor = RCard;
            pnl.BorderRadius = 12;
            pnl.BorderColor = RBorder;
            pnl.BorderThickness = 1;
            pnl.ShadowDecoration.Enabled = false;
            lbl.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lbl.ForeColor = RTextPrimary;
            lbl.BackColor = Color.Transparent;
        }

        private void StyleCardPanel(Guna2Panel pnl)
        {
            pnl.FillColor = Color.Transparent;
            pnl.BorderRadius = 0;
            pnl.BorderThickness = 0;
            pnl.ShadowDecoration.Enabled = false;
        }

        private void StyleDgv(Guna2DataGridView dgv)
        {
            dgv.ThemeStyle.HeaderStyle.BackColor = ColorTranslator.FromHtml("#F9FAFB");
            dgv.ThemeStyle.HeaderStyle.ForeColor = RTextSecondary;
            dgv.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dgv.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgv.ThemeStyle.RowsStyle.ForeColor = RTextPrimary;
            dgv.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgv.ThemeStyle.AlternatingRowsStyle.BackColor = ColorTranslator.FromHtml("#F9FAFB");
            dgv.ThemeStyle.AlternatingRowsStyle.ForeColor = RTextPrimary;
            dgv.ThemeStyle.RowsStyle.SelectionBackColor = RGreenBg;
            dgv.ThemeStyle.RowsStyle.SelectionForeColor = RTextPrimary;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = ColorTranslator.FromHtml("#F3F4F6");
            dgv.RowTemplate.Height = 42;
        }

        // ═══ CHART PAINT HANDLERS ═══
        private void PnlDoughnutChart_Paint(object? sender, PaintEventArgs e)
        {
            if (_categoryData == null || _categoryData.Count == 0) return;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var pnl = (Control)sender!;
            int titleSpace = 36; // Space reserved for the top label

            // Calculate dynamic circle bounds to support DPI scaling and resizing
            int drawWidth = pnl.Width - 140; // Reserve 140px on the right for the legend
            int drawHeight = pnl.Height - titleSpace - 16; // Reserve padding

            // Ensure perfect circle by taking the smallest dimension
            int diameter = Math.Min(drawWidth, drawHeight);
            int r = diameter / 2;
            int inner = (int)(r * 0.55); // Inner hole is 55% of radius

            // Center coordinates
            int cx = (drawWidth / 2) + 16;
            int cy = titleSpace + r + 4;

            float total = (float)_categoryData.Values.Sum(v => (float)v);
            if (total == 0) return;
            float startAngle = -90;
            int ci = 0;
            foreach (var kv in _categoryData)
            {
                float sweep = (float)kv.Value / total * 360f;
                using var brush = new SolidBrush(ChartColors[ci % ChartColors.Length]);
                g.FillPie(brush, cx - r, cy - r, r * 2, r * 2, startAngle, sweep);
                startAngle += sweep;
                ci++;
            }
            using var centerBrush = new SolidBrush(RCard);
            g.FillEllipse(centerBrush, cx - inner, cy - inner, inner * 2, inner * 2);

            // Responsive Legend vertically centered
            int legendX = cx + r + 24;
            int itemHeight = 32;
            int ly = cy - ((_categoryData.Count * itemHeight) / 2); // Center block vertically
            
            ci = 0;
            foreach (var kv in _categoryData)
            {
                using var lb = new SolidBrush(ChartColors[ci % ChartColors.Length]);
                g.FillRectangle(lb, legendX, ly + 2, 12, 12);
                using var tf = new Font("Segoe UI", 7.5F);
                g.DrawString($"{kv.Key}\n{kv.Value:₱#,##0}", tf, Brushes.Gray, legendX + 18, ly - 2);
                ly += itemHeight;
                ci++;
            }
        }

        private void PnlBarChart_Paint(object? sender, PaintEventArgs e)
        {
            if (_hourlySalesData == null || _hourlySalesData.Count == 0) return;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var pnl = (Control)sender!;
            int left = 50, top = 36, right = pnl.Width - 20, bottom = pnl.Height - 30;
            int chartW = right - left, chartH = bottom - top;

            // Only show hours with data or business hours (6-22)
            var activeHours = _hourlySalesData.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToList();
            int minH = activeHours.Count > 0 ? Math.Max(0, activeHours.Min() - 1) : 6;
            int maxH = activeHours.Count > 0 ? Math.Min(23, activeHours.Max() + 1) : 22;
            int hourCount = maxH - minH + 1;
            if (hourCount <= 0) return;

            decimal maxVal = _hourlySalesData.Values.Max();
            if (maxVal == 0) maxVal = 100;

            // Grid
            using var gridPen = new Pen(RBorder, 1);
            for (int i = 0; i <= 4; i++)
            {
                int y = bottom - (int)(chartH * i / 4.0);
                g.DrawLine(gridPen, left, y, right, y);
                decimal val = maxVal * i / 4;
                using var f = new Font("Segoe UI", 7F);
                g.DrawString($"₱{val:#,##0}", f, Brushes.Gray, 2, y - 6);
            }

            // Bars
            float barW = Math.Min(30, (float)chartW / hourCount * 0.7f);
            float gap = (float)chartW / hourCount;
            for (int h = minH; h <= maxH; h++)
            {
                decimal val = _hourlySalesData.ContainsKey(h) ? _hourlySalesData[h] : 0;
                float barH = maxVal > 0 ? (float)((double)val / (double)maxVal * chartH) : 0;
                float x = left + (h - minH) * gap + (gap - barW) / 2;
                float y = bottom - barH;

                using var brush = new LinearGradientBrush(new RectangleF(x, y, barW, Math.Max(barH, 1)), RGreen, ColorTranslator.FromHtml("#86CD77"), 90F);
                if (barH > 2) g.FillRoundedRectangle(brush, x, y, barW, barH, 4);

                using var lf = new Font("Segoe UI", 7F);
                string label = h > 12 ? $"{h - 12}pm" : h == 12 ? "12pm" : h == 0 ? "12am" : $"{h}am";
                var sz = g.MeasureString(label, lf);
                g.DrawString(label, lf, Brushes.Gray, x + barW / 2 - sz.Width / 2, bottom + 2);
            }
        }

        private void PnlRevenueChart_Paint(object? sender, PaintEventArgs e)
        {
            PaintHistoryBar(e.Graphics, (Control)sender!, _historyRevenue, "₱");
        }

        private void PnlTxChart_Paint(object? sender, PaintEventArgs e)
        {
            PaintHistoryBar(e.Graphics, (Control)sender!, _historyTxCounts, "");
        }

        private void PaintHistoryBar(Graphics g, Control pnl, List<(string Label, decimal Value)>? data, string prefix)
        {
            if (data == null || data.Count == 0) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int left = 50, top = 36, right = pnl.Width - 16, bottom = pnl.Height - 28;
            int chartW = right - left, chartH = bottom - top;
            decimal maxVal = data.Max(d => d.Value);
            if (maxVal == 0) maxVal = 1;

            float barW = Math.Min(28, (float)chartW / data.Count * 0.65f);
            float gap = (float)chartW / data.Count;

            using var gridPen = new Pen(RBorder, 1);
            for (int i = 0; i <= 3; i++)
            {
                int y = bottom - (int)(chartH * i / 3.0);
                g.DrawLine(gridPen, left, y, right, y);
                decimal val = maxVal * i / 3;
                using var f = new Font("Segoe UI", 7F);
                g.DrawString($"{prefix}{val:#,##0}", f, Brushes.Gray, 2, y - 6);
            }

            for (int i = 0; i < data.Count; i++)
            {
                float barH = maxVal > 0 ? (float)((double)data[i].Value / (double)maxVal * chartH) : 0;
                float x = left + i * gap + (gap - barW) / 2;
                float y = bottom - barH;

                var color = ChartColors[i % ChartColors.Length];
                using var brush = new SolidBrush(color);
                if (barH > 2) g.FillRoundedRectangle(brush, x, y, barW, barH, 4);

                using var lf = new Font("Segoe UI", 6.5F);
                var sz = g.MeasureString(data[i].Label, lf);
                g.DrawString(data[i].Label, lf, Brushes.Gray, x + barW / 2 - sz.Width / 2, bottom + 2);
            }
        }
    }

    // Extension for rounded rectangle
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, float x, float y, float w, float h, int r)
        {
            if (h < r * 2) r = (int)(h / 2);
            if (w < r * 2) r = (int)(w / 2);
            if (r < 1) { g.FillRectangle(brush, x, y, w, h); return; }
            using var path = new GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 180, 90);
            path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
            path.AddLine(x + w, y + r, x + w, y + h);
            path.AddLine(x + w, y + h, x, y + h);
            path.AddLine(x, y + h, x, y + r);
            path.CloseFigure();
            g.FillPath(brush, path);
        }
    }
}
