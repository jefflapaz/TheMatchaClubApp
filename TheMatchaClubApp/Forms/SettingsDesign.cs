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
            "Store Profile", "Session & Cash", "Receipt Editor",
            "Export & Backup", "Security"
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
            StyleInput(txtStoreName); StyleInput(txtSupportEmail); StyleInput(txtPhone); StyleInput(txtReceiptFooter); StyleInput(txtCashierName);
            StyleFieldLabel(lblStoreNameLabel); StyleFieldLabel(lblSupportEmailLabel); StyleFieldLabel(lblPhoneLabel); StyleFieldLabel(lblReceiptFooterLabel); StyleFieldLabel(lblCashierNameLabel);

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



            // ── Placeholder styling for other sections ──
            StylePlaceholderSection(pnlExportBackup, "💾", "Export & Backup", "Export your data or create a full backup.");
            StylePlaceholderSection(pnlSecurity, "🔒", "Security", "Password and authentication settings.");
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
            card.ShadowDecoration.Enabled = true;
            card.ShadowDecoration.Depth = 4;
            card.ShadowDecoration.Color = Color.FromArgb(6, 0, 0, 0);
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
    }
}
