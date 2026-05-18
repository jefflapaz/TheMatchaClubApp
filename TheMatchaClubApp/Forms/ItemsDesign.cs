using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace TheMatchaClubApp.Forms
{
    public partial class ItemsView
    {
        private static readonly Color IBgColor = ColorTranslator.FromHtml("#FAFAFA");
        private static readonly Color ICardBg = Color.White;
        private static readonly Color IBorderLight = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color ITextPrimary = ColorTranslator.FromHtml("#111827");
        private static readonly Color ITextSecondary = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color ITextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color IGreen = ColorTranslator.FromHtml("#52B743");
        private static readonly Color IGreenBg = ColorTranslator.FromHtml("#F2FAEF");
        private static readonly Color IGreenBorder = ColorTranslator.FromHtml("#E2F3DD");

        private Guna2Button[] _catButtons = Array.Empty<Guna2Button>();

        private void InitializeDesign()
        {
            this.BackColor = IBgColor;
            this.Dock = DockStyle.Fill;

            // Global Sub Header
            pnlGlobalSubHeader.BackColor = ICardBg;
            pnlGlobalSubHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(IBorderLight, 1);
                e.Graphics.DrawLine(pen, 0, pnlGlobalSubHeader.Height - 1, pnlGlobalSubHeader.Width, pnlGlobalSubHeader.Height - 1);
            };

            lblTotalItems.Font = new Font("Segoe UI", 9F);
            lblTotalItems.ForeColor = ITextSecondary;
            lblTotalItems.BackColor = Color.Transparent;

            btnAddItem.FillColor = IGreen;
            btnAddItem.ForeColor = Color.White;
            btnAddItem.BorderRadius = 4;
            btnAddItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAddItem.Cursor = Cursors.Hand;
            btnAddItem.Click += (s, e) => ShowAddDialog();

            // Sidebar
            pnlSidebar.BackColor = Color.Transparent;
            pnlSidebar.FillColor = ICardBg;
            pnlSidebar.BorderThickness = 0;
            pnlSidebar.ShadowDecoration.Enabled = false;
            pnlSidebar.Paint += (s, e) =>
            {
                using var pen = new Pen(IBorderLight, 1);
                e.Graphics.DrawLine(pen, pnlSidebar.Width - 1, 0, pnlSidebar.Width - 1, pnlSidebar.Height);
            };

            lblCategoriesHeader.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblCategoriesHeader.ForeColor = ITextMuted;
            lblCategoriesHeader.BackColor = Color.Transparent;

            // Category buttons
            // Handled dynamically in ItemsView.cs -> PopulateCategories()

            // Right area
            pnlRightArea.BackColor = IBgColor;

            // Main Header
            pnlHeaderMain.BackColor = IBgColor;
            
            lblItemCount.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblItemCount.ForeColor = ITextPrimary;
            lblItemCount.BackColor = Color.Transparent;

            cmbSort.Items.AddRange(new object[] { "Newest", "Oldest", "A → Z", "Z → A" });
            cmbSort.StartIndex = 0;
            cmbSort.Font = new Font("Segoe UI", 9F);
            cmbSort.ForeColor = ITextPrimary;
            cmbSort.BorderColor = IBorderLight;
            cmbSort.BorderRadius = 4;
            cmbSort.FillColor = ICardBg;
            cmbSort.TextOffset = new Point(3, 0);
            cmbSort.SelectedIndexChanged += (s, e) => PopulateItems(_activeCategory);

            flpItems.BackColor = IBgColor;
        }
    }
}
