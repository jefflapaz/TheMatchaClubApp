using System;
using System.Drawing;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class MainShell : Form, IMessageFilter
    {
        private System.Windows.Forms.Timer? _inactivityTimer;
        private DateTime _lastInteractionTime = DateTime.MinValue;
        // ── Lazy-initialized views ─────────────────────────────────
        private DashboardView? _dashboard;
        private QuickSaleView? _quickSale;
        private OrdersView? _orders;
        private ItemsView? _items;
        private CustomersView? _customers;
        private ReportsView? _reports;
        private SettingsView? _settings;

        public MainShell()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();

            // Load all persisted data at startup
            LoadDataAsync();

            // Subscribe to global settings changes for branding sync
            Program.DataService.SettingsChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(UpdateBranding));
            };

            // Default: show Dashboard on launch
            NavDashboard_Click(this, EventArgs.Empty);

            // Register global activity hook & Setup inactivity timer
            Application.AddMessageFilter(this);
            SetupInactivityTimer();
        }

        private async void LoadDataAsync()
        {
            await Program.DataService.LoadAllAsync();
            UpdateBranding();

            // ── Session Recovery: detect unclosed sessions from crashes ──
            var activeSession = Program.SessionService.GetActiveSession();
            if (activeSession != null)
            {
                var elapsed = DateTime.Now - activeSession.OpenedAt;
                MessageBox.Show(
                    $"An active session was recovered.\n\n" +
                    $"Opened by: {activeSession.OpenedBy}\n" +
                    $"Started: {activeSession.OpenedAt:g}\n" +
                    $"Duration: {(int)elapsed.TotalHours}h {elapsed.Minutes:D2}m\n\n" +
                    $"The session is still active. You can continue selling or close it from the Dashboard.",
                    "Session Recovered",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateBranding()
        {
            var settings = Program.DataService.Settings;
            if (lblLogoText != null)
                lblLogoText.Text = settings.StoreName;
        }

        // ────────────────────────────────────────────────────────────
        //  VIEW SWITCHING
        // ────────────────────────────────────────────────────────────
        public void ShowView(UserControl view)
        {
            pnlContent.SuspendLayout();
            foreach (Control c in pnlContent.Controls)
                c.Visible = false;

            if (!pnlContent.Controls.Contains(view))
            {
                view.Dock = DockStyle.Fill;
                pnlContent.Controls.Add(view);
            }
            view.Visible = true;
            view.BringToFront();
            pnlContent.ResumeLayout();
        }

        // ────────────────────────────────────────────────────────────
        //  NAV ACTIVE STATE
        // ────────────────────────────────────────────────────────────
        private void SetActiveNav(NavItem activeItem)
        {
            foreach (var item in _navItems)
                item.SetActive(item == activeItem);
        }

        // ────────────────────────────────────────────────────────────
        //  NAV CLICK HANDLERS
        // ────────────────────────────────────────────────────────────
        private void NavDashboard_Click(object? s, EventArgs e)
        {
            SetActiveNav(navDashboard);
            if (_dashboard == null)
            {
                _dashboard = new DashboardView();
                _dashboard.NewSaleClicked += (_, __) => NavQuickSale_Click(this, EventArgs.Empty);
                _dashboard.ViewReportsClicked += (_, __) => NavReports_Click(this, EventArgs.Empty);
                _dashboard.AddProductClicked += (_, __) => NavItems_Click(this, EventArgs.Empty);
                _dashboard.ViewOrderClicked += (_, orderId) => NavOrders_Click(this, EventArgs.Empty);
            }
            ShowView(_dashboard);
        }

        private void NavQuickSale_Click(object? s, EventArgs e)
        {
            SetActiveNav(navQuickSale);
            _quickSale ??= new QuickSaleView();
            ShowView(_quickSale);
            // Auto-focus search bar for cashier speed
            _quickSale.FocusSearch();
        }

        private void NavOrders_Click(object? s, EventArgs e)
        {
            SetActiveNav(navOrders);
            if (_orders == null)
            {
                _orders = new OrdersView();
                _orders.NavigateToCustomer += (_, customerId) =>
                {
                    NavCustomers_Click(this, EventArgs.Empty);
                    _customers?.SelectCustomerById(customerId);
                };
            }
            ShowView(_orders);
        }

        private void NavItems_Click(object? s, EventArgs e)
        {
            SetActiveNav(navItems);
            _items ??= new ItemsView();
            ShowView(_items);
        }

        private void NavCustomers_Click(object? s, EventArgs e)
        {
            SetActiveNav(navCustomers);
            _customers ??= new CustomersView();
            ShowView(_customers);
        }

        private void NavReports_Click(object? s, EventArgs e)
        {
            SetActiveNav(navReports);
            _reports ??= new ReportsView();
            ShowView(_reports);
        }

        private void NavSettings_Click(object? s, EventArgs e)
        {
            SetActiveNav(navSettings);
            _settings ??= new SettingsView();
            ShowView(_settings);
        }

        // ────────────────────────────────────────────────────────────
        //  SETUP WIZARD
        // ────────────────────────────────────────────────────────────
        public void ShowSetupWizard()
        {
            using var wizard = new SetupWizardForm();
            wizard.ShowDialog(this);
        }

        // ── Inactivity Timeout ───────────────────────────────────────
        private void SetupInactivityTimer()
        {
            _inactivityTimer = new System.Windows.Forms.Timer();
            _inactivityTimer.Tick += InactivityTimer_Tick;
            ResetInactivityTimer();
        }

        private void ResetInactivityTimer()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ResetInactivityTimer));
                return;
            }

            if (_inactivityTimer == null) return;

            // Throttle timer resets to at most once per second
            if ((DateTime.Now - _lastInteractionTime).TotalSeconds < 1) return;
            _lastInteractionTime = DateTime.Now;

            _inactivityTimer.Stop();

            var timeoutMin = Program.DataService.Settings.SessionTimeoutMinutes;
            if (timeoutMin > 0)
            {
                _inactivityTimer.Interval = timeoutMin * 60 * 1000;
                _inactivityTimer.Start();
            }
        }

        private void InactivityTimer_Tick(object? sender, EventArgs e)
        {
            _inactivityTimer?.Stop();
            Application.RemoveMessageFilter(this);
            MessageBox.Show("Terminal locked due to inactivity. Please log in again.", "POS Inactivity Timeout", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        public bool PreFilterMessage(ref Message m)
        {
            // Detect interaction events (Mouse Move/Click, Key Down)
            const int WM_MOUSEMOVE = 0x0200;
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_RBUTTONDOWN = 0x0204;
            const int WM_MBUTTONDOWN = 0x0207;
            const int WM_KEYDOWN = 0x0100;
            const int WM_SYSKEYDOWN = 0x0104;

            if (m.Msg == WM_MOUSEMOVE || m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || 
                m.Msg == WM_MBUTTONDOWN || m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
            {
                ResetInactivityTimer();
            }
            return false;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
            if (_inactivityTimer != null)
            {
                _inactivityTimer.Stop();
                _inactivityTimer.Dispose();
            }
            base.OnFormClosed(e);
        }
    }
}
