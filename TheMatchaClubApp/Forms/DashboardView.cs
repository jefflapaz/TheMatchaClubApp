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

        // ── Navigation Events ───────────────────────────────────────
        public event EventHandler? NewSaleClicked;
        public event EventHandler? ViewReportsClicked;
        public event EventHandler? AddProductClicked;

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
        }

        // ══════════════════════════════════════════════════════════════
        //  EMPTY STATE LOGIC
        // ══════════════════════════════════════════════════════════════
        private void UpdateEmptyState(BusinessSession? session, int orderCount)
        {
            bool showEmpty;

            if (session == null)
            {
                // Check if there was a closed session today
                var closedToday = Program.DataService.Sessions
                    .Any(s => s.IsClosed && s.OpenedAt.Date == DateTime.Today);

                if (closedToday)
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
                showEmpty = true;
            }
            else if (orderCount == 0)
            {
                lblEmptyIcon.Text = "⏳";
                lblEmptyMessage.Text = "Session is open. Waiting for first transaction…";
                btnEmptyAction.Text = "New Sale";
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
        private async void HandleOpenSession()
        {
            if (Program.SessionService.HasActiveSession())
            {
                MessageBox.Show("A session is already active.", "Session Open", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string cashierName = Program.CurrentUser?.FullName ?? "Admin";
            string? input = ShowInputDialog("Enter starting cash amount:", "Open Store Session", "200.00");
            if (string.IsNullOrWhiteSpace(input)) return;
            if (!decimal.TryParse(input, out decimal startingCash) || startingCash < 0)
            {
                MessageBox.Show("Please enter a valid cash amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try { await Program.SessionService.OpenSessionAsync(cashierName, startingCash); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private async void HandleCloseSession()
        {
            if (!Program.SessionService.HasActiveSession())
            {
                MessageBox.Show("No active session to close.", "No Session", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string? input = ShowInputDialog("Enter actual cash counted in register:", "Close Store Session", "0.00");
            if (string.IsNullOrWhiteSpace(input)) return;
            if (!decimal.TryParse(input, out decimal actualCash) || actualCash < 0)
            {
                MessageBox.Show("Please enter a valid cash amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var session = await Program.SessionService.CloseSessionAsync(actualCash, Program.CurrentUser?.FullName);
                decimal diff = session.ActualCash - session.ExpectedCash;
                string status = diff >= 0 ? $"Over: +₱{diff:#,##0.00}" : $"Short: -₱{Math.Abs(diff):#,##0.00}";
                MessageBox.Show(
                    $"Session closed successfully.\n\nRevenue: ₱{session.TotalRevenue:#,##0.00}\nTransactions: {session.TotalTransactions}\n{status}",
                    "Session Closed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private static string? ShowInputDialog(string prompt, string title, string defaultValue)
        {
            using var form = new Form { Text = title, Width = 360, Height = 180, FormBorderStyle = FormBorderStyle.FixedDialog, StartPosition = FormStartPosition.CenterParent, MaximizeBox = false, MinimizeBox = false };
            var lbl = new Label { Text = prompt, Left = 16, Top = 16, Width = 310, AutoSize = true };
            var txt = new TextBox { Text = defaultValue, Left = 16, Top = 44, Width = 310 };
            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Left = 170, Top = 90, Width = 75 };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Left = 251, Top = 90, Width = 75 };
            form.Controls.AddRange(new Control[] { lbl, txt, ok, cancel });
            form.AcceptButton = ok;
            form.CancelButton = cancel;
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
