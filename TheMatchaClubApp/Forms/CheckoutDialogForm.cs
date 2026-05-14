using System;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    /// <summary>
    /// Checkout sub-form: select Dine-In/Take-Out and link/create a Customer.
    /// </summary>
    public partial class CheckoutDialogForm : Form
    {
        // ── Public Results ───────────────────────────────────────────
        public bool IsDineIn => _isDineIn;
        public string SelectedOrderType => _isDineIn ? "Dine-In" : "Take-Out";
        public Customer? SelectedCustomer { get; private set; }
        public decimal CashReceived { get; private set; }
        public decimal ChangeDue { get; private set; }

        private bool _isDineIn = true;
        private decimal _totalAmount;

        // ── Constructor ──────────────────────────────────────────────
        public CheckoutDialogForm(decimal totalAmount)
        {
            _totalAmount = totalAmount;
            InitializeComponent();
            InitializeDesign();

            lblTotalValue.Text = totalAmount.ToString("C2");
            btnConfirm.Enabled = false;
            txtCash.Text = string.Empty;

            LoadCustomerList();

            // Wiring
            cboCustomer.SelectedIndexChanged += CboCustomer_SelectedIndexChanged;
            btnConfirm.Click += BtnConfirm_Click;
            txtCash.TextChanged += TxtCash_TextChanged;
        }

        // ── Load Customers into ComboBox ─────────────────────────────
        private void LoadCustomerList()
        {
            cboCustomer.Items.Clear();
            cboCustomer.Items.Add("— No Customer (Walk-In) —");

            foreach (var c in Program.DataService.Customers)
            {
                cboCustomer.Items.Add($"{c.Name} ({c.Phone})");
            }

            cboCustomer.SelectedIndex = 0;
        }

        // ── Auto-fill when existing customer selected ────────────────
        private void CboCustomer_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int idx = cboCustomer.SelectedIndex;
            if (idx <= 0)
            {
                // Walk-in or none selected — enable new customer fields
                txtNewName.Enabled = true;
                txtNewEmail.Enabled = true;
                return;
            }

            // Existing customer selected — auto-fill name/email
            var customer = Program.DataService.Customers.ElementAtOrDefault(idx - 1);
            if (customer != null)
            {
                txtNewName.Text = customer.Name;
                txtNewEmail.Text = customer.Email;
                txtNewName.Enabled = false;
                txtNewEmail.Enabled = false;
            }
        }

        // ── Cash Input ───────────────────────────────────────────────
        private void TxtCash_TextChanged(object? sender, EventArgs e)
        {
            if (decimal.TryParse(txtCash.Text, out decimal cash))
            {
                decimal change = cash - _totalAmount;
                lblChange.Text = change >= 0 ? change.ToString("C2") : "₱0.00";
                btnConfirm.Enabled = cash >= _totalAmount;
            }
            else
            {
                lblChange.Text = "₱0.00";
                btnConfirm.Enabled = false;
            }
        }

        // ── Confirm ──────────────────────────────────────────────────
        private void BtnConfirm_Click(object? sender, EventArgs e)
        {
            if (!decimal.TryParse(txtCash.Text, out decimal cash) || cash < _totalAmount) return;

            CashReceived = cash;
            ChangeDue = cash - _totalAmount;

            int idx = cboCustomer.SelectedIndex;

            if (idx > 0)
            {
                // Existing customer
                SelectedCustomer = Program.DataService.Customers.ElementAtOrDefault(idx - 1);
            }
            else if (!string.IsNullOrWhiteSpace(txtNewName.Text))
            {
                // New customer — create and persist
                var newCust = new Customer
                {
                    Name = txtNewName.Text.Trim(),
                    Email = txtNewEmail.Text.Trim(),
                    MemberSince = DateTime.Now
                };
                Program.DataService.Customers.Add(newCust);
                _ = Program.DataService.SaveCustomersAsync(); // fire-and-forget
                SelectedCustomer = newCust;
            }
            else
            {
                // Walk-in — no customer linked
                SelectedCustomer = null;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
