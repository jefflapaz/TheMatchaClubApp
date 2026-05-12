using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class SettingsView : UserControl
    {
        private string _activeTab = "Store Profile";
        private Guna.UI2.WinForms.Guna2Button[] _tabButtons = Array.Empty<Guna.UI2.WinForms.Guna2Button>();

        public SettingsView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();
            ShowTab("Store Profile");

            LoadSettings();
            btnSaveAll.Click += async (s, e) => await SaveSettings();
        }

        // ── Load Settings ────────────────────────────────────────────
        private void LoadSettings()
        {
            var settings = Program.DataService.Settings;
            txtStoreName.Text = settings.StoreName;
            txtSupportEmail.Text = settings.Email;
            txtPhone.Text = settings.Phone;
            txtAddress.Text = settings.Address;
        }

        // ── Save Settings with Global Event ──────────────────────────
        private async Task SaveSettings()
        {
            var settings = Program.DataService.Settings;
            settings.StoreName = txtStoreName.Text;
            settings.Email = txtSupportEmail.Text;
            settings.Phone = txtPhone.Text;
            settings.Address = txtAddress.Text;

            await Program.DataService.SaveSettingsAsync();

            var msg = new Guna.UI2.WinForms.Guna2MessageDialog();
            msg.Style = Guna.UI2.WinForms.MessageDialogStyle.Light;
            msg.Caption = "Settings Saved";
            msg.Show("All changes saved successfully.\nBranding and receipt headers updated globally.");
        }

        // ── Tab Navigation ───────────────────────────────────────────
        private void TabBtn_Click(object? sender, EventArgs e)
        {
            if (sender is Guna.UI2.WinForms.Guna2Button btn)
            {
                ShowTab(btn.Text.Trim());
            }
        }

        private void ShowTab(string tabName)
        {
            _activeTab = tabName;
            pnlStoreProfile.Visible = tabName == "Store Profile";
            pnlPlaceholder.Visible = tabName != "Store Profile";
            if (tabName != "Store Profile")
                lblPlaceholderText.Text = $"{tabName} configuration panel loading...";
            UpdateTabStyles();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
