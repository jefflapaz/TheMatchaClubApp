using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace TheMatchaClubApp.Forms
{
    public partial class CloseSessionDialogForm
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
            
            // Financial Summary Section
            pnlSummary = new Guna2Panel();
            lblStartingCash = new Label();
            lblStartingCashVal = new Label();
            lblSalesTotal = new Label();
            lblSalesTotalVal = new Label();
            lblExpectedCash = new Label();
            lblExpectedCashVal = new Label();
            lblDuration = new Label();
            lblDurationVal = new Label();
            lblTotalOrders = new Label();
            lblTotalOrdersVal = new Label();
            lblCashier = new Label();
            lblCashierVal = new Label();
            
            // Input Section
            lblInputLabel = new Label();
            txtActualCash = new Guna2TextBox();
            lblHelper = new Label();
            
            // Difference / Inline Warning Section
            pnlWarning = new Guna2Panel();
            lblDifferenceTitle = new Label();
            lblDifferenceVal = new Label();
            lblWarningText = new Label();
            chkConfirm = new Guna2CheckBox();
            
            // Buttons
            btnCancel = new Guna2Button();
            btnCloseSession = new Guna2Button();

            SuspendLayout();

            // Form Properties
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ColorTranslator.FromHtml("#F3F4F6"); // Dimmed background overlay effect
            this.TransparencyKey = ColorTranslator.FromHtml("#F3F4F6"); // To support shadow
            this.Size = new Size(540, 720); // Window container

            // ── Main Card ──
            pnlMain.Size = new Size(500, 680);
            pnlMain.Location = new Point(20, 20);
            pnlMain.FillColor = Color.White;
            pnlMain.BorderRadius = 16;
            pnlMain.ShadowDecoration.Enabled = true;
            pnlMain.ShadowDecoration.Depth = 15;
            pnlMain.ShadowDecoration.Shadow = new Padding(10);
            pnlMain.ShadowDecoration.Color = Color.FromArgb(40, 0, 0, 0);

            // Title
            lblTitle.Text = "Close Store Session";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = ColorTranslator.FromHtml("#111827");
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Location = new Point(32, 32);
            lblTitle.AutoSize = true;

            // ── Financial Summary Panel ──
            pnlSummary.Location = new Point(32, 80);
            pnlSummary.Size = new Size(436, 170);
            pnlSummary.FillColor = ColorTranslator.FromHtml("#F9FAFB");
            pnlSummary.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            pnlSummary.BorderThickness = 1;
            pnlSummary.BorderRadius = 8;

            int sy = 20; int sgap = 24;
            // Row 1
            SetupSummaryLabel(lblStartingCash, "Starting Cash", 20, sy);
            SetupSummaryValue(lblStartingCashVal, "₱0.00", 200, sy);
            sy += sgap;
            // Row 2
            SetupSummaryLabel(lblSalesTotal, "Sales Total", 20, sy);
            SetupSummaryValue(lblSalesTotalVal, "₱0.00", 200, sy);
            sy += sgap;
            // Row 3 (Expected)
            SetupSummaryLabel(lblExpectedCash, "Expected Cash", 20, sy);
            lblExpectedCash.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            SetupSummaryValue(lblExpectedCashVal, "₱0.00", 200, sy);
            lblExpectedCashVal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblExpectedCashVal.ForeColor = ColorTranslator.FromHtml("#059669");
            sy += sgap + 8;
            // Row 4
            SetupSummaryLabel(lblDuration, "Session Duration", 20, sy);
            SetupSummaryValue(lblDurationVal, "-", 200, sy);
            sy += sgap;
            // Row 5
            SetupSummaryLabel(lblTotalOrders, "Total Orders", 20, sy);
            SetupSummaryValue(lblTotalOrdersVal, "0", 200, sy);
            sy += sgap;
            // Row 6
            SetupSummaryLabel(lblCashier, "Cashier", 20, sy);
            SetupSummaryValue(lblCashierVal, "-", 200, sy);

            pnlSummary.Controls.AddRange(new Control[] {
                lblStartingCash, lblStartingCashVal,
                lblSalesTotal, lblSalesTotalVal,
                lblExpectedCash, lblExpectedCashVal,
                lblDuration, lblDurationVal,
                lblTotalOrders, lblTotalOrdersVal,
                lblCashier, lblCashierVal
            });

            // ── Input Area ──
            lblInputLabel.Text = "Actual Cash Counted";
            lblInputLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblInputLabel.ForeColor = ColorTranslator.FromHtml("#374151");
            lblInputLabel.Location = new Point(32, 280);
            lblInputLabel.AutoSize = true;
            lblInputLabel.BackColor = Color.Transparent;

            txtActualCash.Location = new Point(32, 308);
            txtActualCash.Size = new Size(436, 60);
            txtActualCash.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            txtActualCash.BorderRadius = 8;
            txtActualCash.BorderThickness = 1;
            txtActualCash.BorderColor = ColorTranslator.FromHtml("#D1D5DB");
            txtActualCash.FocusedState.BorderColor = ColorTranslator.FromHtml("#10B981");
            txtActualCash.ForeColor = ColorTranslator.FromHtml("#111827");
            txtActualCash.PlaceholderText = "₱0.00";
            txtActualCash.TextAlign = HorizontalAlignment.Center;
            txtActualCash.BackColor = Color.Transparent;

            lblHelper.Text = "Please count all physical cash currently inside the drawer.";
            lblHelper.Font = new Font("Segoe UI", 8.5F);
            lblHelper.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblHelper.Location = new Point(32, 376);
            lblHelper.Size = new Size(436, 20);
            lblHelper.TextAlign = ContentAlignment.MiddleCenter;
            lblHelper.BackColor = Color.Transparent;

            // ── Difference / Inline Warning Section ──
            pnlWarning.Location = new Point(32, 410);
            pnlWarning.Size = new Size(436, 150);
            pnlWarning.FillColor = ColorTranslator.FromHtml("#F9FAFB");
            pnlWarning.BorderColor = ColorTranslator.FromHtml("#E5E7EB");
            pnlWarning.BorderThickness = 1;
            pnlWarning.BorderRadius = 8;

            lblDifferenceTitle.Text = "Difference";
            lblDifferenceTitle.Font = new Font("Segoe UI", 10F);
            lblDifferenceTitle.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblDifferenceTitle.Location = new Point(0, 16);
            lblDifferenceTitle.Size = new Size(436, 20);
            lblDifferenceTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblDifferenceTitle.BackColor = Color.Transparent;

            lblDifferenceVal.Text = "₱0.00";
            lblDifferenceVal.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblDifferenceVal.ForeColor = ColorTranslator.FromHtml("#D1D5DB"); // Default color
            lblDifferenceVal.Location = new Point(0, 36);
            lblDifferenceVal.Size = new Size(436, 45);
            lblDifferenceVal.TextAlign = ContentAlignment.MiddleCenter;
            lblDifferenceVal.BackColor = Color.Transparent;

            lblWarningText.Text = "Enter actual cash to see difference.";
            lblWarningText.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblWarningText.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lblWarningText.Location = new Point(0, 84);
            lblWarningText.Size = new Size(436, 20);
            lblWarningText.TextAlign = ContentAlignment.MiddleCenter;
            lblWarningText.BackColor = Color.Transparent;

            chkConfirm.Text = "I confirm the counted cash amount is correct.";
            chkConfirm.Font = new Font("Segoe UI", 8.5F);
            chkConfirm.ForeColor = ColorTranslator.FromHtml("#111827");
            chkConfirm.CheckedState.BorderColor = ColorTranslator.FromHtml("#10B981");
            chkConfirm.CheckedState.BorderRadius = 2;
            chkConfirm.CheckedState.BorderThickness = 0;
            chkConfirm.CheckedState.FillColor = ColorTranslator.FromHtml("#10B981");
            chkConfirm.UncheckedState.BorderColor = ColorTranslator.FromHtml("#D1D5DB");
            chkConfirm.UncheckedState.BorderRadius = 2;
            chkConfirm.UncheckedState.BorderThickness = 1;
            chkConfirm.UncheckedState.FillColor = Color.White;
            chkConfirm.Location = new Point(32, 116);
            chkConfirm.Size = new Size(380, 20);
            chkConfirm.BackColor = Color.Transparent;
            chkConfirm.Visible = false; // Only visible if discrepancy

            pnlWarning.Controls.AddRange(new Control[] {
                lblDifferenceTitle, lblDifferenceVal, lblWarningText, chkConfirm
            });

            // ── Buttons ──
            btnCancel.Text = "Cancel";
            btnCancel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCancel.ForeColor = ColorTranslator.FromHtml("#374151");
            btnCancel.FillColor = Color.White;
            btnCancel.HoverState.FillColor = ColorTranslator.FromHtml("#F3F4F6");
            btnCancel.BorderColor = ColorTranslator.FromHtml("#D1D5DB");
            btnCancel.BorderThickness = 1;
            btnCancel.BorderRadius = 8;
            btnCancel.Location = new Point(32, 590);
            btnCancel.Size = new Size(130, 48);
            btnCancel.Cursor = Cursors.Hand;

            btnCloseSession.Text = "Close Session";
            btnCloseSession.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCloseSession.ForeColor = Color.White;
            btnCloseSession.FillColor = ColorTranslator.FromHtml("#10B981");
            btnCloseSession.HoverState.FillColor = ColorTranslator.FromHtml("#059669");
            btnCloseSession.BorderThickness = 0;
            btnCloseSession.BorderRadius = 8;
            btnCloseSession.Location = new Point(178, 590);
            btnCloseSession.Size = new Size(290, 48);
            btnCloseSession.Cursor = Cursors.Hand;
            btnCloseSession.Enabled = false; // Disabled by default until valid amount

            // Assemble
            pnlMain.Controls.AddRange(new Control[] {
                lblTitle, pnlSummary, 
                lblInputLabel, txtActualCash, lblHelper,
                pnlWarning,
                btnCancel, btnCloseSession
            });

            this.Controls.Add(pnlMain);
        }

        private void SetupSummaryLabel(Label lbl, string text, int x, int y)
        {
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 9.5F);
            lbl.ForeColor = ColorTranslator.FromHtml("#6B7280");
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.BackColor = Color.Transparent;
        }

        private void SetupSummaryValue(Label lbl, string text, int x, int y)
        {
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lbl.ForeColor = ColorTranslator.FromHtml("#111827");
            lbl.Location = new Point(x, y);
            lbl.Size = new Size(210, 20);
            lbl.TextAlign = ContentAlignment.MiddleRight;
            lbl.BackColor = Color.Transparent;
        }

        private Guna2Panel pnlMain;
        private Label lblTitle;
        
        private Guna2Panel pnlSummary;
        private Label lblStartingCash, lblStartingCashVal;
        private Label lblSalesTotal, lblSalesTotalVal;
        private Label lblExpectedCash, lblExpectedCashVal;
        private Label lblDuration, lblDurationVal;
        private Label lblTotalOrders, lblTotalOrdersVal;
        private Label lblCashier, lblCashierVal;
        
        private Label lblInputLabel, lblHelper;
        private Guna2TextBox txtActualCash;
        
        private Guna2Panel pnlWarning;
        private Label lblDifferenceTitle, lblDifferenceVal, lblWarningText;
        private Guna2CheckBox chkConfirm;
        
        private Guna2Button btnCancel, btnCloseSession;
    }
}
