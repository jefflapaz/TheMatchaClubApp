namespace TheMatchaClubApp.Forms
{
    partial class CheckoutDialogForm
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            lblSubtitle = new System.Windows.Forms.Label();
            btnClose = new Guna.UI2.WinForms.Guna2Button();

            pnlOrderTypeCard = new Guna.UI2.WinForms.Guna2Panel();
            lblOrderTypeLabel = new System.Windows.Forms.Label();
            pnlOrderType = new System.Windows.Forms.FlowLayoutPanel();
            btnDineIn = new Guna.UI2.WinForms.Guna2Button();
            btnTakeOut = new Guna.UI2.WinForms.Guna2Button();

            pnlCustomerCard = new Guna.UI2.WinForms.Guna2Panel();
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

            pnlPaymentCard = new Guna.UI2.WinForms.Guna2Panel();
            lblPaymentLabel = new System.Windows.Forms.Label();
            lblTotalLabel = new System.Windows.Forms.Label();
            lblTotalValue = new System.Windows.Forms.Label();
            lblCashLabel = new System.Windows.Forms.Label();
            txtCash = new Guna.UI2.WinForms.Guna2TextBox();
            lblChangeLabel = new System.Windows.Forms.Label();
            lblChange = new System.Windows.Forms.Label();

            btnCancel = new Guna.UI2.WinForms.Guna2Button();
            btnConfirm = new Guna.UI2.WinForms.Guna2Button();

            SuspendLayout();

            // ── Header ────────────────────────────────────────────────
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Size = new System.Drawing.Size(660, 85);

            lblTitle.Location = new System.Drawing.Point(20, 20);
            lblTitle.Size = new System.Drawing.Size(300, 35);
            lblTitle.Text = "Complete Order";

            lblSubtitle.Location = new System.Drawing.Point(20, 55);
            lblSubtitle.Size = new System.Drawing.Size(400, 25);
            lblSubtitle.Text = "Review details and process payment";

            btnClose.Location = new System.Drawing.Point(614, 20);
            btnClose.Size = new System.Drawing.Size(36, 36);
            btnClose.Text = "✕";
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // ── Order Type Card ───────────────────────────────────────
            pnlOrderTypeCard.Location = new System.Drawing.Point(20, 100);
            pnlOrderTypeCard.Size = new System.Drawing.Size(620, 100);
            pnlOrderTypeCard.Controls.Add(lblOrderTypeLabel);
            pnlOrderTypeCard.Controls.Add(pnlOrderType);

            lblOrderTypeLabel.Location = new System.Drawing.Point(20, 15);
            lblOrderTypeLabel.Size = new System.Drawing.Size(200, 20);
            lblOrderTypeLabel.Text = "ORDER TYPE";

            pnlOrderType.Location = new System.Drawing.Point(20, 40);
            pnlOrderType.Size = new System.Drawing.Size(580, 44);
            pnlOrderType.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlOrderType.Controls.Add(btnDineIn);
            pnlOrderType.Controls.Add(btnTakeOut);

            btnDineIn.Size = new System.Drawing.Size(286, 40);
            btnDineIn.Text = "Dine-In";
            btnDineIn.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            btnTakeOut.Size = new System.Drawing.Size(286, 40);
            btnTakeOut.Text = "Take-Out";
            btnTakeOut.Margin = new System.Windows.Forms.Padding(0);

            // ── Customer Card ─────────────────────────────────────────
            pnlCustomerCard.Location = new System.Drawing.Point(20, 215);
            pnlCustomerCard.Size = new System.Drawing.Size(620, 215);
            pnlCustomerCard.Controls.Add(lblCustomerLabel);
            pnlCustomerCard.Controls.Add(txtCustomerSearch);
            pnlCustomerCard.Controls.Add(lblNewCustomerLabel);
            pnlCustomerCard.Controls.Add(txtFirstName);
            pnlCustomerCard.Controls.Add(txtLastName);
            pnlCustomerCard.Controls.Add(txtPhone);
            pnlCustomerCard.Controls.Add(txtNewEmail);
            pnlCustomerCard.Controls.Add(pnlSuggestions);

            lblCustomerLabel.Location = new System.Drawing.Point(20, 15);
            lblCustomerLabel.Size = new System.Drawing.Size(200, 20);
            lblCustomerLabel.Text = "CUSTOMER";

            txtCustomerSearch.Location = new System.Drawing.Point(20, 40);
            txtCustomerSearch.Size = new System.Drawing.Size(580, 40);
            txtCustomerSearch.PlaceholderText = "Enter Customer Name";

            // Suggestion overlay
            pnlSuggestions.Location = new System.Drawing.Point(20, 80);
            pnlSuggestions.Size = new System.Drawing.Size(580, 0);
            pnlSuggestions.Visible = false;
            pnlSuggestions.Controls.Add(lstSuggestions);

            lstSuggestions.Dock = System.Windows.Forms.DockStyle.Fill;
            lstSuggestions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lstSuggestions.ItemHeight = 30;
            lstSuggestions.IntegralHeight = false;

            lblNewCustomerLabel.Location = new System.Drawing.Point(20, 95);
            lblNewCustomerLabel.Size = new System.Drawing.Size(300, 18);
            lblNewCustomerLabel.Text = "Linked customer details:";

            txtFirstName.Location = new System.Drawing.Point(20, 118);
            txtFirstName.Size = new System.Drawing.Size(282, 38);
            txtFirstName.PlaceholderText = "First Name";

            txtLastName.Location = new System.Drawing.Point(318, 118);
            txtLastName.Size = new System.Drawing.Size(282, 38);
            txtLastName.PlaceholderText = "Last Name";

            txtPhone.Location = new System.Drawing.Point(20, 166);
            txtPhone.Size = new System.Drawing.Size(282, 38);
            txtPhone.PlaceholderText = "Phone (optional)";

            txtNewEmail.Location = new System.Drawing.Point(318, 166);
            txtNewEmail.Size = new System.Drawing.Size(282, 38);
            txtNewEmail.PlaceholderText = "Email (optional)";

            // ── Payment Card ──────────────────────────────────────────
            pnlPaymentCard.Location = new System.Drawing.Point(20, 445);
            pnlPaymentCard.Size = new System.Drawing.Size(620, 200);
            pnlPaymentCard.Controls.Add(lblPaymentLabel);
            pnlPaymentCard.Controls.Add(lblTotalLabel);
            pnlPaymentCard.Controls.Add(lblTotalValue);
            pnlPaymentCard.Controls.Add(lblCashLabel);
            pnlPaymentCard.Controls.Add(txtCash);
            pnlPaymentCard.Controls.Add(lblChangeLabel);
            pnlPaymentCard.Controls.Add(lblChange);

            lblPaymentLabel.Location = new System.Drawing.Point(20, 15);
            lblPaymentLabel.Size = new System.Drawing.Size(120, 20);
            lblPaymentLabel.Text = "PAYMENT";

            lblTotalLabel.Location = new System.Drawing.Point(20, 50);
            lblTotalLabel.Size = new System.Drawing.Size(120, 45);
            lblTotalLabel.Text = "Total Due";
            lblTotalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            lblTotalValue.Location = new System.Drawing.Point(340, 50);
            lblTotalValue.Size = new System.Drawing.Size(260, 45);
            lblTotalValue.Text = "\u20B10.00";
            lblTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            lblCashLabel.Location = new System.Drawing.Point(20, 100);
            lblCashLabel.Size = new System.Drawing.Size(150, 45);
            lblCashLabel.Text = "Cash Received";
            lblCashLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtCash.Location = new System.Drawing.Point(340, 100);
            txtCash.Size = new System.Drawing.Size(260, 42);
            txtCash.PlaceholderText = "0.00";

            lblChangeLabel.Location = new System.Drawing.Point(20, 150);
            lblChangeLabel.Size = new System.Drawing.Size(120, 45);
            lblChangeLabel.Text = "Change";
            lblChangeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            lblChange.Location = new System.Drawing.Point(340, 150);
            lblChange.Size = new System.Drawing.Size(260, 45);
            lblChange.Text = "\u20B10.00";
            lblChange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ── Validation Error ──────────────────────────────────────
            lblValidation.Location = new System.Drawing.Point(20, 655);
            lblValidation.Size = new System.Drawing.Size(620, 18);
            lblValidation.Text = "";
            lblValidation.Visible = false;
            lblValidation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ── Action Buttons ────────────────────────────────────────
            btnCancel.Location = new System.Drawing.Point(20, 680);
            btnCancel.Size = new System.Drawing.Size(160, 50);
            btnCancel.Text = "Cancel";

            btnConfirm.Location = new System.Drawing.Point(195, 680);
            btnConfirm.Size = new System.Drawing.Size(445, 50);
            btnConfirm.Text = "Confirm && Complete Sale";

            // ── Form ──────────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(660, 750);
            Controls.Add(lblValidation);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(pnlPaymentCard);
            Controls.Add(pnlCustomerCard);
            Controls.Add(pnlOrderTypeCard);
            Controls.Add(pnlHeader);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "CheckoutDialogForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Checkout";

            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        
        private Guna.UI2.WinForms.Guna2Panel pnlOrderTypeCard;
        private System.Windows.Forms.Label lblOrderTypeLabel;
        private System.Windows.Forms.FlowLayoutPanel pnlOrderType;
        private Guna.UI2.WinForms.Guna2Button btnDineIn;
        private Guna.UI2.WinForms.Guna2Button btnTakeOut;
        
        private Guna.UI2.WinForms.Guna2Panel pnlCustomerCard;
        private System.Windows.Forms.Label lblCustomerLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtCustomerSearch;
        private System.Windows.Forms.Panel pnlSuggestions;
        private System.Windows.Forms.ListBox lstSuggestions;
        private System.Windows.Forms.Label lblNewCustomerLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtFirstName;
        private Guna.UI2.WinForms.Guna2TextBox txtLastName;
        private Guna.UI2.WinForms.Guna2TextBox txtPhone;
        private Guna.UI2.WinForms.Guna2TextBox txtNewEmail;
        
        private Guna.UI2.WinForms.Guna2Panel pnlPaymentCard;
        private System.Windows.Forms.Label lblPaymentLabel;
        private System.Windows.Forms.Label lblTotalLabel;
        public System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblCashLabel;
        public Guna.UI2.WinForms.Guna2TextBox txtCash;
        private System.Windows.Forms.Label lblChangeLabel;
        public System.Windows.Forms.Label lblChange;
        
        private System.Windows.Forms.Label lblValidation;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private Guna.UI2.WinForms.Guna2Button btnConfirm;
    }
}
