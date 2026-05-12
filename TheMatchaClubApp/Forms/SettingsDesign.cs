using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

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

        private void InitializeDesign()
        {
            this.BackColor = SBg;
            this.Dock = DockStyle.Fill;

            // Tab sidebar
            pnlTabSidebar.BackColor = Color.Transparent;

            string[] tabs = { "Store Profile", "Receipt Editor", "App Preferences", "Admin Security", "Email (SMTP)", "Data Management" };
            _tabButtons = new Guna2Button[tabs.Length];
            for (int i = 0; i < tabs.Length; i++)
            {
                var btn = new Guna2Button
                {
                    Text = "   " + tabs[i],
                    Size = new Size(208, 48),
                    Margin = new Padding(0, 2, 0, 2),
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

            // Right panel
            pnlRightPanel.BackColor = SBg;

            lblSettingsTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblSettingsTitle.ForeColor = STextPrimary;
            lblSettingsTitle.BackColor = Color.Transparent;

            btnSaveAll.FillColor = SGreen;
            btnSaveAll.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnSaveAll.ForeColor = Color.White;
            btnSaveAll.BorderRadius = 8;
            btnSaveAll.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnSaveAll.BorderThickness = 0;

            // Card 1
            StyleSettingsCard(pnlCard1);
            lblCard1Title.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblCard1Title.ForeColor = STextPrimary;
            lblCard1Title.BackColor = Color.Transparent;
            lblCard1Sub.Font = new Font("Segoe UI", 8F);
            lblCard1Sub.ForeColor = STextSecondary;
            lblCard1Sub.BackColor = Color.Transparent;

            // Logo upload box
            pnlLogoUpload.BackColor = Color.Transparent;
            pnlLogoUpload.FillColor = ColorTranslator.FromHtml("#F9FAFB");
            pnlLogoUpload.BorderColor = SBorder;
            pnlLogoUpload.BorderRadius = 12;
            pnlLogoUpload.BorderThickness = 1;
            pnlLogoUpload.ShadowDecoration.Enabled = false;
            pnlLogoUpload.Cursor = Cursors.Hand;
            pnlLogoUpload.Paint += (s, e) =>
            {
                // Dashed border
                using var pen = new Pen(SBorder, 1.5f) { DashStyle = DashStyle.Dash };
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawRectangle(pen, 4, 4, pnlLogoUpload.Width - 8, pnlLogoUpload.Height - 8);
            };
            lblUploadText.Font = new Font("Segoe UI", 8F);
            lblUploadText.ForeColor = STextMuted;
            lblUploadText.BackColor = Color.Transparent;

            // Input fields
            StyleInput(txtStoreName);
            StyleInput(txtTaxId);
            StyleInput(txtSupportEmail);
            StyleFieldLabel(lblStoreNameLabel);
            StyleFieldLabel(lblTaxIdLabel);
            StyleFieldLabel(lblSupportEmailLabel);

            // Card 2
            StyleSettingsCard(pnlCard2);
            lblCard2Title.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblCard2Title.ForeColor = STextPrimary;
            lblCard2Title.BackColor = Color.Transparent;
            lblCard2Sub.Font = new Font("Segoe UI", 8F);
            lblCard2Sub.ForeColor = STextSecondary;
            lblCard2Sub.BackColor = Color.Transparent;

            StyleInput(txtAddress);
            StyleInput(txtCity);
            StyleInput(txtPostalCode);
            StyleInput(txtPhone);
            StyleInput(txtWebsite);
            StyleFieldLabel(lblAddressLabel);
            StyleFieldLabel(lblCityLabel);
            StyleFieldLabel(lblPostalLabel);
            StyleFieldLabel(lblPhoneLabel);
            StyleFieldLabel(lblWebsiteLabel);

            // Placeholder
            pnlPlaceholder.BackColor = Color.Transparent;
            pnlPlaceholder.FillColor = SCard;
            pnlPlaceholder.BorderRadius = 16;
            pnlPlaceholder.BorderColor = ColorTranslator.FromHtml("#F3F4F6");
            pnlPlaceholder.BorderThickness = 1;
            pnlPlaceholder.ShadowDecoration.Enabled = false;
            lblPlaceholderText.Font = new Font("Segoe UI", 10F);
            lblPlaceholderText.ForeColor = STextMuted;
            lblPlaceholderText.BackColor = Color.Transparent;
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

        private void UpdateTabStyles()
        {
            foreach (var btn in _tabButtons)
            {
                string name = btn.Text.Trim();
                bool active = name == _activeTab;
                if (active)
                {
                    btn.FillColor = SCard;
                    btn.ForeColor = SGreen;
                    btn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
                    // Left border via Paint
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
            // Left green border for active tab
            using var brush = new SolidBrush(SGreen);
            e.Graphics.FillRectangle(brush, 0, 4, 4, ((Control)sender!).Height - 8);
        }
    }
}
