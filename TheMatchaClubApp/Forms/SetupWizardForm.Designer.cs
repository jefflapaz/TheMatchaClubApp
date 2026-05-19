namespace TheMatchaClubApp.Forms
{
    partial class SetupWizardForm
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            // Top nav bar
            pnlTopNav = new System.Windows.Forms.Panel();
            pnlNavLogoCircle = new System.Windows.Forms.Panel();
            lblNavLogoText = new System.Windows.Forms.Label();
            lblBreadcrumb = new System.Windows.Forms.Label();
            lblBreadcrumbActive = new System.Windows.Forms.Label();
            pnlFirstLaunchPill = new Guna.UI2.WinForms.Guna2Panel();
            lblFirstLaunch = new System.Windows.Forms.Label();
            pnlNavAvatar = new System.Windows.Forms.Panel();

            // Stepper
            pnlStepper = new System.Windows.Forms.Panel();

            // Content card
            pnlContentCard = new Guna.UI2.WinForms.Guna2Panel();
            pnlCardHeader = new System.Windows.Forms.Panel();
            lblStepTitle = new System.Windows.Forms.Label();
            lblStepDesc = new System.Windows.Forms.Label();
            lblProgressLabel = new System.Windows.Forms.Label();
            lblProgressPercent = new System.Windows.Forms.Label();

            // Step panels
            pnlStep1 = new System.Windows.Forms.Panel();
            pnlStep2 = new System.Windows.Forms.Panel();
            pnlStep3 = new System.Windows.Forms.Panel();
            pnlStep4 = new System.Windows.Forms.Panel();

            // Step 1 controls
            lblStoreNameLabel = new System.Windows.Forms.Label();
            txtStoreName = new Guna.UI2.WinForms.Guna2TextBox();
            lblEmailLabel = new System.Windows.Forms.Label();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            lblAddressLabel = new System.Windows.Forms.Label();
            txtAddress = new Guna.UI2.WinForms.Guna2TextBox();
            pnlLogoUpload = new Guna.UI2.WinForms.Guna2Panel();
            lblLogoUploadText = new System.Windows.Forms.Label();
            lblTimezoneLabel = new System.Windows.Forms.Label();
            cboTimezone = new Guna.UI2.WinForms.Guna2ComboBox();

            // Step 2 controls
            pnlCsvUpload = new Guna.UI2.WinForms.Guna2Panel();
            lblCsvText = new System.Windows.Forms.Label();
            lblCurrencyLabel = new System.Windows.Forms.Label();
            cboCurrency = new Guna.UI2.WinForms.Guna2ComboBox();
            lblTaxRateLabel = new System.Windows.Forms.Label();
            txtTaxRate = new Guna.UI2.WinForms.Guna2TextBox();
            pnlNoCsvInfo = new Guna.UI2.WinForms.Guna2Panel();
            lblNoCsvText = new System.Windows.Forms.Label();

            // Step 3 controls
            lblPinLabel = new System.Windows.Forms.Label();
            txtPin = new Guna.UI2.WinForms.Guna2TextBox();
            lblConfirmPinLabel = new System.Windows.Forms.Label();
            txtConfirmPin = new Guna.UI2.WinForms.Guna2TextBox();
            lblPinHint = new System.Windows.Forms.Label();
            lblAutoLockLabel = new System.Windows.Forms.Label();
            cboAutoLock = new Guna.UI2.WinForms.Guna2ComboBox();
            lblToggleLabel = new System.Windows.Forms.Label();
            toggleDiscount = new Guna.UI2.WinForms.Guna2ToggleSwitch();

            // Step 4 controls
            pnlReviewIcon = new System.Windows.Forms.Panel();
            lblReviewTitle = new System.Windows.Forms.Label();
            lblReviewStore = new System.Windows.Forms.Label();
            lblReviewProducts = new System.Windows.Forms.Label();
            lblReviewSecurity = new System.Windows.Forms.Label();

            // Footer
            pnlFooterBar = new System.Windows.Forms.Panel();
            btnBack = new Guna.UI2.WinForms.Guna2Button();
            lblStepIndicator = new System.Windows.Forms.Label();
            btnNext = new Guna.UI2.WinForms.Guna2Button();
            btnComplete = new Guna.UI2.WinForms.Guna2Button();

            // Bottom footer
            lblSetupDate = new System.Windows.Forms.Label();
            lblKnowledgeBase = new System.Windows.Forms.Label();
            lblGetSupport = new System.Windows.Forms.Label();

            SuspendLayout();

            // ═══ Top Nav ═══
            pnlTopNav.Controls.Add(pnlNavLogoCircle);
            pnlTopNav.Controls.Add(lblNavLogoText);
            pnlTopNav.Controls.Add(lblBreadcrumb);
            pnlTopNav.Controls.Add(lblBreadcrumbActive);
            pnlTopNav.Controls.Add(pnlFirstLaunchPill);
            pnlTopNav.Controls.Add(pnlNavAvatar);
            pnlTopNav.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopNav.Size = new System.Drawing.Size(920, 64);

            pnlNavLogoCircle.Location = new System.Drawing.Point(20, 18);
            pnlNavLogoCircle.Size = new System.Drawing.Size(28, 28);
            lblNavLogoText.Location = new System.Drawing.Point(56, 18);
            lblNavLogoText.Size = new System.Drawing.Size(140, 28);
            lblNavLogoText.Text = "Matcha Caf\u00E9 POS";
            lblBreadcrumb.Location = new System.Drawing.Point(220, 22);
            lblBreadcrumb.Size = new System.Drawing.Size(100, 20);
            lblBreadcrumb.Text = "Configuration \u203A";
            lblBreadcrumbActive.Location = new System.Drawing.Point(326, 22);
            lblBreadcrumbActive.Size = new System.Drawing.Size(100, 20);
            lblBreadcrumbActive.Text = "Admin Setup";

            pnlFirstLaunchPill.Location = new System.Drawing.Point(700, 20);
            pnlFirstLaunchPill.Size = new System.Drawing.Size(120, 24);
            pnlFirstLaunchPill.Controls.Add(lblFirstLaunch);
            lblFirstLaunch.Dock = System.Windows.Forms.DockStyle.Fill;
            lblFirstLaunch.Text = "FIRST TIME LAUNCH";
            lblFirstLaunch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            pnlNavAvatar.Location = new System.Drawing.Point(830, 16);
            pnlNavAvatar.Size = new System.Drawing.Size(32, 32);

            // ═══ Stepper ═══
            pnlStepper.Location = new System.Drawing.Point(0, 64);
            pnlStepper.Size = new System.Drawing.Size(920, 100);
            pnlStepper.Dock = System.Windows.Forms.DockStyle.Top;

            // ═══ Content Card ═══
            pnlContentCard.Location = new System.Drawing.Point(40, 170);
            pnlContentCard.Size = new System.Drawing.Size(840, 380);
            pnlContentCard.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;

            pnlCardHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlCardHeader.Size = new System.Drawing.Size(840, 64);
            pnlCardHeader.Controls.Add(lblStepTitle);
            pnlCardHeader.Controls.Add(lblStepDesc);
            pnlCardHeader.Controls.Add(lblProgressLabel);
            pnlCardHeader.Controls.Add(lblProgressPercent);

            lblStepTitle.Location = new System.Drawing.Point(24, 12);
            lblStepTitle.Size = new System.Drawing.Size(300, 26);
            lblStepTitle.Text = "Store Identity";
            lblStepDesc.Location = new System.Drawing.Point(24, 38);
            lblStepDesc.Size = new System.Drawing.Size(400, 18);
            lblStepDesc.Text = "Set up your store's basic information";
            lblProgressLabel.Location = new System.Drawing.Point(680, 12);
            lblProgressLabel.Size = new System.Drawing.Size(100, 16);
            lblProgressLabel.Text = "SETUP PROGRESS";
            lblProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lblProgressPercent.Location = new System.Drawing.Point(700, 30);
            lblProgressPercent.Size = new System.Drawing.Size(80, 28);
            lblProgressPercent.Text = "25%";
            lblProgressPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            pnlContentCard.Controls.Add(pnlStep1);
            pnlContentCard.Controls.Add(pnlStep2);
            pnlContentCard.Controls.Add(pnlStep3);
            pnlContentCard.Controls.Add(pnlStep4);
            pnlContentCard.Controls.Add(pnlCardHeader);

            // Step 1
            pnlStep1.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlStep1.Padding = new System.Windows.Forms.Padding(24, 8, 24, 8);
            pnlStep1.Controls.Add(lblStoreNameLabel);
            pnlStep1.Controls.Add(txtStoreName);
            pnlStep1.Controls.Add(lblEmailLabel);
            pnlStep1.Controls.Add(txtEmail);
            pnlStep1.Controls.Add(lblAddressLabel);
            pnlStep1.Controls.Add(txtAddress);
            pnlStep1.Controls.Add(pnlLogoUpload);
            pnlStep1.Controls.Add(lblTimezoneLabel);
            pnlStep1.Controls.Add(cboTimezone);

            lblStoreNameLabel.Location = new System.Drawing.Point(24, 16);
            lblStoreNameLabel.Size = new System.Drawing.Size(100, 18);
            lblStoreNameLabel.Text = "Store Name";
            txtStoreName.Location = new System.Drawing.Point(24, 36);
            txtStoreName.Size = new System.Drawing.Size(360, 40);
            txtStoreName.PlaceholderText = "Matcha Caf\u00E9";

            lblEmailLabel.Location = new System.Drawing.Point(24, 86);
            lblEmailLabel.Size = new System.Drawing.Size(120, 18);
            lblEmailLabel.Text = "Contact Email";
            txtEmail.Location = new System.Drawing.Point(24, 106);
            txtEmail.Size = new System.Drawing.Size(360, 40);
            txtEmail.PlaceholderText = "hello@matchacafe.com";

            lblAddressLabel.Location = new System.Drawing.Point(24, 156);
            lblAddressLabel.Size = new System.Drawing.Size(120, 18);
            lblAddressLabel.Text = "Physical Address";
            txtAddress.Location = new System.Drawing.Point(24, 176);
            txtAddress.Size = new System.Drawing.Size(360, 40);
            txtAddress.PlaceholderText = "123 Green Tea Lane";

            pnlLogoUpload.Location = new System.Drawing.Point(420, 16);
            pnlLogoUpload.Size = new System.Drawing.Size(148, 142);
            pnlLogoUpload.Controls.Add(lblLogoUploadText);
            lblLogoUploadText.Dock = System.Windows.Forms.DockStyle.Fill;
            lblLogoUploadText.Text = "\U0001F4F7\nUpload Logo";
            lblLogoUploadText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            lblTimezoneLabel.Location = new System.Drawing.Point(420, 170);
            lblTimezoneLabel.Size = new System.Drawing.Size(100, 18);
            lblTimezoneLabel.Text = "Timezone";
            cboTimezone.Location = new System.Drawing.Point(420, 190);
            cboTimezone.Size = new System.Drawing.Size(200, 40);

            // Step 2
            pnlStep2.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlStep2.Padding = new System.Windows.Forms.Padding(24, 8, 24, 8);
            pnlStep2.Controls.Add(pnlCsvUpload);
            pnlStep2.Controls.Add(lblCurrencyLabel);
            pnlStep2.Controls.Add(cboCurrency);
            pnlStep2.Controls.Add(lblTaxRateLabel);
            pnlStep2.Controls.Add(txtTaxRate);
            pnlStep2.Controls.Add(pnlNoCsvInfo);

            pnlCsvUpload.Location = new System.Drawing.Point(24, 16);
            pnlCsvUpload.Size = new System.Drawing.Size(360, 140);
            pnlCsvUpload.Controls.Add(lblCsvText);
            lblCsvText.Dock = System.Windows.Forms.DockStyle.Fill;
            lblCsvText.Text = "\U0001F4CA\nDrag & Drop CSV\nor click to browse";
            lblCsvText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            lblCurrencyLabel.Location = new System.Drawing.Point(24, 170);
            lblCurrencyLabel.Size = new System.Drawing.Size(100, 18);
            lblCurrencyLabel.Text = "Currency";
            cboCurrency.Location = new System.Drawing.Point(24, 190);
            cboCurrency.Size = new System.Drawing.Size(200, 40);

            lblTaxRateLabel.Location = new System.Drawing.Point(420, 16);
            lblTaxRateLabel.Size = new System.Drawing.Size(120, 18);
            lblTaxRateLabel.Text = "Default Tax Rate";
            txtTaxRate.Location = new System.Drawing.Point(420, 36);
            txtTaxRate.Size = new System.Drawing.Size(200, 40);
            txtTaxRate.PlaceholderText = "8%";

            pnlNoCsvInfo.Location = new System.Drawing.Point(420, 90);
            pnlNoCsvInfo.Size = new System.Drawing.Size(200, 80);
            pnlNoCsvInfo.Controls.Add(lblNoCsvText);
            lblNoCsvText.Dock = System.Windows.Forms.DockStyle.Fill;
            lblNoCsvText.Text = "No CSV? No Problem.\nAdd products manually later.";
            lblNoCsvText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblNoCsvText.Padding = new System.Windows.Forms.Padding(8);

            // Step 3
            pnlStep3.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlStep3.Padding = new System.Windows.Forms.Padding(24, 8, 24, 8);
            pnlStep3.Controls.Add(lblPinLabel);
            pnlStep3.Controls.Add(txtPin);
            pnlStep3.Controls.Add(lblConfirmPinLabel);
            pnlStep3.Controls.Add(txtConfirmPin);
            pnlStep3.Controls.Add(lblPinHint);
            pnlStep3.Controls.Add(lblAutoLockLabel);
            pnlStep3.Controls.Add(cboAutoLock);
            pnlStep3.Controls.Add(lblToggleLabel);
            pnlStep3.Controls.Add(toggleDiscount);

            lblPinLabel.Location = new System.Drawing.Point(24, 16);
            lblPinLabel.Size = new System.Drawing.Size(120, 18);
            lblPinLabel.Text = "Manager PIN";
            txtPin.Location = new System.Drawing.Point(24, 36);
            txtPin.Size = new System.Drawing.Size(360, 40);
            txtPin.PlaceholderText = "Enter 4-6 digit PIN";
            txtPin.PasswordChar = '\u2022';

            lblConfirmPinLabel.Location = new System.Drawing.Point(24, 86);
            lblConfirmPinLabel.Size = new System.Drawing.Size(120, 18);
            lblConfirmPinLabel.Text = "Confirm PIN";
            txtConfirmPin.Location = new System.Drawing.Point(24, 106);
            txtConfirmPin.Size = new System.Drawing.Size(360, 40);
            txtConfirmPin.PlaceholderText = "Re-enter PIN";
            txtConfirmPin.PasswordChar = '\u2022';

            lblPinHint.Location = new System.Drawing.Point(24, 156);
            lblPinHint.Size = new System.Drawing.Size(360, 32);
            lblPinHint.Text = "Required for voids, refunds, and discounts over 20%";

            lblAutoLockLabel.Location = new System.Drawing.Point(420, 16);
            lblAutoLockLabel.Size = new System.Drawing.Size(120, 18);
            lblAutoLockLabel.Text = "Auto-lock After";
            cboAutoLock.Location = new System.Drawing.Point(420, 36);
            cboAutoLock.Size = new System.Drawing.Size(200, 40);

            lblToggleLabel.Location = new System.Drawing.Point(420, 100);
            lblToggleLabel.Size = new System.Drawing.Size(180, 18);
            lblToggleLabel.Text = "Require PIN for discounts";
            toggleDiscount.Location = new System.Drawing.Point(420, 124);
            toggleDiscount.Size = new System.Drawing.Size(50, 20);

            // Step 4
            pnlStep4.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlStep4.Controls.Add(pnlReviewIcon);
            pnlStep4.Controls.Add(lblReviewTitle);
            pnlStep4.Controls.Add(lblReviewStore);
            pnlStep4.Controls.Add(lblReviewProducts);
            pnlStep4.Controls.Add(lblReviewSecurity);

            pnlReviewIcon.Location = new System.Drawing.Point(340, 20);
            pnlReviewIcon.Size = new System.Drawing.Size(80, 80);
            lblReviewTitle.Location = new System.Drawing.Point(250, 110);
            lblReviewTitle.Size = new System.Drawing.Size(300, 28);
            lblReviewTitle.Text = "You're ready to launch!";
            lblReviewTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblReviewStore.Location = new System.Drawing.Point(200, 160);
            lblReviewStore.Size = new System.Drawing.Size(400, 20);
            lblReviewStore.Text = "Store Details: Configured \u2713";
            lblReviewStore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblReviewProducts.Location = new System.Drawing.Point(200, 184);
            lblReviewProducts.Size = new System.Drawing.Size(400, 20);
            lblReviewProducts.Text = "Products: Ready to add \u2713";
            lblReviewProducts.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblReviewSecurity.Location = new System.Drawing.Point(200, 208);
            lblReviewSecurity.Size = new System.Drawing.Size(400, 20);
            lblReviewSecurity.Text = "Security Level: Standard \u2713";
            lblReviewSecurity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Footer bar
            pnlFooterBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlFooterBar.Size = new System.Drawing.Size(920, 56);
            pnlFooterBar.Controls.Add(btnBack);
            pnlFooterBar.Controls.Add(lblStepIndicator);
            pnlFooterBar.Controls.Add(btnNext);
            pnlFooterBar.Controls.Add(btnComplete);

            btnBack.Location = new System.Drawing.Point(40, 12);
            btnBack.Size = new System.Drawing.Size(100, 36);
            btnBack.Text = "\u2190 Back";
            lblStepIndicator.Location = new System.Drawing.Point(400, 18);
            lblStepIndicator.Size = new System.Drawing.Size(120, 20);
            lblStepIndicator.Text = "Step 1 of 4";
            lblStepIndicator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            btnNext.Location = new System.Drawing.Point(740, 12);
            btnNext.Size = new System.Drawing.Size(140, 36);
            btnNext.Text = "Next Step \u2192";
            btnNext.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnComplete.Location = new System.Drawing.Point(720, 12);
            btnComplete.Size = new System.Drawing.Size(160, 36);
            btnComplete.Text = "Complete Setup \u2713";
            btnComplete.Visible = false;
            btnComplete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // Bottom footer
            lblSetupDate.Location = new System.Drawing.Point(40, 556);
            lblSetupDate.Size = new System.Drawing.Size(300, 20);
            lblSetupDate.Text = "\U0001F4C5 Initial Setup Date: April 25, 2026";
            lblSetupDate.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;

            lblKnowledgeBase.Location = new System.Drawing.Point(680, 556);
            lblKnowledgeBase.Size = new System.Drawing.Size(110, 20);
            lblKnowledgeBase.Text = "KNOWLEDGE BASE";
            lblKnowledgeBase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lblKnowledgeBase.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;

            lblGetSupport.Location = new System.Drawing.Point(800, 556);
            lblGetSupport.Size = new System.Drawing.Size(100, 20);
            lblGetSupport.Text = "GET SUPPORT";
            lblGetSupport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lblGetSupport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;

            // SetupWizardForm
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(920, 680);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Controls.Add(pnlContentCard);
            Controls.Add(pnlFooterBar);
            Controls.Add(pnlStepper);
            Controls.Add(pnlTopNav);
            Controls.Add(lblSetupDate);
            Controls.Add(lblKnowledgeBase);
            Controls.Add(lblGetSupport);
            Name = "SetupWizardForm";
            Text = "Setup Wizard";
            
            // ── App Icon (taskbar + title bar) ──
            var iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resources", "app.ico");
            if (!System.IO.File.Exists(iconPath))
                iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "app.ico");
            if (System.IO.File.Exists(iconPath))
                this.Icon = new Icon(iconPath);
                
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTopNav;
        private System.Windows.Forms.Panel pnlNavLogoCircle;
        private System.Windows.Forms.Label lblNavLogoText;
        private System.Windows.Forms.Label lblBreadcrumb;
        private System.Windows.Forms.Label lblBreadcrumbActive;
        private Guna.UI2.WinForms.Guna2Panel pnlFirstLaunchPill;
        private System.Windows.Forms.Label lblFirstLaunch;
        private System.Windows.Forms.Panel pnlNavAvatar;
        private System.Windows.Forms.Panel pnlStepper;
        private Guna.UI2.WinForms.Guna2Panel pnlContentCard;
        private System.Windows.Forms.Panel pnlCardHeader;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.Label lblStepDesc;
        private System.Windows.Forms.Label lblProgressLabel;
        private System.Windows.Forms.Label lblProgressPercent;
        private System.Windows.Forms.Panel pnlStep1;
        private System.Windows.Forms.Panel pnlStep2;
        private System.Windows.Forms.Panel pnlStep3;
        private System.Windows.Forms.Panel pnlStep4;
        private System.Windows.Forms.Label lblStoreNameLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtStoreName;
        private System.Windows.Forms.Label lblEmailLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private System.Windows.Forms.Label lblAddressLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtAddress;
        private Guna.UI2.WinForms.Guna2Panel pnlLogoUpload;
        private System.Windows.Forms.Label lblLogoUploadText;
        private System.Windows.Forms.Label lblTimezoneLabel;
        private Guna.UI2.WinForms.Guna2ComboBox cboTimezone;
        private Guna.UI2.WinForms.Guna2Panel pnlCsvUpload;
        private System.Windows.Forms.Label lblCsvText;
        private System.Windows.Forms.Label lblCurrencyLabel;
        private Guna.UI2.WinForms.Guna2ComboBox cboCurrency;
        private System.Windows.Forms.Label lblTaxRateLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtTaxRate;
        private Guna.UI2.WinForms.Guna2Panel pnlNoCsvInfo;
        private System.Windows.Forms.Label lblNoCsvText;
        private System.Windows.Forms.Label lblPinLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtPin;
        private System.Windows.Forms.Label lblConfirmPinLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmPin;
        private System.Windows.Forms.Label lblPinHint;
        private System.Windows.Forms.Label lblAutoLockLabel;
        private Guna.UI2.WinForms.Guna2ComboBox cboAutoLock;
        private System.Windows.Forms.Label lblToggleLabel;
        private Guna.UI2.WinForms.Guna2ToggleSwitch toggleDiscount;
        private System.Windows.Forms.Panel pnlReviewIcon;
        private System.Windows.Forms.Label lblReviewTitle;
        private System.Windows.Forms.Label lblReviewStore;
        private System.Windows.Forms.Label lblReviewProducts;
        private System.Windows.Forms.Label lblReviewSecurity;
        private System.Windows.Forms.Panel pnlFooterBar;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private System.Windows.Forms.Label lblStepIndicator;
        private Guna.UI2.WinForms.Guna2Button btnNext;
        private Guna.UI2.WinForms.Guna2Button btnComplete;
        private System.Windows.Forms.Label lblSetupDate;
        private System.Windows.Forms.Label lblKnowledgeBase;
        private System.Windows.Forms.Label lblGetSupport;
    }
}
