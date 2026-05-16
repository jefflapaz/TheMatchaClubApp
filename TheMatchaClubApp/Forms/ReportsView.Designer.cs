namespace TheMatchaClubApp.Forms
{
    partial class ReportsView
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // Main containers
            pnlCloseoutSidebar = new Guna.UI2.WinForms.Guna2Panel();
            pnlLeftArea = new System.Windows.Forms.Panel();

            // Tab bar
            pnlTabBar = new System.Windows.Forms.FlowLayoutPanel();
            btnTabOverview = new Guna.UI2.WinForms.Guna2Button();
            btnTabSales = new Guna.UI2.WinForms.Guna2Button();
            btnTabHistory = new Guna.UI2.WinForms.Guna2Button();

            // Session History filters
            pnlSessionHistoryFilters = new System.Windows.Forms.Panel();
            cmbHistoryMonth = new Guna.UI2.WinForms.Guna2ComboBox();
            cmbHistoryYear = new Guna.UI2.WinForms.Guna2ComboBox();
            lblSessionCount = new System.Windows.Forms.Label();

            // Session selector
            pnlSessionHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            lblSelectedSession = new System.Windows.Forms.Label();
            btnSessionCalendar = new Guna.UI2.WinForms.Guna2Button();
            pnlExportButtons = new System.Windows.Forms.FlowLayoutPanel();
            btnExportCsv = new Guna.UI2.WinForms.Guna2Button();
            btnExportPdf = new Guna.UI2.WinForms.Guna2Button();

            // Page containers
            pnlPageOverview = new System.Windows.Forms.Panel();
            pnlPageSales = new System.Windows.Forms.Panel();
            pnlPageHistory = new System.Windows.Forms.Panel();

            // Overview page controls
            pnlKpiRow = new System.Windows.Forms.FlowLayoutPanel();
            pnlChartsRow = new System.Windows.Forms.Panel();
            pnlDoughnutChart = new Guna.UI2.WinForms.Guna2Panel();
            lblDoughnutTitle = new System.Windows.Forms.Label();
            pnlBarChart = new Guna.UI2.WinForms.Guna2Panel();
            lblBarChartTitle = new System.Windows.Forms.Label();
            pnlTableCard = new Guna.UI2.WinForms.Guna2Panel();
            lblTableHeader = new System.Windows.Forms.Label();
            btnPrintReport = new Guna.UI2.WinForms.Guna2Button();
            pnlRecentTx = new Guna.UI2.WinForms.Guna2Panel();
            lblRecentTxTitle = new System.Windows.Forms.Label();
            pnlInsightsRow = new System.Windows.Forms.FlowLayoutPanel();

            // Sales page controls
            pnlSalesHeader = new System.Windows.Forms.Panel();
            lblSalesTitle = new System.Windows.Forms.Label();
            txtSalesSearch = new Guna.UI2.WinForms.Guna2TextBox();
            dgvAllSales = new Guna.UI2.WinForms.Guna2DataGridView();

            // History page controls
            pnlHistoryCharts = new System.Windows.Forms.Panel();
            pnlRevenueChart = new Guna.UI2.WinForms.Guna2Panel();
            lblRevenueChartTitle = new System.Windows.Forms.Label();
            pnlTxChart = new Guna.UI2.WinForms.Guna2Panel();
            lblTxChartTitle = new System.Windows.Forms.Label();
            pnlHistoryTableCard = new Guna.UI2.WinForms.Guna2Panel();
            lblHistoryTableTitle = new System.Windows.Forms.Label();
            dgvSessionHistory = new Guna.UI2.WinForms.Guna2DataGridView();

            // Sidebar controls
            pnlCloseoutHeader = new System.Windows.Forms.Panel();
            lblCloseoutTitle = new System.Windows.Forms.Label();
            btnOpenStore = new Guna.UI2.WinForms.Guna2Button();
            lblSessionStatus = new System.Windows.Forms.Label();
            lblSessionTime = new System.Windows.Forms.Label();
            lblExpectedCash = new System.Windows.Forms.Label();
            lblExpectedCashValue = new System.Windows.Forms.Label();
            lblDrawerFund = new System.Windows.Forms.Label();
            lblDrawerFundValue = new System.Windows.Forms.Label();
            lblTxCountLabel = new System.Windows.Forms.Label();
            lblTxCountValue = new System.Windows.Forms.Label();
            lblBestSellerLabel = new System.Windows.Forms.Label();
            lblBestSellerValue = new System.Windows.Forms.Label();
            lblActualCashLabel = new System.Windows.Forms.Label();
            txtActualCash = new Guna.UI2.WinForms.Guna2TextBox();
            lblOverShortLabel = new System.Windows.Forms.Label();
            lblOverShortValue = new System.Windows.Forms.Label();
            pnlInfoBox = new Guna.UI2.WinForms.Guna2Panel();
            lblInfoText = new System.Windows.Forms.Label();
            btnCloseDay = new Guna.UI2.WinForms.Guna2Button();

            SuspendLayout();

            // ═══ SIDEBAR (right) ═══
            pnlCloseoutSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            pnlCloseoutSidebar.Width = 320;
            pnlCloseoutSidebar.AutoScroll = true;
            pnlCloseoutSidebar.Controls.Add(btnCloseDay);
            pnlCloseoutSidebar.Controls.Add(btnPrintReport);
            pnlCloseoutSidebar.Controls.Add(pnlInfoBox);
            pnlCloseoutSidebar.Controls.Add(lblOverShortValue);
            pnlCloseoutSidebar.Controls.Add(lblOverShortLabel);
            pnlCloseoutSidebar.Controls.Add(txtActualCash);
            pnlCloseoutSidebar.Controls.Add(lblActualCashLabel);
            pnlCloseoutSidebar.Controls.Add(lblBestSellerValue);
            pnlCloseoutSidebar.Controls.Add(lblBestSellerLabel);
            pnlCloseoutSidebar.Controls.Add(lblTxCountValue);
            pnlCloseoutSidebar.Controls.Add(lblTxCountLabel);
            pnlCloseoutSidebar.Controls.Add(lblDrawerFundValue);
            pnlCloseoutSidebar.Controls.Add(lblDrawerFund);
            pnlCloseoutSidebar.Controls.Add(lblExpectedCashValue);
            pnlCloseoutSidebar.Controls.Add(lblExpectedCash);
            pnlCloseoutSidebar.Controls.Add(lblSessionTime);
            pnlCloseoutSidebar.Controls.Add(lblSessionStatus);
            pnlCloseoutSidebar.Controls.Add(btnOpenStore);
            pnlCloseoutSidebar.Controls.Add(pnlCloseoutHeader);

            pnlCloseoutHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlCloseoutHeader.Height = 50;
            pnlCloseoutHeader.Controls.Add(lblCloseoutTitle);
            lblCloseoutTitle.Location = new System.Drawing.Point(28, 14);
            lblCloseoutTitle.Size = new System.Drawing.Size(230, 24);
            lblCloseoutTitle.Text = "Store Session";

            lblSessionStatus.Location = new System.Drawing.Point(16, 56);
            lblSessionStatus.Size = new System.Drawing.Size(248, 18);
            lblSessionTime.Location = new System.Drawing.Point(16, 74);
            lblSessionTime.Size = new System.Drawing.Size(248, 16);
            btnOpenStore.Location = new System.Drawing.Point(16, 96);
            btnOpenStore.Size = new System.Drawing.Size(248, 40);
            btnOpenStore.Text = "\u2615 Open Store Session";

            lblExpectedCash.Location = new System.Drawing.Point(20, 146);
            lblExpectedCash.Size = new System.Drawing.Size(140, 18);
            lblExpectedCash.Text = "Expected Cash:";
            lblExpectedCashValue.Location = new System.Drawing.Point(160, 146);
            lblExpectedCashValue.Size = new System.Drawing.Size(140, 18);
            lblExpectedCashValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblDrawerFund.Location = new System.Drawing.Point(20, 168);
            lblDrawerFund.Size = new System.Drawing.Size(140, 18);
            lblDrawerFund.Text = "Starting Fund:";
            lblDrawerFundValue.Location = new System.Drawing.Point(160, 168);
            lblDrawerFundValue.Size = new System.Drawing.Size(140, 18);
            lblDrawerFundValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblTxCountLabel.Location = new System.Drawing.Point(20, 194);
            lblTxCountLabel.Size = new System.Drawing.Size(140, 18);
            lblTxCountLabel.Text = "Transactions:";
            lblTxCountValue.Location = new System.Drawing.Point(160, 194);
            lblTxCountValue.Size = new System.Drawing.Size(140, 18);
            lblTxCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblBestSellerLabel.Location = new System.Drawing.Point(20, 216);
            lblBestSellerLabel.Size = new System.Drawing.Size(100, 18);
            lblBestSellerLabel.Text = "Best Seller:";
            lblBestSellerValue.Location = new System.Drawing.Point(120, 216);
            lblBestSellerValue.Size = new System.Drawing.Size(180, 18);
            lblBestSellerValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblActualCashLabel.Location = new System.Drawing.Point(20, 256);
            lblActualCashLabel.Size = new System.Drawing.Size(200, 18);
            lblActualCashLabel.Text = "ACTUAL CASH COUNTED";
            txtActualCash.Location = new System.Drawing.Point(20, 276);
            txtActualCash.Size = new System.Drawing.Size(280, 42);
            txtActualCash.PlaceholderText = "\u20B10.00";

            lblOverShortLabel.Location = new System.Drawing.Point(20, 330);
            lblOverShortLabel.Size = new System.Drawing.Size(120, 18);
            lblOverShortLabel.Text = "Over / Short:";
            lblOverShortValue.Location = new System.Drawing.Point(160, 330);
            lblOverShortValue.Size = new System.Drawing.Size(140, 18);
            lblOverShortValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            pnlInfoBox.Location = new System.Drawing.Point(20, 356);
            pnlInfoBox.Size = new System.Drawing.Size(280, 44);
            pnlInfoBox.Controls.Add(lblInfoText);
            lblInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            lblInfoText.Text = "\u2139 Count all cash including starting fund.";
            lblInfoText.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);

            btnCloseDay.Location = new System.Drawing.Point(20, 410);
            btnCloseDay.Size = new System.Drawing.Size(135, 42);
            btnCloseDay.Text = "\u2713 Close Session";
            
            btnPrintReport.Location = new System.Drawing.Point(165, 410);
            btnPrintReport.Size = new System.Drawing.Size(135, 42);
            btnPrintReport.Text = "\U0001F5A8 Print Report";

            // ═══ LEFT AREA ═══
            pnlLeftArea.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLeftArea.Controls.Add(pnlPageOverview);
            pnlLeftArea.Controls.Add(pnlPageSales);
            pnlLeftArea.Controls.Add(pnlPageHistory);
            pnlLeftArea.Controls.Add(pnlSessionHeader);
            pnlLeftArea.Controls.Add(pnlTabBar);

            // Tab bar
            pnlTabBar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTabBar.Height = 42;
            pnlTabBar.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlTabBar.WrapContents = false;
            pnlTabBar.Padding = new System.Windows.Forms.Padding(16, 6, 16, 0);
            pnlTabBar.Controls.Add(btnTabOverview);
            pnlTabBar.Controls.Add(btnTabSales);
            pnlTabBar.Controls.Add(btnTabHistory);
            btnTabOverview.Size = new System.Drawing.Size(160, 30);
            btnTabOverview.Text = "\u2726 Current Performance";
            btnTabOverview.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            btnTabSales.Size = new System.Drawing.Size(150, 30);
            btnTabSales.Text = "\u2398 Session History";
            btnTabSales.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            btnTabHistory.Size = new System.Drawing.Size(160, 30);
            btnTabHistory.Text = "\U0001F4CA Previous Reports";

            // Session header
            pnlSessionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSessionHeader.Height = 56;
            pnlSessionHeader.Padding = new System.Windows.Forms.Padding(20, 8, 20, 4);
            pnlSessionHeader.Controls.Add(pnlExportButtons);
            pnlSessionHeader.Controls.Add(lblSelectedSession);
            pnlSessionHeader.Controls.Add(lblTitle);

            lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            lblTitle.Width = 200;
            lblTitle.Text = "Current Performance";
            lblSelectedSession.Dock = System.Windows.Forms.DockStyle.Left;
            lblSelectedSession.Width = 260;
            lblSelectedSession.Text = "";
            btnSessionCalendar.Visible = false;

            pnlExportButtons.Dock = System.Windows.Forms.DockStyle.Right;
            pnlExportButtons.Width = 200;
            pnlExportButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            pnlExportButtons.WrapContents = false;
            pnlExportButtons.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            pnlExportButtons.Controls.Add(btnExportPdf);
            pnlExportButtons.Controls.Add(btnExportCsv);
            btnExportCsv.Size = new System.Drawing.Size(80, 26);
            btnExportCsv.Text = "\u2B07 CSV";
            btnExportCsv.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            btnExportPdf.Size = new System.Drawing.Size(80, 26);
            btnExportPdf.Text = "\u2B07 PDF";

            // ═══ PAGE: OVERVIEW ═══
            pnlPageOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlPageOverview.AutoScroll = true;
            pnlPageOverview.Controls.Add(pnlRecentTx);
            pnlPageOverview.Controls.Add(pnlTableCard);
            pnlPageOverview.Controls.Add(pnlInsightsRow);
            pnlPageOverview.Controls.Add(pnlChartsRow);
            pnlPageOverview.Controls.Add(pnlKpiRow);

            pnlKpiRow.Dock = System.Windows.Forms.DockStyle.Top;
            pnlKpiRow.MinimumSize = new System.Drawing.Size(0, 105);
            pnlKpiRow.AutoSize = true;
            pnlKpiRow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pnlKpiRow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlKpiRow.WrapContents = true;
            pnlKpiRow.Padding = new System.Windows.Forms.Padding(16, 12, 16, 4);

            pnlChartsRow.Dock = System.Windows.Forms.DockStyle.Top;
            pnlChartsRow.Height = 220;
            pnlChartsRow.Padding = new System.Windows.Forms.Padding(20, 8, 20, 8);
            pnlChartsRow.Controls.Add(pnlBarChart);
            pnlChartsRow.Controls.Add(pnlDoughnutChart);
            pnlDoughnutChart.Dock = System.Windows.Forms.DockStyle.Left;
            pnlDoughnutChart.Width = 320;
            pnlDoughnutChart.Padding = new System.Windows.Forms.Padding(12);
            pnlDoughnutChart.Controls.Add(lblDoughnutTitle);
            lblDoughnutTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblDoughnutTitle.Height = 24;
            lblDoughnutTitle.Text = "Sales by Category";
            pnlBarChart.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBarChart.Padding = new System.Windows.Forms.Padding(12);
            pnlBarChart.Controls.Add(lblBarChartTitle);
            lblBarChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblBarChartTitle.Height = 24;
            lblBarChartTitle.Text = "Hourly Sales Trend";

            pnlInsightsRow.Dock = System.Windows.Forms.DockStyle.Top;
            pnlInsightsRow.MinimumSize = new System.Drawing.Size(0, 85);
            pnlInsightsRow.AutoSize = true;
            pnlInsightsRow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            pnlInsightsRow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlInsightsRow.WrapContents = true;
            pnlInsightsRow.Padding = new System.Windows.Forms.Padding(16, 12, 16, 4);

            pnlTableCard.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTableCard.Height = 280;
            pnlTableCard.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);
            pnlTableCard.Controls.Add(lblTableHeader);
            lblTableHeader.Dock = System.Windows.Forms.DockStyle.Top;
            lblTableHeader.Height = 28;
            lblTableHeader.Text = "Top 5 Performing Items";
            lblTableHeader.Text = "Top 5 Performing Items";

            pnlRecentTx.Dock = System.Windows.Forms.DockStyle.Top;
            pnlRecentTx.Height = 400;
            pnlRecentTx.Padding = new System.Windows.Forms.Padding(20, 12, 20, 20);
            pnlRecentTx.Controls.Add(lblRecentTxTitle);
            lblRecentTxTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblRecentTxTitle.Height = 28;
            lblRecentTxTitle.Text = "Recent Transactions";
            lblRecentTxTitle.Text = "Recent Transactions";

            // ═══ PAGE: SESSION HISTORY ═══
            pnlPageSales.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlPageSales.Visible = false;
            pnlPageSales.Controls.Add(pnlHistoryTableCard);
            pnlPageSales.Controls.Add(pnlSessionHistoryFilters);

            pnlSessionHistoryFilters.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSessionHistoryFilters.Height = 48;
            pnlSessionHistoryFilters.Padding = new System.Windows.Forms.Padding(20, 10, 20, 4);
            pnlSessionHistoryFilters.Controls.Add(lblSessionCount);
            pnlSessionHistoryFilters.Controls.Add(cmbHistoryYear);
            pnlSessionHistoryFilters.Controls.Add(cmbHistoryMonth);

            cmbHistoryMonth.Dock = System.Windows.Forms.DockStyle.Left;
            cmbHistoryMonth.Width = 130;
            cmbHistoryMonth.Items.AddRange(new object[] { "All Months", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" });
            cmbHistoryMonth.SelectedIndex = 0;
            cmbHistoryMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbHistoryMonth.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            cmbHistoryYear.Dock = System.Windows.Forms.DockStyle.Left;
            cmbHistoryYear.Width = 100;
            cmbHistoryYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbHistoryYear.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            int currentYear = System.DateTime.Now.Year;
            for (int y = currentYear; y >= currentYear - 3; y--)
                cmbHistoryYear.Items.Add(y.ToString());
            cmbHistoryYear.SelectedIndex = 0;

            lblSessionCount.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSessionCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lblSessionCount.Text = "";

            pnlHistoryTableCard.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlHistoryTableCard.Padding = new System.Windows.Forms.Padding(20, 8, 20, 8);
            pnlHistoryTableCard.Controls.Add(dgvSessionHistory);
            pnlHistoryTableCard.Controls.Add(lblHistoryTableTitle);
            lblHistoryTableTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblHistoryTableTitle.Height = 28;
            lblHistoryTableTitle.Text = "Session Archive";
            dgvSessionHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvSessionHistory.AllowUserToAddRows = false;
            dgvSessionHistory.AllowUserToDeleteRows = false;
            dgvSessionHistory.ReadOnly = true;
            dgvSessionHistory.RowHeadersVisible = false;
            dgvSessionHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // ═══ PAGE: PREVIOUS REPORTS (charts only) ═══
            pnlPageHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlPageHistory.Visible = false;
            pnlPageHistory.AutoScroll = true;
            pnlPageHistory.Controls.Add(pnlHistoryCharts);
            pnlHistoryCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlHistoryCharts.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            pnlHistoryCharts.Controls.Add(pnlTxChart);
            pnlHistoryCharts.Controls.Add(pnlRevenueChart);
            pnlRevenueChart.Dock = System.Windows.Forms.DockStyle.Top;
            pnlRevenueChart.Height = 260;
            pnlRevenueChart.Padding = new System.Windows.Forms.Padding(16);
            pnlRevenueChart.Controls.Add(lblRevenueChartTitle);
            lblRevenueChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblRevenueChartTitle.Height = 24;
            lblRevenueChartTitle.Text = "Revenue by Session";
            pnlTxChart.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTxChart.Height = 260;
            pnlTxChart.Padding = new System.Windows.Forms.Padding(16);
            pnlTxChart.Controls.Add(lblTxChartTitle);
            lblTxChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblTxChartTitle.Height = 24;
            lblTxChartTitle.Text = "Transactions by Session";

            // ═══ MAIN ═══
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlLeftArea);
            Controls.Add(pnlCloseoutSidebar);
            Name = "ReportsView";
            Size = new System.Drawing.Size(1004, 600);
            ResumeLayout(false);
        }

        // ═══ FIELD DECLARATIONS ═══
        private Guna.UI2.WinForms.Guna2Panel pnlCloseoutSidebar;
        private System.Windows.Forms.Panel pnlLeftArea;
        private System.Windows.Forms.FlowLayoutPanel pnlTabBar;
        private Guna.UI2.WinForms.Guna2Button btnCloseDay;
        private Guna.UI2.WinForms.Guna2Button btnPrintReport;
        private Guna.UI2.WinForms.Guna2Panel pnlInfoBox;
        private Guna.UI2.WinForms.Guna2Button btnTabOverview;
        private Guna.UI2.WinForms.Guna2Button btnTabSales;
        private Guna.UI2.WinForms.Guna2Button btnTabHistory;
        private System.Windows.Forms.Panel pnlSessionHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSelectedSession;
        private Guna.UI2.WinForms.Guna2Button btnSessionCalendar;
        private System.Windows.Forms.FlowLayoutPanel pnlExportButtons;
        private Guna.UI2.WinForms.Guna2Button btnExportCsv;
        private Guna.UI2.WinForms.Guna2Button btnExportPdf;
        private System.Windows.Forms.Panel pnlPageOverview;
        private System.Windows.Forms.Panel pnlPageSales;
        private System.Windows.Forms.Panel pnlPageHistory;
        private System.Windows.Forms.FlowLayoutPanel pnlKpiRow;
        private System.Windows.Forms.Panel pnlChartsRow;
        private Guna.UI2.WinForms.Guna2Panel pnlDoughnutChart;
        private System.Windows.Forms.Label lblDoughnutTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlBarChart;
        private System.Windows.Forms.Label lblBarChartTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlTableCard;
        private System.Windows.Forms.Label lblTableHeader;
        private Guna.UI2.WinForms.Guna2Panel pnlRecentTx;
        private System.Windows.Forms.Label lblRecentTxTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlInsightsRow;
        private System.Windows.Forms.Panel pnlSalesHeader;
        private System.Windows.Forms.Label lblSalesTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtSalesSearch;
        private Guna.UI2.WinForms.Guna2DataGridView dgvAllSales;
        private System.Windows.Forms.Panel pnlSessionHistoryFilters;
        private Guna.UI2.WinForms.Guna2ComboBox cmbHistoryMonth;
        private Guna.UI2.WinForms.Guna2ComboBox cmbHistoryYear;
        private System.Windows.Forms.Label lblSessionCount;
        private System.Windows.Forms.Panel pnlHistoryCharts;
        private Guna.UI2.WinForms.Guna2Panel pnlRevenueChart;
        private System.Windows.Forms.Label lblRevenueChartTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlTxChart;
        private System.Windows.Forms.Label lblTxChartTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlHistoryTableCard;
        private System.Windows.Forms.Label lblHistoryTableTitle;
        private Guna.UI2.WinForms.Guna2DataGridView dgvSessionHistory;
        private System.Windows.Forms.Panel pnlCloseoutHeader;
        private System.Windows.Forms.Label lblCloseoutTitle;
        private Guna.UI2.WinForms.Guna2Button btnOpenStore;
        private System.Windows.Forms.Label lblSessionStatus;
        private System.Windows.Forms.Label lblSessionTime;
        private System.Windows.Forms.Label lblExpectedCash;
        private System.Windows.Forms.Label lblExpectedCashValue;
        private System.Windows.Forms.Label lblDrawerFund;
        private System.Windows.Forms.Label lblDrawerFundValue;
        private System.Windows.Forms.Label lblTxCountLabel;
        private System.Windows.Forms.Label lblTxCountValue;
        private System.Windows.Forms.Label lblBestSellerLabel;
        private System.Windows.Forms.Label lblBestSellerValue;
        private System.Windows.Forms.Label lblActualCashLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtActualCash;
        private System.Windows.Forms.Label lblOverShortLabel;
        private System.Windows.Forms.Label lblOverShortValue;
        private System.Windows.Forms.Label lblInfoText;
    }
}
