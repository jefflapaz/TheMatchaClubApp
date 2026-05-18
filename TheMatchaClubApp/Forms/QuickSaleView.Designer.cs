namespace TheMatchaClubApp.Forms
{
    partial class QuickSaleView
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            tlpMain = new System.Windows.Forms.TableLayoutPanel();
            pnlCartSidebar = new Guna.UI2.WinForms.Guna2Panel();
            pnlLeftArea = new System.Windows.Forms.Panel();
            pnlTopHeader = new System.Windows.Forms.Panel();
            lblChevron = new System.Windows.Forms.Label();
            lblViewName = new System.Windows.Forms.Label();
            pnlCategoryRow = new System.Windows.Forms.Panel();
            btnCatLeft = new Guna.UI2.WinForms.Guna2Button();
            pnlCategoryScroll = new System.Windows.Forms.Panel();
            flpCategories = new System.Windows.Forms.FlowLayoutPanel();
            btnCatRight = new Guna.UI2.WinForms.Guna2Button();
            pnlProductGrid = new Guna.UI2.WinForms.Guna2Panel();
            flpProducts = new System.Windows.Forms.FlowLayoutPanel();
            pnlCartHeader = new System.Windows.Forms.Panel();
            lblCurrentOrder = new System.Windows.Forms.Label();
            lblOrderMeta = new System.Windows.Forms.Label();
            pnlCartItemsWrapper = new System.Windows.Forms.Panel();
            pnlCartItems = new System.Windows.Forms.Panel();
            pnlCartTotals = new System.Windows.Forms.Panel();
            lblSubtotal = new System.Windows.Forms.Label();
            lblSubtotalValue = new System.Windows.Forms.Label();
            lblTotal = new System.Windows.Forms.Label();
            lblTotalValue = new System.Windows.Forms.Label();
            btnCompleteSale = new Guna.UI2.WinForms.Guna2Button();
            lblCashNote = new System.Windows.Forms.Label();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            pnlSessionOverlay = new Guna.UI2.WinForms.Guna2Panel();
            lblSessionWarning = new System.Windows.Forms.Label();
            btnQuickOpenSession = new Guna.UI2.WinForms.Guna2Button();
            
            // pnlSessionOverlay
            pnlSessionOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlSessionOverlay.BackColor = System.Drawing.Color.FromArgb(200, 255, 255, 255); // Semi-transparent
            pnlSessionOverlay.Visible = false;
            pnlSessionOverlay.Controls.Add(lblSessionWarning);
            pnlSessionOverlay.Controls.Add(btnQuickOpenSession);

            lblSessionWarning.AutoSize = false;
            lblSessionWarning.Size = new System.Drawing.Size(400, 40);
            lblSessionWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblSessionWarning.Text = "Store session is currently closed.";
            lblSessionWarning.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            lblSessionWarning.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            
            btnQuickOpenSession.Size = new System.Drawing.Size(220, 48);
            btnQuickOpenSession.Text = "\u2615 Open Store Session";
            btnQuickOpenSession.BorderRadius = 12;
            
            btnClearCart = new Guna.UI2.WinForms.Guna2Button();
            btnEndSession = new Guna.UI2.WinForms.Guna2Button();
            SuspendLayout();

            // tlpMain
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            tlpMain.Controls.Add(pnlLeftArea, 0, 0);
            tlpMain.Controls.Add(pnlCartSidebar, 1, 0);
            tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpMain.Location = new System.Drawing.Point(0, 0);
            tlpMain.Margin = new System.Windows.Forms.Padding(0);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 1;
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMain.Size = new System.Drawing.Size(1004, 600);

            // pnlCartSidebar
            pnlCartSidebar.Controls.Add(pnlCartItemsWrapper); // Index 0: Fills remaining middle space
            pnlCartSidebar.Controls.Add(pnlCartTotals);       // Index 1: Claims bottom 200px
            pnlCartSidebar.Controls.Add(pnlCartHeader);       // Index 2: Claims top 64px
            pnlCartSidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlCartSidebar.Name = "pnlCartSidebar";
            pnlCartSidebar.Size = new System.Drawing.Size(400, 600);
            pnlCartSidebar.Margin = new System.Windows.Forms.Padding(0);

            // pnlLeftArea
            pnlLeftArea.Controls.Add(pnlSessionOverlay); // Index 0: Overlay
            pnlLeftArea.Controls.Add(pnlProductGrid);    // Index 1: Fills the remaining space below
            pnlLeftArea.Controls.Add(pnlCategoryRow);    // Index 2: Claims next 76px
            pnlLeftArea.Controls.Add(pnlTopHeader);      // Index 3: Claims top 64px
            pnlLeftArea.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlLeftArea.Name = "pnlLeftArea";
            pnlLeftArea.Margin = new System.Windows.Forms.Padding(0);

            pnlTopHeader.Controls.Add(lblChevron);
            pnlTopHeader.Controls.Add(lblViewName);
            pnlTopHeader.Controls.Add(txtSearch);
            pnlTopHeader.Controls.Add(btnEndSession);
            pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopHeader.Size = new System.Drawing.Size(604, 64);

            lblChevron.Location = new System.Drawing.Point(16, 20);
            lblChevron.Size = new System.Drawing.Size(16, 24);
            lblChevron.Text = "\u25B6";

            lblViewName.Location = new System.Drawing.Point(34, 18);
            lblViewName.Size = new System.Drawing.Size(120, 28);
            lblViewName.Text = "Quick Sale";

            txtSearch.Location = new System.Drawing.Point(160, 14);
            txtSearch.Size = new System.Drawing.Size(240, 36);
            txtSearch.PlaceholderText = "Search products...";

            // pnlCategoryRow
            pnlCategoryRow.Controls.Add(pnlCategoryScroll); // Fill center
            pnlCategoryRow.Controls.Add(btnCatRight);        // Right arrow
            pnlCategoryRow.Controls.Add(btnCatLeft);         // Left arrow
            pnlCategoryRow.Dock = System.Windows.Forms.DockStyle.Top;
            pnlCategoryRow.Size = new System.Drawing.Size(604, 56);

            // Left arrow button
            btnCatLeft.Dock = System.Windows.Forms.DockStyle.Left;
            btnCatLeft.Size = new System.Drawing.Size(32, 56);
            btnCatLeft.Text = "\u25C0";
            btnCatLeft.Name = "btnCatLeft";

            // Right arrow button
            btnCatRight.Dock = System.Windows.Forms.DockStyle.Right;
            btnCatRight.Size = new System.Drawing.Size(32, 56);
            btnCatRight.Text = "\u25B6";
            btnCatRight.Name = "btnCatRight";

            // Scroll container (clips the FlowLayoutPanel)
            pnlCategoryScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlCategoryScroll.Controls.Add(flpCategories);
            pnlCategoryScroll.Name = "pnlCategoryScroll";

            // Category FlowLayoutPanel — positioned absolutely inside scroll container
            flpCategories.Location = new System.Drawing.Point(0, 0);
            flpCategories.Size = new System.Drawing.Size(5000, 56);
            flpCategories.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            flpCategories.WrapContents = false;
            flpCategories.AutoSize = true;
            flpCategories.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            flpCategories.Padding = new System.Windows.Forms.Padding(4, 12, 4, 12);

            // pnlProductGrid
            pnlProductGrid.Controls.Add(flpProducts);
            pnlProductGrid.Dock = System.Windows.Forms.DockStyle.Fill;

            flpProducts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flpProducts.Location = new System.Drawing.Point(0, 0);
            flpProducts.Size = new System.Drawing.Size(624, 500); // 20px wider than parent (604)
            flpProducts.WrapContents = true;
            flpProducts.AutoScroll = true;
            flpProducts.Padding = new System.Windows.Forms.Padding(12, 12, 20, 12);

            // Cart header
            pnlCartHeader.Controls.Add(lblCurrentOrder);
            pnlCartHeader.Controls.Add(lblOrderMeta);
            pnlCartHeader.Controls.Add(btnClearCart);
            pnlCartHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlCartHeader.Size = new System.Drawing.Size(400, 64);

            lblCurrentOrder.Location = new System.Drawing.Point(16, 12);
            lblCurrentOrder.Size = new System.Drawing.Size(160, 22);
            lblCurrentOrder.Text = "Current Order";

            lblOrderMeta.Location = new System.Drawing.Point(16, 34);
            lblOrderMeta.Size = new System.Drawing.Size(260, 18);
            lblOrderMeta.Text = "Order #— \u2022 Cashier: —";

            btnEndSession.Location = new System.Drawing.Point(488, 16);
            btnEndSession.Size = new System.Drawing.Size(100, 32);
            btnEndSession.Text = "Close Session";
            btnEndSession.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnEndSession.FillColor = System.Drawing.Color.Transparent;
            btnEndSession.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68); // Red color for close
            btnEndSession.BorderColor = System.Drawing.Color.FromArgb(239, 68, 68);
            btnEndSession.BorderThickness = 1;
            btnEndSession.BorderRadius = 4;
            btnEndSession.Font = new System.Drawing.Font("Segoe UI", 8.5F);

            btnClearCart.Location = new System.Drawing.Point(290, 16);
            btnClearCart.Size = new System.Drawing.Size(95, 32);
            btnClearCart.Text = "Clear Cart";
            btnClearCart.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // Cart items wrapper
            pnlCartItemsWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlCartItemsWrapper.Controls.Add(pnlCartItems);
            pnlCartItemsWrapper.Name = "pnlCartItemsWrapper";

            // Cart items — fills wrapper exactly, vertical scroll only
            pnlCartItems.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlCartItems.AutoScroll = true;

            // Cart totals
            pnlCartTotals.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlCartTotals.Size = new System.Drawing.Size(400, 170);
            pnlCartTotals.Controls.Add(lblSubtotal);
            pnlCartTotals.Controls.Add(lblSubtotalValue);
            pnlCartTotals.Controls.Add(lblTotal);
            pnlCartTotals.Controls.Add(lblTotalValue);
            pnlCartTotals.Controls.Add(btnCompleteSale);
            pnlCartTotals.Controls.Add(lblCashNote);

            lblSubtotal.Location = new System.Drawing.Point(16, 12);
            lblSubtotal.Size = new System.Drawing.Size(80, 20);
            lblSubtotal.Text = "Subtotal";
            lblSubtotalValue.Location = new System.Drawing.Point(240, 12);
            lblSubtotalValue.Size = new System.Drawing.Size(140, 20);
            lblSubtotalValue.Text = "\u20B1 0.00";
            lblSubtotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblTotal.Location = new System.Drawing.Point(16, 56);
            lblTotal.Size = new System.Drawing.Size(80, 24);
            lblTotal.Text = "Total";
            lblTotalValue.Location = new System.Drawing.Point(200, 52);
            lblTotalValue.Size = new System.Drawing.Size(180, 32);
            lblTotalValue.Text = "\u20B1 0.00";
            lblTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            btnCompleteSale.Location = new System.Drawing.Point(16, 100);
            btnCompleteSale.Size = new System.Drawing.Size(368, 52);
            btnCompleteSale.Text = "\u20B1 Complete Sale (Cash)";

            lblCashNote.Location = new System.Drawing.Point(16, 154);
            lblCashNote.Size = new System.Drawing.Size(368, 16);
            lblCashNote.Text = "Cash-only payments supported";
            lblCashNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // QuickSaleView
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tlpMain);
            Name = "QuickSaleView";
            Size = new System.Drawing.Size(1004, 600);
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private Guna.UI2.WinForms.Guna2Panel pnlCartSidebar;
        private System.Windows.Forms.Panel pnlLeftArea;
        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblChevron;
        private System.Windows.Forms.Label lblViewName;
        private System.Windows.Forms.Panel pnlCategoryRow;
        private Guna.UI2.WinForms.Guna2Button btnCatLeft;
        private System.Windows.Forms.Panel pnlCategoryScroll;
        private System.Windows.Forms.FlowLayoutPanel flpCategories;
        private Guna.UI2.WinForms.Guna2Button btnCatRight;
        private Guna.UI2.WinForms.Guna2Panel pnlProductGrid;
        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.Panel pnlCartHeader;
        private System.Windows.Forms.Label lblCurrentOrder;
        private System.Windows.Forms.Label lblOrderMeta;
        private System.Windows.Forms.Panel pnlCartItemsWrapper;
        private System.Windows.Forms.Panel pnlCartItems;
        private System.Windows.Forms.Panel pnlCartTotals;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Label lblSubtotalValue;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalValue;
        private Guna.UI2.WinForms.Guna2Button btnCompleteSale;
        private Guna.UI2.WinForms.Guna2Panel pnlSessionOverlay;
        private System.Windows.Forms.Label lblSessionWarning;
        private Guna.UI2.WinForms.Guna2Button btnQuickOpenSession;
        private Guna.UI2.WinForms.Guna2Button btnClearCart;
        private Guna.UI2.WinForms.Guna2Button btnEndSession;
        private System.Windows.Forms.Label lblCashNote;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
    }
}
