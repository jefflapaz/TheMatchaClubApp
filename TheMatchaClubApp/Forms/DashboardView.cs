using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class DashboardView : UserControl
    {
        // ── Data fields (accessed by paint handlers in partial classes) ──
        internal Dictionary<int, decimal>? _hourlySalesData;
        internal List<(string Name, int Units, decimal Revenue)>? _topProducts;
        internal List<Order>? _recentOrders;
        internal string? _sessionDurationText;
        internal decimal _todaySalesTotal;
        internal int _hoveredTxRow = -1;
        private bool _isLoading;

        // ── Navigation Events ───────────────────────────────────────
        public event EventHandler? NewSaleClicked;
        public event EventHandler? ViewReportsClicked;
        public event EventHandler? AddProductClicked;
        public event EventHandler<string>? ViewOrderClicked;

        // ══════════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ══════════════════════════════════════════════════════════════
        public DashboardView()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();
            WireEvents();
            LoadDashboardData();
        }

        private void WireEvents()
        {
            // Quick action buttons
            btnQuickNewSale.Click += (s, e) => NewSaleClicked?.Invoke(this, EventArgs.Empty);
            btnQuickOpenSession.Click += (s, e) => HandleOpenSession();
            btnQuickCloseSession.Click += (s, e) => HandleCloseSession();
            btnQuickReports.Click += (s, e) => ViewReportsClicked?.Invoke(this, EventArgs.Empty);
            btnQuickAddProduct.Click += (s, e) => AddProductClicked?.Invoke(this, EventArgs.Empty);

            // Empty state action button
            btnEmptyAction.Click += (s, e) =>
            {
                if (btnEmptyAction.Text.Contains("Reports"))
                    ViewReportsClicked?.Invoke(this, EventArgs.Empty);
                else if (!Program.SessionService.HasActiveSession())
                    HandleOpenSession();
                else
                    NewSaleClicked?.Invoke(this, EventArgs.Empty);
            };

            // Recent transactions: hover + click
            pnlRecentTx.MouseMove += PnlRecentTx_MouseMove;
            pnlRecentTx.MouseLeave += (s, e) => { _hoveredTxRow = -1; pnlRecentTx.Cursor = Cursors.Default; pnlRecentTx.Invalidate(); };
            pnlRecentTx.MouseClick += PnlRecentTx_MouseClick;

            // Real-time data subscriptions
            Program.DataService.OrdersChanged += OnDataChanged;
            Program.DataService.ProductsChanged += OnDataChanged;
            Program.DataService.SessionsChanged += OnDataChanged;
            Program.DataService.DataLoaded += OnDataChanged;
            Program.SessionService.SessionOpened += OnDataChanged;
            Program.SessionService.SessionClosed += OnDataChanged;

            // Session duration timer
            tmrSessionDuration.Tick += (s, e) => UpdateSessionDuration();
            tmrSessionDuration.Start();
        }

        private void PnlRecentTx_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_recentOrders == null || _recentOrders.Count == 0) { _hoveredTxRow = -1; return; }
            int headerEnd = 62; // header row + separator
            int rowH = Math.Max(28, Math.Min(32, (pnlRecentTx.Height - 68) / Math.Max(_recentOrders.Count, 1)));
            int row = (e.Y - headerEnd) / rowH;
            int newHover = (row >= 0 && row < _recentOrders.Count) ? row : -1;
            if (newHover != _hoveredTxRow)
            {
                _hoveredTxRow = newHover;
                pnlRecentTx.Cursor = newHover >= 0 ? Cursors.Hand : Cursors.Default;
                pnlRecentTx.Invalidate();
            }
        }

        private void PnlRecentTx_MouseClick(object? sender, MouseEventArgs e)
        {
            if (_hoveredTxRow >= 0 && _recentOrders != null && _hoveredTxRow < _recentOrders.Count)
            {
                var order = _recentOrders[_hoveredTxRow];
                ViewOrderClicked?.Invoke(this, order.OrderId);
            }
        }

        private void OnDataChanged(object? s, EventArgs e)
        {
            if (!IsDisposed && IsHandleCreated)
                BeginInvoke(new Action(LoadDashboardData));
        }

        // ══════════════════════════════════════════════════════════════
        //  LIVE DATA LOADING
        // ══════════════════════════════════════════════════════════════
        private void LoadDashboardData()
        {
            if (_isLoading) return;
            _isLoading = true;
            try
            {
                SuspendLayout();
            var session = Program.SessionService.GetActiveSession();
            var todayOrders = Program.DataService.Orders
                .Where(o => o.Timestamp.Date == DateTime.Today).ToList();

            // If there's an active session, filter to session orders
            List<Order> relevantOrders;
            if (session != null)
                relevantOrders = todayOrders.Where(o => o.SessionId == session.SessionId).ToList();
            else
                relevantOrders = todayOrders;

            // ── KPI Calculations ──────────────────────────────────
            decimal totalSales = relevantOrders.Sum(o => o.Total);
            int orderCount = relevantOrders.Count;
            decimal avgOrder = orderCount > 0 ? totalSales / orderCount : 0m;
            int unitsSold = relevantOrders.SelectMany(o => o.Items).Sum(i => i.Quantity);
            _todaySalesTotal = totalSales;

            decimal cashOnHand = session != null
                ? session.StartingCash + totalSales
                : totalSales;

            // Best seller
            string bestSeller = "—";
            var topItem = relevantOrders.SelectMany(o => o.Items)
                .GroupBy(i => i.ProductName)
                .OrderByDescending(g => g.Sum(i => i.Quantity))
                .FirstOrDefault();
            if (topItem != null) bestSeller = topItem.Key;

            // Peak hour
            string peakHour = "—";
            if (relevantOrders.Count > 0)
            {
                var hourGroup = relevantOrders.GroupBy(o => o.Timestamp.Hour)
                    .OrderByDescending(g => g.Sum(o => o.Total)).First();
                int h = hourGroup.Key;
                peakHour = h > 12 ? $"{h - 12}:00 PM" : h == 12 ? "12:00 PM" : h == 0 ? "12:00 AM" : $"{h}:00 AM";
            }

            // ── Update KPI Labels ─────────────────────────────────
            lblCard1Value.Text = $"₱{totalSales:#,##0.00}";
            lblCard2Value.Text = orderCount.ToString();
            lblCard3Value.Text = $"₱{avgOrder:#,##0.00}";
            lblCard4Value.Text = $"₱{cashOnHand:#,##0.00}";
            lblCard5Value.Text = unitsSold.ToString();
            lblCard6Value.Text = bestSeller.Length > 14 ? bestSeller[..14] + "…" : bestSeller;
            UpdateSessionDuration(); // Card 7
            lblCard8Value.Text = peakHour;

            // ── Comparison vs Previous Session ───────────────────
            var prevSession = Program.DataService.Sessions
                .Where(s => s.IsClosed && (session == null || s.SessionId != session.SessionId))
                .OrderByDescending(s => s.OpenedAt)
                .FirstOrDefault();

            if (prevSession != null && prevSession.TotalRevenue > 0 && totalSales > 0)
            {
                decimal pctChange = (totalSales - prevSession.TotalRevenue) / prevSession.TotalRevenue * 100;
                string arrow = pctChange >= 0 ? "↑" : "↓";
                lblCard1Title.Text = $"Total Sales Today  {arrow} {Math.Abs(pctChange):0.0}% vs last";
            }
            else
            {
                lblCard1Title.Text = "Total Sales Today";
            }

            lblDate.Text = "📅 " + DateTime.Today.ToString("M/d/yyyy");

            // ── Build Chart Data ──────────────────────────────────
            _hourlySalesData = new Dictionary<int, decimal>();
            for (int h = 0; h < 24; h++) _hourlySalesData[h] = 0;
            foreach (var o in relevantOrders)
                _hourlySalesData[o.Timestamp.Hour] += o.Total;

            // ── Top Products ──────────────────────────────────────
            _topProducts = relevantOrders.SelectMany(o => o.Items)
                .GroupBy(i => i.ProductName)
                .Select(g => (Name: g.Key, Units: g.Sum(i => i.Quantity), Revenue: g.Sum(i => i.LineTotal)))
                .OrderByDescending(x => x.Units)
                .Take(5).ToList();

            // ── Recent Transactions ───────────────────────────────
            _recentOrders = relevantOrders
                .OrderByDescending(o => o.Timestamp)
                .Take(5).ToList();

            // ── Update Visibility ─────────────────────────────────
            UpdateEmptyState(session, relevantOrders.Count);
            UpdateQuickActionStates(session);
            UpdateStoreStatus(session);

            // Repaint analytics
            pnlHourlySales.Invalidate();
            pnlTopProducts.Invalidate();
            pnlRecentTx.Invalidate();
            pnlSessionStatus.Invalidate();

                ResumeLayout();
            }
            finally
            {
                _isLoading = false;
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  EMPTY STATE LOGIC
        // ══════════════════════════════════════════════════════════════
        private void UpdateEmptyState(BusinessSession? session, int orderCount)
        {
            bool showEmpty;

            if (session == null)
            {
                var lastSession = Program.DataService.Sessions
                    .Where(s => s.IsClosed)
                    .OrderByDescending(s => s.OpenedAt)
                    .FirstOrDefault();

                var closedToday = Program.DataService.Sessions
                    .Any(s => s.IsClosed && s.OpenedAt.Date == DateTime.Today);

                bool hasAnySessions = Program.DataService.Sessions.Any();

                if (!hasAnySessions)
                {
                    // First-time user
                    lblEmptyIcon.Text = "🏡";
                    lblEmptyMessage.Text = "Welcome! Open your first session to get started.";
                    btnEmptyAction.Text = "Open Session";
                }
                else if (closedToday)
                {
                    lblEmptyIcon.Text = "✅";
                    lblEmptyMessage.Text = "Store session closed. View reports for summary.";
                    btnEmptyAction.Text = "View Reports";
                }
                else
                {
                    lblEmptyIcon.Text = "🔒";
                    lblEmptyMessage.Text = "Open a store session to begin operations.";
                    btnEmptyAction.Text = "Open Session";
                }

                // Show last session snapshot in KPI cards when no session active
                if (lastSession != null)
                {
                    lblCard1Value.Text = $"₱{lastSession.TotalRevenue:#,##0.00}";
                    lblCard1Title.Text = $"Last Session ({lastSession.OpenedAt:MMM dd})";
                    lblCard2Value.Text = lastSession.TotalTransactions.ToString();
                    lblCard5Value.Text = lastSession.TotalUnitsSold.ToString();

                    var lastOrders = Program.SessionService.GetSessionOrders(lastSession.SessionId);
                    var topItem = lastOrders.SelectMany(o => o.Items)
                        .GroupBy(i => i.ProductName)
                        .OrderByDescending(g => g.Sum(i => i.Quantity))
                        .FirstOrDefault();
                    lblCard6Value.Text = topItem?.Key ?? "—";
                }

                showEmpty = true;
            }
            else if (orderCount == 0)
            {
                lblEmptyIcon.Text = "⏳";
                lblEmptyMessage.Text = "Session is open. Waiting for first sale…";
                btnEmptyAction.Text = "🛒  New Sale";
                showEmpty = true;
            }
            else
            {
                showEmpty = false;
            }

            pnlEmptyState.Visible = showEmpty;
            pnlHourlySales.Visible = !showEmpty;
            pnlTopProducts.Visible = !showEmpty;
            pnlRecentTx.Visible = !showEmpty;
            // Session status always visible
        }

        private void UpdateQuickActionStates(BusinessSession? session)
        {
            btnQuickOpenSession.Enabled = session == null;
            btnQuickCloseSession.Enabled = session != null;
        }

        private void UpdateStoreStatus(BusinessSession? session)
        {
            if (session != null)
            {
                lblStoreStatus.Text = "   STORE OPEN";
                lblStoreStatus.ForeColor = Green;
                pnlStoreStatus.FillColor = GreenBg;
                pnlStoreStatus.BorderColor = GreenBorder;
            }
            else
            {
                lblStoreStatus.Text = "   CLOSED";
                lblStoreStatus.ForeColor = Rose;
                pnlStoreStatus.FillColor = RoseBg;
                pnlStoreStatus.BorderColor = Rose;
            }
            pnlStoreStatus.Invalidate();
        }

        // ══════════════════════════════════════════════════════════════
        //  SESSION MANAGEMENT
        // ══════════════════════════════════════════════════════════════
        private bool _isSessionProcessing; // Prevent double-click on session operations

        private async void HandleOpenSession()
        {
            if (_isSessionProcessing) return;
            if (Program.SessionService.HasActiveSession())
            {
                MessageBox.Show("A session is already active.", "Session Open", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string cashierName = Program.GetCurrentCashierName();
            string defaultCash = Program.DataService.Settings.DefaultStartingCash.ToString("F2");
            string? input = ShowInputDialog("Enter starting cash amount:", "Open Store Session", defaultCash);
            if (string.IsNullOrWhiteSpace(input)) return;
            if (!decimal.TryParse(input, out decimal startingCash) || startingCash < 0)
            {
                MessageBox.Show("Please enter a valid cash amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmation
            var confirm = MessageBox.Show(
                $"Open a new store session?\n\nCashier: {cashierName}\nStarting Cash: ₱{startingCash:#,##0.00}",
                "Confirm Open Session",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            _isSessionProcessing = true;
            btnQuickOpenSession.Enabled = false;
            try
            {
                await Program.SessionService.OpenSessionAsync(cashierName, startingCash);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally
            {
                _isSessionProcessing = false;
                btnQuickOpenSession.Enabled = true;
            }
        }

        private async void HandleCloseSession()
        {
            if (_isSessionProcessing) return;
            if (!Program.SessionService.HasActiveSession())
            {
                MessageBox.Show("No active session to close.", "No Session", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var activeSession = Program.SessionService.GetActiveSession()!;
            Program.SessionService.ComputeSessionTotals(activeSession);
            decimal expectedCash = activeSession.StartingCash + activeSession.TotalRevenue;
            var settings = Program.DataService.Settings;

            decimal actualCash = 0;
            if (settings.RequireCashCountOnClose)
            {
                string? input = ShowInputDialog(
                    $"CLOSE SESSION - CASH COUNT\n\nPlease count and enter the actual cash in the register drawer:\n(Expected: ₱{expectedCash:#,##0.00})", 
                    "Close Store Session", 
                    "");
                
                if (input == null) return; // User clicked Cancel or closed the dialog
                
                if (string.IsNullOrWhiteSpace(input) || !decimal.TryParse(input, out actualCash) || actualCash < 0)
                {
                    MessageBox.Show("Please enter a valid cash amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                actualCash = expectedCash; // If count not required, assume actual equals expected
            }

            decimal diff = actualCash - expectedCash;

            // Warning if over/short warnings are enabled and discrepancy exists
            if (settings.EnableOverShortWarnings && diff != 0)
            {
                string statusText = diff > 0 
                    ? $"🟢 OVER by ₱{diff:#,##0.00} (Extra cash in register)" 
                    : $"🔴 SHORT by ₱{Math.Abs(diff):#,##0.00} (Missing cash)";

                var warnResult = MessageBox.Show(
                    $"⚠️ CASH DISCREPANCY DETECTED!\n\n" +
                    $"Expected Cash: ₱{expectedCash:#,##0.00}\n" +
                    $"Actual Counted: ₱{actualCash:#,##0.00}\n\n" +
                    $"{statusText}\n\n" +
                    "Are you absolutely sure you want to close this session with this discrepancy?",
                    "Warning: Cash Discrepancy",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (warnResult != DialogResult.Yes) return;
            }
            else
            {
                // Normal closing confirmation
                string discrepancyInfo = settings.RequireCashCountOnClose 
                    ? $"\nActual Cash: ₱{actualCash:#,##0.00}" 
                    : "";

                var confirm = MessageBox.Show(
                    $"Are you sure you want to close the store session?\n\n" +
                    $"Revenue: ₱{activeSession.TotalRevenue:#,##0.00}\n" +
                    $"Transactions: {activeSession.TotalTransactions}\n" +
                    $"Expected Cash: ₱{expectedCash:#,##0.00}" + 
                    discrepancyInfo,
                    "Confirm Close Session",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;
            }

            _isSessionProcessing = true;
            btnQuickCloseSession.Enabled = false;
            try
            {
                var session = await Program.SessionService.CloseSessionAsync(actualCash, Program.GetCurrentCashierName());
                
                // Show final success notification
                string finalMessage = $"Session closed successfully.\n\nRevenue: ₱{session.TotalRevenue:#,##0.00}\nTransactions: {session.TotalTransactions}";
                if (diff != 0)
                {
                    finalMessage += diff > 0 
                        ? $"\nOver: +₱{diff:#,##0.00}" 
                        : $"\nShort: -₱{Math.Abs(diff):#,##0.00}";
                }
                MessageBox.Show(finalMessage, "Session Closed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Auto-generate Z-Report if enabled
                if (settings.AutoGenerateZReport)
                {
                    AutoGenerateZReport(session);
                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            finally
            {
                _isSessionProcessing = false;
                btnQuickCloseSession.Enabled = true;
            }
        }

        private void AutoGenerateZReport(BusinessSession session)
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string reportsDir = Path.Combine(documentsPath, "TheMatchaClub", "ZReports");
                Directory.CreateDirectory(reportsDir);

                string fileName = $"ZReport_Session_{session.OpenedAt:yyyyMMdd_HHmmss}.pdf";
                string fullPath = Path.Combine(reportsDir, fileName);

                Helpers.ZReportHelper.GenerateZReportPdf(session, fullPath);

                // Open the PDF automatically
                var ps = new System.Diagnostics.ProcessStartInfo(fullPath)
                {
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(ps);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to auto-generate Z-Report: {ex.Message}", "Z-Report Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static string? ShowInputDialog(string prompt, string title, string defaultValue)
        {
            using var form = new Form 
            { 
                Text = title, 
                Width = 380, 
                Height = 220, 
                FormBorderStyle = FormBorderStyle.FixedDialog, 
                StartPosition = FormStartPosition.CenterParent, 
                MaximizeBox = false, 
                MinimizeBox = false 
            };
            
            var lbl = new Label 
            { 
                Text = prompt, 
                Left = 16, 
                Top = 16, 
                Width = 330, 
                AutoSize = true 
            };
            
            // Add label first to calculate its true dynamic height
            form.Controls.Add(lbl);
            
            int txtTop = lbl.Bottom + 12;
            var txt = new TextBox 
            { 
                Text = defaultValue, 
                Left = 16, 
                Top = txtTop, 
                Width = 330 
            };
            
            int btnTop = txt.Bottom + 16;
            var ok = new Button 
            { 
                Text = "OK", 
                DialogResult = DialogResult.OK, 
                Left = 190, 
                Top = btnTop, 
                Width = 75 
            };
            var cancel = new Button 
            { 
                Text = "Cancel", 
                DialogResult = DialogResult.Cancel, 
                Left = 271, 
                Top = btnTop, 
                Width = 75 
            };
            
            form.Controls.AddRange(new Control[] { txt, ok, cancel });
            
            // Adjust form height to dynamically fit all elements perfectly
            form.ClientSize = new Size(380, cancel.Bottom + 16);
            
            form.AcceptButton = ok;
            form.CancelButton = cancel;
            
            // Auto-focus and highlight textbox text for immediate typing convenience
            form.Shown += (s, e) => 
            { 
                txt.Focus(); 
                txt.SelectAll(); 
            };
            
            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }

        // ══════════════════════════════════════════════════════════════
        //  SESSION DURATION TIMER
        // ══════════════════════════════════════════════════════════════
        private void UpdateSessionDuration()
        {
            var session = Program.SessionService.GetActiveSession();
            if (session != null)
            {
                var elapsed = DateTime.Now - session.OpenedAt;
                _sessionDurationText = $"{(int)elapsed.TotalHours}h {elapsed.Minutes:D2}m";
                lblCard7Value.Text = _sessionDurationText;
            }
            else
            {
                _sessionDurationText = null;
                lblCard7Value.Text = "—";
            }
            pnlSessionStatus.Invalidate();
        }
    }
}
