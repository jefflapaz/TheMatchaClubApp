namespace TheMatchaClubApp.Forms
{
    partial class OrdersView
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlReceiptSidebar = new Guna.UI2.WinForms.Guna2Panel();
            pnlLeftArea = new System.Windows.Forms.Panel();
            pnlTopHeader = new System.Windows.Forms.Panel();
            lblChevron = new System.Windows.Forms.Label();
            lblViewName = new System.Windows.Forms.Label();
            btnNewOrder = new Guna.UI2.WinForms.Guna2Button();
            pnlFilterBar = new System.Windows.Forms.Panel();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            btnFilterAll = new Guna.UI2.WinForms.Guna2Button();
            btnFilterDineIn = new Guna.UI2.WinForms.Guna2Button();
            btnFilterTakeaway = new Guna.UI2.WinForms.Guna2Button();
            lblDateDisplay = new System.Windows.Forms.Label();
            btnExport = new Guna.UI2.WinForms.Guna2Button();
            dgvOrders = new Guna.UI2.WinForms.Guna2DataGridView();
            pnlPagination = new System.Windows.Forms.Panel();
            lblPaginationInfo = new System.Windows.Forms.Label();

            // Receipt panel controls
            pnlReceiptHeader = new System.Windows.Forms.Panel();
            lblReceiptTitle = new System.Windows.Forms.Label();
            btnCloseReceipt = new Guna.UI2.WinForms.Guna2Button();
            pnlReceiptBody = new System.Windows.Forms.Panel();
            lblStoreName = new System.Windows.Forms.Label();
            lblStoreAddress = new System.Windows.Forms.Label();
            lblReceiptOrderIdLabel = new System.Windows.Forms.Label();
            lblReceiptOrderId = new System.Windows.Forms.Label();
            lblReceiptDateLabel = new System.Windows.Forms.Label();
            lblReceiptDate = new System.Windows.Forms.Label();
            lblReceiptCustomerLabel = new System.Windows.Forms.Label();
            lblReceiptCustomer = new System.Windows.Forms.Label();
            lblReceiptItems = new System.Windows.Forms.Label();
            lblReceiptSubtotalLabel = new System.Windows.Forms.Label();
            lblReceiptSubtotal = new System.Windows.Forms.Label();
            lblReceiptTaxLabel = new System.Windows.Forms.Label();
            lblReceiptTax = new System.Windows.Forms.Label();
            lblReceiptTotalLabel = new System.Windows.Forms.Label();
            lblReceiptTotal = new System.Windows.Forms.Label();
            lblPaidVia = new System.Windows.Forms.Label();
            lblThankYou = new System.Windows.Forms.Label();
            btnReprint = new Guna.UI2.WinForms.Guna2Button();
            btnEmailReceipt = new Guna.UI2.WinForms.Guna2Button();

            SuspendLayout();

            // pnlReceiptSidebar
            pnlReceiptSidebar.Controls.Add(pnlReceiptBody);
            pnlReceiptSidebar.Controls.Add(pnlReceiptHeader);
            pnlReceiptSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            pnlReceiptSidebar.Size = new System.Drawing.Size(320, 600);

            // pnlLeftArea
            pnlLeftArea.Controls.Add(dgvOrders);
            pnlLeftArea.Controls.Add(pnlPagination);
            pnlLeftArea.Controls.Add(pnlFilterBar);
            pnlLeftArea.Controls.Add(pnlTopHeader);
            pnlLeftArea.Dock = System.Windows.Forms.DockStyle.Fill;

            // Top header
            pnlTopHeader.Controls.Add(lblChevron);
            pnlTopHeader.Controls.Add(lblViewName);
            pnlTopHeader.Controls.Add(btnNewOrder);
            pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopHeader.Size = new System.Drawing.Size(684, 64);

            lblChevron.Location = new System.Drawing.Point(16, 20);
            lblChevron.Size = new System.Drawing.Size(16, 24);
            lblChevron.Text = "\u25B6";
            lblViewName.Location = new System.Drawing.Point(34, 18);
            lblViewName.Size = new System.Drawing.Size(120, 28);
            lblViewName.Text = "Orders";
            btnNewOrder.Location = new System.Drawing.Point(540, 16);
            btnNewOrder.Size = new System.Drawing.Size(120, 32);
            btnNewOrder.Text = "+ New Order";
            btnNewOrder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // Filter bar
            pnlFilterBar.Controls.Add(txtSearch);
            pnlFilterBar.Controls.Add(btnFilterAll);
            pnlFilterBar.Controls.Add(btnFilterDineIn);
            pnlFilterBar.Controls.Add(btnFilterTakeaway);
            pnlFilterBar.Controls.Add(lblDateDisplay);
            pnlFilterBar.Controls.Add(btnExport);
            pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlFilterBar.Size = new System.Drawing.Size(684, 56);

            txtSearch.Location = new System.Drawing.Point(16, 12);
            txtSearch.Size = new System.Drawing.Size(240, 32);
            txtSearch.PlaceholderText = "Order ID, Customer...";
            btnFilterAll.Location = new System.Drawing.Point(270, 14);
            btnFilterAll.Size = new System.Drawing.Size(50, 28);
            btnFilterAll.Text = "All";
            btnFilterDineIn.Location = new System.Drawing.Point(324, 14);
            btnFilterDineIn.Size = new System.Drawing.Size(64, 28);
            btnFilterDineIn.Text = "Dine-in";
            btnFilterTakeaway.Location = new System.Drawing.Point(392, 14);
            btnFilterTakeaway.Size = new System.Drawing.Size(76, 28);
            btnFilterTakeaway.Text = "Takeaway";
            lblDateDisplay.Location = new System.Drawing.Point(480, 18);
            lblDateDisplay.Size = new System.Drawing.Size(120, 20);
            lblDateDisplay.Text = "\U0001F4C5 Today: May 20, 2024";
            btnExport.Location = new System.Drawing.Point(600, 14);
            btnExport.Size = new System.Drawing.Size(70, 28);
            btnExport.Text = "\u2B07 Export";
            btnExport.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // Order table
            ((System.ComponentModel.ISupportInitialize)(dgvOrders)).BeginInit();
            dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvOrders.Name = "dgvOrders";
            dgvOrders.ReadOnly = true;
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.AllowUserToDeleteRows = false;
            dgvOrders.AllowUserToResizeRows = false;
            dgvOrders.RowHeadersVisible = false;
            dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // Pagination
            pnlPagination.Controls.Add(lblPaginationInfo);
            pnlPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlPagination.Size = new System.Drawing.Size(684, 48);
            lblPaginationInfo.Location = new System.Drawing.Point(16, 14);
            lblPaginationInfo.Size = new System.Drawing.Size(200, 20);
            lblPaginationInfo.Text = "Showing 6 of 6 results";

            // Receipt header
            pnlReceiptHeader.Controls.Add(lblReceiptTitle);
            pnlReceiptHeader.Controls.Add(btnCloseReceipt);
            pnlReceiptHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlReceiptHeader.Size = new System.Drawing.Size(320, 48);
            lblReceiptTitle.Location = new System.Drawing.Point(16, 14);
            lblReceiptTitle.Size = new System.Drawing.Size(140, 20);
            lblReceiptTitle.Text = "\U0001F9FE Virtual Receipt";
            btnCloseReceipt.Location = new System.Drawing.Point(280, 10);
            btnCloseReceipt.Size = new System.Drawing.Size(28, 28);
            btnCloseReceipt.Text = "\u2715";

            // Receipt body
            pnlReceiptBody.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlReceiptBody.AutoScroll = true;
            pnlReceiptBody.Controls.Add(lblStoreName);
            pnlReceiptBody.Controls.Add(lblStoreAddress);
            pnlReceiptBody.Controls.Add(lblReceiptOrderIdLabel);
            pnlReceiptBody.Controls.Add(lblReceiptOrderId);
            pnlReceiptBody.Controls.Add(lblReceiptDateLabel);
            pnlReceiptBody.Controls.Add(lblReceiptDate);
            pnlReceiptBody.Controls.Add(lblReceiptCustomerLabel);
            pnlReceiptBody.Controls.Add(lblReceiptCustomer);
            pnlReceiptBody.Controls.Add(lblReceiptItems);
            pnlReceiptBody.Controls.Add(lblReceiptSubtotalLabel);
            pnlReceiptBody.Controls.Add(lblReceiptSubtotal);
            pnlReceiptBody.Controls.Add(lblReceiptTaxLabel);
            pnlReceiptBody.Controls.Add(lblReceiptTax);
            pnlReceiptBody.Controls.Add(lblReceiptTotalLabel);
            pnlReceiptBody.Controls.Add(lblReceiptTotal);
            pnlReceiptBody.Controls.Add(lblPaidVia);
            pnlReceiptBody.Controls.Add(lblThankYou);
            pnlReceiptBody.Controls.Add(btnReprint);
            pnlReceiptBody.Controls.Add(btnEmailReceipt);

            lblStoreName.Location = new System.Drawing.Point(90, 20);
            lblStoreName.Size = new System.Drawing.Size(140, 24);
            lblStoreName.Text = "Matcha Caf\u00E9";
            lblStoreAddress.Location = new System.Drawing.Point(60, 44);
            lblStoreAddress.Size = new System.Drawing.Size(200, 16);
            lblStoreAddress.Text = "123 Green Tea Lane, Suite 4B";

            lblReceiptOrderIdLabel.Location = new System.Drawing.Point(16, 80);
            lblReceiptOrderIdLabel.Size = new System.Drawing.Size(80, 16);
            lblReceiptOrderIdLabel.Text = "Order ID";
            lblReceiptOrderId.Location = new System.Drawing.Point(180, 80);
            lblReceiptOrderId.Size = new System.Drawing.Size(120, 16);
            lblReceiptOrderId.Text = "ORD-8821";
            lblReceiptOrderId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblReceiptDateLabel.Location = new System.Drawing.Point(16, 100);
            lblReceiptDateLabel.Size = new System.Drawing.Size(80, 16);
            lblReceiptDateLabel.Text = "Date";
            lblReceiptDate.Location = new System.Drawing.Point(140, 100);
            lblReceiptDate.Size = new System.Drawing.Size(160, 16);
            lblReceiptDate.Text = "2024-05-20 14:23:45";
            lblReceiptDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblReceiptCustomerLabel.Location = new System.Drawing.Point(16, 120);
            lblReceiptCustomerLabel.Size = new System.Drawing.Size(80, 16);
            lblReceiptCustomerLabel.Text = "Customer";
            lblReceiptCustomer.Location = new System.Drawing.Point(180, 120);
            lblReceiptCustomer.Size = new System.Drawing.Size(120, 16);
            lblReceiptCustomer.Text = "Alex Johnson";
            lblReceiptCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblReceiptItems.Location = new System.Drawing.Point(16, 156);
            lblReceiptItems.Size = new System.Drawing.Size(284, 40);
            lblReceiptItems.Text = "2x Iced Matcha Latte, 1x Avocado T...";

            lblReceiptSubtotalLabel.Location = new System.Drawing.Point(16, 212);
            lblReceiptSubtotalLabel.Size = new System.Drawing.Size(80, 16);
            lblReceiptSubtotalLabel.Text = "Subtotal";
            lblReceiptSubtotal.Location = new System.Drawing.Point(200, 212);
            lblReceiptSubtotal.Size = new System.Drawing.Size(100, 16);
            lblReceiptSubtotal.Text = "$23.15";
            lblReceiptSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblReceiptTaxLabel.Location = new System.Drawing.Point(16, 232);
            lblReceiptTaxLabel.Size = new System.Drawing.Size(80, 16);
            lblReceiptTaxLabel.Text = "Tax";
            lblReceiptTax.Location = new System.Drawing.Point(200, 232);
            lblReceiptTax.Size = new System.Drawing.Size(100, 16);
            lblReceiptTax.Text = "$1.85";
            lblReceiptTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblReceiptTotalLabel.Location = new System.Drawing.Point(16, 260);
            lblReceiptTotalLabel.Size = new System.Drawing.Size(80, 20);
            lblReceiptTotalLabel.Text = "TOTAL";
            lblReceiptTotal.Location = new System.Drawing.Point(200, 260);
            lblReceiptTotal.Size = new System.Drawing.Size(100, 20);
            lblReceiptTotal.Text = "$25.00";
            lblReceiptTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblPaidVia.Location = new System.Drawing.Point(60, 296);
            lblPaidVia.Size = new System.Drawing.Size(200, 28);
            lblPaidVia.Text = "PAID VIA CASH";
            lblPaidVia.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            lblThankYou.Location = new System.Drawing.Point(100, 336);
            lblThankYou.Size = new System.Drawing.Size(120, 20);
            lblThankYou.Text = "Thank you!";
            lblThankYou.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            btnReprint.Location = new System.Drawing.Point(16, 370);
            btnReprint.Size = new System.Drawing.Size(140, 36);
            btnReprint.Text = "\U0001F5A8 Reprint";
            btnEmailReceipt.Location = new System.Drawing.Point(164, 370);
            btnEmailReceipt.Size = new System.Drawing.Size(140, 36);
            btnEmailReceipt.Text = "\u2709 Email";

            // OrdersView
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlLeftArea);
            Controls.Add(pnlReceiptSidebar);
            Name = "OrdersView";
            Size = new System.Drawing.Size(1004, 600);
            ((System.ComponentModel.ISupportInitialize)(dgvOrders)).EndInit();
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlReceiptSidebar;
        private System.Windows.Forms.Panel pnlLeftArea;
        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblChevron;
        private System.Windows.Forms.Label lblViewName;
        private Guna.UI2.WinForms.Guna2Button btnNewOrder;
        private System.Windows.Forms.Panel pnlFilterBar;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnFilterAll;
        private Guna.UI2.WinForms.Guna2Button btnFilterDineIn;
        private Guna.UI2.WinForms.Guna2Button btnFilterTakeaway;
        private System.Windows.Forms.Label lblDateDisplay;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private Guna.UI2.WinForms.Guna2DataGridView dgvOrders;
        private System.Windows.Forms.Panel pnlPagination;
        private System.Windows.Forms.Label lblPaginationInfo;
        private System.Windows.Forms.Panel pnlReceiptHeader;
        private System.Windows.Forms.Label lblReceiptTitle;
        private Guna.UI2.WinForms.Guna2Button btnCloseReceipt;
        private System.Windows.Forms.Panel pnlReceiptBody;
        private System.Windows.Forms.Label lblStoreName;
        private System.Windows.Forms.Label lblStoreAddress;
        private System.Windows.Forms.Label lblReceiptOrderIdLabel;
        private System.Windows.Forms.Label lblReceiptOrderId;
        private System.Windows.Forms.Label lblReceiptDateLabel;
        private System.Windows.Forms.Label lblReceiptDate;
        private System.Windows.Forms.Label lblReceiptCustomerLabel;
        private System.Windows.Forms.Label lblReceiptCustomer;
        private System.Windows.Forms.Label lblReceiptItems;
        private System.Windows.Forms.Label lblReceiptSubtotalLabel;
        private System.Windows.Forms.Label lblReceiptSubtotal;
        private System.Windows.Forms.Label lblReceiptTaxLabel;
        private System.Windows.Forms.Label lblReceiptTax;
        private System.Windows.Forms.Label lblReceiptTotalLabel;
        private System.Windows.Forms.Label lblReceiptTotal;
        private System.Windows.Forms.Label lblPaidVia;
        private System.Windows.Forms.Label lblThankYou;
        private Guna.UI2.WinForms.Guna2Button btnReprint;
        private Guna.UI2.WinForms.Guna2Button btnEmailReceipt;
    }
}
