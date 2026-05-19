using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TheMatchaClub.Services;
using TheMatchaClubDomain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace TheMatchaClubApp.Forms
{
    public partial class ReportsView : UserControl
    {
        private BusinessSession? _selectedSession;
        private Dictionary<string, decimal> _categoryData = new();
        private Dictionary<int, decimal> _hourlySalesData = new();
        private List<(string Label, decimal Value)>? _historyRevenue;
        private List<(string Label, decimal Value)>? _historyTxCounts;
        private List<(string Name, string Category, int Units, decimal Revenue)>? _topProducts;
        private List<Order>? _recentOrders;

        public ReportsView()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            InitializeComponent();
            SetupColumns();
            InitializeDesign();
            WireEvents();

            _selectedSession = Program.SessionService.GetActiveSession();
            LoadAllPages();
            UpdateSessionUI();
        }

        private System.Windows.Forms.Timer tmrLiveRefresh = new() { Interval = 5000 };

        private void SetupColumns()
        {
            dgvSessionHistory.Columns.AddRange(
                new DataGridViewTextBoxColumn { HeaderText = "Date", FillWeight = 14 },
                new DataGridViewTextBoxColumn { HeaderText = "Time", FillWeight = 14 },
                new DataGridViewTextBoxColumn { HeaderText = "Duration", FillWeight = 10 },
                new DataGridViewTextBoxColumn { HeaderText = "Tx", FillWeight = 8, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                new DataGridViewTextBoxColumn { HeaderText = "Units", FillWeight = 8, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                new DataGridViewTextBoxColumn { HeaderText = "Revenue", FillWeight = 14, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } },
                new DataGridViewTextBoxColumn { HeaderText = "Opened By", FillWeight = 14 },
                new DataGridViewTextBoxColumn { HeaderText = "Status", FillWeight = 10 }
            );
        }

        private void WireEvents()
        {
            btnTabOverview.Click += (s, e) => ShowPage("overview");
            btnTabSales.Click += (s, e) => ShowPage("sessions");
            btnTabHistory.Click += (s, e) => ShowPage("history");
            btnCloseDay.Click += BtnCloseDay_Click;
            btnOpenStore.Click += BtnOpenStore_Click;
            btnExportCsv.Click += BtnExportCsv_Click;
            btnExportPdf.Click += BtnExportPdf_Click;
            btnPrintReport.Click += BtnExportPdf_Click;

            // Month/Year filter events
            cmbHistoryMonth.SelectedIndexChanged += (s, e) => LoadSessionHistoryPage();
            cmbHistoryYear.SelectedIndexChanged += (s, e) => LoadSessionHistoryPage();

            // Auto-refresh for live performance
            tmrLiveRefresh.Tick += (s, e) =>
            {
                if (_selectedSession != null && !IsDisposed)
                {
                    _selectedSessionOrders = Program.SessionService.GetSessionOrders(_selectedSession.SessionId);
                    LoadOverviewPage();
                    UpdateSessionUI();
                }
            };
            tmrLiveRefresh.Start();

            Program.DataService.OrdersChanged += (s, e) => { if (!IsDisposed) BeginInvoke(new Action(() => { LoadAllPages(); UpdateSessionUI(); })); };
            Program.DataService.DataLoaded += (s, e) => { if (!IsDisposed) BeginInvoke(new Action(() => { LoadAllPages(); UpdateSessionUI(); })); };
            Program.SessionService.SessionOpened += (s, e) => { if (!IsDisposed) BeginInvoke(new Action(() => { LoadAllPages(); UpdateSessionUI(); })); };
            Program.SessionService.SessionClosed += (s, e) => { if (!IsDisposed) BeginInvoke(new Action(() => { LoadAllPages(); UpdateSessionUI(); })); };

            dgvSessionHistory.CellDoubleClick += DgvSessionHistory_CellDoubleClick;
            dgvSessionHistory.CellFormatting += DgvSessionHistory_CellFormatting;
        }

        private void DgvSessionHistory_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvSessionHistory.Columns.Count < 8) return;
            var statusVal = dgvSessionHistory.Rows[e.RowIndex].Cells[7].Value?.ToString() ?? "";
            if (statusVal.Contains("Active"))
            {
                dgvSessionHistory.Rows[e.RowIndex].DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#52B743");
            }
        }

        // ── Tab Navigation ─────────────────────────────────────
        private void ShowPage(string page)
        {
            pnlPageOverview.Visible = page == "overview";
            pnlPageSales.Visible = page == "sessions";
            pnlPageHistory.Visible = page == "history";
            StyleTabBtn(btnTabOverview, page == "overview");
            StyleTabBtn(btnTabSales, page == "sessions");
            StyleTabBtn(btnTabHistory, page == "history");
            lblTitle.Text = page switch { "sessions" => "Session History", "history" => "Previous Reports", _ => "Current Performance" };

            if (page == "overview" && _selectedSession != null)
                lblSelectedSession.Text = "●  LIVE SESSION";
            else
                lblSelectedSession.Text = "";
        }



        // ── Load All Pages ─────────────────────────────────────
        private List<Order> _selectedSessionOrders = new();

        private void LoadAllPages()
        {
            _selectedSession = Program.SessionService.GetActiveSession();
            if (_selectedSession != null)
            {
                lblSelectedSession.Text = "●  LIVE SESSION";
                _selectedSessionOrders = Program.SessionService.GetSessionOrders(_selectedSession.SessionId);
            }
            else
            {
                lblSelectedSession.Text = "";
                _selectedSessionOrders = new List<Order>();
            }

            LoadOverviewPage();
            LoadSessionHistoryPage();
            LoadHistoryCharts();
        }

        // ── OVERVIEW PAGE (Current Performance) ────────────────
        private void LoadOverviewPage()
        {
            bool hasSession = _selectedSession != null;

            // Show/hide analytics based on session state
            pnlChartsRow.Visible = hasSession;
            pnlInsightsRow.Visible = hasSession;
            pnlTableCard.Visible = hasSession;
            pnlRecentTx.Visible = hasSession;

            if (!hasSession)
            {
                pnlKpiRow.Controls.Clear();
                pnlKpiRow.Controls.Add(CreateEmptyStatePanel());
                return;
            }

            var sid = _selectedSession!.SessionId;
            var orders = _selectedSessionOrders;
            
            // Use frozen totals if session is closed to ensure historical integrity
            decimal revenue = _selectedSession.IsClosed ? _selectedSession.TotalRevenue : orders.Sum(o => o.Total);
            int txCount = _selectedSession.IsClosed ? _selectedSession.TotalTransactions : orders.Count;
            int units = _selectedSession.IsClosed ? _selectedSession.TotalUnitsSold : orders.SelectMany(o => o.Items).Sum(i => i.Quantity);
            decimal avgOrder = txCount > 0 ? revenue / txCount : 0;

            // KPIs
            pnlKpiRow.Controls.Clear();
            pnlKpiRow.Controls.Add(CreateKpiCard("Total Revenue", Fmt(revenue)));
            pnlKpiRow.Controls.Add(CreateKpiCard("Transactions", txCount.ToString()));
            pnlKpiRow.Controls.Add(CreateKpiCard("Units Sold", units.ToString()));
            pnlKpiRow.Controls.Add(CreateKpiCard("Avg. Order", Fmt(avgOrder)));

            // Charts
            _categoryData = Program.SessionService.GetCategorySalesData(sid);
            _hourlySalesData = Program.SessionService.GetHourlySalesData(sid);
            pnlDoughnutChart.Invalidate();
            pnlBarChart.Invalidate();

            // Insight cards
            pnlInsightsRow.Controls.Clear();
            var topItem = Program.SessionService.GetTopItems(sid, 1).FirstOrDefault();
            pnlInsightsRow.Controls.Add(CreateInsightCard("\U0001F3C6 Best Seller", topItem.Name ?? "\u2014"));

            var hourly = _hourlySalesData.Where(h => h.Value > 0).OrderByDescending(h => h.Value).FirstOrDefault();
            string peakHour = hourly.Value > 0 ? (hourly.Key > 12 ? $"{hourly.Key - 12}:00 PM" : hourly.Key == 12 ? "12:00 PM" : $"{hourly.Key}:00 AM") : "\u2014";
            pnlInsightsRow.Controls.Add(CreateInsightCard("\u23F0 Peak Hour", peakHour));

            var topCat = _categoryData.OrderByDescending(c => c.Value).FirstOrDefault();
            pnlInsightsRow.Controls.Add(CreateInsightCard("\U0001F4CA Top Category", topCat.Key ?? "\u2014"));

            var largest = orders.OrderByDescending(o => o.Total).FirstOrDefault();
            pnlInsightsRow.Controls.Add(CreateInsightCard("\U0001F4B0 Largest Order", largest != null ? Fmt(largest.Total) : "\u2014"));

            // Top 5 Performing Items
            _topProducts = Program.SessionService.GetTopItems(sid, 5);
            pnlTableCard.Invalidate();

            // Recent Transactions
            _recentOrders = orders.OrderByDescending(o => o.Timestamp).Take(6).ToList();
            pnlRecentTx.Invalidate();
        }

        private Panel CreateEmptyStatePanel()
        {
            var pnl = new Panel { Size = new Size(700, 100), Margin = new Padding(20, 10, 0, 0) };
            var icon = new Label { Text = "⏸", Font = new Font("Segoe UI", 22F), Location = new Point(0, 8), AutoSize = true, BackColor = Color.Transparent };
            var msg = new Label { Text = "No Active Session", Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#111827"), Location = new Point(50, 10), AutoSize = true, BackColor = Color.Transparent };
            var sub = new Label { Text = "Open a store session from the sidebar to start tracking live performance.", Font = new Font("Segoe UI", 9.5F), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(50, 42), AutoSize = true, BackColor = Color.Transparent };
            pnl.Controls.AddRange(new Control[] { icon, msg, sub });
            return pnl;
        }

        private Guna.UI2.WinForms.Guna2Panel CreateKpiCard(string title, string value)
        {
            var pnl = new Guna.UI2.WinForms.Guna2Panel { Size = new Size(180, 84), BorderRadius = 10, BorderColor = ColorTranslator.FromHtml("#F3F4F6"), BorderThickness = 1, FillColor = Color.White, Margin = new Padding(8, 0, 8, 0) };
            pnl.ShadowDecoration.Enabled = true;
            pnl.ShadowDecoration.Shadow = new Padding(0, 0, 5, 5);
            pnl.ShadowDecoration.Color = Color.FromArgb(20, 0, 0, 0);
            pnl.Controls.Add(new Label { Text = title.ToUpper(), Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(14, 14), AutoSize = true, BackColor = Color.Transparent });
            pnl.Controls.Add(new Label { Text = value, Font = new Font("Segoe UI", 16F, FontStyle.Bold), Location = new Point(12, 38), AutoSize = true, BackColor = Color.Transparent });
            return pnl;
        }

        private Guna.UI2.WinForms.Guna2Panel CreateInsightCard(string title, string value)
        {
            var pnl = new Guna.UI2.WinForms.Guna2Panel { Size = new Size(180, 60), BorderRadius = 8, BorderColor = ColorTranslator.FromHtml("#E5E7EB"), BorderThickness = 1, FillColor = ColorTranslator.FromHtml("#F9FAFB"), Margin = new Padding(8, 0, 8, 0) };
            pnl.ShadowDecoration.Enabled = true;
            pnl.ShadowDecoration.Shadow = new Padding(0, 0, 3, 3);
            pnl.ShadowDecoration.Color = Color.FromArgb(10, 0, 0, 0);
            pnl.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 8F), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(12, 8), AutoSize = true, BackColor = Color.Transparent });
            pnl.Controls.Add(new Label { Text = value, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#111827"), Location = new Point(12, 28), AutoSize = true, BackColor = Color.Transparent });
            return pnl;
        }

        // ── SESSION HISTORY PAGE (month/year filter) ──────────
        private List<BusinessSession> _historySessions = new();

        private void LoadSessionHistoryPage()
        {
            var allSessions = Program.SessionService.GetAllSessions();

            int selectedMonth = cmbHistoryMonth.SelectedIndex; // 0 = All
            int selectedYear = int.TryParse(cmbHistoryYear.SelectedItem?.ToString(), out int y) ? y : DateTime.Now.Year;

            _historySessions = allSessions.Where(s =>
            {
                if (s.OpenedAt.Year != selectedYear) return false;
                if (selectedMonth > 0 && s.OpenedAt.Month != selectedMonth) return false;
                return true;
            }).OrderByDescending(s => s.OpenedAt).ToList();

            lblSessionCount.Text = $"{_historySessions.Count} session{(_historySessions.Count != 1 ? "s" : "")}";

            dgvSessionHistory.Rows.Clear();
            foreach (var s in _historySessions)
            {
                string time = $"{s.OpenedAt:hh:mm tt}" + (s.IsClosed ? $" - {s.ClosedAt:hh:mm tt}" : "");
                string duration = s.IsClosed && s.ClosedAt.HasValue ? $"{(s.ClosedAt.Value - s.OpenedAt).TotalHours:0.0}h" : "Active";
                string status = s.IsClosed ? "✅ Closed" : "🟢 Active";
                dgvSessionHistory.Rows.Add(
                    s.OpenedAt.ToString("MMM dd, yyyy"), time, duration,
                    s.TotalTransactions.ToString(), s.TotalUnitsSold.ToString(),
                    Fmt(s.TotalRevenue), s.OpenedBy, status
                );
            }
        }

        // ── PREVIOUS REPORTS (charts only) ────────────────────
        private void LoadHistoryCharts()
        {
            var allSessions = Program.SessionService.GetAllSessions();
            var recent = allSessions.Where(s => s.IsClosed).Take(12).Reverse().ToList();
            _historyRevenue = recent.Select(s => (s.OpenedAt.ToString("MMM dd"), s.TotalRevenue)).ToList();
            _historyTxCounts = recent.Select(s => (s.OpenedAt.ToString("MMM dd"), (decimal)s.TotalTransactions)).ToList();
            pnlRevenueChart.Invalidate();
            pnlTxChart.Invalidate();
        }

        private void DgvSessionHistory_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _historySessions.Count) return;
            var session = _historySessions[e.RowIndex];
            using var detail = new SessionDetailForm(session);
            detail.ShowDialog(this.FindForm());
        }

        // ── Over/Short ─────────────────────────────────────────


        // ── Close Session ──────────────────────────────────────
        private async void BtnCloseDay_Click(object? sender, EventArgs e)
        {
            var activeSession = Program.SessionService.GetActiveSession();
            if (activeSession == null) { MessageBox.Show("No active session.", "Session", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            var settings = Program.DataService.Settings;
            decimal actualCash = 0;

            if (Program.DataService.Settings.RequirePasswordForCloseSession)
            {
                using var authDialog = new PasswordPromptDialog("Enter password to close the session.");
                if (authDialog.ShowDialog(this) != DialogResult.OK) return;
            }

            if (settings.RequireCashCountOnClose)
            {
                using var closeDialog = new CloseSessionDialogForm(activeSession);
                
                // Dim background
                Form bg = new Form();
                bg.StartPosition = FormStartPosition.Manual;
                bg.FormBorderStyle = FormBorderStyle.None;
                bg.Opacity = 0.50d;
                bg.BackColor = Color.Black;
                bg.WindowState = FormWindowState.Maximized;
                bg.TopMost = false;
                bg.Location = this.FindForm()!.Location;
                bg.ShowInTaskbar = false;
                bg.Show();

                closeDialog.Owner = bg;
                var result = closeDialog.ShowDialog();
                
                bg.Dispose();

                if (result != DialogResult.OK) return; // User canceled

                actualCash = closeDialog.ActualCashCounted;
            }
            else
            {
                Program.SessionService.ComputeSessionTotals(activeSession);
                actualCash = activeSession.StartingCash + activeSession.TotalRevenue;
            }

            btnCloseDay.Enabled = false;
            try 
            {
                var closed = await Program.SessionService.CloseSessionAsync(actualCash, Program.GetCurrentCashierName());
                decimal overShort = closed.ActualCash - closed.ExpectedCash;
                var best = Program.SessionService.GetSessionOrders(closed.SessionId).SelectMany(o => o.Items).GroupBy(i => i.ProductName).OrderByDescending(g => g.Sum(i => i.Quantity)).FirstOrDefault();

                MessageBox.Show(
                    $"═══ Z-REPORT ═══\nSession: {closed.OpenedAt:MMM dd, yyyy}\nOpened: {closed.OpenedAt:hh:mm tt} by {closed.OpenedBy}\nClosed: {closed.ClosedAt:hh:mm tt} by {closed.ClosedBy}\n────────────────\nTransactions: {closed.TotalTransactions}\nUnits Sold: {closed.TotalUnitsSold}\nRevenue: {Fmt(closed.TotalRevenue)}\n────────────────\nStarting Cash: {Fmt(closed.StartingCash)}\nExpected: {Fmt(closed.ExpectedCash)}\nActual: {Fmt(closed.ActualCash)}\nOver/Short: {(overShort >= 0 ? "+" : "")}{Fmt(overShort)}\n────────────────\nBest Seller: {best?.Key ?? "N/A"}\n\nSession locked.",
                    "Z-Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtActualCash.Text = "";
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCloseDay.Enabled = true;
            }
        }

        // ── Open Session ───────────────────────────────────────
        private async void BtnOpenStore_Click(object? sender, EventArgs e)
        {
            if (Program.SessionService.HasActiveSession()) return;
            string cashierName = Program.GetCurrentCashierName();
            decimal defaultCash = Program.DataService.Settings.DefaultStartingCash;

            using var openDialog = new OpenSessionDialogForm(cashierName, defaultCash);
            
            Form bg = new Form();
            bg.StartPosition = FormStartPosition.Manual;
            bg.FormBorderStyle = FormBorderStyle.None;
            bg.Opacity = 0.50d;
            bg.BackColor = Color.Black;
            bg.WindowState = FormWindowState.Maximized;
            bg.TopMost = false;
            bg.Location = this.FindForm()!.Location;
            bg.ShowInTaskbar = false;
            bg.Show();

            openDialog.Owner = bg;
            var result = openDialog.ShowDialog();
            
            bg.Dispose();

            if (result != DialogResult.OK) return;

            decimal startingCash = openDialog.StartingCash;

            btnOpenStore.Enabled = false;
            try 
            { 
                await Program.SessionService.OpenSessionAsync(cashierName, startingCash);
            }
            catch (InvalidOperationException ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            finally { btnOpenStore.Enabled = true; }
        }

        // ── Session UI ─────────────────────────────────────────
        private void UpdateSessionUI()
        {
            var session = Program.SessionService.GetActiveSession();
            bool isActive = session != null;
            btnOpenStore.Visible = !isActive;
            btnCloseDay.Visible = isActive;
            btnPrintReport.Visible = isActive;
            
            // Permanently hide redundant inline cash count controls since we use CloseSessionDialogForm now
            txtActualCash.Visible = false;
            lblActualCashLabel.Visible = false;
            lblOverShortLabel.Visible = false;
            lblOverShortValue.Visible = false;
            pnlInfoBox.Visible = false;

            if (isActive)
            {
                lblSessionStatus.Text = "\u2705 Session Active";
                lblSessionStatus.ForeColor = ColorTranslator.FromHtml("#52B743");
                lblSessionTime.Text = $"Opened {session!.OpenedAt:hh:mm tt} by {session.OpenedBy}";
                lblDrawerFundValue.Text = Fmt(session.StartingCash);
                var orders = Program.SessionService.GetSessionOrders(session.SessionId);
                lblExpectedCashValue.Text = Fmt(session.StartingCash + orders.Sum(o => o.Total));
                lblTxCountValue.Text = orders.Count.ToString();
                var best = orders.SelectMany(o => o.Items).GroupBy(i => i.ProductName).OrderByDescending(g => g.Sum(i => i.Quantity)).FirstOrDefault();
                lblBestSellerValue.Text = best?.Key ?? "\u2014";
            }
            else
            {
                lblSessionStatus.Text = "\u26AA No active session";
                lblSessionStatus.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
                lblSessionTime.Text = "";
                lblExpectedCashValue.Text = "\u2014";
                lblTxCountValue.Text = "\u2014";
                lblBestSellerValue.Text = "\u2014";
                lblDrawerFundValue.Text = "\u2014";
            }
        }

        // ── Export CSV ──────────────────────────────────────────
        private void BtnExportCsv_Click(object? sender, EventArgs e)
        {
            if (_selectedSession == null) return;
            var orders = _selectedSessionOrders;
            using var dlg = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv", FileName = $"Session_{_selectedSession.OpenedAt:yyyyMMdd}.csv" };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            
            var sb = new StringBuilder();
            sb.AppendLine($"Session ID, {_selectedSession.SessionId}");
            sb.AppendLine($"Opened At, {_selectedSession.OpenedAt:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Opened By, {_selectedSession.OpenedBy}");
            sb.AppendLine();
            sb.AppendLine("Order ID,Customer,Items,Amount,Time\n");
            
            foreach (var o in orders.OrderByDescending(o => o.Timestamp))
            {
                var cust = Program.DataService.Customers.FirstOrDefault(c => c.Id == o.CustomerId);
                sb.AppendLine($"\"{o.OrderId}\",\"{cust?.Name ?? "Walk-in"}\",\"{string.Join("; ", o.Items.Select(i => $"{i.Quantity}x {i.ProductName}"))}\",{o.Total},{o.Timestamp:hh:mm tt}");
            }
            File.WriteAllText(dlg.FileName, sb.ToString());
            MessageBox.Show($"Exported to:\n{dlg.FileName}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── Export PDF ──────────────────────────────────────────
        private void BtnExportPdf_Click(object? sender, EventArgs e)
        {
            if (_selectedSession == null) return;
            var sid = _selectedSession.SessionId;
            var orders = Program.SessionService.GetSessionOrders(sid);
            var items = Program.SessionService.GetAllItemSales(sid);
            using var dlg = new SaveFileDialog { Filter = "PDF (*.pdf)|*.pdf", FileName = $"Session_{_selectedSession.OpenedAt:yyyyMMdd}.pdf" };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            Document.Create(c => c.Page(page =>
            {
                page.Size(PageSizes.A4); page.Margin(30); page.DefaultTextStyle(x => x.FontSize(10));
                page.Header().Column(col =>
                {
                    col.Item().Text("S.I.P. \u2014 Session Report").Bold().FontSize(18).FontColor("#52B743");
                    col.Item().Text($"Session ID: {_selectedSession.SessionId}").FontSize(8).FontColor("#9CA3AF");
                    col.Item().Text($"Opened: {_selectedSession.OpenedAt:MMM dd, yyyy hh:mm tt} by {_selectedSession.OpenedBy}").FontSize(9).FontColor("#6B7280");
                    if (!string.IsNullOrWhiteSpace(Program.DataService.Settings.CurrentOperatingLocation))
                        col.Item().Text($"Location: {Program.DataService.Settings.CurrentOperatingLocation}").FontSize(9).FontColor("#6B7280");
                    if (_selectedSession.IsClosed)
                        col.Item().Text($"Closed: {_selectedSession.ClosedAt:MMM dd, yyyy hh:mm tt} by {_selectedSession.ClosedBy}").FontSize(9).FontColor("#6B7280");
                    
                    col.Item().PaddingTop(5).PaddingBottom(10).LineHorizontal(1).LineColor("#E5E7EB");
                });
                page.Content().Column(col =>
                {
                    col.Item().PaddingBottom(6).Text("Product Sales").Bold().FontSize(12);
                    col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c2 => { c2.RelativeColumn(3); c2.RelativeColumn(1.5f); c2.RelativeColumn(1); c2.RelativeColumn(1.5f); });
                        t.Header(h => { foreach (var hdr in new[] { "Product", "Category", "Units", "Revenue" }) h.Cell().Background("#52B743").Padding(6).Text(hdr).FontColor("#FFF").Bold().FontSize(9); });
                        foreach (var item in items) { t.Cell().BorderBottom(1).BorderColor("#E5E7EB").Padding(5).Text(item.Name).FontSize(9); t.Cell().BorderBottom(1).BorderColor("#E5E7EB").Padding(5).Text(item.Category).FontSize(9); t.Cell().BorderBottom(1).BorderColor("#E5E7EB").Padding(5).Text(item.Units.ToString()).FontSize(9); t.Cell().BorderBottom(1).BorderColor("#E5E7EB").Padding(5).Text(Fmt(item.Revenue)).FontSize(9); }
                    });
                });
                page.Footer().AlignCenter().Text(t => { t.Span("S.I.P. \u2014 ").FontSize(8).FontColor("#9CA3AF"); t.Span($"Generated {DateTime.Now:MMM dd, yyyy}").FontSize(8).FontColor("#9CA3AF"); });
            })).GeneratePdf(dlg.FileName);
            MessageBox.Show($"PDF exported to:\n{dlg.FileName}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string Fmt(decimal amount) => $"\u20b1{amount:#,##0.00}";

        protected override void Dispose(bool disposing) { if (disposing) { tmrLiveRefresh.Stop(); tmrLiveRefresh.Dispose(); components?.Dispose(); } base.Dispose(disposing); }
    }
}
