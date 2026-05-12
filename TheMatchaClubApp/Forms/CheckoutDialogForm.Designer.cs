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

            // Order Type section
            lblOrderTypeLabel = new System.Windows.Forms.Label();
            pnlOrderType = new System.Windows.Forms.FlowLayoutPanel();
            btnDineIn = new Guna.UI2.WinForms.Guna2Button();
            btnTakeOut = new Guna.UI2.WinForms.Guna2Button();

            // Customer section
            lblCustomerLabel = new System.Windows.Forms.Label();
            cboCustomer = new Guna.UI2.WinForms.Guna2ComboBox();
            lblNewNameLabel = new System.Windows.Forms.Label();
            txtNewName = new Guna.UI2.WinForms.Guna2TextBox();
            lblNewEmailLabel = new System.Windows.Forms.Label();
            txtNewEmail = new Guna.UI2.WinForms.Guna2TextBox();

            // Confirm button
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
            pnlOrderType.Size = new System.Drawing.Size(380, 48);
            pnlOrderType.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlOrderType.Controls.Add(btnDineIn);
            pnlOrderType.Controls.Add(btnTakeOut);

            btnDineIn.Size = new System.Drawing.Size(180, 40);
            btnDineIn.Text = "🍽  Dine-In";
            btnDineIn.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);

            btnTakeOut.Size = new System.Drawing.Size(180, 40);
            btnTakeOut.Text = "📦  Take-Out";

            // ── Customer ──────────────────────────────────────────────
            lblCustomerLabel.Location = new System.Drawing.Point(20, 145);
            lblCustomerLabel.Size = new System.Drawing.Size(200, 20);
            lblCustomerLabel.Text = "LINK CUSTOMER";

            cboCustomer.Location = new System.Drawing.Point(20, 170);
            cboCustomer.Size = new System.Drawing.Size(380, 42);

            lblNewNameLabel.Location = new System.Drawing.Point(20, 225);
            lblNewNameLabel.Size = new System.Drawing.Size(180, 20);
            lblNewNameLabel.Text = "Or enter new customer:";

            txtNewName.Location = new System.Drawing.Point(20, 248);
            txtNewName.Size = new System.Drawing.Size(185, 42);
            txtNewName.PlaceholderText = "Customer Name";

            lblNewEmailLabel.Location = new System.Drawing.Point(215, 225);
            lblNewEmailLabel.Size = new System.Drawing.Size(180, 20);
            lblNewEmailLabel.Text = "";

            txtNewEmail.Location = new System.Drawing.Point(215, 248);
            txtNewEmail.Size = new System.Drawing.Size(185, 42);
            txtNewEmail.PlaceholderText = "Email (optional)";

            // ── Confirm ───────────────────────────────────────────────
            btnConfirm.Location = new System.Drawing.Point(20, 310);
            btnConfirm.Size = new System.Drawing.Size(380, 50);
            btnConfirm.Text = "✓  Confirm & Complete Sale";

            // ── Form ──────────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(420, 380);
            Controls.Add(btnConfirm);
            Controls.Add(txtNewEmail);
            Controls.Add(lblNewEmailLabel);
            Controls.Add(txtNewName);
            Controls.Add(lblNewNameLabel);
            Controls.Add(cboCustomer);
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
        private Guna.UI2.WinForms.Guna2ComboBox cboCustomer;
        private System.Windows.Forms.Label lblNewNameLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtNewName;
        private System.Windows.Forms.Label lblNewEmailLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtNewEmail;
        private Guna.UI2.WinForms.Guna2Button btnConfirm;
    }
}
