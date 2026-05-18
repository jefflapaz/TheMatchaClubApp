using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using TheMatchaClubApp.Core;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class SettingsView
    {
        private static readonly Color SBg = ColorTranslator.FromHtml("#FAFAFA");
        private static readonly Color SCard = Color.White;
        private static readonly Color SBorder = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color STextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color STextSecondary = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color STextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color SGreen = ColorTranslator.FromHtml("#52B743");

        private static readonly string[] TabNames = {
            "Store Profile", "Session Cash", "Receipt Editor",
            "Export Backup", "Security"
        };
        private static readonly string[] TabIcons = {
            "🏪", "💰", "🧾", "💾", "🔒"
        };

        private void InitializeDesign()
        {
            this.BackColor = SBg;
            this.Dock = DockStyle.Fill;

            // ── Tab sidebar ──
            pnlTabSidebar.BackColor = SCard;
            pnlTabSidebar.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
                e.Graphics.DrawLine(pen, pnlTabSidebar.Width - 1, 0, pnlTabSidebar.Width - 1, pnlTabSidebar.Height);
            };
            _tabButtons = new Guna2Button[TabNames.Length];
            for (int i = 0; i < TabNames.Length; i++)
            {
                var btn = new Guna2Button
                {
                    Text = $"  {TabIcons[i]}  {TabNames[i]}",
                    Size = new Size(196, 40),
                    Margin = new Padding(0, 1, 0, 1),
                    BorderRadius = 0,
                    TextAlign = HorizontalAlignment.Left,
                    Font = new Font("Segoe UI", 9F),
                    Cursor = Cursors.Hand,
                    BorderThickness = 0
                };
                btn.Click += TabBtn_Click;
                _tabButtons[i] = btn;
                flpTabs.Controls.Add(btn);
            }
            UpdateTabStyles();

            pnlRightPanel.BackColor = SBg;
            pnlRightPanel.Padding = new Padding(0);
            lblSettingsTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblSettingsTitle.ForeColor = STextPrimary;
            lblSettingsTitle.BackColor = Color.Transparent;

            // Responsive layout handler
            pnlRightPanel.Resize += (s, e) =>
            {
                int pw = pnlRightPanel.Width;
                btnSaveAll.Location = new Point(pw - btnSaveAll.Width - 28, 14);
                foreach (var sp in _sectionPanels)
                {
                    sp.Size = new Size(pw, pnlRightPanel.Height - 58);
                    
                    if (sp == pnlReceiptEditor) continue; // Receipt Editor has its own custom responsive layout
                    
                    int cw = sp.ClientSize.Width;
                    foreach (Control c in sp.Controls)
                    {
                        if (c is Guna2Panel card)
                        {
                            card.Width = cw - 48; // 24px left + 24px right margin
                        }
                    }
                }
            };

            btnSaveAll.FillColor = SGreen;
            btnSaveAll.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnSaveAll.ForeColor = Color.White;
            btnSaveAll.BorderRadius = 8;
            btnSaveAll.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnSaveAll.BorderThickness = 0;

            // ── Store Profile cards ──
            StyleSettingsCard(pnlCardProfile);
            StyleCardHeader(lblCardProfileTitle, lblCardProfileSub);
            StyleSettingsCard(pnlCardLocation);
            StyleCardHeader(lblCardLocationTitle, lblCardLocationSub);
            StyleSettingsCard(pnlCardSmtp);
            StyleCardHeader(lblCardSmtpTitle, lblCardSmtpSub);

            // Logo upload
            pnlLogoUpload.BackColor = Color.Transparent;
            pnlLogoUpload.FillColor = ColorTranslator.FromHtml("#F9FAFB");
            pnlLogoUpload.BorderColor = SBorder;
            pnlLogoUpload.BorderRadius = 12;
            pnlLogoUpload.BorderThickness = 1;
            pnlLogoUpload.ShadowDecoration.Enabled = false;
            pnlLogoUpload.Cursor = Cursors.Hand;
            pnlLogoUpload.Paint += (s, e) =>
            {
                using var pen = new Pen(SBorder, 1.5f) { DashStyle = DashStyle.Dash };
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawRectangle(pen, 4, 4, pnlLogoUpload.Width - 8, pnlLogoUpload.Height - 8);
            };
            lblUploadText.Font = new Font("Segoe UI", 8F);
            lblUploadText.ForeColor = STextMuted;
            lblUploadText.BackColor = Color.Transparent;

            // Store Profile inputs
            StyleInput(txtStoreName); StyleInput(txtSupportEmail); StyleInput(txtPhone); StyleInput(txtReceiptFooter);
            StyleFieldLabel(lblStoreNameLabel); StyleFieldLabel(lblSupportEmailLabel); StyleFieldLabel(lblPhoneLabel); StyleFieldLabel(lblReceiptFooterLabel);

            // Location inputs
            StyleInput(txtPopupLocation); StyleInput(txtOperatingLocation);
            StyleFieldLabel(lblPopupLocationLabel); StyleFieldLabel(lblOperatingLocationLabel);

            // SMTP inputs
            StyleInput(txtSmtpServer); StyleInput(txtSmtpPort); StyleInput(txtSmtpPassword);
            StyleFieldLabel(lblSmtpServerLabel); StyleFieldLabel(lblSmtpPortLabel); StyleFieldLabel(lblSmtpPasswordLabel);

            // ── Session & Cash card ──
            StyleSettingsCard(pnlCardSession);
            StyleCardHeader(lblCardSessionTitle, lblCardSessionSub);
            StyleInput(txtDefaultCash); StyleInput(txtSessionTimeout);
            StyleFieldLabel(lblDefaultCashLabel); StyleFieldLabel(lblSessionTimeoutLabel);
            StyleHelpLabel(lblDefaultCashHelp); StyleHelpLabel(lblSessionTimeoutHelp);

            // Toggle labels
            StyleToggleLabel(lblRequireCashCount);
            StyleToggleLabel(lblOverShortWarnings);
            StyleToggleLabel(lblAutoZReport);
            StyleToggleLabel(lblAutoLockQuickSale);

            StyleHelpLabel(lblRequireCashCountHelp);
            StyleHelpLabel(lblOverShortWarningsHelp);
            StyleHelpLabel(lblAutoZReportHelp);
            StyleHelpLabel(lblAutoLockQuickSaleHelp);

            // Toggle switches
            StyleToggle(chkRequireCashCount);
            StyleToggle(chkOverShortWarnings);
            StyleToggle(chkAutoZReport);
            StyleToggle(chkAutoLockQuickSale);

            // ── Receipt Editor card ──
            StyleSettingsCard(pnlCardReceipt);
            StyleCardHeader(lblCardReceiptTitle, lblCardReceiptSub);
            StyleToggleLabel(lblShowCashier); StyleToggle(chkShowCashier);
            StyleToggleLabel(lblShowCustomer); StyleToggle(chkShowCustomer);
            StyleToggleLabel(lblShowOrderType); StyleToggle(chkShowOrderType);
            StyleToggleLabel(lblShowSessionNum); StyleToggle(chkShowSessionNum);
            StyleFieldLabel(lblPaperWidthLabel); StyleFieldLabel(lblReceiptFooterEditorLabel);
            StyleInput(txtReceiptFooterEditor);
            cmbPaperWidth.BorderRadius = 8; cmbPaperWidth.BorderColor = SBorder;
            cmbPaperWidth.FocusedState.BorderColor = SGreen; cmbPaperWidth.ForeColor = STextPrimary;
            cmbPaperWidth.BackColor = Color.Transparent; cmbPaperWidth.FillColor = SCard;
            cmbPaperWidth.Font = new Font("Segoe UI", 9F);

            // Receipt live preview
            pnlReceiptPreview.BackColor = Color.Transparent;
            pnlReceiptPreview.FillColor = Color.White;
            pnlReceiptPreview.BorderColor = SBorder;
            pnlReceiptPreview.BorderRadius = 4;
            pnlReceiptPreview.BorderThickness = 1;
            pnlReceiptPreview.ShadowDecoration.Enabled = true;
            pnlReceiptPreview.ShadowDecoration.Depth = 8;
            pnlReceiptPreview.ShadowDecoration.Color = Color.FromArgb(20, 0, 0, 0);
            pnlReceiptPreview.Paint += PnlReceiptPreview_Paint;

            // Preview title label
            lblReceiptPreviewTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblReceiptPreviewTitle.ForeColor = STextMuted;
            lblReceiptPreviewTitle.BackColor = Color.Transparent;
            lblReceiptPreviewTitle.TextAlign = ContentAlignment.MiddleCenter;



            // ── Export & Backup section ──
            BuildExportBackupSection();

            // ── Security section ──
            BuildSecuritySection();
        }

        private void StylePlaceholderSection(Panel section, string icon, string title, string description)
        {
            var card = new Guna2Panel
            {
                Location = new Point(24, 8),
                Size = new Size(720, 200),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            StyleSettingsCard(card);

            var lblIcon = new Label
            {
                Text = icon, Font = new Font("Segoe UI", 32F),
                Location = new Point(24, 30), AutoSize = true, BackColor = Color.Transparent
            };
            var lblTitle = new Label
            {
                Text = title, Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold),
                ForeColor = STextPrimary, Location = new Point(24, 100), AutoSize = true, BackColor = Color.Transparent
            };
            var lblDesc = new Label
            {
                Text = description, Font = new Font("Segoe UI", 9F),
                ForeColor = STextSecondary, Location = new Point(24, 130), Size = new Size(660, 40), BackColor = Color.Transparent
            };
            var lblStatus = new Label
            {
                Text = "✅ Connected to live data", Font = new Font("Segoe UI", 8F),
                ForeColor = SGreen, Location = new Point(24, 165), AutoSize = true, BackColor = Color.Transparent
            };

            card.Controls.Add(lblIcon); card.Controls.Add(lblTitle); card.Controls.Add(lblDesc); card.Controls.Add(lblStatus);
            section.Controls.Add(card);
        }

        private void StyleSettingsCard(Guna2Panel card)
        {
            card.BackColor = Color.Transparent;
            card.FillColor = SCard;
            card.BorderRadius = 16;
            card.BorderColor = ColorTranslator.FromHtml("#F3F4F6");
            card.BorderThickness = 1;
            card.ShadowDecoration.Enabled = false;
        }

        private void StyleCardHeader(Label title, Label sub)
        {
            title.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            title.ForeColor = STextPrimary;
            title.BackColor = Color.Transparent;
            sub.Font = new Font("Segoe UI", 8F);
            sub.ForeColor = STextSecondary;
            sub.BackColor = Color.Transparent;
        }

        private void StyleInput(Guna2TextBox txt)
        {
            txt.BorderRadius = 8;
            txt.BorderColor = SBorder;
            txt.FocusedState.BorderColor = SGreen;
            txt.ForeColor = STextPrimary;
            txt.BackColor = Color.Transparent;
            txt.FillColor = SCard;
            txt.Font = new Font("Segoe UI", 9F);
            txt.PlaceholderForeColor = STextMuted;
        }

        private void StyleFieldLabel(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lbl.ForeColor = STextSecondary;
            lbl.BackColor = Color.Transparent;
        }

        private void StyleToggleLabel(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 9.5F);
            lbl.ForeColor = STextPrimary;
            lbl.BackColor = Color.Transparent;
        }

        private void StyleToggle(Guna2ToggleSwitch toggle)
        {
            toggle.CheckedState.FillColor = SGreen;
            toggle.CheckedState.InnerColor = Color.White;
            toggle.UncheckedState.FillColor = ColorTranslator.FromHtml("#D1D5DB");
            toggle.UncheckedState.InnerColor = Color.White;
        }

        private void StyleHelpLabel(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 7.8F);
            lbl.ForeColor = STextSecondary;
            lbl.BackColor = Color.Transparent;
        }

        private void UpdateTabStyles()
        {
            for (int i = 0; i < _tabButtons.Length; i++)
            {
                var btn = _tabButtons[i];
                string name = TabNames[i];
                bool active = name == _activeTab;
                if (active)
                {
                    btn.FillColor = SCard;
                    btn.ForeColor = SGreen;
                    btn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
                    btn.Paint -= TabBtn_Paint;
                    btn.Paint += TabBtn_Paint;
                }
                else
                {
                    btn.FillColor = Color.Transparent;
                    btn.ForeColor = STextSecondary;
                    btn.Font = new Font("Segoe UI", 9F);
                    btn.Paint -= TabBtn_Paint;
                }
            }
        }

        private void TabBtn_Paint(object? sender, PaintEventArgs e)
        {
            using var brush = new SolidBrush(SGreen);
            e.Graphics.FillRectangle(brush, 0, 4, 4, ((Control)sender!).Height - 8);
        }

        private void PnlReceiptPreview_Paint(object? sender, PaintEventArgs e)
        {
            // Build a live settings snapshot from current field values
            var liveSettings = new StoreSettings
            {
                StoreName = txtStoreName.Text,
                Address = Program.DataService.Settings.Address,
                CurrentOperatingLocation = txtOperatingLocation.Text,
                Phone = txtPhone.Text,
                Email = txtSupportEmail.Text,
                ReceiptShowCashierName = chkShowCashier.Checked,
                ReceiptShowCustomerName = chkShowCustomer.Checked,
                ReceiptShowOrderType = chkShowOrderType.Checked,
                ReceiptShowSessionNumber = chkShowSessionNum.Checked,
                ReceiptFooterMessage = txtReceiptFooterEditor?.Text ?? "Thank you!"
            };

            ReceiptRenderer.Render(e.Graphics, pnlReceiptPreview.ClientRectangle, null, liveSettings);
        }

        // ══════════════════════════════════════════════════════════════
        //  EXPORT & BACKUP SECTION — FULL UI BUILD
        // ══════════════════════════════════════════════════════════════

        private void BuildExportBackupSection()
        {
            // ── Card 1: Export Data ──
            var cardExport = new Guna2Panel
            {
                Location = new Point(24, 8),
                Size = new Size(720, 220),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            StyleSettingsCard(cardExport);

            var lblExportTitle = new Label
            {
                Text = "Export Data", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = STextPrimary, Location = new Point(24, 20), Size = new Size(300, 24), BackColor = Color.Transparent
            };
            var lblExportSub = new Label
            {
                Text = "Export your POS data as CSV spreadsheet files for external analysis.",
                Font = new Font("Segoe UI", 8F), ForeColor = STextSecondary,
                Location = new Point(24, 44), Size = new Size(660, 18), BackColor = Color.Transparent
            };

            btnExportSales = CreateExportButton("📊  Export Sales CSV", new Point(24, 80));
            btnExportCustomers = CreateExportButton("👥  Export Customers CSV", new Point(24, 130));
            btnExportProducts = CreateExportButton("📦  Export Products CSV", new Point(260, 80));

            // Record counts
            var lblSalesCount = new Label
            {
                Text = $"{Program.DataService.Orders.Count} orders available",
                Font = new Font("Segoe UI", 7.5F), ForeColor = STextMuted,
                Location = new Point(24, 118), AutoSize = true, BackColor = Color.Transparent
            };
            var lblCustCount = new Label
            {
                Text = $"{Program.DataService.Customers.Count} customers available",
                Font = new Font("Segoe UI", 7.5F), ForeColor = STextMuted,
                Location = new Point(24, 168), AutoSize = true, BackColor = Color.Transparent
            };
            var lblProdCount = new Label
            {
                Text = $"{Program.DataService.Products.Count} products available",
                Font = new Font("Segoe UI", 7.5F), ForeColor = STextMuted,
                Location = new Point(260, 118), AutoSize = true, BackColor = Color.Transparent
            };

            // Export folder info
            var lblExportFolder = new Label
            {
                Text = $"📁 Export folder: Documents/MatchaPOS/Exports",
                Font = new Font("Segoe UI", 7.5F), ForeColor = STextMuted,
                Location = new Point(24, 192), AutoSize = true, BackColor = Color.Transparent
            };

            cardExport.Controls.AddRange(new Control[] {
                lblExportTitle, lblExportSub,
                btnExportSales, btnExportCustomers, btnExportProducts,
                lblSalesCount, lblCustCount, lblProdCount, lblExportFolder
            });

            // ── Card 2: Backup System ──
            var cardBackup = new Guna2Panel
            {
                Location = new Point(24, 244),
                Size = new Size(720, 180),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            StyleSettingsCard(cardBackup);

            var lblBackupTitle = new Label
            {
                Text = "Backup System", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = STextPrimary, Location = new Point(24, 20), Size = new Size(300, 24), BackColor = Color.Transparent
            };
            var lblBackupSub = new Label
            {
                Text = "Create a full local backup or restore from a previous backup file.",
                Font = new Font("Segoe UI", 8F), ForeColor = STextSecondary,
                Location = new Point(24, 44), Size = new Size(660, 18), BackColor = Color.Transparent
            };

            btnCreateBackup = new Guna2Button
            {
                Text = "🔒  Create Full Backup",
                Size = new Size(220, 42),
                Location = new Point(24, 76),
                BorderRadius = 10,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                FillColor = SGreen,
                HoverState = { FillColor = ColorTranslator.FromHtml("#46A037") },
                BorderThickness = 0,
                Cursor = Cursors.Hand
            };

            btnRestoreBackup = new Guna2Button
            {
                Text = "📂  Restore Backup",
                Size = new Size(220, 42),
                Location = new Point(260, 76),
                BorderRadius = 10,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = STextPrimary,
                FillColor = Color.White,
                HoverState = { FillColor = ColorTranslator.FromHtml("#F9FAFB") },
                BorderColor = SBorder,
                BorderThickness = 1,
                Cursor = Cursors.Hand
            };

            var lblBackupWarning = new Label
            {
                Text = "⚠️  Restoring a backup will overwrite all current data. A confirmation will be shown first.",
                Font = new Font("Segoe UI", 7.5F), ForeColor = ColorTranslator.FromHtml("#D97706"),
                Location = new Point(24, 130), Size = new Size(660, 18), BackColor = Color.Transparent
            };
            var lblBackupFolder = new Label
            {
                Text = "📁 Backup folder: Documents/MatchaPOS/Backups",
                Font = new Font("Segoe UI", 7.5F), ForeColor = STextMuted,
                Location = new Point(24, 152), AutoSize = true, BackColor = Color.Transparent
            };

            cardBackup.Controls.AddRange(new Control[] {
                lblBackupTitle, lblBackupSub,
                btnCreateBackup, btnRestoreBackup,
                lblBackupWarning, lblBackupFolder
            });

            // ── Card 3: Backup Information ──
            var cardInfo = new Guna2Panel
            {
                Location = new Point(24, 440),
                Size = new Size(720, 200),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            StyleSettingsCard(cardInfo);

            var lblInfoTitle = new Label
            {
                Text = "Backup Information", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = STextPrimary, Location = new Point(24, 20), Size = new Size(300, 24), BackColor = Color.Transparent
            };
            var lblInfoSub = new Label
            {
                Text = "Current system and backup status overview.",
                Font = new Font("Segoe UI", 8F), ForeColor = STextSecondary,
                Location = new Point(24, 44), Size = new Size(660, 18), BackColor = Color.Transparent
            };

            // Status rows
            int iy = 76;
            int igap = 34;

            lblInfoLastBackup = CreateInfoRow(cardInfo, "Last Backup", "—", ref iy);
            iy += igap;
            lblInfoBackupSize = CreateInfoRow(cardInfo, "Backup File Size", "—", ref iy);
            iy += igap;
            lblInfoDbStatus = CreateInfoRow(cardInfo, "Database Status", "—", ref iy);
            iy += igap;
            lblInfoDbSize = CreateInfoRow(cardInfo, "Database Size", "—", ref iy);

            cardInfo.Controls.AddRange(new Control[] { lblInfoTitle, lblInfoSub });

            // Add all cards to the panel
            pnlExportBackup.Controls.AddRange(new Control[] { cardExport, cardBackup, cardInfo });
        }

        private void BuildSecuritySection()
        {
            // ── Card 1: Account Security ──
            var cardAccount = new Guna2Panel
            {
                Location = new Point(24, 8),
                Size = new Size(720, 240),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            StyleSettingsCard(cardAccount);

            var lblAccountTitle = new Label { Text = "Account Security", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = STextPrimary, Location = new Point(24, 20), Size = new Size(300, 24), BackColor = Color.Transparent };
            var lblAccountSub = new Label { Text = "Manage your admin account credentials and password.", Font = new Font("Segoe UI", 8F), ForeColor = STextSecondary, Location = new Point(24, 44), Size = new Size(660, 18), BackColor = Color.Transparent };
            cardAccount.Controls.AddRange(new Control[] { lblAccountTitle, lblAccountSub });

            // Account Name & Email (Readonly)
            var lblAdminName = new Label { Text = "Admin/Cashier Name", Location = new Point(24, 76), AutoSize = true };
            StyleFieldLabel(lblAdminName);
            txtSecurityAdminName = new Guna2TextBox { Location = new Point(24, 96), Size = new Size(200, 36), ReadOnly = true, FillColor = ColorTranslator.FromHtml("#F9FAFB") };
            StyleInput(txtSecurityAdminName);
            
            var lblUsername = new Label { Text = "Username", Location = new Point(240, 76), AutoSize = true };
            StyleFieldLabel(lblUsername);
            txtSecurityUsername = new Guna2TextBox { Location = new Point(240, 96), Size = new Size(200, 36), ReadOnly = true, FillColor = ColorTranslator.FromHtml("#F9FAFB") };
            StyleInput(txtSecurityUsername);

            var lblEmail = new Label { Text = "Email Address", Location = new Point(456, 76), AutoSize = true };
            StyleFieldLabel(lblEmail);
            txtSecurityEmail = new Guna2TextBox { Location = new Point(456, 96), Size = new Size(200, 36), ReadOnly = true, FillColor = ColorTranslator.FromHtml("#F9FAFB") };
            StyleInput(txtSecurityEmail);

            // Passwords
            var lblCurrentPass = new Label { Text = "Current Password", Location = new Point(24, 142), AutoSize = true };
            StyleFieldLabel(lblCurrentPass);
            txtCurrentPassword = new Guna2TextBox { Location = new Point(24, 162), Size = new Size(200, 36), PasswordChar = '•' };
            StyleInput(txtCurrentPassword);

            var lblNewPass = new Label { Text = "New Password", Location = new Point(240, 142), AutoSize = true };
            StyleFieldLabel(lblNewPass);
            txtNewPassword = new Guna2TextBox { Location = new Point(240, 162), Size = new Size(200, 36), PasswordChar = '•' };
            StyleInput(txtNewPassword);

            var lblConfirmPass = new Label { Text = "Confirm New Password", Location = new Point(456, 142), AutoSize = true };
            StyleFieldLabel(lblConfirmPass);
            txtConfirmPassword = new Guna2TextBox { Location = new Point(456, 162), Size = new Size(200, 36), PasswordChar = '•' };
            StyleInput(txtConfirmPassword);

            // Show/Hide toggle checkbox
            var chkShowPass = new Guna2CheckBox
            {
                Text = "Show Passwords",
                Location = new Point(24, 206),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F),
                ForeColor = STextSecondary,
                Cursor = Cursors.Hand
            };
            chkShowPass.CheckedChanged += (s, e) => {
                char pc = chkShowPass.Checked ? '\0' : '•';
                txtCurrentPassword.PasswordChar = pc;
                txtNewPassword.PasswordChar = pc;
                txtConfirmPassword.PasswordChar = pc;
            };

            btnChangePassword = new Guna2Button
            {
                Text = "Update Password",
                Size = new Size(160, 36),
                Location = new Point(496, 200),
                BorderRadius = 8,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                FillColor = SGreen,
                Cursor = Cursors.Hand
            };

            cardAccount.Controls.AddRange(new Control[] { lblAdminName, txtSecurityAdminName, lblUsername, txtSecurityUsername, lblEmail, txtSecurityEmail, lblCurrentPass, txtCurrentPassword, lblNewPass, txtNewPassword, lblConfirmPass, txtConfirmPassword, chkShowPass, btnChangePassword });
            
            // ── Card 2: Sensitive Action Protection ──
            var cardAction = new Guna2Panel
            {
                Location = new Point(24, 260),
                Size = new Size(720, 150),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            StyleSettingsCard(cardAction);

            var lblActionTitle = new Label { Text = "Sensitive Action Protection", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = STextPrimary, Location = new Point(24, 20), Size = new Size(300, 24), BackColor = Color.Transparent };
            var lblActionSub = new Label { Text = "Require password confirmation before performing critical POS actions.", Font = new Font("Segoe UI", 8F), ForeColor = STextSecondary, Location = new Point(24, 44), Size = new Size(660, 18), BackColor = Color.Transparent };
            cardAction.Controls.AddRange(new Control[] { lblActionTitle, lblActionSub });

            chkReqPassDeleteProduct = new Guna2ToggleSwitch { Location = new Point(24, 76) }; StyleToggle(chkReqPassDeleteProduct);
            var lblReqPassDelProd = new Label { Text = "Require password before deleting products", Location = new Point(70, 78), AutoSize = true }; StyleToggleLabel(lblReqPassDelProd);
            
            chkReqPassDeleteOrder = new Guna2ToggleSwitch { Location = new Point(360, 76) }; StyleToggle(chkReqPassDeleteOrder);
            var lblReqPassDelOrder = new Label { Text = "Require password before deleting orders", Location = new Point(406, 78), AutoSize = true }; StyleToggleLabel(lblReqPassDelOrder);

            chkReqPassCloseSession = new Guna2ToggleSwitch { Location = new Point(24, 110) }; StyleToggle(chkReqPassCloseSession);
            var lblReqPassCloseSess = new Label { Text = "Require password before closing session", Location = new Point(70, 112), AutoSize = true }; StyleToggleLabel(lblReqPassCloseSess);
            
            chkReqPassSettings = new Guna2ToggleSwitch { Location = new Point(360, 110) }; StyleToggle(chkReqPassSettings);
            var lblReqPassSet = new Label { Text = "Require password before accessing Settings", Location = new Point(406, 112), AutoSize = true }; StyleToggleLabel(lblReqPassSet);

            cardAction.Controls.AddRange(new Control[] { chkReqPassDeleteProduct, lblReqPassDelProd, chkReqPassDeleteOrder, lblReqPassDelOrder, chkReqPassCloseSession, lblReqPassCloseSess, chkReqPassSettings, lblReqPassSet });

            // ── Card 3: Session Protection & Activity ──
            var cardSession = new Guna2Panel
            {
                Location = new Point(24, 422),
                Size = new Size(720, 140),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            StyleSettingsCard(cardSession);

            var lblSessionTitle = new Label { Text = "Session Protection & Activity", Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), ForeColor = STextPrimary, Location = new Point(24, 20), Size = new Size(300, 24), BackColor = Color.Transparent };
            var lblSessionSub = new Label { Text = "Manage auto-lock settings and view account activity.", Font = new Font("Segoe UI", 8F), ForeColor = STextSecondary, Location = new Point(24, 44), Size = new Size(660, 18), BackColor = Color.Transparent };
            cardSession.Controls.AddRange(new Control[] { lblSessionTitle, lblSessionSub });

            var lblAutoLock = new Label { Text = "Auto-Lock POS After Inactivity", Location = new Point(24, 76), AutoSize = true }; StyleFieldLabel(lblAutoLock);
            cmbAutoLock = new Guna2ComboBox
            {
                Location = new Point(24, 96),
                Size = new Size(200, 36),
                BorderRadius = 8,
                BorderColor = SBorder,
                FocusedState = { BorderColor = SGreen },
                FillColor = SCard,
                ForeColor = STextPrimary,
                Font = new Font("Segoe UI", 9F)
            };
            cmbAutoLock.Items.AddRange(new string[] { "Never", "5 Minutes", "15 Minutes", "30 Minutes" });
            cmbAutoLock.SelectedIndex = 0;
            cardSession.Controls.AddRange(new Control[] { lblAutoLock, cmbAutoLock });

            var lblLoginTitle = new Label { Text = "Last Login", Font = new Font("Segoe UI", 9F), ForeColor = STextSecondary, Location = new Point(320, 76), Size = new Size(160, 20), BackColor = Color.Transparent };
            lblSecurityLastLogin = new Label { Text = "—", Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = STextPrimary, Location = new Point(480, 76), Size = new Size(200, 20), BackColor = Color.Transparent };
            var lblPassTitle = new Label { Text = "Last Password Change", Font = new Font("Segoe UI", 9F), ForeColor = STextSecondary, Location = new Point(320, 106), Size = new Size(160, 20), BackColor = Color.Transparent };
            lblSecurityLastPassChange = new Label { Text = "—", Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = STextPrimary, Location = new Point(480, 106), Size = new Size(200, 20), BackColor = Color.Transparent };
            
            cardSession.Controls.AddRange(new Control[] { lblLoginTitle, lblSecurityLastLogin, lblPassTitle, lblSecurityLastPassChange });

            pnlSecurity.Controls.AddRange(new Control[] { cardAccount, cardAction, cardSession });
        }

        private Guna2Button CreateExportButton(string text, Point location)
        {
            return new Guna2Button
            {
                Text = text,
                Size = new Size(220, 34),
                Location = location,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9F),
                ForeColor = STextPrimary,
                FillColor = ColorTranslator.FromHtml("#F3F4F6"),
                HoverState = { FillColor = ColorTranslator.FromHtml("#E5E7EB") },
                BorderThickness = 0,
                Cursor = Cursors.Hand,
                TextAlign = HorizontalAlignment.Left
            };
        }

        private Label CreateInfoRow(Control parent, string label, string value, ref int y)
        {
            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9F),
                ForeColor = STextSecondary,
                Location = new Point(24, y),
                Size = new Size(180, 20),
                BackColor = Color.Transparent
            };
            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = STextPrimary,
                Location = new Point(210, y),
                Size = new Size(480, 20),
                BackColor = Color.Transparent
            };
            parent.Controls.Add(lblLabel);
            parent.Controls.Add(lblValue);
            return lblValue; // Return the value label for dynamic updates
        }

        // ── Export & Backup control fields ──
        internal Guna2Button btnExportSales = null!;
        internal Guna2Button btnExportCustomers = null!;
        internal Guna2Button btnExportProducts = null!;
        internal Guna2Button btnCreateBackup = null!;
        internal Guna2Button btnRestoreBackup = null!;
        internal Label lblInfoLastBackup = null!;
        internal Label lblInfoBackupSize = null!;
        internal Label lblInfoDbStatus = null!;
        internal Label lblInfoDbSize = null!;

        // ── Security control fields ──
        internal Guna2TextBox txtSecurityAdminName = null!;
        internal Guna2TextBox txtSecurityUsername = null!;
        internal Guna2TextBox txtSecurityEmail = null!;
        internal Guna2TextBox txtCurrentPassword = null!;
        internal Guna2TextBox txtNewPassword = null!;
        internal Guna2TextBox txtConfirmPassword = null!;
        internal Guna2Button btnChangePassword = null!;
        internal Guna2ToggleSwitch chkReqPassDeleteProduct = null!;
        internal Guna2ToggleSwitch chkReqPassDeleteOrder = null!;
        internal Guna2ToggleSwitch chkReqPassCloseSession = null!;
        internal Guna2ToggleSwitch chkReqPassSettings = null!;
        internal Guna2ComboBox cmbAutoLock = null!;
        internal Label lblSecurityLastLogin = null!;
        internal Label lblSecurityLastPassChange = null!;
    }
}
