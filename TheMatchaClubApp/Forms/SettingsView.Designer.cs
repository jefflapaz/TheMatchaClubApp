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
            lblSettingsTitle = new System.Windows.Forms.Label();
            lblSupportEmailLabel = new System.Windows.Forms.Label();
            btnSaveAll = new Guna.UI2.WinForms.Guna2Button();

            // Section panels
            pnlStoreProfile = new System.Windows.Forms.Panel();
            pnlSessionCash = new System.Windows.Forms.Panel();
            pnlReceiptEditor = new System.Windows.Forms.Panel();

            pnlExportBackup = new System.Windows.Forms.Panel();
            pnlSecurity = new System.Windows.Forms.Panel();

            // ── Store Profile controls ──
            pnlCardProfile = new Guna.UI2.WinForms.Guna2Panel();
            lblCardProfileTitle = new System.Windows.Forms.Label();
            lblCardProfileSub = new System.Windows.Forms.Label();
            pnlLogoUpload = new Guna.UI2.WinForms.Guna2Panel();
            lblUploadText = new System.Windows.Forms.Label();
            txtStoreName = new Guna.UI2.WinForms.Guna2TextBox();
            lblStoreNameLabel = new System.Windows.Forms.Label();
            txtSupportEmail = new Guna.UI2.WinForms.Guna2TextBox();
            lblSupportEmailLabel = new System.Windows.Forms.Label();
            txtPhone = new Guna.UI2.WinForms.Guna2TextBox();
            lblPhoneLabel = new System.Windows.Forms.Label();
            txtReceiptFooter = new Guna.UI2.WinForms.Guna2TextBox();
            lblReceiptFooterLabel = new System.Windows.Forms.Label();

            pnlCardLocation = new Guna.UI2.WinForms.Guna2Panel();
            lblCardLocationTitle = new System.Windows.Forms.Label();
            lblCardLocationSub = new System.Windows.Forms.Label();
            txtPopupLocation = new Guna.UI2.WinForms.Guna2TextBox();
            lblPopupLocationLabel = new System.Windows.Forms.Label();
            txtOperatingLocation = new Guna.UI2.WinForms.Guna2TextBox();
            lblOperatingLocationLabel = new System.Windows.Forms.Label();

            pnlCardSmtp = new Guna.UI2.WinForms.Guna2Panel();
            lblCardSmtpTitle = new System.Windows.Forms.Label();
            lblCardSmtpSub = new System.Windows.Forms.Label();
            txtSmtpServer = new Guna.UI2.WinForms.Guna2TextBox();
            lblSmtpServerLabel = new System.Windows.Forms.Label();
            txtSmtpPort = new Guna.UI2.WinForms.Guna2TextBox();
            lblSmtpPortLabel = new System.Windows.Forms.Label();
            txtSmtpPassword = new Guna.UI2.WinForms.Guna2TextBox();
            lblSmtpPasswordLabel = new System.Windows.Forms.Label();

            // ── Session & Cash controls ──
            pnlCardSession = new Guna.UI2.WinForms.Guna2Panel();
            lblCardSessionTitle = new System.Windows.Forms.Label();
            lblCardSessionSub = new System.Windows.Forms.Label();
            txtDefaultCash = new Guna.UI2.WinForms.Guna2TextBox();
            lblDefaultCashLabel = new System.Windows.Forms.Label();
            txtSessionTimeout = new Guna.UI2.WinForms.Guna2TextBox();
            lblSessionTimeoutLabel = new System.Windows.Forms.Label();
            chkRequireCashCount = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            lblRequireCashCount = new System.Windows.Forms.Label();
            chkOverShortWarnings = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            lblOverShortWarnings = new System.Windows.Forms.Label();
            chkAutoZReport = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            lblAutoZReport = new System.Windows.Forms.Label();
            chkAutoLockQuickSale = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            lblAutoLockQuickSale = new System.Windows.Forms.Label();
            lblDefaultCashHelp = new System.Windows.Forms.Label();
            lblSessionTimeoutHelp = new System.Windows.Forms.Label();
            lblRequireCashCountHelp = new System.Windows.Forms.Label();
            lblOverShortWarningsHelp = new System.Windows.Forms.Label();
            lblAutoZReportHelp = new System.Windows.Forms.Label();
            lblAutoLockQuickSaleHelp = new System.Windows.Forms.Label();

            SuspendLayout();

            // ── Tab sidebar ──
            pnlTabSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            pnlTabSidebar.Size = new System.Drawing.Size(210, 600);
            pnlTabSidebar.Controls.Add(flpTabs);
            flpTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            flpTabs.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpTabs.WrapContents = false;
            flpTabs.Padding = new System.Windows.Forms.Padding(6, 12, 6, 0);

            // ── Right panel ──
            pnlRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRightPanel.Controls.Add(pnlStoreProfile);
            pnlRightPanel.Controls.Add(pnlSessionCash);
            pnlRightPanel.Controls.Add(pnlReceiptEditor);

            pnlRightPanel.Controls.Add(pnlExportBackup);
            pnlRightPanel.Controls.Add(pnlSecurity);
            pnlRightPanel.Controls.Add(lblSettingsTitle);
            pnlRightPanel.Controls.Add(btnSaveAll);

            lblSettingsTitle.Location = new System.Drawing.Point(28, 14);
            lblSettingsTitle.Size = new System.Drawing.Size(400, 34);
            lblSettingsTitle.Text = "System Settings";

            btnSaveAll.Size = new System.Drawing.Size(150, 38);
            btnSaveAll.Text = "💾  Save Changes";
            btnSaveAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // ── Section panels (all share same layout) ──
            var sectionStyle = new System.Action<System.Windows.Forms.Panel>(p =>
            {
                p.Location = new System.Drawing.Point(0, 58);
                p.Dock = System.Windows.Forms.DockStyle.None;
                p.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
                p.AutoScroll = true;
                p.Visible = false;
                p.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            });
            sectionStyle(pnlStoreProfile); pnlStoreProfile.Visible = true;
            sectionStyle(pnlSessionCash);
            sectionStyle(pnlReceiptEditor);

            sectionStyle(pnlExportBackup);
            sectionStyle(pnlSecurity);

            // ══════════════════════════════════════════════════════════
            //  STORE PROFILE SECTION
            // ══════════════════════════════════════════════════════════

            // Card: Business Identity
            pnlCardProfile.Location = new System.Drawing.Point(24, 8);
            pnlCardProfile.Size = new System.Drawing.Size(720, 260);
            pnlCardProfile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            pnlCardProfile.Controls.Add(lblCardProfileTitle);
            pnlCardProfile.Controls.Add(lblCardProfileSub);
            pnlCardProfile.Controls.Add(pnlLogoUpload);
            pnlCardProfile.Controls.Add(lblStoreNameLabel); pnlCardProfile.Controls.Add(txtStoreName);
            pnlCardProfile.Controls.Add(lblSupportEmailLabel); pnlCardProfile.Controls.Add(txtSupportEmail);
            pnlCardProfile.Controls.Add(lblPhoneLabel); pnlCardProfile.Controls.Add(txtPhone);

            lblCardProfileTitle.Location = new System.Drawing.Point(24, 20);
            lblCardProfileTitle.Size = new System.Drawing.Size(300, 24);
            lblCardProfileTitle.Text = "Business Identity";
            lblCardProfileSub.Location = new System.Drawing.Point(24, 44);
            lblCardProfileSub.Size = new System.Drawing.Size(500, 18);
            lblCardProfileSub.Text = "Core information displayed on receipts, reports, and customer-facing UI.";

            pnlLogoUpload.Location = new System.Drawing.Point(24, 72);
            pnlLogoUpload.Size = new System.Drawing.Size(128, 128);
            pnlLogoUpload.Controls.Add(lblUploadText);
            lblUploadText.Dock = System.Windows.Forms.DockStyle.Fill;
            lblUploadText.Text = "\U0001F4F7\nUPLOAD\nPNG/JPG";
            lblUploadText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            lblStoreNameLabel.Location = new System.Drawing.Point(172, 72); lblStoreNameLabel.Size = new System.Drawing.Size(100, 18); lblStoreNameLabel.Text = "Store Name";
            txtStoreName.Location = new System.Drawing.Point(172, 92); txtStoreName.Size = new System.Drawing.Size(250, 40); txtStoreName.PlaceholderText = "The Matcha Club";

            lblSupportEmailLabel.Location = new System.Drawing.Point(440, 72); lblSupportEmailLabel.Size = new System.Drawing.Size(120, 18); lblSupportEmailLabel.Text = "Support Email";
            txtSupportEmail.Location = new System.Drawing.Point(440, 92); txtSupportEmail.Size = new System.Drawing.Size(250, 40); txtSupportEmail.PlaceholderText = "info@thematchaclub.ph";

            lblPhoneLabel.Location = new System.Drawing.Point(172, 148); lblPhoneLabel.Size = new System.Drawing.Size(120, 18); lblPhoneLabel.Text = "Phone Number";
            txtPhone.Location = new System.Drawing.Point(172, 168); txtPhone.Size = new System.Drawing.Size(250, 40); txtPhone.PlaceholderText = "+63 912 345 6789";

            // Card: Pop-up Location
            pnlCardLocation.Location = new System.Drawing.Point(24, 284);
            pnlCardLocation.Size = new System.Drawing.Size(720, 180);
            pnlCardLocation.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            pnlCardLocation.Controls.Add(lblCardLocationTitle); pnlCardLocation.Controls.Add(lblCardLocationSub);
            pnlCardLocation.Controls.Add(lblPopupLocationLabel); pnlCardLocation.Controls.Add(txtPopupLocation);
            pnlCardLocation.Controls.Add(lblOperatingLocationLabel); pnlCardLocation.Controls.Add(txtOperatingLocation);

            lblCardLocationTitle.Location = new System.Drawing.Point(24, 20); lblCardLocationTitle.Size = new System.Drawing.Size(300, 24); lblCardLocationTitle.Text = "Pop-up / Mobile Location";
            lblCardLocationSub.Location = new System.Drawing.Point(24, 44); lblCardLocationSub.Size = new System.Drawing.Size(500, 18); lblCardLocationSub.Text = "Appears dynamically on receipts, session reports, and PDF exports.";

            lblPopupLocationLabel.Location = new System.Drawing.Point(24, 72); lblPopupLocationLabel.Size = new System.Drawing.Size(160, 18); lblPopupLocationLabel.Text = "Pop-up Location Name";
            txtPopupLocation.Location = new System.Drawing.Point(24, 92); txtPopupLocation.Size = new System.Drawing.Size(320, 40); txtPopupLocation.PlaceholderText = "e.g. Ayala Mall Pop-up Booth";

            lblOperatingLocationLabel.Location = new System.Drawing.Point(370, 72); lblOperatingLocationLabel.Size = new System.Drawing.Size(180, 18); lblOperatingLocationLabel.Text = "Current Operating Location";
            txtOperatingLocation.Location = new System.Drawing.Point(370, 92); txtOperatingLocation.Size = new System.Drawing.Size(320, 40); txtOperatingLocation.PlaceholderText = "e.g. Makati City, Metro Manila";

            // Card: SMTP
            pnlCardSmtp.Location = new System.Drawing.Point(24, 480);
            pnlCardSmtp.Size = new System.Drawing.Size(720, 200);
            pnlCardSmtp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            pnlCardSmtp.Controls.Add(lblCardSmtpTitle); pnlCardSmtp.Controls.Add(lblCardSmtpSub);
            pnlCardSmtp.Controls.Add(lblSmtpServerLabel); pnlCardSmtp.Controls.Add(txtSmtpServer);
            pnlCardSmtp.Controls.Add(lblSmtpPortLabel); pnlCardSmtp.Controls.Add(txtSmtpPort);
            pnlCardSmtp.Controls.Add(lblSmtpPasswordLabel); pnlCardSmtp.Controls.Add(txtSmtpPassword);

            lblCardSmtpTitle.Location = new System.Drawing.Point(24, 20); lblCardSmtpTitle.Size = new System.Drawing.Size(300, 24); lblCardSmtpTitle.Text = "Email Configuration (SMTP)";
            lblCardSmtpSub.Location = new System.Drawing.Point(24, 44); lblCardSmtpSub.Size = new System.Drawing.Size(500, 18); lblCardSmtpSub.Text = "Used for sending email receipts to customers. Uses your Support Email as the sender address.";

            lblSmtpServerLabel.Location = new System.Drawing.Point(24, 72); lblSmtpServerLabel.Size = new System.Drawing.Size(100, 18); lblSmtpServerLabel.Text = "SMTP Server";
            txtSmtpServer.Location = new System.Drawing.Point(24, 92); txtSmtpServer.Size = new System.Drawing.Size(320, 40); txtSmtpServer.PlaceholderText = "smtp.gmail.com";

            lblSmtpPortLabel.Location = new System.Drawing.Point(370, 72); lblSmtpPortLabel.Size = new System.Drawing.Size(100, 18); lblSmtpPortLabel.Text = "SMTP Port";
            txtSmtpPort.Location = new System.Drawing.Point(370, 92); txtSmtpPort.Size = new System.Drawing.Size(120, 40); txtSmtpPort.PlaceholderText = "587";

            lblSmtpPasswordLabel.Location = new System.Drawing.Point(24, 140); lblSmtpPasswordLabel.Size = new System.Drawing.Size(150, 18); lblSmtpPasswordLabel.Text = "SMTP App Password";
            txtSmtpPassword.Location = new System.Drawing.Point(24, 160); txtSmtpPassword.Size = new System.Drawing.Size(320, 40); txtSmtpPassword.PlaceholderText = "••••••••"; txtSmtpPassword.UseSystemPasswordChar = true;

            pnlStoreProfile.Controls.Add(pnlCardProfile);
            pnlStoreProfile.Controls.Add(pnlCardLocation);
            pnlStoreProfile.Controls.Add(pnlCardSmtp);

            // ══════════════════════════════════════════════════════════
            //  SESSION & CASH SECTION
            // ══════════════════════════════════════════════════════════
            pnlCardSession.Location = new System.Drawing.Point(24, 8);
            pnlCardSession.Size = new System.Drawing.Size(720, 500);
            pnlCardSession.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;

            lblCardSessionTitle.Location = new System.Drawing.Point(24, 20); lblCardSessionTitle.Size = new System.Drawing.Size(300, 24); lblCardSessionTitle.Text = "Session & Cash Management";
            lblCardSessionSub.Location = new System.Drawing.Point(24, 44); lblCardSessionSub.Size = new System.Drawing.Size(600, 18); lblCardSessionSub.Text = "Control how store sessions, cash handling, and closing procedures behave.";

            lblDefaultCashLabel.Location = new System.Drawing.Point(24, 76); lblDefaultCashLabel.Size = new System.Drawing.Size(160, 18); lblDefaultCashLabel.Text = "Default Starting Cash (₱)";
            txtDefaultCash.Location = new System.Drawing.Point(24, 96); txtDefaultCash.Size = new System.Drawing.Size(200, 40); txtDefaultCash.PlaceholderText = "200.00";
            lblDefaultCashHelp.Location = new System.Drawing.Point(24, 140); lblDefaultCashHelp.Size = new System.Drawing.Size(220, 32); lblDefaultCashHelp.Text = "Initial cash float amount set on opening new store sessions.";

            lblSessionTimeoutLabel.Location = new System.Drawing.Point(260, 76); lblSessionTimeoutLabel.Size = new System.Drawing.Size(200, 18); lblSessionTimeoutLabel.Text = "Session Timeout (min, 0=off)";
            txtSessionTimeout.Location = new System.Drawing.Point(260, 96); txtSessionTimeout.Size = new System.Drawing.Size(120, 40); txtSessionTimeout.PlaceholderText = "0";
            lblSessionTimeoutHelp.Location = new System.Drawing.Point(260, 140); lblSessionTimeoutHelp.Size = new System.Drawing.Size(220, 32); lblSessionTimeoutHelp.Text = "Minutes of inactivity before locking POS. Set to 0 to disable.";

            // Toggle rows
            int toggleY = 190;
            int toggleGap = 72;

            lblRequireCashCount.Location = new System.Drawing.Point(24, toggleY); lblRequireCashCount.Size = new System.Drawing.Size(500, 20); lblRequireCashCount.Text = "Require actual cash count before closing session";
            chkRequireCashCount.Location = new System.Drawing.Point(620, toggleY - 2); chkRequireCashCount.Size = new System.Drawing.Size(60, 26);
            lblRequireCashCountHelp.Location = new System.Drawing.Point(24, toggleY + 22); lblRequireCashCountHelp.Size = new System.Drawing.Size(580, 18); lblRequireCashCountHelp.Text = "Forces cashier to physically count and input drawer cash before session shutdown.";

            toggleY += toggleGap;
            lblOverShortWarnings.Location = new System.Drawing.Point(24, toggleY); lblOverShortWarnings.Size = new System.Drawing.Size(500, 20); lblOverShortWarnings.Text = "Enable Over/Short cash warnings";
            chkOverShortWarnings.Location = new System.Drawing.Point(620, toggleY - 2); chkOverShortWarnings.Size = new System.Drawing.Size(60, 26);
            lblOverShortWarningsHelp.Location = new System.Drawing.Point(24, toggleY + 22); lblOverShortWarningsHelp.Size = new System.Drawing.Size(580, 18); lblOverShortWarningsHelp.Text = "Prompts a verification warning if entered cash differs from calculated expected cash.";

            toggleY += toggleGap;
            lblAutoZReport.Location = new System.Drawing.Point(24, toggleY); lblAutoZReport.Size = new System.Drawing.Size(500, 20); lblAutoZReport.Text = "Auto-generate Z-report after closing session";
            chkAutoZReport.Location = new System.Drawing.Point(620, toggleY - 2); chkAutoZReport.Size = new System.Drawing.Size(60, 26);
            lblAutoZReportHelp.Location = new System.Drawing.Point(24, toggleY + 22); lblAutoZReportHelp.Size = new System.Drawing.Size(580, 18); lblAutoZReportHelp.Text = "Automatically compiles, saves, and opens the session performance report PDF.";

            toggleY += toggleGap;
            lblAutoLockQuickSale.Location = new System.Drawing.Point(24, toggleY); lblAutoLockQuickSale.Size = new System.Drawing.Size(500, 20); lblAutoLockQuickSale.Text = "Auto-lock Quick Sale if no active session";
            chkAutoLockQuickSale.Location = new System.Drawing.Point(620, toggleY - 2); chkAutoLockQuickSale.Size = new System.Drawing.Size(60, 26);
            lblAutoLockQuickSaleHelp.Location = new System.Drawing.Point(24, toggleY + 22); lblAutoLockQuickSaleHelp.Size = new System.Drawing.Size(580, 18); lblAutoLockQuickSaleHelp.Text = "Blocks order processing and locks down Quick Sale features if no session is active.";

            pnlCardSession.Controls.Add(lblCardSessionTitle); pnlCardSession.Controls.Add(lblCardSessionSub);
            pnlCardSession.Controls.Add(lblDefaultCashLabel); pnlCardSession.Controls.Add(txtDefaultCash); pnlCardSession.Controls.Add(lblDefaultCashHelp);
            pnlCardSession.Controls.Add(lblSessionTimeoutLabel); pnlCardSession.Controls.Add(txtSessionTimeout); pnlCardSession.Controls.Add(lblSessionTimeoutHelp);
            pnlCardSession.Controls.Add(lblRequireCashCount); pnlCardSession.Controls.Add(chkRequireCashCount); pnlCardSession.Controls.Add(lblRequireCashCountHelp);
            pnlCardSession.Controls.Add(lblOverShortWarnings); pnlCardSession.Controls.Add(chkOverShortWarnings); pnlCardSession.Controls.Add(lblOverShortWarningsHelp);
            pnlCardSession.Controls.Add(lblAutoZReport); pnlCardSession.Controls.Add(chkAutoZReport); pnlCardSession.Controls.Add(lblAutoZReportHelp);
            pnlCardSession.Controls.Add(lblAutoLockQuickSale); pnlCardSession.Controls.Add(chkAutoLockQuickSale); pnlCardSession.Controls.Add(lblAutoLockQuickSaleHelp);
            pnlSessionCash.Controls.Add(pnlCardSession);

            // ══════════════════════════════════════════════════════════
            //  RECEIPT EDITOR SECTION
            // ══════════════════════════════════════════════════════════
            pnlCardReceipt = new Guna.UI2.WinForms.Guna2Panel();
            lblCardReceiptTitle = new System.Windows.Forms.Label();
            lblCardReceiptSub = new System.Windows.Forms.Label();
            chkShowCashier = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            lblShowCashier = new System.Windows.Forms.Label();
            chkShowCustomer = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            lblShowCustomer = new System.Windows.Forms.Label();
            chkShowOrderType = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            lblShowOrderType = new System.Windows.Forms.Label();
            chkShowSessionNum = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            lblShowSessionNum = new System.Windows.Forms.Label();
            cmbPaperWidth = new Guna.UI2.WinForms.Guna2ComboBox();
            lblPaperWidthLabel = new System.Windows.Forms.Label();
            txtReceiptFooterEditor = new Guna.UI2.WinForms.Guna2TextBox();
            lblReceiptFooterEditorLabel = new System.Windows.Forms.Label();
            pnlReceiptPreview = new Guna.UI2.WinForms.Guna2Panel();

            pnlCardReceipt.Location = new System.Drawing.Point(24, 8);
            pnlCardReceipt.Size = new System.Drawing.Size(340, 480);
            pnlCardReceipt.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;

            lblCardReceiptTitle.Location = new System.Drawing.Point(24, 20); lblCardReceiptTitle.Size = new System.Drawing.Size(300, 24); lblCardReceiptTitle.Text = "Receipt Configuration";
            lblCardReceiptSub.Location = new System.Drawing.Point(24, 44); lblCardReceiptSub.Size = new System.Drawing.Size(300, 18); lblCardReceiptSub.Text = "Control what appears on all receipts.";

            int ry = 76; int rgap = 40;

            lblShowCashier.Location = new System.Drawing.Point(24, ry); lblShowCashier.Size = new System.Drawing.Size(220, 20); lblShowCashier.Text = "Show cashier name";
            chkShowCashier.Location = new System.Drawing.Point(270, ry - 2); chkShowCashier.Size = new System.Drawing.Size(60, 26);

            ry += rgap;
            lblShowCustomer.Location = new System.Drawing.Point(24, ry); lblShowCustomer.Size = new System.Drawing.Size(220, 20); lblShowCustomer.Text = "Show customer name";
            chkShowCustomer.Location = new System.Drawing.Point(270, ry - 2); chkShowCustomer.Size = new System.Drawing.Size(60, 26);

            ry += rgap;
            lblShowOrderType.Location = new System.Drawing.Point(24, ry); lblShowOrderType.Size = new System.Drawing.Size(220, 20); lblShowOrderType.Text = "Show order type";
            chkShowOrderType.Location = new System.Drawing.Point(270, ry - 2); chkShowOrderType.Size = new System.Drawing.Size(60, 26);

            ry += rgap;
            lblShowSessionNum.Location = new System.Drawing.Point(24, ry); lblShowSessionNum.Size = new System.Drawing.Size(220, 20); lblShowSessionNum.Text = "Show session number";
            chkShowSessionNum.Location = new System.Drawing.Point(270, ry - 2); chkShowSessionNum.Size = new System.Drawing.Size(60, 26);

            ry += rgap + 8;
            lblPaperWidthLabel.Location = new System.Drawing.Point(24, ry); lblPaperWidthLabel.Size = new System.Drawing.Size(130, 18); lblPaperWidthLabel.Text = "Paper Width";
            cmbPaperWidth.Location = new System.Drawing.Point(24, ry + 20); cmbPaperWidth.Size = new System.Drawing.Size(140, 40);
            cmbPaperWidth.Items.AddRange(new object[] { "58mm", "80mm" });

            lblReceiptFooterEditorLabel.Location = new System.Drawing.Point(24, ry + 68); lblReceiptFooterEditorLabel.Size = new System.Drawing.Size(130, 18); lblReceiptFooterEditorLabel.Text = "Footer Message";
            txtReceiptFooterEditor.Location = new System.Drawing.Point(24, ry + 88); txtReceiptFooterEditor.Size = new System.Drawing.Size(290, 40); txtReceiptFooterEditor.PlaceholderText = "Thank you for your purchase!";

            pnlCardReceipt.Controls.Add(lblCardReceiptTitle); pnlCardReceipt.Controls.Add(lblCardReceiptSub);
            pnlCardReceipt.Controls.Add(lblShowCashier); pnlCardReceipt.Controls.Add(chkShowCashier);
            pnlCardReceipt.Controls.Add(lblShowCustomer); pnlCardReceipt.Controls.Add(chkShowCustomer);
            pnlCardReceipt.Controls.Add(lblShowOrderType); pnlCardReceipt.Controls.Add(chkShowOrderType);
            pnlCardReceipt.Controls.Add(lblShowSessionNum); pnlCardReceipt.Controls.Add(chkShowSessionNum);
            pnlCardReceipt.Controls.Add(lblPaperWidthLabel); pnlCardReceipt.Controls.Add(cmbPaperWidth);
            pnlCardReceipt.Controls.Add(lblReceiptFooterEditorLabel); pnlCardReceipt.Controls.Add(txtReceiptFooterEditor);

            // Live preview panel — RIGHT side, true 80mm proportions
            lblReceiptPreviewTitle = new System.Windows.Forms.Label();
            lblReceiptPreviewTitle.Text = "80mm RECEIPT PREVIEW";
            lblReceiptPreviewTitle.Location = new System.Drawing.Point(380, 8);
            lblReceiptPreviewTitle.Size = new System.Drawing.Size(300, 22);

            pnlReceiptPreview.Location = new System.Drawing.Point(380, 32);
            pnlReceiptPreview.Size = new System.Drawing.Size(300, 480);
            pnlReceiptPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;

            // Responsive receipt editor layout
            pnlReceiptEditor.Resize += (s, e) =>
            {
                int pw = pnlReceiptEditor.Width;
                int previewW = System.Math.Min(320, System.Math.Max(260, (pw - 380) - 24));
                int previewX = pw - previewW - 24;
                pnlReceiptPreview.Location = new System.Drawing.Point(previewX, 32);
                pnlReceiptPreview.Size = new System.Drawing.Size(previewW, pnlReceiptEditor.Height - 56);
                lblReceiptPreviewTitle.Location = new System.Drawing.Point(previewX, 8);
                pnlCardReceipt.Size = new System.Drawing.Size(System.Math.Min(360, previewX - 48), pnlReceiptEditor.Height - 32);
            };

            pnlReceiptEditor.Controls.Add(pnlCardReceipt);
            pnlReceiptEditor.Controls.Add(lblReceiptPreviewTitle);
            pnlReceiptEditor.Controls.Add(pnlReceiptPreview);



            // SettingsView
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlRightPanel);
            Controls.Add(pnlTabSidebar);
            Name = "SettingsView";
            Size = new System.Drawing.Size(1004, 600);
            ResumeLayout(false);
        }

        // ── Sidebar ──
        private System.Windows.Forms.Panel pnlTabSidebar;
        private System.Windows.Forms.FlowLayoutPanel flpTabs;
        private System.Windows.Forms.Panel pnlRightPanel;
        private System.Windows.Forms.Label lblSettingsTitle;
        private Guna.UI2.WinForms.Guna2Button btnSaveAll;

        // ── Section panels ──
        private System.Windows.Forms.Panel pnlStoreProfile;
        private System.Windows.Forms.Panel pnlSessionCash;
        private System.Windows.Forms.Panel pnlReceiptEditor;

        private System.Windows.Forms.Panel pnlExportBackup;
        private System.Windows.Forms.Panel pnlSecurity;

        // ── Store Profile ──
        private Guna.UI2.WinForms.Guna2Panel pnlCardProfile;
        private System.Windows.Forms.Label lblCardProfileTitle, lblCardProfileSub;
        private Guna.UI2.WinForms.Guna2Panel pnlLogoUpload;
        private System.Windows.Forms.Label lblUploadText;
        private Guna.UI2.WinForms.Guna2TextBox txtStoreName;
        private System.Windows.Forms.Label lblStoreNameLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtSupportEmail;
        private System.Windows.Forms.Label lblSupportEmailLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtPhone;
        private System.Windows.Forms.Label lblPhoneLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtReceiptFooter;
        private System.Windows.Forms.Label lblReceiptFooterLabel;

        private Guna.UI2.WinForms.Guna2Panel pnlCardLocation;
        private System.Windows.Forms.Label lblCardLocationTitle, lblCardLocationSub;
        private Guna.UI2.WinForms.Guna2TextBox txtPopupLocation;
        private System.Windows.Forms.Label lblPopupLocationLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtOperatingLocation;
        private System.Windows.Forms.Label lblOperatingLocationLabel;

        private Guna.UI2.WinForms.Guna2Panel pnlCardSmtp;
        private System.Windows.Forms.Label lblCardSmtpTitle, lblCardSmtpSub;
        private Guna.UI2.WinForms.Guna2TextBox txtSmtpServer;
        private System.Windows.Forms.Label lblSmtpServerLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtSmtpPort;
        private System.Windows.Forms.Label lblSmtpPortLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtSmtpPassword;
        private System.Windows.Forms.Label lblSmtpPasswordLabel;

        // ── Session & Cash ──
        private Guna.UI2.WinForms.Guna2Panel pnlCardSession;
        private System.Windows.Forms.Label lblCardSessionTitle, lblCardSessionSub;
        private Guna.UI2.WinForms.Guna2TextBox txtDefaultCash;
        private System.Windows.Forms.Label lblDefaultCashLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtSessionTimeout;
        private System.Windows.Forms.Label lblSessionTimeoutLabel;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkRequireCashCount;
        private System.Windows.Forms.Label lblRequireCashCount;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkOverShortWarnings;
        private System.Windows.Forms.Label lblOverShortWarnings;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkAutoZReport;
        private System.Windows.Forms.Label lblAutoZReport;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkAutoLockQuickSale;
        private System.Windows.Forms.Label lblAutoLockQuickSale;
        private System.Windows.Forms.Label lblDefaultCashHelp;
        private System.Windows.Forms.Label lblSessionTimeoutHelp;
        private System.Windows.Forms.Label lblRequireCashCountHelp;
        private System.Windows.Forms.Label lblOverShortWarningsHelp;
        private System.Windows.Forms.Label lblAutoZReportHelp;
        private System.Windows.Forms.Label lblAutoLockQuickSaleHelp;

        // ── Receipt Editor ──
        private Guna.UI2.WinForms.Guna2Panel pnlCardReceipt;
        private System.Windows.Forms.Label lblCardReceiptTitle, lblCardReceiptSub;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkShowCashier;
        private System.Windows.Forms.Label lblShowCashier;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkShowCustomer;
        private System.Windows.Forms.Label lblShowCustomer;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkShowOrderType;
        private System.Windows.Forms.Label lblShowOrderType;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkShowSessionNum;
        private System.Windows.Forms.Label lblShowSessionNum;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPaperWidth;
        private System.Windows.Forms.Label lblPaperWidthLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtReceiptFooterEditor;
        private System.Windows.Forms.Label lblReceiptFooterEditorLabel;
        private Guna.UI2.WinForms.Guna2Panel pnlReceiptPreview;
        private System.Windows.Forms.Label lblReceiptPreviewTitle;


    }
}
