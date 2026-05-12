namespace TheMatchaClubApp.Forms
{
    partial class QuickSaleView
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlCartSidebar = new Guna.UI2.WinForms.Guna2Panel();
            pnlLeftArea = new System.Windows.Forms.Panel();
            pnlTopHeader = new System.Windows.Forms.Panel();
            lblChevron = new System.Windows.Forms.Label();
            lblViewName = new System.Windows.Forms.Label();
            btnAlert = new Guna.UI2.WinForms.Guna2Button();
            pnlCategoryRow = new System.Windows.Forms.Panel();
            flpCategories = new System.Windows.Forms.FlowLayoutPanel();
            pnlProductGrid = new Guna.UI2.WinForms.Guna2Panel();
            flpProducts = new System.Windows.Forms.FlowLayoutPanel();
            pnlCartHeader = new System.Windows.Forms.Panel();
            lblCurrentOrder = new System.Windows.Forms.Label();
            lblOrderMeta = new System.Windows.Forms.Label();
            btnEatIn = new Guna.UI2.WinForms.Guna2Button();
            pnlCartItems = new System.Windows.Forms.Panel();
            pnlCartTotals = new System.Windows.Forms.Panel();
            lblSubtotal = new System.Windows.Forms.Label();
            lblSubtotalValue = new System.Windows.Forms.Label();
            lblTax = new System.Windows.Forms.Label();
            lblTaxValue = new System.Windows.Forms.Label();
            lblTotal = new System.Windows.Forms.Label();
            lblTotalValue = new System.Windows.Forms.Label();
            btnPrint = new Guna.UI2.WinForms.Guna2Button();
            btnEmail = new Guna.UI2.WinForms.Guna2Button();
            btnCompleteSale = new Guna.UI2.WinForms.Guna2Button();
            lblCashNote = new System.Windows.Forms.Label();
            SuspendLayout();

            // pnlCartSidebar
            pnlCartSidebar.Controls.Add(pnlCartItems);
            pnlCartSidebar.Controls.Add(pnlCartTotals);
            pnlCartSidebar.Controls.Add(pnlCartHeader);
            pnlCartSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            pnlCartSidebar.Name = "pnlCartSidebar";
            pnlCartSidebar.Size = new System.Drawing.Size(320, 600);

            // pnlLeftArea
            pnlLeftArea.Controls.Add(pnlProductGrid);
            pnlLeftArea.Controls.Add(pnlCategoryRow);
            pnlLeftArea.Controls.Add(pnlTopHeader);
            pnlLeftArea.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLeftArea.Name = "pnlLeftArea";

            // pnlTopHeader
            pnlTopHeader.Controls.Add(lblChevron);
            pnlTopHeader.Controls.Add(lblViewName);
            pnlTopHeader.Controls.Add(btnAlert);
            pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopHeader.Size = new System.Drawing.Size(684, 64);

            lblChevron.Location = new System.Drawing.Point(16, 20);
            lblChevron.Size = new System.Drawing.Size(16, 24);
            lblChevron.Text = "\u25B6";

            lblViewName.Location = new System.Drawing.Point(34, 18);
            lblViewName.Size = new System.Drawing.Size(120, 28);
            lblViewName.Text = "Quick Sale";

            btnAlert.Location = new System.Drawing.Point(600, 16);
            btnAlert.Size = new System.Drawing.Size(32, 32);
            btnAlert.Text = "\u26A0";
            btnAlert.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // pnlCategoryRow
            pnlCategoryRow.Controls.Add(flpCategories);
            pnlCategoryRow.Dock = System.Windows.Forms.DockStyle.Top;
            pnlCategoryRow.Size = new System.Drawing.Size(684, 48);

            flpCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            flpCategories.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            flpCategories.WrapContents = false;
            flpCategories.AutoScroll = false;
            flpCategories.Padding = new System.Windows.Forms.Padding(12, 8, 0, 0);

            // pnlProductGrid
            pnlProductGrid.Controls.Add(flpProducts);
            pnlProductGrid.Dock = System.Windows.Forms.DockStyle.Fill;

            flpProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            flpProducts.WrapContents = true;
            flpProducts.AutoScroll = true;
            flpProducts.Padding = new System.Windows.Forms.Padding(12);

            // Cart header
            pnlCartHeader.Controls.Add(lblCurrentOrder);
            pnlCartHeader.Controls.Add(lblOrderMeta);
            pnlCartHeader.Controls.Add(btnEatIn);
            pnlCartHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlCartHeader.Size = new System.Drawing.Size(320, 64);

            lblCurrentOrder.Location = new System.Drawing.Point(16, 12);
            lblCurrentOrder.Size = new System.Drawing.Size(160, 22);
            lblCurrentOrder.Text = "Current Order";

            lblOrderMeta.Location = new System.Drawing.Point(16, 34);
            lblOrderMeta.Size = new System.Drawing.Size(180, 18);
            lblOrderMeta.Text = "Order #1025 \u2022 Cashier: Admin";

            btnEatIn.Location = new System.Drawing.Point(230, 16);
            btnEatIn.Size = new System.Drawing.Size(72, 32);
            btnEatIn.Text = "Eat-In";

            // Cart items
            pnlCartItems.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlCartItems.AutoScroll = true;

            // Cart totals
            pnlCartTotals.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlCartTotals.Size = new System.Drawing.Size(320, 220);
            pnlCartTotals.Controls.Add(lblSubtotal);
            pnlCartTotals.Controls.Add(lblSubtotalValue);
            pnlCartTotals.Controls.Add(lblTax);
            pnlCartTotals.Controls.Add(lblTaxValue);
            pnlCartTotals.Controls.Add(lblTotal);
            pnlCartTotals.Controls.Add(lblTotalValue);
            pnlCartTotals.Controls.Add(btnPrint);
            pnlCartTotals.Controls.Add(btnEmail);
            pnlCartTotals.Controls.Add(btnCompleteSale);
            pnlCartTotals.Controls.Add(lblCashNote);

            lblSubtotal.Location = new System.Drawing.Point(16, 8);
            lblSubtotal.Size = new System.Drawing.Size(80, 18);
            lblSubtotal.Text = "Subtotal";
            lblSubtotalValue.Location = new System.Drawing.Point(200, 8);
            lblSubtotalValue.Size = new System.Drawing.Size(100, 18);
            lblSubtotalValue.Text = "$0.00";
            lblSubtotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblTax.Location = new System.Drawing.Point(16, 30);
            lblTax.Size = new System.Drawing.Size(100, 18);
            lblTax.Text = "Sales Tax (8%)";
            lblTaxValue.Location = new System.Drawing.Point(200, 30);
            lblTaxValue.Size = new System.Drawing.Size(100, 18);
            lblTaxValue.Text = "$0.00";
            lblTaxValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblTotal.Location = new System.Drawing.Point(16, 60);
            lblTotal.Size = new System.Drawing.Size(80, 24);
            lblTotal.Text = "Total";
            lblTotalValue.Location = new System.Drawing.Point(180, 60);
            lblTotalValue.Size = new System.Drawing.Size(120, 24);
            lblTotalValue.Text = "$0.00";
            lblTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            btnPrint.Location = new System.Drawing.Point(16, 96);
            btnPrint.Size = new System.Drawing.Size(140, 36);
            btnPrint.Text = "\U0001F5A8 Print";
            btnEmail.Location = new System.Drawing.Point(164, 96);
            btnEmail.Size = new System.Drawing.Size(140, 36);
            btnEmail.Text = "\u2709 Email";

            btnCompleteSale.Location = new System.Drawing.Point(16, 142);
            btnCompleteSale.Size = new System.Drawing.Size(288, 52);
            btnCompleteSale.Text = "$ Complete Sale (Cash)";

            lblCashNote.Location = new System.Drawing.Point(16, 198);
            lblCashNote.Size = new System.Drawing.Size(288, 16);
            lblCashNote.Text = "Cash-only payments supported";
            lblCashNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // QuickSaleView
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlLeftArea);
            Controls.Add(pnlCartSidebar);
            Name = "QuickSaleView";
            Size = new System.Drawing.Size(1004, 600);
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlCartSidebar;
        private System.Windows.Forms.Panel pnlLeftArea;
        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblChevron;
        private System.Windows.Forms.Label lblViewName;
        private Guna.UI2.WinForms.Guna2Button btnAlert;
        private System.Windows.Forms.Panel pnlCategoryRow;
        private System.Windows.Forms.FlowLayoutPanel flpCategories;
        private Guna.UI2.WinForms.Guna2Panel pnlProductGrid;
        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.Panel pnlCartHeader;
        private System.Windows.Forms.Label lblCurrentOrder;
        private System.Windows.Forms.Label lblOrderMeta;
        private Guna.UI2.WinForms.Guna2Button btnEatIn;
        private System.Windows.Forms.Panel pnlCartItems;
        private System.Windows.Forms.Panel pnlCartTotals;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Label lblSubtotalValue;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblTaxValue;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalValue;
        private Guna.UI2.WinForms.Guna2Button btnPrint;
        private Guna.UI2.WinForms.Guna2Button btnEmail;
        private Guna.UI2.WinForms.Guna2Button btnCompleteSale;
        private System.Windows.Forms.Label lblCashNote;
    }
}
