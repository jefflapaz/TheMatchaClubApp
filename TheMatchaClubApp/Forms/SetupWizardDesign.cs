using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace TheMatchaClubApp.Forms
{
    public partial class SetupWizardForm
    {
        private static readonly Color WBg = ColorTranslator.FromHtml("#FAFAFA");
        private static readonly Color WCard = Color.White;
        private static readonly Color WBorder = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color WTextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color WTextSecondary = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color WTextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color WGreen = ColorTranslator.FromHtml("#52B743");
        private static readonly Color WGreenBg = ColorTranslator.FromHtml("#F2FAEF");
        private static readonly Color WGreenBorder = ColorTranslator.FromHtml("#E2F3DD");

        private void InitializeDesign()
        {
            this.BackColor = WBg;

            // ── Top Nav ──
            pnlTopNav.BackColor = WCard;
            pnlTopNav.Paint += (s, e) =>
            {
                using var pen = new Pen(WBorder, 1);
                e.Graphics.DrawLine(pen, 0, pnlTopNav.Height - 1, pnlTopNav.Width, pnlTopNav.Height - 1);
            };

            pnlNavLogoCircle.BackColor = Color.Transparent;
            pnlNavLogoCircle.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(WGreen);
                g.FillEllipse(brush, 0, 0, 27, 27);
                using var font = new Font("Segoe UI", 14F);
                using var textBrush = new SolidBrush(Color.White);
                g.DrawString("\U0001F375", font, textBrush, 2, 2);
            };

            lblNavLogoText.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblNavLogoText.ForeColor = WTextPrimary;
            lblNavLogoText.BackColor = Color.Transparent;
            lblNavLogoText.TextAlign = ContentAlignment.MiddleLeft;

            lblBreadcrumb.Font = new Font("Segoe UI", 8F);
            lblBreadcrumb.ForeColor = WTextMuted;
            lblBreadcrumb.BackColor = Color.Transparent;

            lblBreadcrumbActive.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblBreadcrumbActive.ForeColor = WGreen;
            lblBreadcrumbActive.BackColor = Color.Transparent;

            pnlFirstLaunchPill.BackColor = Color.Transparent;
            pnlFirstLaunchPill.FillColor = WGreenBg;
            pnlFirstLaunchPill.BorderColor = WGreenBorder;
            pnlFirstLaunchPill.BorderThickness = 1;
            pnlFirstLaunchPill.BorderRadius = 11;
            pnlFirstLaunchPill.ShadowDecoration.Enabled = false;
            lblFirstLaunch.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblFirstLaunch.ForeColor = WGreen;
            lblFirstLaunch.BackColor = Color.Transparent;

            pnlNavAvatar.BackColor = Color.Transparent;
            pnlNavAvatar.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(ColorTranslator.FromHtml("#E0E7FF"));
                g.FillEllipse(brush, 0, 0, 31, 31);
                using var font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
                using var tBrush = new SolidBrush(ColorTranslator.FromHtml("#4F46E5"));
                var sz = g.MeasureString("A", font);
                g.DrawString("A", font, tBrush, (32 - sz.Width) / 2, (32 - sz.Height) / 2);
            };

            // ── Stepper ──
            pnlStepper.BackColor = WBg;
            pnlStepper.Paint += PnlStepper_Paint;

            // ── Content Card ──
            pnlContentCard.BackColor = Color.Transparent;
            pnlContentCard.FillColor = WCard;
            pnlContentCard.BorderRadius = 16;
            pnlContentCard.BorderColor = ColorTranslator.FromHtml("#F3F4F6");
            pnlContentCard.BorderThickness = 1;
            pnlContentCard.ShadowDecoration.Enabled = true;
            pnlContentCard.ShadowDecoration.Color = Color.FromArgb(10, 0, 0, 0);
            pnlContentCard.ShadowDecoration.Depth = 12;

            // Card header
            pnlCardHeader.BackColor = WCard;
            pnlCardHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(WBorder, 1);
                e.Graphics.DrawLine(pen, 0, pnlCardHeader.Height - 1, pnlCardHeader.Width, pnlCardHeader.Height - 1);
            };
            lblStepTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblStepTitle.ForeColor = WTextPrimary;
            lblStepTitle.BackColor = Color.Transparent;
            lblStepDesc.Font = new Font("Segoe UI", 9F);
            lblStepDesc.ForeColor = WTextSecondary;
            lblStepDesc.BackColor = Color.Transparent;
            lblProgressLabel.Font = new Font("Segoe UI", 8F);
            lblProgressLabel.ForeColor = WTextMuted;
            lblProgressLabel.BackColor = Color.Transparent;
            lblProgressPercent.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblProgressPercent.ForeColor = WGreen;
            lblProgressPercent.BackColor = Color.Transparent;

            // Step panels
            pnlStep1.BackColor = WCard;
            pnlStep2.BackColor = WCard;
            pnlStep3.BackColor = WCard;
            pnlStep4.BackColor = WCard;

            // Step 1 inputs
            StyleWizardInput(txtStoreName);
            StyleWizardInput(txtEmail);
            StyleWizardInput(txtAddress);
            StyleWizardLabel(lblStoreNameLabel);
            StyleWizardLabel(lblEmailLabel);
            StyleWizardLabel(lblAddressLabel);
            StyleWizardLabel(lblTimezoneLabel);

            pnlLogoUpload.BackColor = Color.Transparent;
            pnlLogoUpload.FillColor = ColorTranslator.FromHtml("#F9FAFB");
            pnlLogoUpload.BorderColor = WBorder;
            pnlLogoUpload.BorderRadius = 12;
            pnlLogoUpload.BorderThickness = 1;
            pnlLogoUpload.ShadowDecoration.Enabled = false;
            pnlLogoUpload.Cursor = Cursors.Hand;
            lblLogoUploadText.Font = new Font("Segoe UI", 9F);
            lblLogoUploadText.ForeColor = WTextMuted;
            lblLogoUploadText.BackColor = Color.Transparent;

            StyleWizardCombo(cboTimezone);
            cboTimezone.Items.AddRange(new object[] { "UTC-8 (Pacific)", "UTC-5 (Eastern)", "UTC+0 (GMT)", "UTC+8 (SGT)" });

            // Step 2
            pnlCsvUpload.BackColor = Color.Transparent;
            pnlCsvUpload.FillColor = WGreenBg;
            pnlCsvUpload.BorderColor = WGreenBorder;
            pnlCsvUpload.BorderRadius = 12;
            pnlCsvUpload.BorderThickness = 1;
            pnlCsvUpload.ShadowDecoration.Enabled = false;
            pnlCsvUpload.Cursor = Cursors.Hand;
            lblCsvText.Font = new Font("Segoe UI", 10F);
            lblCsvText.ForeColor = WGreen;
            lblCsvText.BackColor = Color.Transparent;

            StyleWizardLabel(lblCurrencyLabel);
            StyleWizardLabel(lblTaxRateLabel);
            StyleWizardCombo(cboCurrency);
            cboCurrency.Items.AddRange(new object[] { "USD ($)", "EUR (€)", "GBP (£)", "JPY (¥)" });
            StyleWizardInput(txtTaxRate);

            pnlNoCsvInfo.BackColor = Color.Transparent;
            pnlNoCsvInfo.FillColor = ColorTranslator.FromHtml("#F9FAFB");
            pnlNoCsvInfo.BorderRadius = 12;
            pnlNoCsvInfo.BorderThickness = 0;
            pnlNoCsvInfo.ShadowDecoration.Enabled = false;
            lblNoCsvText.Font = new Font("Segoe UI", 8F);
            lblNoCsvText.ForeColor = WTextSecondary;
            lblNoCsvText.BackColor = Color.Transparent;

            // Step 3
            StyleWizardLabel(lblPinLabel);
            StyleWizardLabel(lblConfirmPinLabel);
            StyleWizardLabel(lblAutoLockLabel);
            StyleWizardInput(txtPin);
            StyleWizardInput(txtConfirmPin);
            StyleWizardCombo(cboAutoLock);
            cboAutoLock.Items.AddRange(new object[] { "5 minutes", "10 minutes", "15 minutes", "30 minutes", "Never" });

            lblPinHint.Font = new Font("Segoe UI", 8F);
            lblPinHint.ForeColor = WTextMuted;
            lblPinHint.BackColor = Color.Transparent;

            lblToggleLabel.Font = new Font("Segoe UI", 9F);
            lblToggleLabel.ForeColor = WTextPrimary;
            lblToggleLabel.BackColor = Color.Transparent;

            toggleDiscount.CheckedState.FillColor = WGreen;
            toggleDiscount.CheckedState.InnerColor = Color.White;
            toggleDiscount.UncheckedState.FillColor = Color.FromArgb(200, 200, 200);
            toggleDiscount.UncheckedState.InnerColor = Color.White;

            // Step 4
            pnlReviewIcon.BackColor = Color.Transparent;
            pnlReviewIcon.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(WGreen);
                g.FillEllipse(brush, 0, 0, 79, 79);
                using var font = new Font("Segoe UI", 32F, FontStyle.Bold);
                using var tBrush = new SolidBrush(Color.White);
                g.DrawString("\u2713", font, tBrush, 16, 12);
            };

            lblReviewTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblReviewTitle.ForeColor = WTextPrimary;
            lblReviewTitle.BackColor = Color.Transparent;

            lblReviewStore.Font = new Font("Segoe UI", 9F);
            lblReviewStore.ForeColor = WTextSecondary;
            lblReviewStore.BackColor = Color.Transparent;
            lblReviewProducts.Font = new Font("Segoe UI", 9F);
            lblReviewProducts.ForeColor = WTextSecondary;
            lblReviewProducts.BackColor = Color.Transparent;
            lblReviewSecurity.Font = new Font("Segoe UI", 9F);
            lblReviewSecurity.ForeColor = WTextSecondary;
            lblReviewSecurity.BackColor = Color.Transparent;

            // ── Footer ──
            pnlFooterBar.BackColor = Color.FromArgb(30, WBg);
            pnlFooterBar.Paint += (s, e) =>
            {
                using var pen = new Pen(WBorder, 1);
                e.Graphics.DrawLine(pen, 0, 0, pnlFooterBar.Width, 0);
            };

            btnBack.FillColor = Color.Transparent;
            btnBack.ForeColor = WTextSecondary;
            btnBack.Font = new Font("Segoe UI", 9F);
            btnBack.BorderThickness = 0;
            btnBack.Click += BtnBack_Click;

            lblStepIndicator.Font = new Font("Segoe UI", 9F);
            lblStepIndicator.ForeColor = WTextMuted;
            lblStepIndicator.BackColor = Color.Transparent;

            btnNext.FillColor = WGreen;
            btnNext.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnNext.ForeColor = Color.White;
            btnNext.BorderRadius = 8;
            btnNext.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnNext.BorderThickness = 0;
            btnNext.Click += BtnNext_Click;

            btnComplete.FillColor = ColorTranslator.FromHtml("#374151");
            btnComplete.HoverState.FillColor = ColorTranslator.FromHtml("#1F2937");
            btnComplete.ForeColor = Color.White;
            btnComplete.BorderRadius = 8;
            btnComplete.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnComplete.BorderThickness = 0;
            btnComplete.Click += BtnComplete_Click;

            // Bottom footer
            lblSetupDate.Font = new Font("Segoe UI", 8F);
            lblSetupDate.ForeColor = WTextSecondary;
            lblSetupDate.BackColor = Color.Transparent;
            lblKnowledgeBase.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblKnowledgeBase.ForeColor = WTextMuted;
            lblKnowledgeBase.BackColor = Color.Transparent;
            lblKnowledgeBase.Cursor = Cursors.Hand;
            lblGetSupport.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblGetSupport.ForeColor = WTextMuted;
            lblGetSupport.BackColor = Color.Transparent;
            lblGetSupport.Cursor = Cursors.Hand;
        }

        private void StyleWizardInput(Guna2TextBox txt)
        {
            txt.BorderRadius = 8;
            txt.BorderColor = WBorder;
            txt.FocusedState.BorderColor = WGreen;
            txt.ForeColor = WTextPrimary;
            txt.BackColor = Color.Transparent;
            txt.FillColor = WCard;
            txt.Font = new Font("Segoe UI", 9F);
            txt.PlaceholderForeColor = WTextMuted;
        }

        private void StyleWizardLabel(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lbl.ForeColor = WTextSecondary;
            lbl.BackColor = Color.Transparent;
        }

        private void StyleWizardCombo(Guna2ComboBox cbo)
        {
            cbo.BorderRadius = 8;
            cbo.BorderColor = WBorder;
            cbo.FocusedState.BorderColor = WGreen;
            cbo.Font = new Font("Segoe UI", 9F);
            cbo.ForeColor = WTextPrimary;
            cbo.BackColor = Color.Transparent;
            cbo.FillColor = WCard;
        }

        private void UpdateStepper()
        {
            pnlStepper.Invalidate();
        }

        private void PnlStepper_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            string[] stepLabels = { "STORE INFO", "PRODUCTS", "SECURITY", "REVIEW" };
            int circleSize = 48;
            int totalWidth = 4 * circleSize + 3 * 80;
            int startX = (pnlStepper.Width - totalWidth) / 2;
            int cy = 16;

            for (int i = 0; i < 4; i++)
            {
                int cx = startX + i * (circleSize + 80);
                bool isDone = i + 1 < _currentStep;
                bool isActive = i + 1 == _currentStep;
                bool isFuture = i + 1 > _currentStep;

                // Connector line (before this circle, except first)
                if (i > 0)
                {
                    int prevCx = startX + (i - 1) * (circleSize + 80) + circleSize;
                    Color lineColor = isDone || isActive ? Color.FromArgb(128, WGreen) : WBorder;
                    using var linePen = new Pen(lineColor, 2);
                    g.DrawLine(linePen, prevCx, cy + circleSize / 2, cx, cy + circleSize / 2);
                }

                // Active ring effect
                if (isActive)
                {
                    using var ringPen = new Pen(Color.FromArgb(50, WGreen), 4);
                    g.DrawEllipse(ringPen, cx - 4, cy - 4, circleSize + 8, circleSize + 8);
                }

                // Circle
                Color fillColor = isFuture ? WCard : WGreen;
                using var circleBrush = new SolidBrush(fillColor);
                g.FillEllipse(circleBrush, cx, cy, circleSize, circleSize);

                if (isFuture)
                {
                    using var borderPen = new Pen(WBorder, 1.5f);
                    g.DrawEllipse(borderPen, cx, cy, circleSize, circleSize);
                }

                // Icon/checkmark
                using var iconFont = new Font("Segoe UI", isDone ? 16F : 12F, FontStyle.Bold);
                Color iconColor = isFuture ? WTextMuted : Color.White;
                using var iconBrush = new SolidBrush(iconColor);
                string icon = isDone ? "\u2713" : (i + 1).ToString();
                var sz = g.MeasureString(icon, iconFont);
                g.DrawString(icon, iconFont, iconBrush,
                    cx + (circleSize - sz.Width) / 2, cy + (circleSize - sz.Height) / 2);

                // Label
                using var labelFont = new Font("Segoe UI", 8F, FontStyle.Bold);
                Color labelColor = isFuture ? WTextMuted : WGreen;
                using var labelBrush = new SolidBrush(labelColor);
                var lsz = g.MeasureString(stepLabels[i], labelFont);
                g.DrawString(stepLabels[i], labelFont, labelBrush,
                    cx + (circleSize - lsz.Width) / 2, cy + circleSize + 6);
            }
        }
    }
}
