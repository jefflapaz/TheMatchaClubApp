using System.ComponentModel;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    partial class OrderDetailForm
    {
        private IContainer? components = null;

        // ── Header ──────────────────────────────────
        internal Panel pnlHeader;
        internal Label lblOrderTitle;
        internal Label lblOrderMeta;
        internal Label lblStatusBadge;
        internal Guna.UI2.WinForms.Guna2Button btnClose;

        // ── Tab Bar ─────────────────────────────────
        internal Panel pnlTabBar;
        internal Guna.UI2.WinForms.Guna2Button btnTabOverview;
        internal Guna.UI2.WinForms.Guna2Button btnTabReceipt;
        internal Guna.UI2.WinForms.Guna2Button btnTabTimeline;

        // ── Content ─────────────────────────────────
        internal Panel pnlContent;
        internal Panel pnlOverviewTab;
        internal Panel pnlReceiptTab;
        internal Panel pnlTimelineTab;

        // ── Overview Controls ───────────────────────
        internal FlowLayoutPanel flpKpiCards;
        internal Panel pnlCustomerCard;
        internal Label lblCustomerTitle;
        internal Label lblCustomerName;
        internal Label lblCustomerEmail;
        internal Panel pnlPaymentCard;
        internal Label lblPaymentTitle;
        internal Label lblPaymentMethod;
        internal Label lblCashTendered;
        internal Label lblChangeGiven;
        internal Panel pnlOrderInfoCard;
        internal Label lblOrderInfoTitle;
        internal Label lblOrderType;
        internal Label lblCashier;
        internal Label lblTimestamp;
        internal Panel pnlItemsCard;
        internal DataGridView dgvItems;

        // ── Receipt Tab ─────────────────────────────
        internal Panel pnlReceiptPreview;

        // ── Timeline Tab ────────────────────────────
        internal Label lblTimelinePlaceholder;

        // ── Footer ──────────────────────────────────
        internal Panel pnlFooter;
        internal Guna.UI2.WinForms.Guna2Button btnPrint;
        internal Guna.UI2.WinForms.Guna2Button btnExportPdf;
        internal Guna.UI2.WinForms.Guna2Button btnEmail;
        internal Guna.UI2.WinForms.Guna2Button btnCloseBottom;

        private void InitializeComponent()
        {
            components = new Container();
            this.SuspendLayout();

            // ═══════════════════════════════════════════
            //  HEADER
            // ═══════════════════════════════════════════
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = System.Drawing.Color.White };

            lblOrderTitle = new Label
            {
                Text = "Order Detail",
                Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827"),
                Location = new System.Drawing.Point(32, 16),
                AutoSize = true
            };

            lblOrderMeta = new Label
            {
                Text = "May 16, 2026 • 2:30 PM • Customer: Walk-In",
                Font = new System.Drawing.Font("Segoe UI", 9.5F),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"),
                Location = new System.Drawing.Point(34, 50),
                AutoSize = true
            };

            lblStatusBadge = new Label
            {
                Text = "COMPLETED",
                Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.ColorTranslator.FromHtml("#52B743"),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Size = new System.Drawing.Size(90, 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
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
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            pnlHeader.Controls.AddRange(new Control[] { lblOrderTitle, lblOrderMeta, lblStatusBadge, btnClose });

            // ═══════════════════════════════════════════
            //  TAB BAR
            // ═══════════════════════════════════════════
            pnlTabBar = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = System.Drawing.Color.White };

            btnTabOverview = new Guna.UI2.WinForms.Guna2Button { Text = "Overview", Size = new System.Drawing.Size(120, 34), Location = new System.Drawing.Point(32, 5), BorderRadius = 8, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };
            btnTabReceipt = new Guna.UI2.WinForms.Guna2Button { Text = "Receipt", Size = new System.Drawing.Size(110, 34), Location = new System.Drawing.Point(158, 5), BorderRadius = 8, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };
            btnTabTimeline = new Guna.UI2.WinForms.Guna2Button { Text = "Timeline", Size = new System.Drawing.Size(110, 34), Location = new System.Drawing.Point(274, 5), BorderRadius = 8, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };

            pnlTabBar.Controls.AddRange(new Control[] { btnTabOverview, btnTabReceipt, btnTabTimeline });

            // ═══════════════════════════════════════════
            //  CONTENT AREA
            // ═══════════════════════════════════════════
            pnlContent = new Panel { Dock = DockStyle.Fill, BackColor = System.Drawing.ColorTranslator.FromHtml("#F9FAFB"), Padding = new Padding(0) };

            // ── Overview Tab ────────────────────────
            pnlOverviewTab = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = System.Drawing.Color.Transparent };

            flpKpiCards = new FlowLayoutPanel
            {
                Location = new System.Drawing.Point(32, 16),
                Size = new System.Drawing.Size(920, 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = false,
                BackColor = System.Drawing.Color.Transparent
            };

            // Customer Card
            pnlCustomerCard = new Panel { Location = new System.Drawing.Point(32, 130), Size = new System.Drawing.Size(280, 130), BackColor = System.Drawing.Color.White };
            lblCustomerTitle = new Label { Text = "CUSTOMER", Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.ColorTranslator.FromHtml("#9CA3AF"), Location = new System.Drawing.Point(16, 14), AutoSize = true };
            lblCustomerName = new Label { Text = "Walk-In", Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827"), Location = new System.Drawing.Point(16, 38), AutoSize = true };
            lblCustomerEmail = new Label { Text = "", Font = new System.Drawing.Font("Segoe UI", 9F), ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"), Location = new System.Drawing.Point(16, 64), AutoSize = true };
            pnlCustomerCard.Controls.AddRange(new Control[] { lblCustomerTitle, lblCustomerName, lblCustomerEmail });

            // Payment Card
            pnlPaymentCard = new Panel { Location = new System.Drawing.Point(324, 130), Size = new System.Drawing.Size(280, 130), BackColor = System.Drawing.Color.White };
            lblPaymentTitle = new Label { Text = "PAYMENT", Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.ColorTranslator.FromHtml("#9CA3AF"), Location = new System.Drawing.Point(16, 14), AutoSize = true };
            lblPaymentMethod = new Label { Text = "Cash", Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827"), Location = new System.Drawing.Point(16, 38), AutoSize = true };
            lblCashTendered = new Label { Text = "Tendered: ₱0.00", Font = new System.Drawing.Font("Segoe UI", 9F), ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"), Location = new System.Drawing.Point(16, 64), AutoSize = true };
            lblChangeGiven = new Label { Text = "Change: ₱0.00", Font = new System.Drawing.Font("Segoe UI", 9F), ForeColor = System.Drawing.ColorTranslator.FromHtml("#52B743"), Location = new System.Drawing.Point(16, 84), AutoSize = true };
            pnlPaymentCard.Controls.AddRange(new Control[] { lblPaymentTitle, lblPaymentMethod, lblCashTendered, lblChangeGiven });

            // Order Info Card
            pnlOrderInfoCard = new Panel { Location = new System.Drawing.Point(616, 130), Size = new System.Drawing.Size(280, 130), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = System.Drawing.Color.White };
            lblOrderInfoTitle = new Label { Text = "ORDER INFO", Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.ColorTranslator.FromHtml("#9CA3AF"), Location = new System.Drawing.Point(16, 14), AutoSize = true };
            lblOrderType = new Label { Text = "Dine-In", Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.ColorTranslator.FromHtml("#111827"), Location = new System.Drawing.Point(16, 38), AutoSize = true };
            lblCashier = new Label { Text = "Cashier: Admin", Font = new System.Drawing.Font("Segoe UI", 9F), ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"), Location = new System.Drawing.Point(16, 64), AutoSize = true };
            lblTimestamp = new Label { Text = "2026-05-16 14:30:00", Font = new System.Drawing.Font("Segoe UI", 9F), ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"), Location = new System.Drawing.Point(16, 84), AutoSize = true };
            pnlOrderInfoCard.Controls.AddRange(new Control[] { lblOrderInfoTitle, lblOrderType, lblCashier, lblTimestamp });

            // Items Grid Card
            pnlItemsCard = new Panel { Location = new System.Drawing.Point(32, 276), Size = new System.Drawing.Size(864, 300), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = System.Drawing.Color.White };
            var lblItemsTitle = new Label { Text = "ORDER ITEMS", Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.ColorTranslator.FromHtml("#9CA3AF"), Location = new System.Drawing.Point(16, 14), AutoSize = true };

            dgvItems = new DataGridView
            {
                Location = new System.Drawing.Point(16, 40),
                Size = new System.Drawing.Size(832, 244),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F6"),
                ColumnHeadersHeight = 36,
                RowTemplate = { Height = 36 }
            };
            pnlItemsCard.Controls.Add(lblItemsTitle);
            pnlItemsCard.Controls.Add(dgvItems);

            pnlOverviewTab.Controls.AddRange(new Control[] { flpKpiCards, pnlCustomerCard, pnlPaymentCard, pnlOrderInfoCard, pnlItemsCard });

            // ── Receipt Tab ─────────────────────────
            pnlReceiptTab = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = System.Drawing.Color.Transparent, AutoScroll = true };
            pnlReceiptPreview = new Panel { Location = new System.Drawing.Point(0, 0), Size = new System.Drawing.Size(360, 800), BackColor = System.Drawing.Color.White };
            pnlReceiptTab.Controls.Add(pnlReceiptPreview);

            // ── Timeline Tab ────────────────────────
            pnlTimelineTab = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = System.Drawing.Color.Transparent };
            lblTimelinePlaceholder = new Label
            {
                Text = "🕐  Timeline\n\nOrder timeline and audit trail coming soon.\nThis feature will track status changes,\nreprints, and modifications.",
                Font = new System.Drawing.Font("Segoe UI", 10F),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#9CA3AF"),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White
            };
            pnlTimelineTab.Controls.Add(lblTimelinePlaceholder);

            pnlContent.Controls.AddRange(new Control[] { pnlOverviewTab, pnlReceiptTab, pnlTimelineTab });

            // ═══════════════════════════════════════════
            //  FOOTER
            // ═══════════════════════════════════════════
            pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 56, BackColor = System.Drawing.Color.White };

            btnPrint = new Guna.UI2.WinForms.Guna2Button { Text = "🖨 Print", Size = new System.Drawing.Size(100, 36), Location = new System.Drawing.Point(32, 10), BorderRadius = 8, FillColor = System.Drawing.Color.White, ForeColor = System.Drawing.ColorTranslator.FromHtml("#52B743"), BorderColor = System.Drawing.ColorTranslator.FromHtml("#52B743"), BorderThickness = 1, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold) };
            btnExportPdf = new Guna.UI2.WinForms.Guna2Button { Text = "📄 PDF", Size = new System.Drawing.Size(100, 36), Location = new System.Drawing.Point(140, 10), BorderRadius = 8, FillColor = System.Drawing.ColorTranslator.FromHtml("#52B743"), ForeColor = System.Drawing.Color.White, Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };
            btnEmail = new Guna.UI2.WinForms.Guna2Button { Text = "✉ Email", Size = new System.Drawing.Size(100, 36), Location = new System.Drawing.Point(248, 10), BorderRadius = 8, FillColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F6"), ForeColor = System.Drawing.ColorTranslator.FromHtml("#374151"), Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0 };
            btnCloseBottom = new Guna.UI2.WinForms.Guna2Button { Text = "Close", Size = new System.Drawing.Size(90, 36), BorderRadius = 8, FillColor = System.Drawing.ColorTranslator.FromHtml("#F3F4F6"), ForeColor = System.Drawing.ColorTranslator.FromHtml("#6B7280"), Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), BorderThickness = 0, Anchor = AnchorStyles.Top | AnchorStyles.Right };

            pnlFooter.Controls.AddRange(new Control[] { btnPrint, btnExportPdf, btnEmail, btnCloseBottom });

            // ═══════════════════════════════════════════
            //  FORM
            // ═══════════════════════════════════════════
            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlTabBar);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlFooter);

            this.Text = "Order Detail";
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
