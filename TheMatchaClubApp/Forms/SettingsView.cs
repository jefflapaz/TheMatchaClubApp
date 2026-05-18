using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheMatchaClubApp.Core;
using TheMatchaClubApp.Helpers;

namespace TheMatchaClubApp.Forms
{
    public partial class SettingsView : UserControl
    {
        private string _activeTab = "Store Profile";
        private Guna.UI2.WinForms.Guna2Button[] _tabButtons = Array.Empty<Guna.UI2.WinForms.Guna2Button>();

        private readonly Panel[] _sectionPanels;

        public SettingsView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();

            _sectionPanels = new Panel[]
            {
                pnlStoreProfile, pnlSessionCash, pnlReceiptEditor,
                pnlExportBackup, pnlSecurity
            };

            InitializeDesign();
            ShowTab("Store Profile");
            LoadSettings();

            btnSaveAll.Click += async (s, e) => await SaveSettings();
            pnlLogoUpload.Click += PnlLogoUpload_Click;
            lblUploadText.Click += PnlLogoUpload_Click;

            // Receipt editor toggle changes refresh live preview
            chkShowCashier.CheckedChanged += (s, e) => pnlReceiptPreview.Invalidate();
            chkShowCustomer.CheckedChanged += (s, e) => pnlReceiptPreview.Invalidate();
            chkShowOrderType.CheckedChanged += (s, e) => pnlReceiptPreview.Invalidate();
            chkShowSessionNum.CheckedChanged += (s, e) => pnlReceiptPreview.Invalidate();
            txtReceiptFooterEditor.TextChanged += (s, e) => pnlReceiptPreview.Invalidate();

            // Store name changes also update preview
            txtStoreName.TextChanged += (s, e) => pnlReceiptPreview.Invalidate();
            txtOperatingLocation.TextChanged += (s, e) => pnlReceiptPreview.Invalidate();
            txtPhone.TextChanged += (s, e) => pnlReceiptPreview.Invalidate();
            txtSupportEmail.TextChanged += (s, e) => pnlReceiptPreview.Invalidate();
            txtCashierName.TextChanged += (s, e) => pnlReceiptPreview.Invalidate();

            // Input validations for Session & Cash textboxes
            txtDefaultCash.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
                if (e.KeyChar == '.' && txtDefaultCash.Text.Contains('.'))
                {
                    e.Handled = true;
                }
            };

            txtDefaultCash.Leave += (s, e) =>
            {
                if (decimal.TryParse(txtDefaultCash.Text.Trim(), out decimal val))
                {
                    txtDefaultCash.Text = val.ToString("F2");
                }
                else
                {
                    txtDefaultCash.Text = "0.00";
                }
            };

            txtSessionTimeout.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            txtSessionTimeout.Leave += (s, e) =>
            {
                if (!int.TryParse(txtSessionTimeout.Text.Trim(), out _))
                {
                    txtSessionTimeout.Text = "0";
                }
            };

            // ── Export & Backup handlers ──
            btnExportSales.Click += async (s, e) => await HandleExportSales();
            btnExportCustomers.Click += async (s, e) => await HandleExportCustomers();
            btnExportProducts.Click += async (s, e) => await HandleExportProducts();
            btnCreateBackup.Click += async (s, e) => await HandleCreateBackup();
            btnRestoreBackup.Click += async (s, e) => await HandleRestoreBackup();

            // Refresh backup info when Export & Backup tab is shown
            RefreshBackupInfo();
        }

        // ── Load Settings ────────────────────────────────────────────
        private void LoadSettings()
        {
            var s = Program.DataService.Settings;

            // Store Profile
            txtStoreName.Text = s.StoreName;
            txtSupportEmail.Text = s.Email;
            txtPhone.Text = s.Phone;
            txtCashierName.Text = string.IsNullOrWhiteSpace(s.CashierName) 
                ? (Program.CurrentUser?.FullName ?? "Admin") 
                : s.CashierName;

            // Location
            txtPopupLocation.Text = s.PopupLocationName;
            txtOperatingLocation.Text = s.CurrentOperatingLocation;

            // SMTP
            txtSmtpServer.Text = s.SmtpServer;
            txtSmtpPort.Text = s.SmtpPort.ToString();
            txtSmtpPassword.Text = s.SmtpPassword;

            // Session & Cash
            txtDefaultCash.Text = s.DefaultStartingCash.ToString("F2");
            txtSessionTimeout.Text = s.SessionTimeoutMinutes.ToString();
            chkRequireCashCount.Checked = s.RequireCashCountOnClose;
            chkOverShortWarnings.Checked = s.EnableOverShortWarnings;
            chkAutoZReport.Checked = s.AutoGenerateZReport;
            chkAutoLockQuickSale.Checked = s.AutoLockQuickSaleIfNoSession;

            // Receipt Editor
            chkShowCashier.Checked = s.ReceiptShowCashierName;
            chkShowCustomer.Checked = s.ReceiptShowCustomerName;
            chkShowOrderType.Checked = s.ReceiptShowOrderType;
            chkShowSessionNum.Checked = s.ReceiptShowSessionNumber;
            cmbPaperWidth.SelectedItem = s.ReceiptPaperWidth;
            if (cmbPaperWidth.SelectedIndex < 0) cmbPaperWidth.SelectedIndex = 1; // default 80mm
            txtReceiptFooterEditor.Text = s.ReceiptFooterMessage;


            // Logo preview
            UpdateLogoPreview(s.StoreLogoPath);
        }

        // ── Save Settings ────────────────────────────────────────────
        private async Task SaveSettings()
        {
            var s = Program.DataService.Settings;

            s.StoreName = txtStoreName.Text.Trim();
            s.Email = txtSupportEmail.Text.Trim();
            s.Phone = txtPhone.Text.Trim();
            s.CashierName = txtCashierName.Text.Trim();

            // Location
            s.PopupLocationName = txtPopupLocation.Text.Trim();
            s.CurrentOperatingLocation = txtOperatingLocation.Text.Trim();

            // SMTP
            s.SmtpServer = txtSmtpServer.Text.Trim();
            if (int.TryParse(txtSmtpPort.Text.Trim(), out int port)) s.SmtpPort = port;
            s.SmtpPassword = txtSmtpPassword.Text;

            // Session & Cash
            if (decimal.TryParse(txtDefaultCash.Text.Trim(), out decimal cash)) s.DefaultStartingCash = cash;
            if (int.TryParse(txtSessionTimeout.Text.Trim(), out int timeout)) s.SessionTimeoutMinutes = timeout;
            s.RequireCashCountOnClose = chkRequireCashCount.Checked;
            s.EnableOverShortWarnings = chkOverShortWarnings.Checked;
            s.AutoGenerateZReport = chkAutoZReport.Checked;
            s.AutoLockQuickSaleIfNoSession = chkAutoLockQuickSale.Checked;

            // Receipt Editor
            s.ReceiptShowCashierName = chkShowCashier.Checked;
            s.ReceiptShowCustomerName = chkShowCustomer.Checked;
            s.ReceiptShowOrderType = chkShowOrderType.Checked;
            s.ReceiptShowSessionNumber = chkShowSessionNum.Checked;
            s.ReceiptPaperWidth = cmbPaperWidth.SelectedItem?.ToString() ?? "80mm";
            s.ReceiptFooterMessage = txtReceiptFooterEditor.Text.Trim();



            await Program.DataService.SaveSettingsAsync();

            // Toast notification
            ShowSaveToast();
        }

        private void ShowSaveToast()
        {
            var toast = new Label
            {
                Text = "  ✅  Settings saved successfully",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#52B743"),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(320, 44),
                Padding = new Padding(12)
            };
            toast.Location = new Point((this.Width - toast.Width) / 2, this.Height - 80);
            this.Controls.Add(toast);
            toast.BringToFront();

            var timer = new System.Windows.Forms.Timer { Interval = 2500 };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                this.Controls.Remove(toast);
                toast.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }

        // ── Logo Upload ──────────────────────────────────────────────
        private void PnlLogoUpload_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp", Title = "Select Store Logo" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string destPath = Program.DataService.CopyImageToLocal(ofd.FileName);
                Program.DataService.Settings.StoreLogoPath = destPath;
                UpdateLogoPreview(destPath);
            }
        }

        private void UpdateLogoPreview(string path)
        {
            string fullPath = Program.DataService.GetFullImagePath(path);
            if (!string.IsNullOrWhiteSpace(fullPath) && File.Exists(fullPath))
            {
                lblUploadText.Text = "✅";
                try
                {
                    pnlLogoUpload.BackgroundImage = Image.FromFile(fullPath);
                    pnlLogoUpload.BackgroundImageLayout = ImageLayout.Zoom;
                }
                catch { lblUploadText.Text = "📷\nUPLOAD\nPNG/JPG"; }
            }
        }

        // ── Tab Navigation ───────────────────────────────────────────
        private void TabBtn_Click(object? sender, EventArgs e)
        {
            if (sender is Guna.UI2.WinForms.Guna2Button btn)
            {
                // Extract tab name (strip emoji prefix)
                for (int i = 0; i < _tabButtons.Length; i++)
                {
                    if (_tabButtons[i] == btn)
                    {
                        string[] tabNames = { "Store Profile", "Session & Cash", "Receipt Editor",
                                              "Export & Backup", "Security" };
                        ShowTab(tabNames[i]);
                        break;
                    }
                }
            }
        }

        private void ShowTab(string tabName)
        {
            _activeTab = tabName;
            string[] tabNames = { "Store Profile", "Session & Cash", "Receipt Editor", "Export & Backup", "Security" };

            for (int i = 0; i < _sectionPanels.Length; i++)
                _sectionPanels[i].Visible = tabNames[i] == tabName;

            lblSettingsTitle.Text = tabName;
            UpdateTabStyles();
        }

        // ══════════════════════════════════════════════════════════════
        //  EXPORT & BACKUP HANDLERS
        // ══════════════════════════════════════════════════════════════

        private async Task HandleExportSales()
        {
            try
            {
                btnExportSales.Text = "⏳  Exporting...";
                btnExportSales.Enabled = false;
                string path = await BackupService.ExportSalesCsvAsync(Program.DataService.Orders);
                btnExportSales.Text = "📊  Export Sales CSV";
                btnExportSales.Enabled = true;
                ShowExportSuccess($"Sales export completed successfully.\n\n📁 {path}", path);
            }
            catch (Exception ex)
            {
                btnExportSales.Text = "📊  Export Sales CSV";
                btnExportSales.Enabled = true;
                MessageBox.Show($"Export failed: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task HandleExportCustomers()
        {
            try
            {
                btnExportCustomers.Text = "⏳  Exporting...";
                btnExportCustomers.Enabled = false;
                string path = await BackupService.ExportCustomersCsvAsync(Program.DataService.Customers, Program.DataService.Orders);
                btnExportCustomers.Text = "👥  Export Customers CSV";
                btnExportCustomers.Enabled = true;
                ShowExportSuccess($"Customers export completed successfully.\n\n📁 {path}", path);
            }
            catch (Exception ex)
            {
                btnExportCustomers.Text = "👥  Export Customers CSV";
                btnExportCustomers.Enabled = true;
                MessageBox.Show($"Export failed: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task HandleExportProducts()
        {
            try
            {
                btnExportProducts.Text = "⏳  Exporting...";
                btnExportProducts.Enabled = false;
                string path = await BackupService.ExportProductsCsvAsync(Program.DataService.Products);
                btnExportProducts.Text = "📦  Export Products CSV";
                btnExportProducts.Enabled = true;
                ShowExportSuccess($"Products export completed successfully.\n\n📁 {path}", path);
            }
            catch (Exception ex)
            {
                btnExportProducts.Text = "📦  Export Products CSV";
                btnExportProducts.Enabled = true;
                MessageBox.Show($"Export failed: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task HandleCreateBackup()
        {
            try
            {
                btnCreateBackup.Text = "⏳  Creating backup...";
                btnCreateBackup.Enabled = false;
                string path = await BackupService.CreateFullBackupAsync();
                btnCreateBackup.Text = "🔒  Create Full Backup";
                btnCreateBackup.Enabled = true;
                RefreshBackupInfo();

                var result = MessageBox.Show(
                    $"✅ Backup created successfully!\n\n📁 {path}\n\nWould you like to open the backup folder?",
                    "Backup Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Process.Start("explorer.exe", Path.GetDirectoryName(path)!);
                }
            }
            catch (Exception ex)
            {
                btnCreateBackup.Text = "🔒  Create Full Backup";
                btnCreateBackup.Enabled = true;
                MessageBox.Show($"Backup failed: {ex.Message}", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task HandleRestoreBackup()
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Select Backup File to Restore",
                Filter = "ZIP Backup Files|*.zip",
                InitialDirectory = BackupService.GetDefaultBackupFolder()
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            // Validate backup
            try
            {
                var files = BackupService.ValidateBackup(ofd.FileName);
                bool hasData = files.Any(f => f.EndsWith(".json"));
                if (!hasData)
                {
                    MessageBox.Show("This file does not appear to be a valid MatchaPOS backup.\nNo data files found.",
                        "Invalid Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not read backup file: {ex.Message}",
                    "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirm restore
            var confirm = MessageBox.Show(
                "⚠️ WARNING: RESTORE BACKUP\n\n" +
                "This will OVERWRITE all current data including:\n" +
                "• Orders & Sales History\n" +
                "• Customer Records\n" +
                "• Products & Categories\n" +
                "• Session History\n" +
                "• Store Settings\n\n" +
                "This action cannot be undone.\n\n" +
                "Are you sure you want to proceed?",
                "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                btnRestoreBackup.Text = "⏳  Restoring...";
                btnRestoreBackup.Enabled = false;

                await BackupService.RestoreBackupAsync(ofd.FileName);

                // Reload all data
                await Program.DataService.LoadAllAsync();
                LoadSettings();
                RefreshBackupInfo();

                btnRestoreBackup.Text = "📂  Restore Backup";
                btnRestoreBackup.Enabled = true;

                MessageBox.Show("✅ Backup restored successfully!\n\nAll data has been reloaded.",
                    "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                btnRestoreBackup.Text = "📂  Restore Backup";
                btnRestoreBackup.Enabled = true;
                MessageBox.Show($"Restore failed: {ex.Message}", "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowExportSuccess(string message, string filePath)
        {
            var result = MessageBox.Show(
                $"{message}\n\nWould you like to open the export folder?",
                "Export Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(filePath)!);
            }
        }

        private void RefreshBackupInfo()
        {
            var (backupPath, backupDate, backupSize) = BackupService.GetLastBackupInfo();

            lblInfoLastBackup.Text = backupDate.HasValue
                ? backupDate.Value.ToString("MMM dd, yyyy  hh:mm tt")
                : "No backups yet";

            lblInfoBackupSize.Text = backupSize.HasValue
                ? BackupService.FormatSize(backupSize.Value)
                : "—";

            long dbSize = BackupService.GetDatabaseSizeBytes();
            lblInfoDbStatus.Text = "✅ Connected — Local JSON Store";
            lblInfoDbSize.Text = BackupService.FormatSize(dbSize);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
