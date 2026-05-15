namespace TheMatchaClubApp.Forms
{
    partial class DashboardView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── Top Header ────────────────────────────────────────────
            pnlTopHeader = new Panel();
            lblChevron = new Label();
            lblViewName = new Label();
            pnlStoreStatus = new Guna.UI2.WinForms.Guna2Panel();
            lblStoreStatus = new Label();
            lblDate = new Label();
            btnNotification = new Guna.UI2.WinForms.Guna2Button();
            picAvatar = new Guna.UI2.WinForms.Guna2CirclePictureBox();

            // ── Quick Actions ─────────────────────────────────────────
            pnlQuickActions = new Panel();
            btnQuickNewSale = new Guna.UI2.WinForms.Guna2Button();
            btnQuickOpenSession = new Guna.UI2.WinForms.Guna2Button();
            btnQuickCloseSession = new Guna.UI2.WinForms.Guna2Button();
            btnQuickReports = new Guna.UI2.WinForms.Guna2Button();
            btnQuickAddProduct = new Guna.UI2.WinForms.Guna2Button();

            // ── KPI Cards ─────────────────────────────────────────────
            pnlCard1 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard1Title = new Label();
            lblCard1Value = new Label();
            pnlCard2 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard2Title = new Label();
            lblCard2Value = new Label();
            pnlCard3 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard3Title = new Label();
            lblCard3Value = new Label();
            pnlCard4 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard4Title = new Label();
            lblCard4Value = new Label();
            pnlCard5 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard5Title = new Label();
            lblCard5Value = new Label();
            pnlCard6 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard6Title = new Label();
            lblCard6Value = new Label();
            pnlCard7 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard7Title = new Label();
            lblCard7Value = new Label();
            pnlCard8 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard8Title = new Label();
            lblCard8Value = new Label();

            // ── Analytics Panels ──────────────────────────────────────
            pnlHourlySales = new Guna.UI2.WinForms.Guna2Panel();
            lblHourlySalesTitle = new Label();
            pnlTopProducts = new Guna.UI2.WinForms.Guna2Panel();
            lblTopProductsTitle = new Label();
            pnlRecentTx = new Guna.UI2.WinForms.Guna2Panel();
            lblRecentTxTitle = new Label();
            pnlSessionStatus = new Guna.UI2.WinForms.Guna2Panel();
            lblSessionStatusTitle = new Label();

            // ── Empty State ───────────────────────────────────────────
            pnlEmptyState = new Guna.UI2.WinForms.Guna2Panel();
            lblEmptyIcon = new Label();
            lblEmptyMessage = new Label();
            btnEmptyAction = new Guna.UI2.WinForms.Guna2Button();

            // ── Timer ─────────────────────────────────────────────────
            tmrSessionDuration = new System.Windows.Forms.Timer(components);

            // Begin init
            pnlTopHeader.SuspendLayout();
            pnlStoreStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            pnlQuickActions.SuspendLayout();
            pnlCard1.SuspendLayout();
            pnlCard2.SuspendLayout();
            pnlCard3.SuspendLayout();
            pnlCard4.SuspendLayout();
            pnlCard5.SuspendLayout();
            pnlCard6.SuspendLayout();
            pnlCard7.SuspendLayout();
            pnlCard8.SuspendLayout();
            pnlHourlySales.SuspendLayout();
            pnlTopProducts.SuspendLayout();
            pnlRecentTx.SuspendLayout();
            pnlSessionStatus.SuspendLayout();
            pnlEmptyState.SuspendLayout();
            SuspendLayout();

            // ════════════════════════════════════════════════════════════
            //  TOP HEADER (preserved from original)
            // ════════════════════════════════════════════════════════════
            pnlTopHeader.Controls.Add(lblChevron);
            pnlTopHeader.Controls.Add(lblViewName);
            pnlTopHeader.Controls.Add(pnlStoreStatus);
            pnlTopHeader.Controls.Add(lblDate);
            pnlTopHeader.Controls.Add(btnNotification);
            pnlTopHeader.Controls.Add(picAvatar);
            pnlTopHeader.Dock = DockStyle.Top;
            pnlTopHeader.Location = new Point(0, 0);
            pnlTopHeader.Name = "pnlTopHeader";
            pnlTopHeader.Size = new Size(1004, 64);
            pnlTopHeader.TabIndex = 0;

            lblChevron.Location = new Point(16, 20);
            lblChevron.Name = "lblChevron";
            lblChevron.Size = new Size(16, 24);
            lblChevron.Text = "►";

            lblViewName.Location = new Point(34, 18);
            lblViewName.Name = "lblViewName";
            lblViewName.Size = new Size(120, 28);
            lblViewName.Text = "Dashboard";

            pnlStoreStatus.Controls.Add(lblStoreStatus);
            pnlStoreStatus.Location = new Point(620, 21);
            pnlStoreStatus.Name = "pnlStoreStatus";
            pnlStoreStatus.Size = new Size(100, 22);

            lblStoreStatus.Dock = DockStyle.Fill;
            lblStoreStatus.Name = "lblStoreStatus";
            lblStoreStatus.Size = new Size(100, 22);
            lblStoreStatus.Text = "   STORE OPEN";
            lblStoreStatus.TextAlign = ContentAlignment.MiddleCenter;

            lblDate.Location = new Point(726, 20);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(100, 24);
            lblDate.Text = "📅 5/15/2026";

            btnNotification.Font = new Font("Segoe UI", 9F);
            btnNotification.ForeColor = Color.White;
            btnNotification.Location = new Point(830, 16);
            btnNotification.Name = "btnNotification";
            btnNotification.Size = new Size(32, 32);
            btnNotification.Text = "🔔";

            picAvatar.ImageRotate = 0F;
            picAvatar.Location = new Point(868, 16);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new Size(32, 32);
            picAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
            picAvatar.TabStop = false;

            // ════════════════════════════════════════════════════════════
            //  QUICK ACTIONS ROW
            // ════════════════════════════════════════════════════════════
            pnlQuickActions.Name = "pnlQuickActions";
            pnlQuickActions.Location = new Point(24, 76);
            pnlQuickActions.Size = new Size(960, 36);
            pnlQuickActions.Controls.Add(btnQuickNewSale);
            pnlQuickActions.Controls.Add(btnQuickOpenSession);
            pnlQuickActions.Controls.Add(btnQuickCloseSession);
            pnlQuickActions.Controls.Add(btnQuickReports);
            pnlQuickActions.Controls.Add(btnQuickAddProduct);

            btnQuickNewSale.Name = "btnQuickNewSale";
            btnQuickNewSale.Text = "🛒  New Sale";
            btnQuickNewSale.Size = new Size(120, 32);
            btnQuickNewSale.Location = new Point(0, 0);

            btnQuickOpenSession.Name = "btnQuickOpenSession";
            btnQuickOpenSession.Text = "▶  Open Session";
            btnQuickOpenSession.Size = new Size(140, 32);
            btnQuickOpenSession.Location = new Point(128, 0);

            btnQuickCloseSession.Name = "btnQuickCloseSession";
            btnQuickCloseSession.Text = "⏹  Close Session";
            btnQuickCloseSession.Size = new Size(140, 32);
            btnQuickCloseSession.Location = new Point(276, 0);

            btnQuickReports.Name = "btnQuickReports";
            btnQuickReports.Text = "📊  View Reports";
            btnQuickReports.Size = new Size(140, 32);
            btnQuickReports.Location = new Point(424, 0);

            btnQuickAddProduct.Name = "btnQuickAddProduct";
            btnQuickAddProduct.Text = "➕  Add Product";
            btnQuickAddProduct.Size = new Size(140, 32);
            btnQuickAddProduct.Location = new Point(572, 0);

            // ════════════════════════════════════════════════════════════
            //  KPI CARDS (8 cards)
            // ════════════════════════════════════════════════════════════
            SetupCardDesigner(pnlCard1, lblCard1Title, lblCard1Value, "pnlCard1", "Total Sales Today", "₱0.00", new Point(24, 120));
            SetupCardDesigner(pnlCard2, lblCard2Title, lblCard2Value, "pnlCard2", "Number of Orders", "0", new Point(260, 120));
            SetupCardDesigner(pnlCard3, lblCard3Title, lblCard3Value, "pnlCard3", "Average Order Value", "₱0.00", new Point(496, 120));
            SetupCardDesigner(pnlCard4, lblCard4Title, lblCard4Value, "pnlCard4", "Cash on Hand", "₱0.00", new Point(732, 120));
            SetupCardDesigner(pnlCard5, lblCard5Title, lblCard5Value, "pnlCard5", "Units Sold", "0", new Point(24, 200));
            SetupCardDesigner(pnlCard6, lblCard6Title, lblCard6Value, "pnlCard6", "Best Seller", "—", new Point(260, 200));
            SetupCardDesigner(pnlCard7, lblCard7Title, lblCard7Value, "pnlCard7", "Session Duration", "—", new Point(496, 200));
            SetupCardDesigner(pnlCard8, lblCard8Title, lblCard8Value, "pnlCard8", "Peak Sales Hour", "—", new Point(732, 200));

            // ════════════════════════════════════════════════════════════
            //  ANALYTICS PANELS
            // ════════════════════════════════════════════════════════════
            // Hourly Sales Chart
            pnlHourlySales.Name = "pnlHourlySales";
            pnlHourlySales.Location = new Point(24, 288);
            pnlHourlySales.Size = new Size(600, 200);
            pnlHourlySales.Controls.Add(lblHourlySalesTitle);
            lblHourlySalesTitle.Name = "lblHourlySalesTitle";
            lblHourlySalesTitle.Text = "📈  Hourly Sales Trend";
            lblHourlySalesTitle.Location = new Point(16, 12);
            lblHourlySalesTitle.Size = new Size(300, 20);
            lblHourlySalesTitle.AutoSize = true;

            // Top Selling Products
            pnlTopProducts.Name = "pnlTopProducts";
            pnlTopProducts.Location = new Point(24, 496);
            pnlTopProducts.Size = new Size(600, 180);
            pnlTopProducts.Controls.Add(lblTopProductsTitle);
            lblTopProductsTitle.Name = "lblTopProductsTitle";
            lblTopProductsTitle.Text = "🏆  Top Selling Products";
            lblTopProductsTitle.Location = new Point(16, 12);
            lblTopProductsTitle.Size = new Size(300, 20);
            lblTopProductsTitle.AutoSize = true;

            // Recent Transactions
            pnlRecentTx.Name = "pnlRecentTx";
            pnlRecentTx.Location = new Point(24, 684);
            pnlRecentTx.Size = new Size(600, 180);
            pnlRecentTx.Controls.Add(lblRecentTxTitle);
            lblRecentTxTitle.Name = "lblRecentTxTitle";
            lblRecentTxTitle.Text = "🧾  Recent Transactions";
            lblRecentTxTitle.Location = new Point(16, 12);
            lblRecentTxTitle.Size = new Size(300, 20);
            lblRecentTxTitle.AutoSize = true;

            // Session Status Panel
            pnlSessionStatus.Name = "pnlSessionStatus";
            pnlSessionStatus.Location = new Point(636, 288);
            pnlSessionStatus.Size = new Size(340, 420);
            pnlSessionStatus.Controls.Add(lblSessionStatusTitle);
            lblSessionStatusTitle.Name = "lblSessionStatusTitle";
            lblSessionStatusTitle.Text = "⚙  Live Session Status";
            lblSessionStatusTitle.Location = new Point(16, 12);
            lblSessionStatusTitle.Size = new Size(300, 20);
            lblSessionStatusTitle.AutoSize = true;

            // ════════════════════════════════════════════════════════════
            //  EMPTY STATE OVERLAY
            // ════════════════════════════════════════════════════════════
            pnlEmptyState.Name = "pnlEmptyState";
            pnlEmptyState.Location = new Point(24, 288);
            pnlEmptyState.Size = new Size(940, 300);
            pnlEmptyState.Controls.Add(lblEmptyIcon);
            pnlEmptyState.Controls.Add(lblEmptyMessage);
            pnlEmptyState.Controls.Add(btnEmptyAction);
            pnlEmptyState.Visible = false;

            lblEmptyIcon.Name = "lblEmptyIcon";
            lblEmptyIcon.Text = "🔒";
            lblEmptyIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblEmptyIcon.Location = new Point(370, 60);
            lblEmptyIcon.Size = new Size(200, 60);

            lblEmptyMessage.Name = "lblEmptyMessage";
            lblEmptyMessage.Text = "Open a store session to begin operations.";
            lblEmptyMessage.TextAlign = ContentAlignment.MiddleCenter;
            lblEmptyMessage.Location = new Point(220, 130);
            lblEmptyMessage.Size = new Size(500, 50);

            btnEmptyAction.Name = "btnEmptyAction";
            btnEmptyAction.Text = "Open Session";
            btnEmptyAction.Size = new Size(160, 40);
            btnEmptyAction.Location = new Point(390, 190);

            // ════════════════════════════════════════════════════════════
            //  TIMER
            // ════════════════════════════════════════════════════════════
            tmrSessionDuration.Interval = 1000;

            // ════════════════════════════════════════════════════════════
            //  DASHBOARD VIEW
            // ════════════════════════════════════════════════════════════
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(pnlEmptyState);
            Controls.Add(pnlSessionStatus);
            Controls.Add(pnlRecentTx);
            Controls.Add(pnlTopProducts);
            Controls.Add(pnlHourlySales);
            Controls.Add(pnlCard8);
            Controls.Add(pnlCard7);
            Controls.Add(pnlCard6);
            Controls.Add(pnlCard5);
            Controls.Add(pnlCard4);
            Controls.Add(pnlCard3);
            Controls.Add(pnlCard2);
            Controls.Add(pnlCard1);
            Controls.Add(pnlQuickActions);
            Controls.Add(pnlTopHeader);
            Name = "DashboardView";
            Size = new Size(1004, 700);

            pnlTopHeader.ResumeLayout(false);
            pnlStoreStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            pnlQuickActions.ResumeLayout(false);
            pnlCard1.ResumeLayout(false);
            pnlCard2.ResumeLayout(false);
            pnlCard3.ResumeLayout(false);
            pnlCard4.ResumeLayout(false);
            pnlCard5.ResumeLayout(false);
            pnlCard6.ResumeLayout(false);
            pnlCard7.ResumeLayout(false);
            pnlCard8.ResumeLayout(false);
            pnlHourlySales.ResumeLayout(false);
            pnlHourlySales.PerformLayout();
            pnlTopProducts.ResumeLayout(false);
            pnlTopProducts.PerformLayout();
            pnlRecentTx.ResumeLayout(false);
            pnlRecentTx.PerformLayout();
            pnlSessionStatus.ResumeLayout(false);
            pnlSessionStatus.PerformLayout();
            pnlEmptyState.ResumeLayout(false);
            ResumeLayout(false);
        }

        /// <summary>Helper to reduce repetition for KPI card setup in Designer.</summary>
        private void SetupCardDesigner(Guna.UI2.WinForms.Guna2Panel card, Label title, Label value,
            string name, string titleText, string valueText, Point location)
        {
            card.Name = name;
            card.Location = location;
            card.Size = new Size(220, 72);
            card.Controls.Add(title);
            card.Controls.Add(value);

            title.Name = name.Replace("pnl", "lblTitle");
            title.Text = titleText;
            title.Location = new Point(52, 12);
            title.Size = new Size(160, 16);
            title.AutoSize = true;

            value.Name = name.Replace("pnl", "lblValue");
            value.Text = valueText;
            value.Location = new Point(52, 32);
            value.Size = new Size(160, 28);
            value.AutoSize = true;
        }

        // ── Top Header ─────────────────────────────────────────────────
        private Panel pnlTopHeader;
        private Label lblChevron;
        private Label lblViewName;
        private Guna.UI2.WinForms.Guna2Panel pnlStoreStatus;
        private Label lblStoreStatus;
        private Label lblDate;
        private Guna.UI2.WinForms.Guna2Button btnNotification;
        private Guna.UI2.WinForms.Guna2CirclePictureBox picAvatar;

        // ── Quick Actions ──────────────────────────────────────────────
        private Panel pnlQuickActions;
        private Guna.UI2.WinForms.Guna2Button btnQuickNewSale;
        private Guna.UI2.WinForms.Guna2Button btnQuickOpenSession;
        private Guna.UI2.WinForms.Guna2Button btnQuickCloseSession;
        private Guna.UI2.WinForms.Guna2Button btnQuickReports;
        private Guna.UI2.WinForms.Guna2Button btnQuickAddProduct;

        // ── KPI Cards ──────────────────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Panel pnlCard1;
        private Label lblCard1Title;
        private Label lblCard1Value;
        private Guna.UI2.WinForms.Guna2Panel pnlCard2;
        private Label lblCard2Title;
        private Label lblCard2Value;
        private Guna.UI2.WinForms.Guna2Panel pnlCard3;
        private Label lblCard3Title;
        private Label lblCard3Value;
        private Guna.UI2.WinForms.Guna2Panel pnlCard4;
        private Label lblCard4Title;
        private Label lblCard4Value;
        private Guna.UI2.WinForms.Guna2Panel pnlCard5;
        private Label lblCard5Title;
        private Label lblCard5Value;
        private Guna.UI2.WinForms.Guna2Panel pnlCard6;
        private Label lblCard6Title;
        private Label lblCard6Value;
        private Guna.UI2.WinForms.Guna2Panel pnlCard7;
        private Label lblCard7Title;
        private Label lblCard7Value;
        private Guna.UI2.WinForms.Guna2Panel pnlCard8;
        private Label lblCard8Title;
        private Label lblCard8Value;

        // ── Analytics Panels ───────────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Panel pnlHourlySales;
        private Label lblHourlySalesTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlTopProducts;
        private Label lblTopProductsTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlRecentTx;
        private Label lblRecentTxTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlSessionStatus;
        private Label lblSessionStatusTitle;

        // ── Empty State ────────────────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Panel pnlEmptyState;
        private Label lblEmptyIcon;
        private Label lblEmptyMessage;
        private Guna.UI2.WinForms.Guna2Button btnEmptyAction;

        // ── Timer ──────────────────────────────────────────────────────
        private System.Windows.Forms.Timer tmrSessionDuration;
    }
}
