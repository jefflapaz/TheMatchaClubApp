namespace TheMatchaClubApp.Forms
{
    partial class CheckoutDialogForm
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            btnClose = new Guna.UI2.WinForms.Guna2Button();

            lblOrderTypeLabel = new System.Windows.Forms.Label();
            pnlOrderType = new System.Windows.Forms.FlowLayoutPanel();
            btnDineIn = new Guna.UI2.WinForms.Guna2Button();
            btnTakeOut = new Guna.UI2.WinForms.Guna2Button();

            lblCustomerLabel = new System.Windows.Forms.Label();
            txtCustomerSearch = new Guna.UI2.WinForms.Guna2TextBox();
            pnlSuggestions = new System.Windows.Forms.Panel();
            lstSuggestions = new System.Windows.Forms.ListBox();
            lblNewCustomerLabel = new System.Windows.Forms.Label();
            txtFirstName = new Guna.UI2.WinForms.Guna2TextBox();
            txtLastName = new Guna.UI2.WinForms.Guna2TextBox();
            txtPhone = new Guna.UI2.WinForms.Guna2TextBox();
            txtNewEmail = new Guna.UI2.WinForms.Guna2TextBox();
            lblValidation = new System.Windows.Forms.Label();

            lblTotalLabel = new System.Windows.Forms.Label();
            lblTotalValue = new System.Windows.Forms.Label();
            lblCashLabel = new System.Windows.Forms.Label();
            txtCash = new Guna.UI2.WinForms.Guna2TextBox();
            lblChangeLabel = new System.Windows.Forms.Label();
            lblChange = new System.Windows.Forms.Label();
            btnConfirm = new Guna.UI2.WinForms.Guna2Button();

            SuspendLayout();

            // ── Header ────────────────────────────────────────────────
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Size = new System.Drawing.Size(420, 50);

            lblTitle.Location = new System.Drawing.Point(20, 12);
            lblTitle.Size = new System.Drawing.Size(300, 28);
            lblTitle.Text = "Complete Order";

            btnClose.Location = new System.Drawing.Point(378, 10);
            btnClose.Size = new System.Drawing.Size(30, 30);
            btnClose.Text = "✕";
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // ── Order Type ────────────────────────────────────────────
            lblOrderTypeLabel.Location = new System.Drawing.Point(20, 60);
            lblOrderTypeLabel.Size = new System.Drawing.Size(200, 20);
            lblOrderTypeLabel.Text = "ORDER TYPE";

            pnlOrderType.Location = new System.Drawing.Point(20, 84);
            pnlOrderType.Size = new System.Drawing.Size(380, 44);
            pnlOrderType.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlOrderType.Controls.Add(btnDineIn);
            pnlOrderType.Controls.Add(btnTakeOut);

            btnDineIn.Size = new System.Drawing.Size(180, 40);
            btnDineIn.Text = "🍽  Dine-In";
            btnDineIn.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            btnTakeOut.Size = new System.Drawing.Size(180, 40);
            btnTakeOut.Text = "📦  Take-Out";

            // ── Customer Search ───────────────────────────────────────
            lblCustomerLabel.Location = new System.Drawing.Point(20, 140);
            lblCustomerLabel.Size = new System.Drawing.Size(200, 20);
            lblCustomerLabel.Text = "CUSTOMER";

            txtCustomerSearch.Location = new System.Drawing.Point(20, 163);
            txtCustomerSearch.Size = new System.Drawing.Size(380, 40);
            txtCustomerSearch.PlaceholderText = "Enter Customer Name";

            // Suggestion overlay
            pnlSuggestions.Location = new System.Drawing.Point(20, 203);
            pnlSuggestions.Size = new System.Drawing.Size(380, 0);
            pnlSuggestions.Visible = false;
            pnlSuggestions.Controls.Add(lstSuggestions);

            lstSuggestions.Dock = System.Windows.Forms.DockStyle.Fill;
            lstSuggestions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lstSuggestions.ItemHeight = 30;
            lstSuggestions.IntegralHeight = false;

            // ── New Customer Fields ───────────────────────────────────
            lblNewCustomerLabel.Location = new System.Drawing.Point(20, 212);
            lblNewCustomerLabel.Size = new System.Drawing.Size(250, 18);
            lblNewCustomerLabel.Text = "New customer details:";

            txtFirstName.Location = new System.Drawing.Point(20, 233);
            txtFirstName.Size = new System.Drawing.Size(185, 38);
            txtFirstName.PlaceholderText = "First Name";

            txtLastName.Location = new System.Drawing.Point(215, 233);
            txtLastName.Size = new System.Drawing.Size(185, 38);
            txtLastName.PlaceholderText = "Last Name";

            txtPhone.Location = new System.Drawing.Point(20, 278);
            txtPhone.Size = new System.Drawing.Size(185, 38);
            txtPhone.PlaceholderText = "Phone (optional)";

            txtNewEmail.Location = new System.Drawing.Point(215, 278);
            txtNewEmail.Size = new System.Drawing.Size(185, 38);
            txtNewEmail.PlaceholderText = "Email (optional)";

            // ── Payment ───────────────────────────────────────────────
            lblTotalLabel.Location = new System.Drawing.Point(20, 332);
            lblTotalLabel.Size = new System.Drawing.Size(120, 20);
            lblTotalLabel.Text = "TOTAL DUE:";

            lblTotalValue.Location = new System.Drawing.Point(140, 328);
            lblTotalValue.Size = new System.Drawing.Size(260, 32);
            lblTotalValue.Text = "\u20B10.00";

            lblCashLabel.Location = new System.Drawing.Point(20, 374);
            lblCashLabel.Size = new System.Drawing.Size(120, 20);
            lblCashLabel.Text = "CASH IN:";

            txtCash.Location = new System.Drawing.Point(140, 368);
            txtCash.Size = new System.Drawing.Size(260, 42);
            txtCash.PlaceholderText = "Enter amount...";

            lblChangeLabel.Location = new System.Drawing.Point(20, 426);
            lblChangeLabel.Size = new System.Drawing.Size(120, 20);
            lblChangeLabel.Text = "CHANGE:";

            lblChange.Location = new System.Drawing.Point(140, 423);
            lblChange.Size = new System.Drawing.Size(260, 32);
            lblChange.Text = "\u20B10.00";

            // ── Validation Error ──────────────────────────────────────
            lblValidation.Location = new System.Drawing.Point(20, 452);
            lblValidation.Size = new System.Drawing.Size(380, 18);
            lblValidation.Text = "";
            lblValidation.Visible = false;
            lblValidation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── Confirm ───────────────────────────────────────────────
            btnConfirm.Location = new System.Drawing.Point(20, 472);
            btnConfirm.Size = new System.Drawing.Size(380, 50);
            btnConfirm.Text = "\u2713  Confirm & Complete Sale";

            // ── Form ──────────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(420, 545);
            Controls.Add(pnlSuggestions);
            Controls.Add(lblValidation);
            Controls.Add(btnConfirm);
            Controls.Add(lblChange);
            Controls.Add(lblChangeLabel);
            Controls.Add(txtCash);
            Controls.Add(lblCashLabel);
            Controls.Add(lblTotalValue);
            Controls.Add(lblTotalLabel);
            Controls.Add(txtNewEmail);
            Controls.Add(txtPhone);
            Controls.Add(txtLastName);
            Controls.Add(txtFirstName);
            Controls.Add(lblNewCustomerLabel);
            Controls.Add(txtCustomerSearch);
            Controls.Add(lblCustomerLabel);
            Controls.Add(pnlOrderType);
            Controls.Add(lblOrderTypeLabel);
            Controls.Add(pnlHeader);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "CheckoutDialogForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Checkout";

            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private System.Windows.Forms.Label lblOrderTypeLabel;
        private System.Windows.Forms.FlowLayoutPanel pnlOrderType;
        private Guna.UI2.WinForms.Guna2Button btnDineIn;
        private Guna.UI2.WinForms.Guna2Button btnTakeOut;
        private System.Windows.Forms.Label lblCustomerLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtCustomerSearch;
        private System.Windows.Forms.Panel pnlSuggestions;
        private System.Windows.Forms.ListBox lstSuggestions;
        private System.Windows.Forms.Label lblNewCustomerLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtFirstName;
        private Guna.UI2.WinForms.Guna2TextBox txtLastName;
        private Guna.UI2.WinForms.Guna2TextBox txtPhone;
        private Guna.UI2.WinForms.Guna2TextBox txtNewEmail;
        private System.Windows.Forms.Label lblValidation;
        private Guna.UI2.WinForms.Guna2Button btnConfirm;
        private System.Windows.Forms.Label lblTotalLabel;
        public System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblCashLabel;
        public Guna.UI2.WinForms.Guna2TextBox txtCash;
        private System.Windows.Forms.Label lblChangeLabel;
        public System.Windows.Forms.Label lblChange;
    }
}
