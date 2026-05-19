using System;
using System.Drawing;
using System.Windows.Forms;
using TheMatchaClub.Services;
using TheMatchaClubDomain.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class CloseSessionDialogForm : Form
    {
        private BusinessSession _session;
        private decimal _expectedCash;
        private bool _enableOverShortWarnings;

        public decimal ActualCashCounted { get; private set; }

        public CloseSessionDialogForm(BusinessSession activeSession)
        {
            InitializeComponent();
            _session = activeSession;
            _enableOverShortWarnings = Program.DataService.Settings.EnableOverShortWarnings;
            
            // Calculate totals
            Program.SessionService.ComputeSessionTotals(_session);
            _expectedCash = _session.StartingCash + _session.TotalRevenue;

            PopulateSummary();
            WireEvents();
        }

        private void PopulateSummary()
        {
            lblStartingCashVal.Text = $"₱{_session.StartingCash:#,##0.00}";
            lblSalesTotalVal.Text = $"₱{_session.TotalRevenue:#,##0.00}";
            lblExpectedCashVal.Text = $"₱{_expectedCash:#,##0.00}";
            
            var duration = DateTime.Now - _session.OpenedAt;
            lblDurationVal.Text = $"{(int)duration.TotalHours}h {duration.Minutes}m";
            
            lblTotalOrdersVal.Text = _session.TotalTransactions.ToString();
            lblCashierVal.Text = _session.OpenedBy;
        }

        private void WireEvents()
        {
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            
            txtActualCash.TextChanged += TxtActualCash_TextChanged;
            txtActualCash.KeyPress += TxtActualCash_KeyPress;
            
            chkConfirm.CheckedChanged += (s, e) => ValidateState();
            
            btnCloseSession.Click += BtnCloseSession_Click;

            // Optional: Draw subtle border to separate header
            pnlMain.Paint += (s, e) => {
                e.Graphics.DrawLine(Pens.LightGray, 32, 64, pnlMain.Width - 32, 64);
            };
        }

        private void TxtActualCash_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Allow numbers, control characters, and single decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && txtActualCash.Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        private void TxtActualCash_TextChanged(object? sender, EventArgs e)
        {
            ValidateState();
        }

        private void ValidateState()
        {
            if (string.IsNullOrWhiteSpace(txtActualCash.Text))
            {
                lblDifferenceVal.Text = "₱0.00";
                lblDifferenceVal.ForeColor = ColorTranslator.FromHtml("#D1D5DB");
                lblWarningText.Text = "Enter actual cash to see difference.";
                lblWarningText.ForeColor = ColorTranslator.FromHtml("#6B7280");
                chkConfirm.Visible = false;
                btnCloseSession.Enabled = false;
                return;
            }

            if (decimal.TryParse(txtActualCash.Text, out decimal actual))
            {
                decimal diff = actual - _expectedCash;
                lblDifferenceVal.Text = $"₱{Math.Abs(diff):#,##0.00}";

                bool isDiscrepancy = diff != 0;

                if (diff == 0)
                {
                    lblDifferenceVal.ForeColor = ColorTranslator.FromHtml("#10B981"); // Green
                    lblWarningText.Text = "Balanced. Cash count matches expected.";
                    lblWarningText.ForeColor = ColorTranslator.FromHtml("#10B981");
                    chkConfirm.Visible = false;
                }
                else if (diff < 0)
                {
                    lblDifferenceVal.ForeColor = ColorTranslator.FromHtml("#EF4444"); // Red
                    lblWarningText.Text = $"Cash drawer is SHORT by ₱{Math.Abs(diff):#,##0.00}";
                    lblWarningText.ForeColor = ColorTranslator.FromHtml("#EF4444");
                    if (_enableOverShortWarnings) chkConfirm.Visible = true;
                }
                else if (diff > 0)
                {
                    lblDifferenceVal.ForeColor = ColorTranslator.FromHtml("#F59E0B"); // Orange/Yellow
                    lblWarningText.Text = $"Cash drawer is OVER by ₱{Math.Abs(diff):#,##0.00}";
                    lblWarningText.ForeColor = ColorTranslator.FromHtml("#F59E0B");
                    if (_enableOverShortWarnings) chkConfirm.Visible = true;
                }

                // Enable logic
                if (isDiscrepancy && _enableOverShortWarnings)
                {
                    btnCloseSession.Enabled = chkConfirm.Checked;
                }
                else
                {
                    btnCloseSession.Enabled = true;
                }
            }
        }

        private void BtnCloseSession_Click(object? sender, EventArgs e)
        {
            if (decimal.TryParse(txtActualCash.Text, out decimal actual))
            {
                ActualCashCounted = actual;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
