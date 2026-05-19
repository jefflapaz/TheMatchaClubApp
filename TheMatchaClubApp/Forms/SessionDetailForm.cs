using TheMatchaClub.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TheMatchaClubDomain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestInfrastructure = QuestPDF.Infrastructure;
using System.Drawing.Printing;

namespace TheMatchaClubApp.Forms
{
    public partial class SessionDetailForm : Form
    {
        private readonly BusinessSession _session;
        private readonly List<Order> _orders;
        private Dictionary<int, decimal> _hourlyData = new();
        private Dictionary<string, decimal> _categoryData = new();

        public SessionDetailForm(BusinessSession session)
        {
            _session = session;
            _orders = Program.SessionService.GetSessionOrders(session.SessionId);
            
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();
            SetupGridColumns();
            WireEvents();
            LoadData();
            ShowTab("overview");
        }

        private void SetupGridColumns()
        {
            dgvTransactions.Columns.AddRange(
                new DataGridViewTextBoxColumn { HeaderText = "Order ID", FillWeight = 18 },
                new DataGridViewTextBoxColumn { HeaderText = "Time", FillWeight = 12 },
                new DataGridViewTextBoxColumn { HeaderText = "Customer", FillWeight = 18 },
                new DataGridViewTextBoxColumn { HeaderText = "Type", FillWeight = 10 },
                new DataGridViewTextBoxColumn { HeaderText = "Items", FillWeight = 8, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                new DataGridViewTextBoxColumn { HeaderText = "Amount", FillWeight = 14, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } },
                new DataGridViewTextBoxColumn { HeaderText = "Payment", FillWeight = 10 }
            );

            dgvProducts.Columns.AddRange(
                new DataGridViewTextBoxColumn { HeaderText = "#", FillWeight = 6 },
                new DataGridViewTextBoxColumn { HeaderText = "Product", FillWeight = 30 },
                new DataGridViewTextBoxColumn { HeaderText = "Category", FillWeight = 18 },
                new DataGridViewTextBoxColumn { HeaderText = "Units Sold", FillWeight = 14, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                new DataGridViewTextBoxColumn { HeaderText = "Revenue", FillWeight = 16, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } },
                new DataGridViewTextBoxColumn { HeaderText = "% Share", FillWeight = 12, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            );
        }

        private void WireEvents()
        {
            btnClose.Click += (s, e) => Close();
            btnCloseBottom.Click += (s, e) => Close();
            btnTabOverview.Click += (s, e) => ShowTab("overview");
            btnTabTransactions.Click += (s, e) => ShowTab("transactions");
            btnTabProducts.Click += (s, e) => ShowTab("products");
            btnExportCsv.Click += BtnExportCsv_Click;
            btnExportPdf.Click += BtnExportPdf_Click;
            btnPrint.Click += BtnPrint_Click;

            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };

            // Rounded corner border for modal
            this.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            };

            pnlHourlyChart.Paint += PnlHourlyChart_Paint;
            pnlInsightsRow.Paint += PnlInsights_Paint;
            pnlCategoryBreakdown.Paint += PnlCategoryBreakdown_Paint;
        }

        private void ShowTab(string tab)
        {
            pnlOverviewTab.Visible = tab == "overview";
            pnlTransactionsTab.Visible = tab == "transactions";
            pnlProductsTab.Visible = tab == "products";

            StyleTab(btnTabOverview, tab == "overview");
            StyleTab(btnTabTransactions, tab == "transactions");
            StyleTab(btnTabProducts, tab == "products");
        }

        private void StyleTab(Guna.UI2.WinForms.Guna2Button btn, bool active)
        {
            btn.FillColor = active ? ColorTranslator.FromHtml("#52B743") : ColorTranslator.FromHtml("#F3F4F6");
            btn.ForeColor = active ? Color.White : ColorTranslator.FromHtml("#374151");
        }

        private string Fmt(decimal v) => $"₱{v:#,##0.00}";

        // ══════════════════════════════════════════════
        //  DATA LOADING
        // ══════════════════════════════════════════════
        private void LoadData()
        {
            // Header
            string timeRange = _session.IsClosed && _session.ClosedAt.HasValue
                ? $"{_session.OpenedAt:hh:mm tt} – {_session.ClosedAt:hh:mm tt}"
                : $"{_session.OpenedAt:hh:mm tt} – Active";
            lblSessionTitle.Text = $"Session Report — {_session.OpenedAt:MMM dd, yyyy}";
            lblSessionMeta.Text = $"{timeRange}  •  Cashier: {_session.OpenedBy}";

            if (_session.IsClosed)
            {
                lblStatusBadge.Text = "CLOSED";
                lblStatusBadge.BackColor = ColorTranslator.FromHtml("#6B7280");
            }
            else
            {
                lblStatusBadge.Text = "ACTIVE";
                lblStatusBadge.BackColor = ColorTranslator.FromHtml("#52B743");
            }

            // Totals
            decimal revenue = _session.IsClosed ? _session.TotalRevenue : _orders.Sum(o => o.Total);
            int txCount = _session.IsClosed ? _session.TotalTransactions : _orders.Count;
            int units = _session.IsClosed ? _session.TotalUnitsSold : _orders.SelectMany(o => o.Items).Sum(i => i.Quantity);
            decimal avgOrder = txCount > 0 ? revenue / txCount : 0;
            var duration = (_session.IsClosed && _session.ClosedAt.HasValue ? _session.ClosedAt.Value : DateTime.Now) - _session.OpenedAt;

            // KPI Cards
            flpKpiCards.Controls.Clear();
            flpKpiCards.Controls.Add(CreateKpiCard("Revenue", Fmt(revenue), "#52B743"));
            flpKpiCards.Controls.Add(CreateKpiCard("Transactions", txCount.ToString(), "#3B82F6"));
            flpKpiCards.Controls.Add(CreateKpiCard("Units Sold", units.ToString(), "#8B5CF6"));
            flpKpiCards.Controls.Add(CreateKpiCard("Avg. Order", Fmt(avgOrder), "#F59E0B"));
            flpKpiCards.Controls.Add(CreateKpiCard("Duration", $"{(int)duration.TotalHours}h {duration.Minutes:D2}m", "#6B7280"));

            // Cash Reconciliation
            lblStartingCashValue.Text = Fmt(_session.StartingCash);
            lblExpectedCashValue.Text = Fmt(_session.ExpectedCash);
            lblActualCashValue.Text = Fmt(_session.ActualCash);
            decimal diff = _session.ActualCash - _session.ExpectedCash;
            lblOverShortValue.Text = (diff >= 0 ? "+" : "") + Fmt(diff);
            lblOverShortValue.ForeColor = diff >= 0 ? ColorTranslator.FromHtml("#52B743") : ColorTranslator.FromHtml("#EF4444");

            // Charts
            _hourlyData = Program.SessionService.GetHourlySalesData(_session.SessionId);
            _categoryData = Program.SessionService.GetCategorySalesData(_session.SessionId);
            pnlHourlyChart.Invalidate();
            pnlInsightsRow.Invalidate();
            pnlCategoryBreakdown.Invalidate();

            // Transactions tab
            LoadTransactions();
            LoadProducts();
        }

        private Guna.UI2.WinForms.Guna2Panel CreateKpiCard(string title, string value, string accentColor)
        {
            var pnl = new Guna.UI2.WinForms.Guna2Panel
            {
                Size = new Size(160, 84),
                BorderRadius = 10,
                FillColor = Color.White,
                BorderColor = ColorTranslator.FromHtml("#F3F4F6"),
                BorderThickness = 1,
                Margin = new Padding(0, 0, 12, 0)
            };
            pnl.ShadowDecoration.Enabled = true;
            pnl.ShadowDecoration.Depth = 4;
            pnl.ShadowDecoration.Color = Color.FromArgb(15, 0, 0, 0);

            var accent = new Panel { Size = new Size(4, 40), Location = new Point(0, 22), BackColor = ColorTranslator.FromHtml(accentColor) };
            var lblT = new Label { Text = title.ToUpper(), Font = new Font("Segoe UI", 7.5F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#9CA3AF"), Location = new Point(14, 14), AutoSize = true };
            var lblV = new Label { Text = value, Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#111827"), Location = new Point(14, 38), AutoSize = true };

            pnl.Controls.AddRange(new Control[] { accent, lblT, lblV });
            return pnl;
        }

        private void LoadTransactions()
        {
            dgvTransactions.Rows.Clear();
            foreach (var o in _orders.OrderByDescending(o => o.Timestamp))
            {
                int itemCount = o.Items.Sum(i => i.Quantity);
                dgvTransactions.Rows.Add(
                    o.OrderId, o.Timestamp.ToString("hh:mm tt"),
                    o.CustomerName ?? "Walk-In",
                    o.OrderType ?? (o.IsDineIn ? "Dine-In" : "Take-Out"),
                    itemCount.ToString(), Fmt(o.Total),
                    o.PaymentMethod ?? "Cash"
                );
            }
            lblTxSummary.Text = $"{_orders.Count} transactions  •  {Fmt(_orders.Sum(o => o.Total))} total revenue";
        }

        private void LoadProducts()
        {
            var products = Program.SessionService.GetAllItemSales(_session.SessionId);
            decimal totalRev = products.Sum(p => p.Revenue);
            dgvProducts.Rows.Clear();
            int rank = 1;
            foreach (var p in products)
            {
                string share = totalRev > 0 ? $"{p.Revenue / totalRev * 100:0.0}%" : "—";
                dgvProducts.Rows.Add(rank.ToString(), p.Name, p.Category, p.Units.ToString(), Fmt(p.Revenue), share);
                rank++;
            }
        }

        // ══════════════════════════════════════════════
        //  PAINT HANDLERS
        // ══════════════════════════════════════════════
        private void PnlHourlyChart_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var pnl = (Panel)sender!;
            int pad = 50, bottom = pnl.Height - 30, top = 20;
            int chartW = pnl.Width - pad - 20;
            int chartH = bottom - top;

            using var titleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            g.DrawString("Hourly Sales", titleFont, Brushes.Black, 16, 8);

            if (_hourlyData.Count == 0 || _hourlyData.Values.Max() == 0) return;

            decimal maxVal = _hourlyData.Values.Max();
            int barCount = _hourlyData.Count(h => h.Key >= 6 && h.Key <= 22);
            if (barCount == 0) return;
            float barW = (float)chartW / barCount - 4;

            using var barBrush = new SolidBrush(ColorTranslator.FromHtml("#52B743"));
            using var labelFont = new Font("Segoe UI", 7F);
            using var gridPen = new Pen(ColorTranslator.FromHtml("#F3F4F6"), 1);

            int idx = 0;
            for (int h = 6; h <= 22; h++)
            {
                decimal val = _hourlyData.ContainsKey(h) ? _hourlyData[h] : 0;
                float barH = maxVal > 0 ? (float)(val / maxVal) * chartH : 0;
                float x = pad + idx * (barW + 4);
                float y = bottom - barH;

                if (barH > 2) g.FillRoundedRectangle(barBrush, x, y, barW, barH, 3);

                string label = h > 12 ? $"{h - 12}p" : h == 12 ? "12p" : $"{h}a";
                var sz = g.MeasureString(label, labelFont);
                g.DrawString(label, labelFont, Brushes.Gray, x + (barW - sz.Width) / 2, bottom + 4);
                idx++;
            }
        }

        private void PnlInsights_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            using var titleFont = new Font("Segoe UI", 11F, FontStyle.Bold);
            using var labelFont = new Font("Segoe UI", 9F);
            using var valueFont = new Font("Segoe UI", 11F, FontStyle.Bold);

            g.DrawString("Session Insights", titleFont, Brushes.Black, 16, 12);

            var topItem = Program.SessionService.GetTopItems(_session.SessionId, 1).FirstOrDefault();
            var peakHour = _hourlyData.Where(h => h.Value > 0).OrderByDescending(h => h.Value).FirstOrDefault();
            var topCat = _categoryData.OrderByDescending(c => c.Value).FirstOrDefault();
            var largest = _orders.OrderByDescending(o => o.Total).FirstOrDefault();

            string[] labels = { "Best Seller", "Peak Hour", "Top Category", "Largest Order" };
            string[] values = {
                topItem.Name ?? "—",
                peakHour.Value > 0 ? FormatHour(peakHour.Key) : "—",
                topCat.Key ?? "—",
                largest != null ? Fmt(largest.Total) : "—"
            };

            int y = 44;
            for (int i = 0; i < labels.Length; i++)
            {
                g.DrawString(labels[i], labelFont, Brushes.Gray, 16, y);
                g.DrawString(values[i], valueFont, Brushes.Black, 160, y - 2);
                y += 28;
            }
        }

        private void PnlCategoryBreakdown_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            using var titleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            g.DrawString("Category Breakdown", titleFont, Brushes.Black, 16, 12);

            if (_categoryData.Count == 0) return;
            decimal total = _categoryData.Values.Sum();
            if (total == 0) return;

            string[] colors = { "#52B743", "#3B82F6", "#F59E0B", "#8B5CF6", "#EF4444", "#EC4899", "#14B8A6" };
            int y = 44;
            int idx = 0;
            using var barBg = new SolidBrush(ColorTranslator.FromHtml("#F3F4F6"));
            using var labelFont = new Font("Segoe UI", 8.5F);
            using var valueFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);

            foreach (var cat in _categoryData.OrderByDescending(c => c.Value))
            {
                float pct = (float)(cat.Value / total);
                string color = colors[idx % colors.Length];
                using var fill = new SolidBrush(ColorTranslator.FromHtml(color));

                int barMaxW = Math.Max(100, pnlCategoryBreakdown.Width - 350);
                g.DrawString(cat.Key, labelFont, Brushes.Black, 16, y);
                g.FillRectangle(barBg, 140, y + 2, barMaxW, 14);
                g.FillRoundedRectangle(fill, 140, y + 2, barMaxW * pct, 14, 4);
                g.DrawString($"{pct * 100:0.0}%  ({Fmt(cat.Value)})", valueFont, Brushes.Gray, 140 + barMaxW + 10, y);
                y += 24;
                idx++;
            }
        }

        private string FormatHour(int h) => h > 12 ? $"{h - 12}:00 PM" : h == 12 ? "12:00 PM" : h == 0 ? "12:00 AM" : $"{h}:00 AM";

        // ══════════════════════════════════════════════
        //  EXPORTS
        // ══════════════════════════════════════════════
        private void BtnExportCsv_Click(object? sender, EventArgs e)
        {
            try
            {
                using var dlg = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv", FileName = $"Session_{_session.OpenedAt:yyyyMMdd}_{DateTime.Now:HHmmss}.csv" };
                if (dlg.ShowDialog() != DialogResult.OK) return;

                var sb = new StringBuilder();
                sb.AppendLine("Session Report");
                sb.AppendLine($"Date, {_session.OpenedAt:MMM dd, yyyy}");
                sb.AppendLine($"Cashier, {_session.OpenedBy}");
                sb.AppendLine($"Opened, {_session.OpenedAt:hh:mm tt}");
                if (_session.IsClosed) sb.AppendLine($"Closed, {_session.ClosedAt:hh:mm tt}");
                sb.AppendLine();

                sb.AppendLine("TRANSACTIONS");
                sb.AppendLine("Order ID,Time,Customer,Type,Items,Amount,Payment");
                foreach (var o in _orders.OrderByDescending(o => o.Timestamp))
                {
                    int items = o.Items.Sum(i => i.Quantity);
                    sb.AppendLine($"{o.OrderId},{o.Timestamp:HH:mm},{o.CustomerName},{o.OrderType},{items},{o.Total:F2},{o.PaymentMethod}");
                }

                sb.AppendLine();
                sb.AppendLine("PRODUCT PERFORMANCE");
                sb.AppendLine("Product,Category,Units Sold,Revenue");
                var products = Program.SessionService.GetAllItemSales(_session.SessionId);
                foreach (var p in products)
                {
                    sb.AppendLine($"{p.Name},{p.Category},{p.Units},{p.Revenue:F2}");
                }

                File.WriteAllText(dlg.FileName, sb.ToString());
                MessageBox.Show($"CSV exported successfully:\n{dlg.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportPdf_Click(object? sender, EventArgs e)
        {
            try
            {
                using var dlg = new SaveFileDialog { Filter = "PDF (*.pdf)|*.pdf", FileName = $"SessionReport_{_session.OpenedAt:yyyyMMdd}.pdf" };
                if (dlg.ShowDialog() != DialogResult.OK) return;

                ZReportHelper.GenerateZReportPdf(_session, dlg.FileName, Program.SessionService, Program.DataService.Settings);

                MessageBox.Show($"PDF exported successfully:\n{dlg.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF Export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            try
            {
                var pd = new PrintDocument();
                pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
                pd.PrintPage += (s, pe) =>
                {
                    var g = pe.Graphics!;
                    float y = pe.MarginBounds.Top;
                    float x = pe.MarginBounds.Left;
                    float w = pe.MarginBounds.Width;

                    using var titleFont = new Font("Segoe UI", 16, FontStyle.Bold);
                    using var subFont = new Font("Segoe UI", 10, FontStyle.Bold);
                    using var bodyFont = new Font("Segoe UI", 9);
                    using var pen = new Pen(Color.LightGray, 1);

                    g.DrawString("Session Report", titleFont, Brushes.Black, x, y);
                    y += 30;
                    g.DrawString($"Date: {_session.OpenedAt:MMM dd, yyyy}", bodyFont, Brushes.Black, x, y);
                    y += 18;
                    g.DrawString($"Cashier: {_session.OpenedBy}", bodyFont, Brushes.Black, x, y);
                    y += 30;

                    g.DrawLine(pen, x, y, x + w, y);
                    y += 10;

                    g.DrawString("Revenue:", subFont, Brushes.Black, x, y);
                    g.DrawString(Fmt(_orders.Sum(o => o.Total)), bodyFont, Brushes.Black, x + 150, y);
                    y += 20;
                    g.DrawString("Transactions:", subFont, Brushes.Black, x, y);
                    g.DrawString(_orders.Count.ToString(), bodyFont, Brushes.Black, x + 150, y);
                    y += 30;

                    g.DrawString("Product Performance", subFont, Brushes.Black, x, y);
                    y += 20;

                    var products = Program.SessionService.GetAllItemSales(_session.SessionId);
                    foreach (var p in products.Take(20)) // Limit for printing
                    {
                        g.DrawString($"{p.Name} ({p.Units})", bodyFont, Brushes.Black, x, y);
                        g.DrawString(Fmt(p.Revenue), bodyFont, Brushes.Black, x + w - 80, y);
                        y += 18;
                    }
                };

                var dlg = new PrintPreviewDialog { Document = pd, WindowState = FormWindowState.Maximized };
                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Printing failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
