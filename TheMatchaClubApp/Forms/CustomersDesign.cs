using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class CustomersView
    {
        private static readonly Color CBgColor = ColorTranslator.FromHtml("#FAFAFA");
        private static readonly Color CCardBg = Color.White;
        private static readonly Color CBorderLight = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color CHeaderBg = ColorTranslator.FromHtml("#F9FAFB");
        private static readonly Color CTextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color CTextSecondary = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color CTextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color CTextBody = ColorTranslator.FromHtml("#374151");
        private static readonly Color CGreen = ColorTranslator.FromHtml("#52B743");
        private static readonly Color CGreenBg = ColorTranslator.FromHtml("#F2FAEF");

        private void InitializeDesign()
        {
            this.BackColor = CBgColor;
            this.Dock = DockStyle.Fill;

            // Top header
            pnlTopHeader.BackColor = CCardBg;
            pnlTopHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(CBorderLight, 1);
                e.Graphics.DrawLine(pen, 0, pnlTopHeader.Height - 1, pnlTopHeader.Width, pnlTopHeader.Height - 1);
            };

            lblChevron.Font = new Font("Segoe UI", 8F);
            lblChevron.ForeColor = CTextMuted;
            lblChevron.BackColor = Color.Transparent;

            lblViewName.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblViewName.ForeColor = CTextPrimary;
            lblViewName.BackColor = Color.Transparent;

            btnAddCustomer.FillColor = CGreen;
            btnAddCustomer.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnAddCustomer.ForeColor = Color.White;
            btnAddCustomer.BorderRadius = 8;
            btnAddCustomer.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnAddCustomer.BorderThickness = 0;

            // Split Container
            splitContainerMain.BackColor = CBorderLight;
            splitContainerMain.Panel1.BackColor = CCardBg;
            splitContainerMain.Panel2.BackColor = CBgColor;

            // Directory Pane
            txtSearch.BorderRadius = 6;
            txtSearch.FillColor = CCardBg;
            txtSearch.PlaceholderForeColor = CTextMuted;
            txtSearch.Font = new Font("Segoe UI", 9F);
            
            pnlFilters.BackColor = CCardBg;
            
            StyleFilterButton(btnFilterAll, true);
            StyleFilterButton(btnFilterRegular, false);
            StyleFilterButton(btnFilterNew, false);

            // Profile Pane
            lblProfileName.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblProfileName.ForeColor = CTextPrimary;
            
            lblProfileEmail.Font = new Font("Segoe UI", 10F);
            lblProfileEmail.ForeColor = CTextSecondary;
            
            lblProfilePhone.Font = new Font("Segoe UI", 10F);
            lblProfilePhone.ForeColor = CTextSecondary;
            
            chipStatus.FillColor = CGreenBg;
            chipStatus.ForeColor = CGreen;
            chipStatus.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            chipStatus.BorderColor = CGreen;
            chipStatus.BorderThickness = 1;

            StyleOutlineButton(btnEmail);
            StyleOutlineButton(btnEditProfile);
            StyleOutlineButton(btnExport);

            lblHistoryTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblHistoryTitle.ForeColor = CTextPrimary;

            StyleOutlineButton(btnViewOrders);
            btnViewOrders.Font = new Font("Segoe UI", 9F);
            btnViewOrders.Size = new Size(130, 30);

            // DataGrid - Matcha Green Header
            dgvHistory.ThemeStyle.HeaderStyle.BackColor = CGreen;
            dgvHistory.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvHistory.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI Semibold", 9F);
            dgvHistory.ThemeStyle.RowsStyle.BackColor = CCardBg;
            dgvHistory.ThemeStyle.RowsStyle.ForeColor = CTextBody;
            dgvHistory.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvHistory.ThemeStyle.RowsStyle.SelectionBackColor = CHeaderBg;
            dgvHistory.ThemeStyle.RowsStyle.SelectionForeColor = CTextPrimary;
            dgvHistory.GridColor = CBorderLight;
            dgvHistory.RowTemplate.Height = 50;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // Intelligence
            pnlPreferences.BorderRadius = 8;
            pnlPreferences.BorderThickness = 1;
            pnlPreferences.BorderColor = CBorderLight;
            pnlPreferences.FillColor = CCardBg;
            
            pnlAdminNotes.BorderRadius = 8;
            pnlAdminNotes.BorderThickness = 1;
            pnlAdminNotes.BorderColor = CBorderLight;
            pnlAdminNotes.FillColor = CCardBg;

            lblPrefTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPrefTitle.ForeColor = CTextMuted;
            lblNotesTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNotesTitle.ForeColor = CTextMuted;

            lblFavCatLabel.Font = new Font("Segoe UI", 9F);
            lblFavCatLabel.ForeColor = CTextBody;
            lblModLabel.Font = new Font("Segoe UI", 9F);
            lblModLabel.ForeColor = CTextBody;
            lblTimeLabel.Font = new Font("Segoe UI", 9F);
            lblTimeLabel.ForeColor = CTextBody;

            StyleIntelligenceChip(lblFavCatValue);
            StyleIntelligenceChip(lblModValue);
            StyleIntelligenceChip(lblTimeValue);

            txtAdminNotes.BorderRadius = 6;
            txtAdminNotes.Font = new Font("Segoe UI", 9F);
            txtAdminNotes.ForeColor = CTextBody;

            btnSaveNote.FillColor = CGreen;
            btnSaveNote.ForeColor = Color.White;
            btnSaveNote.BorderRadius = 6;
            btnSaveNote.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            // Calendar Popup
            pnlCalendarPopup.BorderRadius = 8;
            pnlCalendarPopup.FillColor = CCardBg;
            pnlCalendarPopup.BorderColor = CBorderLight;
            pnlCalendarPopup.BorderThickness = 1;
            // Add slight shadow using WinForms properties if supported, but let's keep it simple
            
            lblCalendarTitle.Font = new Font("Segoe UI Semibold", 10F);
            lblCalendarTitle.ForeColor = CTextPrimary;
            
            btnCalendarClose.FillColor = Color.Transparent;
            btnCalendarClose.ForeColor = CTextSecondary;
            btnCalendarClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void StyleFilterButton(Guna.UI2.WinForms.Guna2Button btn, bool active)
        {
            btn.BorderRadius = 15;
            btn.Font = new Font("Segoe UI Semibold", 9F);
            if (active)
            {
                btn.FillColor = CGreen;
                btn.ForeColor = Color.White;
                btn.BorderThickness = 0;
            }
            else
            {
                btn.FillColor = CCardBg;
                btn.ForeColor = CTextBody;
                btn.BorderColor = CBorderLight;
                btn.BorderThickness = 1;
            }
        }

        private void StyleOutlineButton(Guna.UI2.WinForms.Guna2Button btn)
        {
            btn.FillColor = CCardBg;
            btn.ForeColor = CTextPrimary;
            btn.BorderColor = CBorderLight;
            btn.BorderThickness = 1;
            btn.BorderRadius = 6;
            btn.Font = new Font("Segoe UI Semibold", 9F);
        }

        private void StyleIntelligenceChip(Guna.UI2.WinForms.Guna2Chip chip)
        {
            chip.FillColor = CCardBg;
            chip.ForeColor = CTextBody;
            chip.BorderColor = CBorderLight;
            chip.BorderThickness = 1;
            chip.Font = new Font("Segoe UI Semibold", 8F);
            chip.TextAlign = HorizontalAlignment.Left;
        }
    }
}
