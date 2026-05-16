using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    /// <summary>
    /// Checkout sub-form: select Dine-In/Take-Out, search/create Customer, and process cash payment.
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
        private Customer? _selectedExistingCustomer;
        private bool _suppressSearch;
        private bool _isProcessing; // Prevent double-click

        // ── Constructor ──────────────────────────────────────────────
        public CheckoutDialogForm(decimal totalAmount)
        {
            _totalAmount = totalAmount;
            InitializeComponent();
            InitializeDesign();

            lblTotalValue.Text = totalAmount.ToString("C2");
            btnConfirm.Enabled = false;
            txtCash.Text = string.Empty;

            // Wiring
            btnConfirm.Click += BtnConfirm_Click;
            txtCash.TextChanged += TxtCash_TextChanged;

            // Customer search autocomplete
            txtCustomerSearch.TextChanged += TxtCustomerSearch_TextChanged;
            lstSuggestions.Click += LstSuggestions_Click;
            lstSuggestions.MouseMove += LstSuggestions_MouseMove;

            // Hide suggestions when focusing other fields
            txtCash.GotFocus += (s, e) => HideSuggestions();
            txtFirstName.GotFocus += (s, e) => HideSuggestions();
            txtLastName.GotFocus += (s, e) => HideSuggestions();
            txtPhone.GotFocus += (s, e) => HideSuggestions();
            txtNewEmail.GotFocus += (s, e) => HideSuggestions();

            // Clear validation errors when user modifies customer fields
            txtCustomerSearch.TextChanged += (s, e) => ClearValidation();
            txtFirstName.TextChanged += (s, e) => ClearValidation();
            txtLastName.TextChanged += (s, e) => ClearValidation();

            // ── Keyboard shortcuts ──
            // ENTER in Cash field → confirm if valid
            txtCash.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && btnConfirm.Enabled && !_isProcessing)
                {
                    e.SuppressKeyPress = true;
                    BtnConfirm_Click(btnConfirm, EventArgs.Empty);
                }
            };

            // ESC → cancel
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            };

            // Auto-focus cash field on dialog shown
            this.Shown += (s, e) => txtCash.Focus();
        }

        // ══════════════════════════════════════════════════════════════
        //  CUSTOMER SEARCH & AUTOCOMPLETE
        // ══════════════════════════════════════════════════════════════

        private void TxtCustomerSearch_TextChanged(object? sender, EventArgs e)
        {
            if (_suppressSearch) return;

            // Clear previous selection if user starts typing again
            if (_selectedExistingCustomer != null)
            {
                _selectedExistingCustomer = null;
                SetNewCustomerFieldsEnabled(true);
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtPhone.Text = "";
                txtNewEmail.Text = "";
            }

            string query = txtCustomerSearch.Text.Trim();
            if (query.Length < 1)
            {
                HideSuggestions();
                return;
            }

            var matches = Program.DataService.Customers
                .Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                          || (!string.IsNullOrEmpty(c.Phone) && c.Phone.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(c => c.Name)
                .Take(5)
                .ToList();

            if (matches.Count == 0)
            {
                HideSuggestions();
                return;
            }

            ShowSuggestions(matches);
        }

        private void ShowSuggestions(List<Customer> matches)
        {
            lstSuggestions.Items.Clear();
            lstSuggestions.Tag = matches;

            foreach (var c in matches)
            {
                string display = string.IsNullOrWhiteSpace(c.Phone)
                    ? c.Name
                    : $"{c.Name}  •  {c.Phone}";
                lstSuggestions.Items.Add(display);
            }

            int totalHeight = Math.Min(matches.Count, 5) * lstSuggestions.ItemHeight + 4;
            pnlSuggestions.Size = new System.Drawing.Size(380, totalHeight);
            pnlSuggestions.Visible = true;
            pnlSuggestions.BringToFront();
        }

        private void HideSuggestions()
        {
            pnlSuggestions.Visible = false;
        }

        private void LstSuggestions_Click(object? sender, EventArgs e)
        {
            if (lstSuggestions.SelectedIndex < 0) return;
            if (lstSuggestions.Tag is not List<Customer> customers) return;

            SelectExistingCustomer(customers[lstSuggestions.SelectedIndex]);
        }

        private void LstSuggestions_MouseMove(object? sender, MouseEventArgs e)
        {
            int idx = lstSuggestions.IndexFromPoint(e.Location);
            if (idx >= 0 && idx != lstSuggestions.SelectedIndex)
                lstSuggestions.SelectedIndex = idx;
        }

        private void SelectExistingCustomer(Customer customer)
        {
            _selectedExistingCustomer = customer;

            _suppressSearch = true;
            txtCustomerSearch.Text = customer.Name;
            _suppressSearch = false;

            // Parse first/last name from Name field
            var parts = customer.Name.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            txtFirstName.Text = parts.Length > 0 ? parts[0] : "";
            txtLastName.Text = parts.Length > 1 ? parts[1] : "";
            txtPhone.Text = customer.Phone;
            txtNewEmail.Text = customer.Email;

            SetNewCustomerFieldsEnabled(false);
            HideSuggestions();
        }

        private void SetNewCustomerFieldsEnabled(bool enabled)
        {
            txtFirstName.Enabled = enabled;
            txtLastName.Enabled = enabled;
            txtPhone.Enabled = enabled;
            txtNewEmail.Enabled = enabled;

            lblNewCustomerLabel.Text = enabled
                ? "New customer details:"
                : "Linked customer details:";
        }

        // ══════════════════════════════════════════════════════════════
        //  CASH INPUT
        // ══════════════════════════════════════════════════════════════

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

        // ══════════════════════════════════════════════════════════════
        //  CONFIRM & COMPLETE
        // ══════════════════════════════════════════════════════════════

        private void ClearValidation()
        {
            lblValidation.Text = "";
            lblValidation.Visible = false;
        }

        private void ShowValidationError(string message)
        {
            lblValidation.Text = message;
            lblValidation.Visible = true;
        }

        private async void BtnConfirm_Click(object? sender, EventArgs e)
        {
            if (_isProcessing) return; // Prevent double-click
            ClearValidation();

            if (!decimal.TryParse(txtCash.Text, out decimal cash) || cash < _totalAmount) return;

            _isProcessing = true;
            btnConfirm.Enabled = false;
            btnConfirm.Text = "Processing...";

            try
            {
                CashReceived = cash;
                ChangeDue = cash - _totalAmount;

                // ── Path A: Existing customer selected from autocomplete ──
                if (_selectedExistingCustomer != null)
                {
                    SelectedCustomer = _selectedExistingCustomer;
                }
                // ── Path B: New customer via dedicated fields ──
                else if (!string.IsNullOrWhiteSpace(txtFirstName.Text) || !string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    string firstName = txtFirstName.Text.Trim();
                    string lastName = txtLastName.Text.Trim();
                    string fullName = $"{firstName} {lastName}".Trim();

                    var newCust = new Customer
                    {
                        Name = fullName,
                        Phone = txtPhone.Text.Trim(),
                        Email = txtNewEmail.Text.Trim(),
                        MemberSince = DateTime.Now
                    };
                    Program.DataService.Customers.Add(newCust);
                    await Program.DataService.SaveCustomersAsync();
                    SelectedCustomer = newCust;
                }
                // ── Path C: Walk-in (everything empty) ──
                else if (string.IsNullOrWhiteSpace(txtCustomerSearch.Text))
                {
                    SelectedCustomer = null;
                }
                // ── Invalid: search text present but no selection or fields filled ──
                else
                {
                    ShowValidationError("Customer not found. Please select from suggestions or fill in details.");
                    _isProcessing = false;
                    btnConfirm.Enabled = true;
                    btnConfirm.Text = "✓  Confirm & Complete Sale";
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                _isProcessing = false;
                btnConfirm.Enabled = true;
                btnConfirm.Text = "✓  Confirm & Complete Sale";
                ShowValidationError($"Error: {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
