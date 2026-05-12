namespace TheMatchaClubApp.Forms
{
    partial class ReportsView
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlCloseoutSidebar = new Guna.UI2.WinForms.Guna2Panel();
            pnlLeftArea = new System.Windows.Forms.Panel();

            // Left area controls
            pnlTopSection = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            lblSubTitle = new System.Windows.Forms.Label();
            pnlFilterTabs = new System.Windows.Forms.FlowLayoutPanel();
            btnToday = new Guna.UI2.WinForms.Guna2Button();
            btnYesterday = new Guna.UI2.WinForms.Guna2Button();
            btnThisWeek = new Guna.UI2.WinForms.Guna2Button();
            btnCustomDate = new Guna.UI2.WinForms.Guna2Button();
            btnExportCsv = new Guna.UI2.WinForms.Guna2Button();

            pnlKpiRow = new System.Windows.Forms.FlowLayoutPanel();
            pnlTableCard = new Guna.UI2.WinForms.Guna2Panel();
            pnlTableInner = new System.Windows.Forms.Panel();
            lblTableHeader = new System.Windows.Forms.Label();
            lblViewAll = new System.Windows.Forms.Label();

            // Closeout sidebar controls
            pnlCloseoutHeader = new System.Windows.Forms.Panel();
            lblCloseoutTitle = new System.Windows.Forms.Label();
            lblExpectedCash = new System.Windows.Forms.Label();
            lblExpectedCashValue = new System.Windows.Forms.Label();
            lblDrawerFund = new System.Windows.Forms.Label();
            lblDrawerFundValue = new System.Windows.Forms.Label();
            lblActualCashLabel = new System.Windows.Forms.Label();
            txtActualCash = new Guna.UI2.WinForms.Guna2TextBox();
            pnlInfoBox = new Guna.UI2.WinForms.Guna2Panel();
            lblInfoText = new System.Windows.Forms.Label();
            btnCloseDay = new Guna.UI2.WinForms.Guna2Button();
            pnlNavTaxes = new Guna.UI2.WinForms.Guna2Panel();
            lblNavTaxes = new System.Windows.Forms.Label();
            pnlNavPrevReports = new Guna.UI2.WinForms.Guna2Panel();
            lblNavPrevReports = new System.Windows.Forms.Label();

            SuspendLayout();

            // Closeout sidebar
            pnlCloseoutSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            pnlCloseoutSidebar.Size = new System.Drawing.Size(320, 600);
            pnlCloseoutSidebar.Controls.Add(pnlNavPrevReports);
            pnlCloseoutSidebar.Controls.Add(pnlNavTaxes);
            pnlCloseoutSidebar.Controls.Add(btnCloseDay);
            pnlCloseoutSidebar.Controls.Add(pnlInfoBox);
            pnlCloseoutSidebar.Controls.Add(txtActualCash);
            pnlCloseoutSidebar.Controls.Add(lblActualCashLabel);
            pnlCloseoutSidebar.Controls.Add(lblDrawerFundValue);
            pnlCloseoutSidebar.Controls.Add(lblDrawerFund);
            pnlCloseoutSidebar.Controls.Add(lblExpectedCashValue);
            pnlCloseoutSidebar.Controls.Add(lblExpectedCash);
            pnlCloseoutSidebar.Controls.Add(pnlCloseoutHeader);

            pnlCloseoutHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlCloseoutHeader.Size = new System.Drawing.Size(320, 60);
            pnlCloseoutHeader.Controls.Add(lblCloseoutTitle);
            lblCloseoutTitle.Location = new System.Drawing.Point(28, 16);
            lblCloseoutTitle.Size = new System.Drawing.Size(260, 24);
            lblCloseoutTitle.Text = "End-of-Day Closeout";

            lblExpectedCash.Location = new System.Drawing.Point(16, 70);
            lblExpectedCash.Size = new System.Drawing.Size(160, 18);
            lblExpectedCash.Text = "Expected Cash Total:";
            lblExpectedCashValue.Location = new System.Drawing.Point(200, 70);
            lblExpectedCashValue.Size = new System.Drawing.Size(100, 18);
            lblExpectedCashValue.Text = "$3,629.50";
            lblExpectedCashValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblDrawerFund.Location = new System.Drawing.Point(16, 94);
            lblDrawerFund.Size = new System.Drawing.Size(160, 18);
            lblDrawerFund.Text = "Drawer Starting Fund:";
            lblDrawerFundValue.Location = new System.Drawing.Point(200, 94);
            lblDrawerFundValue.Size = new System.Drawing.Size(100, 18);
            lblDrawerFundValue.Text = "$200.00";
            lblDrawerFundValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblActualCashLabel.Location = new System.Drawing.Point(16, 126);
            lblActualCashLabel.Size = new System.Drawing.Size(200, 18);
            lblActualCashLabel.Text = "ACTUAL CASH COUNTED";

            txtActualCash.Location = new System.Drawing.Point(16, 148);
            txtActualCash.Size = new System.Drawing.Size(288, 42);
            txtActualCash.PlaceholderText = "$0.00";

            pnlInfoBox.Location = new System.Drawing.Point(16, 200);
            pnlInfoBox.Size = new System.Drawing.Size(288, 64);
            pnlInfoBox.Controls.Add(lblInfoText);
            lblInfoText.Dock = System.Windows.Forms.DockStyle.Fill;
            lblInfoText.Text = "\u2139 Count all cash in the register drawer including the starting fund.";
            lblInfoText.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);

            btnCloseDay.Location = new System.Drawing.Point(16, 276);
            btnCloseDay.Size = new System.Drawing.Size(288, 48);
            btnCloseDay.Text = "\u2713 Close Day & Print Z-Report";

            pnlNavTaxes.Location = new System.Drawing.Point(16, 340);
            pnlNavTaxes.Size = new System.Drawing.Size(288, 56);
            pnlNavTaxes.Controls.Add(lblNavTaxes);
            lblNavTaxes.Dock = System.Windows.Forms.DockStyle.Fill;
            lblNavTaxes.Text = "   Taxes & Fees                                    \u203A";
            lblNavTaxes.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);

            pnlNavPrevReports.Location = new System.Drawing.Point(16, 404);
            pnlNavPrevReports.Size = new System.Drawing.Size(288, 56);
            pnlNavPrevReports.Controls.Add(lblNavPrevReports);
            lblNavPrevReports.Dock = System.Windows.Forms.DockStyle.Fill;
            lblNavPrevReports.Text = "   Previous Reports                               \u203A";
            lblNavPrevReports.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);

            // Left area
            pnlLeftArea.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLeftArea.Controls.Add(pnlTableCard);
            pnlLeftArea.Controls.Add(pnlKpiRow);
            pnlLeftArea.Controls.Add(pnlTopSection);

            // Top section
            pnlTopSection.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopSection.Size = new System.Drawing.Size(684, 100);
            pnlTopSection.Controls.Add(lblTitle);
            pnlTopSection.Controls.Add(lblSubTitle);
            pnlTopSection.Controls.Add(pnlFilterTabs);
            pnlTopSection.Controls.Add(btnExportCsv);

            lblTitle.Location = new System.Drawing.Point(24, 16);
            lblTitle.Size = new System.Drawing.Size(250, 28);
            lblTitle.Text = "Performance Overview";
            lblSubTitle.Location = new System.Drawing.Point(24, 44);
            lblSubTitle.Size = new System.Drawing.Size(300, 18);
            lblSubTitle.Text = "Track your store's performance metrics";

            pnlFilterTabs.Location = new System.Drawing.Point(24, 66);
            pnlFilterTabs.Size = new System.Drawing.Size(400, 32);
            pnlFilterTabs.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlFilterTabs.WrapContents = false;
            pnlFilterTabs.Controls.Add(btnToday);
            pnlFilterTabs.Controls.Add(btnYesterday);
            pnlFilterTabs.Controls.Add(btnThisWeek);
            pnlFilterTabs.Controls.Add(btnCustomDate);

            btnToday.Size = new System.Drawing.Size(64, 28);
            btnToday.Text = "Today";
            btnToday.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnYesterday.Size = new System.Drawing.Size(80, 28);
            btnYesterday.Text = "Yesterday";
            btnYesterday.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnThisWeek.Size = new System.Drawing.Size(80, 28);
            btnThisWeek.Text = "This Week";
            btnThisWeek.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            btnCustomDate.Size = new System.Drawing.Size(90, 28);
            btnCustomDate.Text = "\U0001F4C5 Custom";

            btnExportCsv.Location = new System.Drawing.Point(560, 66);
            btnExportCsv.Size = new System.Drawing.Size(100, 28);
            btnExportCsv.Text = "\u2B07 Export CSV";
            btnExportCsv.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // KPI row
            pnlKpiRow.Dock = System.Windows.Forms.DockStyle.Top;
            pnlKpiRow.Size = new System.Drawing.Size(684, 100);
            pnlKpiRow.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlKpiRow.WrapContents = false;
            pnlKpiRow.Padding = new System.Windows.Forms.Padding(20, 8, 20, 8);

            // Table card
            pnlTableCard.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlTableCard.Controls.Add(pnlTableInner);
            pnlTableCard.Controls.Add(lblTableHeader);
            pnlTableCard.Controls.Add(lblViewAll);

            lblTableHeader.Location = new System.Drawing.Point(24, 16);
            lblTableHeader.Size = new System.Drawing.Size(200, 24);
            lblTableHeader.Text = "Top Performing Items";
            lblViewAll.Location = new System.Drawing.Point(460, 18);
            lblViewAll.Size = new System.Drawing.Size(160, 20);
            lblViewAll.Text = "View All Products \u2192";
            lblViewAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lblViewAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            pnlTableInner.Location = new System.Drawing.Point(0, 48);
            pnlTableInner.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlTableInner.Padding = new System.Windows.Forms.Padding(0, 48, 0, 0);

            // ReportsView
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlLeftArea);
            Controls.Add(pnlCloseoutSidebar);
            Name = "ReportsView";
            Size = new System.Drawing.Size(1004, 600);
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlCloseoutSidebar;
        private System.Windows.Forms.Panel pnlLeftArea;
        private System.Windows.Forms.Panel pnlTopSection;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlFilterTabs;
        private Guna.UI2.WinForms.Guna2Button btnToday;
        private Guna.UI2.WinForms.Guna2Button btnYesterday;
        private Guna.UI2.WinForms.Guna2Button btnThisWeek;
        private Guna.UI2.WinForms.Guna2Button btnCustomDate;
        private Guna.UI2.WinForms.Guna2Button btnExportCsv;
        private System.Windows.Forms.FlowLayoutPanel pnlKpiRow;
        private Guna.UI2.WinForms.Guna2Panel pnlTableCard;
        private System.Windows.Forms.Panel pnlTableInner;
        private System.Windows.Forms.Label lblTableHeader;
        private System.Windows.Forms.Label lblViewAll;
        private System.Windows.Forms.Panel pnlCloseoutHeader;
        private System.Windows.Forms.Label lblCloseoutTitle;
        private System.Windows.Forms.Label lblExpectedCash;
        private System.Windows.Forms.Label lblExpectedCashValue;
        private System.Windows.Forms.Label lblDrawerFund;
        private System.Windows.Forms.Label lblDrawerFundValue;
        private System.Windows.Forms.Label lblActualCashLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtActualCash;
        private Guna.UI2.WinForms.Guna2Panel pnlInfoBox;
        private System.Windows.Forms.Label lblInfoText;
        private Guna.UI2.WinForms.Guna2Button btnCloseDay;
        private Guna.UI2.WinForms.Guna2Panel pnlNavTaxes;
        private System.Windows.Forms.Label lblNavTaxes;
        private Guna.UI2.WinForms.Guna2Panel pnlNavPrevReports;
        private System.Windows.Forms.Label lblNavPrevReports;
    }
}
