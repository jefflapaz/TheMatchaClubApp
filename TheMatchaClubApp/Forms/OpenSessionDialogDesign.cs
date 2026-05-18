using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace TheMatchaClubApp.Forms
{
    public partial class OpenSessionDialogForm
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            
            // Core
            pnlMain = new Guna2Panel();
            lblTitle = new Label();
            
            // Info Section
            pnlInfo = new Guna2Panel();
            lblCashier = new Label();
            lblCashierVal = new Label();
            lblDateTime = new Label();
            lblDateTimeVal = new Label();
            
            // Input Section
            lblInputLabel = new Label();
            txtStartingCash = new Guna2TextBox();
            lblHelper = new Label();
            
            // Presets
            btnPreset200 = new Guna2Button();
            btnPreset500 = new Guna2Button();
            btnPreset1000 = new Guna2Button();
            
            // Buttons
            btnCancel = new Guna2Button();
            btnOpenSession = new Guna2Button();

            SuspendLayout();

            // Form Properties
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ColorTranslator.FromHtml("#F3F4F6"); // Dimmed background overlay effect
            this.TransparencyKey = ColorTranslator.FromHtml("#F3F4F6"); // To support shadow
            this.Size = new Size(540, 560); // Window container

            // ── Main Card ──
            pnlMain.Size = new Size(500, 520);
            pnlMain.Location = new Point(20, 20);
            pnlMain.FillColor = Color.White;
            pnlMain.BorderRadius = 16;
            pnlMain.ShadowDecoration.Enabled = true;
            pnlMain.ShadowDecoration.Depth = 15;
            pnlMain.ShadowDecoration.Shadow = new Padding(10);
            pnlMain.ShadowDecoration.Color = Color.FromArgb(40, 0, 0, 0);

            // Title
            lblTitle.Text = "Open Store Session";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = ColorTranslator.FromHtml("#111827");
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Location = new Point(32, 32);
            lblTitle.AutoSize = true;

            // ── Info Panel ──
            pnlInfo.Location = new Point(32, 80);
            pnlInfo.Size = new Size(436, 96);
            pnlInfo.FillColor = ColorTranslator.FromHtml("#F9FAFB");
            pnlInfo.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            pnlInfo.BorderThickness = 1;
            pnlInfo.BorderRadius = 8;

            int sy = 20; int sgap = 32;
            // Row 1
            SetupInfoLabel(lblCashier, "Cashier", 20, sy);
            SetupInfoValue(lblCashierVal, "-", 150, sy);
            sy += sgap;
            // Row 2
            SetupInfoLabel(lblDateTime, "Date / Time", 20, sy);
            SetupInfoValue(lblDateTimeVal, "-", 150, sy);

            pnlInfo.Controls.AddRange(new Control[] {
                lblCashier, lblCashierVal,
                lblDateTime, lblDateTimeVal
            });

            // ── Input Area ──
            lblInputLabel.Text = "Starting Cash Fund";
            lblInputLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblInputLabel.ForeColor = ColorTranslator.FromHtml("#374151");
            lblInputLabel.Location = new Point(32, 200);
            lblInputLabel.AutoSize = true;
            lblInputLabel.BackColor = Color.Transparent;

            txtStartingCash.Location = new Point(32, 228);
            txtStartingCash.Size = new Size(436, 60);
            txtStartingCash.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            txtStartingCash.BorderRadius = 8;
            txtStartingCash.BorderThickness = 1;
            txtStartingCash.BorderColor = ColorTranslator.FromHtml("#D1D5DB");
            txtStartingCash.FocusedState.BorderColor = ColorTranslator.FromHtml("#10B981");
            txtStartingCash.ForeColor = ColorTranslator.FromHtml("#111827");
            txtStartingCash.PlaceholderText = "₱0.00";
            txtStartingCash.TextAlign = HorizontalAlignment.Center;
            txtStartingCash.BackColor = Color.Transparent;

            lblHelper.Text = "Enter the opening cash currently placed inside the drawer.";
            lblHelper.Font = new Font("Segoe UI", 8.5F);
            lblHelper.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblHelper.Location = new Point(32, 296);
            lblHelper.Size = new Size(436, 20);
            lblHelper.TextAlign = ContentAlignment.MiddleCenter;
            lblHelper.BackColor = Color.Transparent;

            // ── Presets ──
            SetupPresetButton(btnPreset200, "₱200", 32, 332);
            SetupPresetButton(btnPreset500, "₱500", 182, 332);
            SetupPresetButton(btnPreset1000, "₱1000", 332, 332);

            // ── Buttons ──
            btnCancel.Text = "Cancel";
            btnCancel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCancel.ForeColor = ColorTranslator.FromHtml("#374151");
            btnCancel.FillColor = Color.White;
            btnCancel.HoverState.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            btnCancel.BorderColor = ColorTranslator.FromHtml("#D1D5DB");
            btnCancel.BorderThickness = 1;
            btnCancel.BorderRadius = 8;
            btnCancel.Location = new Point(32, 420);
            btnCancel.Size = new Size(130, 48);
            btnCancel.Cursor = Cursors.Hand;

            btnOpenSession.Text = "Open Session";
            btnOpenSession.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnOpenSession.ForeColor = Color.White;
            btnOpenSession.FillColor = ColorTranslator.FromHtml("#10B981");
            btnOpenSession.HoverState.FillColor = ColorTranslator.FromHtml("#059669");
            btnOpenSession.BorderThickness = 0;
            btnOpenSession.BorderRadius = 8;
            btnOpenSession.Location = new Point(178, 420);
            btnOpenSession.Size = new Size(290, 48);
            btnOpenSession.Cursor = Cursors.Hand;
            btnOpenSession.Enabled = false; // Disabled by default

            // Assemble
            pnlMain.Controls.AddRange(new Control[] {
                lblTitle, pnlInfo, 
                lblInputLabel, txtStartingCash, lblHelper,
                btnPreset200, btnPreset500, btnPreset1000,
                btnCancel, btnOpenSession
            });

            this.Controls.Add(pnlMain);
        }

        private void SetupInfoLabel(Label lbl, string text, int x, int y)
        {
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 9.5F);
            lbl.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.BackColor = Color.Transparent;
        }

        private void SetupInfoValue(Label lbl, string text, int x, int y)
        {
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lbl.ForeColor = ColorTranslator.FromHtml("#111827");
            lbl.Location = new Point(x, y);
            lbl.Size = new Size(260, 20);
            lbl.TextAlign = ContentAlignment.MiddleRight;
            lbl.BackColor = Color.Transparent;
        }

        private void SetupPresetButton(Guna2Button btn, string text, int x, int y)
        {
            btn.Text = text;
            btn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btn.ForeColor = ColorTranslator.FromHtml("#374151");
            btn.FillColor = Color.White;
            btn.HoverState.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            btn.BorderColor = ColorTranslator.FromHtml("#D1D5DB");
            btn.BorderThickness = 1;
            btn.BorderRadius = 6;
            btn.Location = new Point(x, y);
            btn.Size = new Size(136, 40);
            btn.Cursor = Cursors.Hand;
            btn.BackColor = Color.Transparent;
        }

        private Guna2Panel pnlMain;
        private Label lblTitle;
        
        private Guna2Panel pnlInfo;
        private Label lblCashier, lblCashierVal;
        private Label lblDateTime, lblDateTimeVal;
        
        private Label lblInputLabel, lblHelper;
        private Guna2TextBox txtStartingCash;
        
        private Guna2Button btnPreset200, btnPreset500, btnPreset1000;
        
        private Guna2Button btnCancel, btnOpenSession;
    }
}
