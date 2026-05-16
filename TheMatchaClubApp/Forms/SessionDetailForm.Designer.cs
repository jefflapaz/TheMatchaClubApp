using System.ComponentModel;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    partial class SessionDetailForm
    {
        private IContainer? components = null;

        // ── Header ──────────────────────────────────
        internal Panel pnlHeader;
        internal Label lblSessionTitle;
        internal Label lblSessionMeta;
        internal Label lblStatusBadge;
        internal Guna.UI2.WinForms.Guna2Button btnClose;

        // ── Tab Bar ─────────────────────────────────
        internal Panel pnlTabBar;
        internal Guna.UI2.WinForms.Guna2Button btnTabOverview;
        internal Guna.UI2.WinForms.Guna2Button btnTabTransactions;
        internal Guna.UI2.WinForms.Guna2Button btnTabProducts;

        // ── Content Panels ──────────────────────────
        internal Panel pnlContent;
        internal Panel pnlOverviewTab;
        internal Panel pnlTransactionsTab;
        internal Panel pnlProductsTab;

        // ── Overview Tab Controls ───────────────────
        internal FlowLayoutPanel flpKpiCards;
        internal Panel pnlCashRecon;
        internal Label lblCashTitle;
        internal Label lblStartingCashLabel; internal Label lblStartingCashValue;
        internal Label lblExpectedCashLabel; internal Label lblExpectedCashValue;
        internal Label lblActualCashLabel; internal Label lblActualCashValue;
        internal Label lblOverShortLabel; internal Label lblOverShortValue;
        internal Panel pnlInsightsRow;
        internal Panel pnlHourlyChart;

        // ── Transactions Tab Controls ───────────────
        internal DataGridView dgvTransactions;
        internal Label lblTxSummary;

        // ── Products Tab Controls ───────────────────
        internal DataGridView dgvProducts;
        internal Panel pnlCategoryBreakdown;
        internal Label lblProductsTitle;

        // ── Footer ──────────────────────────────────
        internal Panel pnlFooter;
        internal Guna.UI2.WinForms.Guna2Button btnExportPdf;
        internal Guna.UI2.WinForms.Guna2Button btnExportCsv;
        internal Guna.UI2.WinForms.Guna2Button btnPrint;
        internal Guna.UI2.WinForms.Guna2Button btnCloseBottom;

        private void InitializeComponent()
        {
            components = new Container();
            this.SuspendLayout();

            // ═══════════════════════════════════════════
            //  HEADER
            // ═══════════════════════════════════════════
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = System.Drawing.Color.White };

            lblSessionTitle = new Label
            {
                Text = "Session Report",
                Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827"),
                Location = new System.Drawing.Point(32, 16),
                AutoSize = true
            };

            lblSessionMeta = new Label
            {
                Text = "May 16, 2026 • 8:00 AM – 5:00 PM • Cashier: Admin",
                Font = new System.Drawing.Font("Segoe UI", 9.5F),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"),
                Location = new System.Drawing.Point(34, 50),
                AutoSize = true
            };

            lblStatusBadge = new Label
            {
                Text = "CLOSED",
                Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.ColorTranslator.FromHtml("#52B743"),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Size = new System.Drawing.Size(72, 24),
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
            };

            btnClose = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "✕",
                Size = new System.Drawing.Size(40, 40),
                BorderRadius = 20,
                FillColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F6"),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"),
                Font = new System.Drawing.Font("Segoe UI", 14F),
                BorderThickness = 0,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
            };

            pnlHeader.Controls.AddRange(new System.Windows.Forms.Control[] { lblSessionTitle, lblSessionMeta, lblStatusBadge, btnClose });

            // ═══════════════════════════════════════════
            //  TAB BAR
            // ═══════════════════════════════════════════
            pnlTabBar = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = System.Drawing.Color.White };

            btnTabOverview = new Guna.UI2.WinForms.Guna2Button { Text = "Overview", Size = new System.Drawing.Size(120, 34), Location = new System.Drawing.Point(32, 5), BorderRadius = 8, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };
            btnTabTransactions = new Guna.UI2.WinForms.Guna2Button { Text = "Transactions", Size = new System.Drawing.Size(130, 34), Location = new System.Drawing.Point(158, 5), BorderRadius = 8, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };
            btnTabProducts = new Guna.UI2.WinForms.Guna2Button { Text = "Products", Size = new System.Drawing.Size(110, 34), Location = new System.Drawing.Point(294, 5), BorderRadius = 8, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };

            pnlTabBar.Controls.AddRange(new System.Windows.Forms.Control[] { btnTabOverview, btnTabTransactions, btnTabProducts });

            // ═══════════════════════════════════════════
            //  CONTENT AREA
            // ═══════════════════════════════════════════
            pnlContent = new Panel { Dock = DockStyle.Fill, BackColor = System.Drawing.ColorTranslator.FromHtml("#F9FAFB"), Padding = new Padding(0) };

            // ── Overview Tab ─────────────────────────
            pnlOverviewTab = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = System.Drawing.Color.Transparent };

            flpKpiCards = new FlowLayoutPanel
            {
                Location = new System.Drawing.Point(32, 16),
                Size = new System.Drawing.Size(900, 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false,
                BackColor = System.Drawing.Color.Transparent
            };

            pnlCashRecon = new Panel
            {
                Location = new System.Drawing.Point(32, 124),
                Size = new System.Drawing.Size(400, 160),
                BackColor = System.Drawing.Color.White
            };

            lblCashTitle = new Label { Text = "Cash Reconciliation", Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(16, 12), AutoSize = true, ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827") };
            lblStartingCashLabel = new Label { Text = "Starting Cash", Font = new System.Drawing.Font("Segoe UI", 9F), Location = new System.Drawing.Point(16, 44), AutoSize = true, ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280") };
            lblStartingCashValue = new Label { Text = "₱0.00", Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(280, 44), AutoSize = true, ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827"), Anchor = AnchorStyles.Top | AnchorStyles.Right };
            lblExpectedCashLabel = new Label { Text = "Expected Cash", Font = new System.Drawing.Font("Segoe UI", 9F), Location = new System.Drawing.Point(16, 68), AutoSize = true, ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280") };
            lblExpectedCashValue = new Label { Text = "₱0.00", Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(280, 68), AutoSize = true, ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827"), Anchor = AnchorStyles.Top | AnchorStyles.Right };
            lblActualCashLabel = new Label { Text = "Actual Cash", Font = new System.Drawing.Font("Segoe UI", 9F), Location = new System.Drawing.Point(16, 92), AutoSize = true, ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280") };
            lblActualCashValue = new Label { Text = "₱0.00", Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(280, 92), AutoSize = true, ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827"), Anchor = AnchorStyles.Top | AnchorStyles.Right };
            lblOverShortLabel = new Label { Text = "Over / Short", Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(16, 122), AutoSize = true, ForeColor = System.Drawing.ColorTranslator.FromHtml("#374151") };
            lblOverShortValue = new Label { Text = "₱0.00", Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(280, 122), AutoSize = true, ForeColor = System.Drawing.ColorTranslator.FromHtml("#52B743"), Anchor = AnchorStyles.Top | AnchorStyles.Right };

            pnlCashRecon.Controls.AddRange(new System.Windows.Forms.Control[] { lblCashTitle, lblStartingCashLabel, lblStartingCashValue, lblExpectedCashLabel, lblExpectedCashValue, lblActualCashLabel, lblActualCashValue, lblOverShortLabel, lblOverShortValue });

            pnlInsightsRow = new Panel
            {
                Location = new System.Drawing.Point(448, 124),
                Size = new System.Drawing.Size(500, 160),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = System.Drawing.Color.White
            };

            pnlHourlyChart = new Panel
            {
                Location = new System.Drawing.Point(32, 300),
                Size = new System.Drawing.Size(916, 200),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = System.Drawing.Color.White
            };

            pnlOverviewTab.Controls.AddRange(new System.Windows.Forms.Control[] { flpKpiCards, pnlCashRecon, pnlInsightsRow, pnlHourlyChart });

            // ── Transactions Tab ─────────────────────
            pnlTransactionsTab = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = System.Drawing.Color.Transparent, Padding = new Padding(32, 16, 32, 16) };

            lblTxSummary = new Label { Text = "0 transactions", Dock = DockStyle.Top, Height = 30, Font = new System.Drawing.Font("Segoe UI", 9.5F), ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"), TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            dgvTransactions = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F6"),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 36 }
            };
            pnlTransactionsTab.Controls.Add(dgvTransactions);
            pnlTransactionsTab.Controls.Add(lblTxSummary);

            // ── Products Tab ─────────────────────────
            pnlProductsTab = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = System.Drawing.Color.Transparent, Padding = new Padding(32, 16, 32, 16) };

            lblProductsTitle = new Label { Text = "Product Performance", Dock = DockStyle.Top, Height = 30, Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827"), TextAlign = System.Drawing.ContentAlignment.MiddleLeft };

            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F6"),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 36 }
            };

            pnlCategoryBreakdown = new Panel { Dock = DockStyle.Bottom, Height = 300, BackColor = System.Drawing.Color.White };

            pnlProductsTab.Controls.Add(dgvProducts);
            pnlProductsTab.Controls.Add(lblProductsTitle);
            pnlProductsTab.Controls.Add(pnlCategoryBreakdown);

            pnlContent.Controls.AddRange(new System.Windows.Forms.Control[] { pnlOverviewTab, pnlTransactionsTab, pnlProductsTab });

            // ═══════════════════════════════════════════
            //  FOOTER
            // ═══════════════════════════════════════════
            pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 56, BackColor = System.Drawing.Color.White };

            btnExportPdf = new Guna.UI2.WinForms.Guna2Button { Text = "📄 Export PDF", Size = new System.Drawing.Size(130, 36), Location = new System.Drawing.Point(32, 10), BorderRadius = 8, FillColor = System.Drawing.ColorTranslator.FromHtml("#52B743"), ForeColor = System.Drawing.Color.White, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };
            btnExportCsv = new Guna.UI2.WinForms.Guna2Button { Text = "📊 Export CSV", Size = new System.Drawing.Size(130, 36), Location = new System.Drawing.Point(170, 10), BorderRadius = 8, FillColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F6"), ForeColor = System.Drawing.ColorTranslator.FromHtml("#374151"), Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };
            btnPrint = new Guna.UI2.WinForms.Guna2Button { Text = "🖨 Print", Size = new System.Drawing.Size(100, 36), Location = new System.Drawing.Point(308, 10), BorderRadius = 8, FillColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F6"), ForeColor = System.Drawing.ColorTranslator.FromHtml("#374151"), Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };
            btnCloseBottom = new Guna.UI2.WinForms.Guna2Button { Text = "Close", Size = new System.Drawing.Size(90, 36), BorderRadius = 8, FillColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F6"), ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"), Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0, Anchor = AnchorStyles.Top | AnchorStyles.Right };

            pnlFooter.Controls.AddRange(new System.Windows.Forms.Control[] { btnExportPdf, btnExportCsv, btnPrint, btnCloseBottom });

            // ═══════════════════════════════════════════
            //  FORM
            // ═══════════════════════════════════════════
            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlTabBar);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlFooter);

            this.Text = "Session Detail";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9FAFB");
            this.ShowInTaskbar = false;
            this.DoubleBuffered = true;

            this.ResumeLayout(false);
        }
    }
}
