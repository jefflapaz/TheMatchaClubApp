using System;
using System.Drawing;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class OpenSessionDialogForm : Form
    {
        public decimal StartingCash { get; private set; }

        public OpenSessionDialogForm(string cashierName, decimal defaultCash)
        {
            InitializeComponent();

            lblCashierVal!.Text = cashierName;
            lblDateTimeVal!.Text = DateTime.Now.ToString("MMM dd, yyyy \u2014 h:mm tt");
            
            txtStartingCash!.Text = $"₱{defaultCash:#,##0.00}";
            StartingCash = defaultCash;

            WireEvents();
            ValidateInput();
        }

        private void WireEvents()
        {
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            btnOpenSession.Click += BtnOpenSession_Click;
            
            txtStartingCash.TextChanged += (s, e) => ValidateInput();
            txtStartingCash.Leave += TxtStartingCash_Leave;
            txtStartingCash.KeyPress += TxtStartingCash_KeyPress;

            btnPreset200.Click += (s, e) => ApplyPreset(200);
            btnPreset500.Click += (s, e) => ApplyPreset(500);
            btnPreset1000.Click += (s, e) => ApplyPreset(1000);

            this.Load += (s, e) => {
                txtStartingCash.Focus();
                txtStartingCash.Select(txtStartingCash.Text.Length, 0);
            };
        }

        private void ApplyPreset(decimal amount)
        {
            txtStartingCash.Text = $"₱{amount:#,##0.00}";
            ValidateInput();
        }

        private void ValidateInput()
        {
            if (TryParseAmount(txtStartingCash.Text, out decimal actual))
            {
                StartingCash = actual;
                btnOpenSession.Enabled = true;
            }
            else
            {
                btnOpenSession.Enabled = false;
            }
        }

        private void TxtStartingCash_Leave(object? sender, EventArgs e)
        {
            if (TryParseAmount(txtStartingCash.Text, out decimal val))
            {
                txtStartingCash.Text = $"₱{val:#,##0.00}";
            }
        }

        private void TxtStartingCash_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && txtStartingCash.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void BtnOpenSession_Click(object? sender, EventArgs e)
        {
            if (btnOpenSession.Enabled)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool TryParseAmount(string input, out decimal amount)
        {
            string clean = input.Replace("₱", "").Replace(",", "").Trim();
            return decimal.TryParse(clean, out amount) && amount >= 0;
        }
    }
}
