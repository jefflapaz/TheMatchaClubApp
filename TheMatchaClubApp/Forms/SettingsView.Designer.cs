namespace TheMatchaClubApp.Forms
{
    partial class SettingsView
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlTabSidebar = new System.Windows.Forms.Panel();
            flpTabs = new System.Windows.Forms.FlowLayoutPanel();
            pnlRightPanel = new System.Windows.Forms.Panel();

            // Right panel header
            lblSettingsTitle = new System.Windows.Forms.Label();
            btnSaveAll = new Guna.UI2.WinForms.Guna2Button();

            // Store Profile panel
            pnlStoreProfile = new System.Windows.Forms.Panel();
            pnlCard1 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard1Title = new System.Windows.Forms.Label();
            lblCard1Sub = new System.Windows.Forms.Label();
            pnlLogoUpload = new Guna.UI2.WinForms.Guna2Panel();
            lblUploadText = new System.Windows.Forms.Label();
            txtStoreName = new Guna.UI2.WinForms.Guna2TextBox();
            lblStoreNameLabel = new System.Windows.Forms.Label();
            txtTaxId = new Guna.UI2.WinForms.Guna2TextBox();
            lblTaxIdLabel = new System.Windows.Forms.Label();
            txtSupportEmail = new Guna.UI2.WinForms.Guna2TextBox();
            lblSupportEmailLabel = new System.Windows.Forms.Label();

            pnlCard2 = new Guna.UI2.WinForms.Guna2Panel();
            lblCard2Title = new System.Windows.Forms.Label();
            lblCard2Sub = new System.Windows.Forms.Label();
            txtAddress = new Guna.UI2.WinForms.Guna2TextBox();
            lblAddressLabel = new System.Windows.Forms.Label();
            txtCity = new Guna.UI2.WinForms.Guna2TextBox();
            lblCityLabel = new System.Windows.Forms.Label();
            txtPostalCode = new Guna.UI2.WinForms.Guna2TextBox();
            lblPostalLabel = new System.Windows.Forms.Label();
            txtPhone = new Guna.UI2.WinForms.Guna2TextBox();
            lblPhoneLabel = new System.Windows.Forms.Label();
            txtWebsite = new Guna.UI2.WinForms.Guna2TextBox();
            lblWebsiteLabel = new System.Windows.Forms.Label();

            // Placeholder panel
            pnlPlaceholder = new Guna.UI2.WinForms.Guna2Panel();
            lblPlaceholderText = new System.Windows.Forms.Label();

            SuspendLayout();

            // Tab sidebar
            pnlTabSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            pnlTabSidebar.Size = new System.Drawing.Size(224, 600);
            pnlTabSidebar.Controls.Add(flpTabs);

            flpTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            flpTabs.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpTabs.WrapContents = false;
            flpTabs.Padding = new System.Windows.Forms.Padding(8, 16, 8, 0);

            // Right panel
            pnlRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRightPanel.Controls.Add(pnlStoreProfile);
            pnlRightPanel.Controls.Add(pnlPlaceholder);
            pnlRightPanel.Controls.Add(lblSettingsTitle);
            pnlRightPanel.Controls.Add(btnSaveAll);

            lblSettingsTitle.Location = new System.Drawing.Point(24, 16);
            lblSettingsTitle.Size = new System.Drawing.Size(200, 30);
            lblSettingsTitle.Text = "System Settings";

            btnSaveAll.Location = new System.Drawing.Point(580, 16);
            btnSaveAll.Size = new System.Drawing.Size(140, 36);
            btnSaveAll.Text = "Save All Changes";
            btnSaveAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // Store Profile
            pnlStoreProfile.Location = new System.Drawing.Point(0, 56);
            pnlStoreProfile.Size = new System.Drawing.Size(780, 540);
            pnlStoreProfile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            pnlStoreProfile.AutoScroll = true;
            pnlStoreProfile.Controls.Add(pnlCard1);
            pnlStoreProfile.Controls.Add(pnlCard2);

            // Card 1 — Business Identity
            pnlCard1.Location = new System.Drawing.Point(24, 8);
            pnlCard1.Size = new System.Drawing.Size(720, 260);
            pnlCard1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnlCard1.Controls.Add(lblCard1Title);
            pnlCard1.Controls.Add(lblCard1Sub);
            pnlCard1.Controls.Add(pnlLogoUpload);
            pnlCard1.Controls.Add(lblStoreNameLabel);
            pnlCard1.Controls.Add(txtStoreName);
            pnlCard1.Controls.Add(lblTaxIdLabel);
            pnlCard1.Controls.Add(txtTaxId);
            pnlCard1.Controls.Add(lblSupportEmailLabel);
            pnlCard1.Controls.Add(txtSupportEmail);

            lblCard1Title.Location = new System.Drawing.Point(24, 20);
            lblCard1Title.Size = new System.Drawing.Size(200, 24);
            lblCard1Title.Text = "Business Identity";
            lblCard1Sub.Location = new System.Drawing.Point(24, 44);
            lblCard1Sub.Size = new System.Drawing.Size(500, 18);
            lblCard1Sub.Text = "This information will appear on reports and customer-facing interfaces.";

            pnlLogoUpload.Location = new System.Drawing.Point(24, 72);
            pnlLogoUpload.Size = new System.Drawing.Size(128, 128);
            pnlLogoUpload.Controls.Add(lblUploadText);
            lblUploadText.Dock = System.Windows.Forms.DockStyle.Fill;
            lblUploadText.Text = "\U0001F4F7\nUPLOAD\nPNG/JPG";
            lblUploadText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            lblStoreNameLabel.Location = new System.Drawing.Point(172, 72);
            lblStoreNameLabel.Size = new System.Drawing.Size(100, 18);
            lblStoreNameLabel.Text = "Store Name";
            txtStoreName.Location = new System.Drawing.Point(172, 92);
            txtStoreName.Size = new System.Drawing.Size(250, 40);
            txtStoreName.PlaceholderText = "Matcha Caf\u00E9";

            lblTaxIdLabel.Location = new System.Drawing.Point(440, 72);
            lblTaxIdLabel.Size = new System.Drawing.Size(100, 18);
            lblTaxIdLabel.Text = "Tax ID";
            txtTaxId.Location = new System.Drawing.Point(440, 92);
            txtTaxId.Size = new System.Drawing.Size(250, 40);
            txtTaxId.PlaceholderText = "XX-XXXXXXX";

            lblSupportEmailLabel.Location = new System.Drawing.Point(172, 148);
            lblSupportEmailLabel.Size = new System.Drawing.Size(120, 18);
            lblSupportEmailLabel.Text = "Support Email";
            txtSupportEmail.Location = new System.Drawing.Point(172, 168);
            txtSupportEmail.Size = new System.Drawing.Size(518, 40);
            txtSupportEmail.PlaceholderText = "support@matchacafe.com";

            // Card 2 — Contact & Location
            pnlCard2.Location = new System.Drawing.Point(24, 280);
            pnlCard2.Size = new System.Drawing.Size(720, 240);
            pnlCard2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnlCard2.Controls.Add(lblCard2Title);
            pnlCard2.Controls.Add(lblCard2Sub);
            pnlCard2.Controls.Add(lblAddressLabel);
            pnlCard2.Controls.Add(txtAddress);
            pnlCard2.Controls.Add(lblCityLabel);
            pnlCard2.Controls.Add(txtCity);
            pnlCard2.Controls.Add(lblPostalLabel);
            pnlCard2.Controls.Add(txtPostalCode);
            pnlCard2.Controls.Add(lblPhoneLabel);
            pnlCard2.Controls.Add(txtPhone);
            pnlCard2.Controls.Add(lblWebsiteLabel);
            pnlCard2.Controls.Add(txtWebsite);

            lblCard2Title.Location = new System.Drawing.Point(24, 20);
            lblCard2Title.Size = new System.Drawing.Size(200, 24);
            lblCard2Title.Text = "Contact & Location";
            lblCard2Sub.Location = new System.Drawing.Point(24, 44);
            lblCard2Sub.Size = new System.Drawing.Size(500, 18);
            lblCard2Sub.Text = "Physical location and contact details for your store.";

            lblAddressLabel.Location = new System.Drawing.Point(24, 72);
            lblAddressLabel.Size = new System.Drawing.Size(120, 18);
            lblAddressLabel.Text = "Address Line 1";
            txtAddress.Location = new System.Drawing.Point(24, 92);
            txtAddress.Size = new System.Drawing.Size(666, 40);
            txtAddress.PlaceholderText = "123 Green Tea Lane, Suite 4B";

            lblCityLabel.Location = new System.Drawing.Point(24, 140);
            lblCityLabel.Size = new System.Drawing.Size(80, 18);
            lblCityLabel.Text = "City";
            txtCity.Location = new System.Drawing.Point(24, 160);
            txtCity.Size = new System.Drawing.Size(320, 40);
            txtCity.PlaceholderText = "Portland";

            lblPostalLabel.Location = new System.Drawing.Point(370, 140);
            lblPostalLabel.Size = new System.Drawing.Size(100, 18);
            lblPostalLabel.Text = "Postal Code";
            txtPostalCode.Location = new System.Drawing.Point(370, 160);
            txtPostalCode.Size = new System.Drawing.Size(320, 40);
            txtPostalCode.PlaceholderText = "97201";

            lblPhoneLabel.Location = new System.Drawing.Point(24, 208);
            lblPhoneLabel.Size = new System.Drawing.Size(120, 18);
            lblPhoneLabel.Text = "Phone Number";
            txtPhone.Location = new System.Drawing.Point(24, 228);
            txtPhone.Size = new System.Drawing.Size(320, 40);
            txtPhone.PlaceholderText = "+1 (555) 000-0000";
            txtPhone.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;

            lblWebsiteLabel.Location = new System.Drawing.Point(370, 208);
            lblWebsiteLabel.Size = new System.Drawing.Size(100, 18);
            lblWebsiteLabel.Text = "Website URL";
            txtWebsite.Location = new System.Drawing.Point(370, 228);
            txtWebsite.Size = new System.Drawing.Size(320, 40);
            txtWebsite.PlaceholderText = "https://matchacafe.com";

            // Placeholder panel
            pnlPlaceholder.Location = new System.Drawing.Point(0, 56);
            pnlPlaceholder.Size = new System.Drawing.Size(780, 540);
            pnlPlaceholder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            pnlPlaceholder.Visible = false;
            pnlPlaceholder.Controls.Add(lblPlaceholderText);
            lblPlaceholderText.Dock = System.Windows.Forms.DockStyle.Fill;
            lblPlaceholderText.Text = "Configuration panel loading...";
            lblPlaceholderText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // SettingsView
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlRightPanel);
            Controls.Add(pnlTabSidebar);
            Name = "SettingsView";
            Size = new System.Drawing.Size(1004, 600);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTabSidebar;
        private System.Windows.Forms.FlowLayoutPanel flpTabs;
        private System.Windows.Forms.Panel pnlRightPanel;
        private System.Windows.Forms.Label lblSettingsTitle;
        private Guna.UI2.WinForms.Guna2Button btnSaveAll;
        private System.Windows.Forms.Panel pnlStoreProfile;
        private Guna.UI2.WinForms.Guna2Panel pnlCard1;
        private System.Windows.Forms.Label lblCard1Title;
        private System.Windows.Forms.Label lblCard1Sub;
        private Guna.UI2.WinForms.Guna2Panel pnlLogoUpload;
        private System.Windows.Forms.Label lblUploadText;
        private Guna.UI2.WinForms.Guna2TextBox txtStoreName;
        private System.Windows.Forms.Label lblStoreNameLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtTaxId;
        private System.Windows.Forms.Label lblTaxIdLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtSupportEmail;
        private System.Windows.Forms.Label lblSupportEmailLabel;
        private Guna.UI2.WinForms.Guna2Panel pnlCard2;
        private System.Windows.Forms.Label lblCard2Title;
        private System.Windows.Forms.Label lblCard2Sub;
        private Guna.UI2.WinForms.Guna2TextBox txtAddress;
        private System.Windows.Forms.Label lblAddressLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtCity;
        private System.Windows.Forms.Label lblCityLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtPostalCode;
        private System.Windows.Forms.Label lblPostalLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtPhone;
        private System.Windows.Forms.Label lblPhoneLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtWebsite;
        private System.Windows.Forms.Label lblWebsiteLabel;
        private Guna.UI2.WinForms.Guna2Panel pnlPlaceholder;
        private System.Windows.Forms.Label lblPlaceholderText;
    }
}
