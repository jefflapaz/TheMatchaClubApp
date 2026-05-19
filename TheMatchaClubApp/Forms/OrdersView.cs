using TheMatchaClub.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheMatchaClubDomain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.IO;
using System.Diagnostics;
using QuestInfrastructure = QuestPDF.Infrastructure;

namespace TheMatchaClubApp.Forms
{
    public partial class OrdersView : UserControl
    {
        private BindingList<Order> _boundOrders = new();
        private List<Order> _allOrders = new();
        private string _activeFilter = "All"; // "All", "Dine-In", "Take-Out"

        /// <summary>Raised when cashier clicks a customer name to navigate to their profile.</summary>
        public event EventHandler<Guid>? NavigateToCustomer;

        public OrdersView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();
            SetupGrid();

            // Wire up filters
            btnFilterAll.Click += (s, e) => ApplyFilter("All");
            btnFilterDineIn.Click += (s, e) => ApplyFilter("Dine-In");
            btnFilterTakeOut.Click += (s, e) => ApplyFilter("Take-Out");
            txtSearch.TextChanged += (s, e) => ApplyFilter(_activeFilter);

            // Wire up actions
            btnPrintReceipt.Click += BtnPrintReceipt_Click;
            btnExportPDF.Click += BtnExportPDF_Click;
            btnEmailReceipt.Click += BtnEmailReceipt_Click;
            dgvOrders.CellDoubleClick += DgvOrders_CellDoubleClick;
            cmbDateFilter.SelectedIndexChanged += (s, e) => {
                dtpCustomDate.Visible = cmbDateFilter.SelectedIndex == 4; // Custom Date
                UpdateToolbarLayout();
                ApplyFilter(_activeFilter);
            };
            dtpCustomDate.ValueChanged += (s, e) => ApplyFilter(_activeFilter);
            pnlFilterBar.Resize += (s, e) => UpdateToolbarLayout();

            // Bind data service events
            Program.DataService.OrdersChanged += (s, e) => LoadOrdersAsync();

            LoadOrdersAsync();
            UpdateToolbarLayout();
        }

        // ══════════════════════════════════════════════════════════════
        //  GRID SETUP & DATA BINDING
        // ══════════════════════════════════════════════════════════════

        private void SetupGrid()
        {
            dgvOrders.AutoGenerateColumns = false;
            dgvOrders.RowHeadersVisible = false;
            dgvOrders.AllowUserToAddRows = false;
            dgvOrders.ReadOnly = true;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ORDER ID", DataPropertyName = "OrderId", FillWeight = 20 });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "DATE", DataPropertyName = "Timestamp", FillWeight = 15, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "CUSTOMER", DataPropertyName = "CustomerName", FillWeight = 15 });
            
            // Items column requires custom formatting since it's a collection
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { Name = "ItemsCol", HeaderText = "ITEMS", FillWeight = 25 });
            
            dgvOrders.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TYPE", DataPropertyName = "OrderType", FillWeight = 10 });
            
            var totalCol = new DataGridViewTextBoxColumn { 
                HeaderText = "TOTAL", 
                DataPropertyName = "Total", 
                FillWeight = 15, 
                DefaultCellStyle = new DataGridViewCellStyle { 
                    Format = "C2", 
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Padding = new Padding(0, 0, 20, 0)
                } 
            };
            totalCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            totalCol.HeaderCell.Style.Padding = new Padding(0, 0, 20, 0);
            dgvOrders.Columns.Add(totalCol);

            dgvOrders.CellFormatting += DgvOrders_CellFormatting;
            dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;

            // Clickable customer names
            dgvOrders.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex != 2) return; // Column 2 = Customer
                var order = _boundOrders[e.RowIndex];
                if (order.CustomerId.HasValue && order.CustomerId != Guid.Empty)
                    NavigateToCustomer?.Invoke(this, order.CustomerId.Value);
            };
            dgvOrders.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex != 2) return;
                var order = _boundOrders[e.RowIndex];
                dgvOrders.Cursor = (order.CustomerId.HasValue && order.CustomerId != Guid.Empty) ? Cursors.Hand : Cursors.Default;
            };
            dgvOrders.CellMouseLeave += (s, e) => dgvOrders.Cursor = Cursors.Default;

            dgvOrders.DataSource = _boundOrders;
        }

        private void DgvOrders_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvOrders.Columns[e.ColumnIndex].Name == "ItemsCol")
            {
                var order = _boundOrders[e.RowIndex];
                if (order.Items != null && order.Items.Any())
                {
                    e.Value = string.Join(", ", order.Items.Select(i => $"{i.Quantity}x {i.ProductName}"));
                    e.FormattingApplied = true;
                }
            }
        }

        private async void LoadOrdersAsync()
        {
            // Execute fetching on background thread
            var orders = await Task.Run(() => 
            {
                // Order descending by timestamp
                return Program.DataService.Orders.OrderByDescending(o => o.Timestamp).ToList();
            });

            // Update UI on main thread
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateOrdersList(orders)));
            }
            else
            {
                UpdateOrdersList(orders);
            }
        }

        private void UpdateOrdersList(List<Order> orders)
        {
            _allOrders = orders;
            ApplyFilter(_activeFilter);
        }

        // ══════════════════════════════════════════════════════════════
        //  LAYOUT
        // ══════════════════════════════════════════════════════════════
        
        private void UpdateToolbarLayout()
        {
            int startX = cmbDateFilter.Right + 12;
            
            if (dtpCustomDate.Visible)
            {
                dtpCustomDate.Left = startX;
                startX = dtpCustomDate.Right + 12;
            }
            
            btnFilterAll.Left = startX;
            btnFilterDineIn.Left = btnFilterAll.Right + 8;
            btnFilterTakeOut.Left = btnFilterDineIn.Right + 8;
        }

        // ══════════════════════════════════════════════════════════════
        //  FILTERING
        // ══════════════════════════════════════════════════════════════

        private void ApplyFilter(string filterType)
        {
            _activeFilter = filterType;
            string searchText = txtSearch.Text.Trim().ToLower();

            // Update button visual states
            StyleFilterPill(btnFilterAll, filterType == "All");
            StyleFilterPill(btnFilterDineIn, filterType == "Dine-In");
            StyleFilterPill(btnFilterTakeOut, filterType == "Take-Out");

            _boundOrders.Clear();
            var filtered = _allOrders.AsEnumerable();
            
            // Apply Date Filter
            switch (cmbDateFilter.SelectedIndex)
            {
                case 1: // Today
                    filtered = filtered.Where(o => o.Timestamp.Date == DateTime.Today);
                    break;
                case 2: // This Week
                    var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    filtered = filtered.Where(o => o.Timestamp.Date >= startOfWeek);
                    break;
                case 3: // This Month
                    var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    filtered = filtered.Where(o => o.Timestamp.Date >= startOfMonth);
                    break;
                case 4: // Custom Date
                    filtered = filtered.Where(o => o.Timestamp.Date == dtpCustomDate.Value.Date);
                    break;
            }
            
            // Apply Order Type Filter
            if (filterType == "Dine-In")
                filtered = filtered.Where(o => o.IsDineIn);
            else if (filterType == "Take-Out")
                filtered = filtered.Where(o => !o.IsDineIn);

            // Apply Text Search
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(o => 
                    (o.OrderId != null && o.OrderId.ToLower().Contains(searchText)) || 
                    (o.CustomerName != null && o.CustomerName.ToLower().Contains(searchText)) ||
                    (o.Items != null && o.Items.Any(i => i.ProductName != null && i.ProductName.ToLower().Contains(searchText))));
            }

            foreach (var o in filtered)
            {
                _boundOrders.Add(o);
            }

            lblPaginationInfo.Text = $"Showing {_boundOrders.Count} of {_allOrders.Count} orders";

            // Empty state
            var existingEmpty = pnlLeftArea.Controls["pnlEmptyState"];
            if (_boundOrders.Count == 0)
            {
                ClearReceipt();
                if (existingEmpty == null)
                {
                    var pnlEmpty = new Panel { Name = "pnlEmptyState", Dock = DockStyle.Fill, BackColor = Color.White };
                    var icon = new Label { Text = "📋", Font = new Font("Segoe UI", 32F), Location = new Point(0, 0), AutoSize = true, BackColor = Color.Transparent };
                    var msg = new Label { Text = "No Orders Found", Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#111827"), Location = new Point(0, 60), AutoSize = true, BackColor = Color.Transparent };
                    var sub = new Label { Text = "Try adjusting your search or filters.", Font = new Font("Segoe UI", 9.5F), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(0, 88), AutoSize = true, BackColor = Color.Transparent };
                    // Center on load
                    pnlEmpty.Layout += (ls, le) =>
                    {
                        int cx = (pnlEmpty.Width - 200) / 2;
                        int cy = (pnlEmpty.Height - 120) / 2;
                        icon.Location = new Point(cx + 70, cy);
                        msg.Location = new Point(cx + 10, cy + 60);
                        sub.Location = new Point(cx, cy + 88);
                    };
                    pnlEmpty.Controls.AddRange(new Control[] { icon, msg, sub });
                    pnlLeftArea.Controls.Add(pnlEmpty);
                    pnlEmpty.BringToFront();
                }
                else { existingEmpty.Visible = true; existingEmpty.BringToFront(); }
                dgvOrders.Visible = false;
            }
            else
            {
                if (existingEmpty != null) existingEmpty.Visible = false;
                dgvOrders.Visible = true;
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  RECEIPT RENDERING
        // ══════════════════════════════════════════════════════════════

        private void DgvOrders_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0 && dgvOrders.SelectedRows[0].DataBoundItem is Order order)
            {
                RenderReceipt(order);
            }
        }

        private void ClearReceipt()
        {
            lblReceiptOrderId.Text = "—";
            lblReceiptDate.Text = "—";
            lblReceiptCustomer.Text = "—";
            lblReceiptItems.Visible = false;
            lblReceiptSubtotal.Text = "₱0.00";
            lblReceiptTotal.Text = "₱0.00";

            // Remove old item panels
            var oldItems = pnlReceiptBody.Controls.OfType<Panel>().Where(p => p.Name.StartsWith("item_")).ToList();
            foreach (var p in oldItems) pnlReceiptBody.Controls.Remove(p);
        }

        private void RenderReceipt(Order order)
        {
            pnlReceiptBody.SuspendLayout();
            var settings = Program.DataService.Settings;
            lblReceiptItems.Visible = false;
            lblStoreName.Text = settings.StoreName;

            string addressParts = settings.Address;
            if (!string.IsNullOrWhiteSpace(settings.CurrentOperatingLocation))
                addressParts = settings.CurrentOperatingLocation;
            lblStoreAddress.Text = $"{addressParts}\n{settings.Phone}\n{settings.Email}";

            int centerX = pnlReceiptBody.Width / 2;

            // Logo Positioning - centered, clean
            pnlReceiptLogo.Location = new Point(centerX - 22, 20);
            lblReceiptLogo.Location = new Point(0, 0);

            // Store Info - centered under logo
            lblStoreName.Location = new Point(16, 70);
            lblStoreName.Width = pnlReceiptBody.Width - 32;
            lblStoreName.TextAlign = ContentAlignment.MiddleCenter;

            lblStoreAddress.Location = new Point(16, 95);
            lblStoreAddress.Width = pnlReceiptBody.Width - 32;
            lblStoreAddress.Height = 55;
            lblStoreAddress.TextAlign = ContentAlignment.MiddleCenter;

            // Order Metadata
            int metaTop = 165;
            lblReceiptOrderIdLabel.Top = metaTop;
            lblReceiptOrderId.Top = metaTop;
            lblReceiptOrderId.Left = pnlReceiptBody.Width - lblReceiptOrderId.Width - 16;

            lblReceiptDateLabel.Top = metaTop + 20;
            lblReceiptDate.Top = metaTop + 20;
            lblReceiptDate.Left = pnlReceiptBody.Width - lblReceiptDate.Width - 16;

            // Conditionally show customer
            lblReceiptCustomerLabel.Visible = settings.ReceiptShowCustomerName;
            lblReceiptCustomer.Visible = settings.ReceiptShowCustomerName;
            int nextMeta = metaTop + 40;
            if (settings.ReceiptShowCustomerName)
            {
                lblReceiptCustomerLabel.Top = nextMeta;
                lblReceiptCustomer.Top = nextMeta;
                lblReceiptCustomer.Left = pnlReceiptBody.Width - lblReceiptCustomer.Width - 16;
                nextMeta += 20;
            }

            // Cashier
            lblReceiptCashierLabel.Top = nextMeta;
            lblReceiptCashier.Top = nextMeta;
            lblReceiptCashier.Left = pnlReceiptBody.Width - lblReceiptCashier.Width - 16;
            lblReceiptCashier.Text = order.CashierName ?? Program.GetCurrentCashierName();
            nextMeta += 20;

            // Order Type
            lblReceiptOrderTypeLabel.Top = nextMeta;
            lblReceiptOrderType.Top = nextMeta;
            lblReceiptOrderType.Left = pnlReceiptBody.Width - lblReceiptOrderType.Width - 16;
            lblReceiptOrderType.Text = order.OrderType ?? "Dine-In";
            nextMeta += 20;

            lblReceiptOrderId.Text = order.OrderId;
            lblReceiptDate.Text = order.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            lblReceiptCustomer.Text = string.IsNullOrEmpty(order.CustomerName) ? "Walk-In" : order.CustomerName;

            // Remove old item panels
            var oldItems = pnlReceiptBody.Controls.OfType<Panel>().Where(p => p.Name.StartsWith("item_")).ToList();
            foreach (var p in oldItems) pnlReceiptBody.Controls.Remove(p);

            // Items Header
            int currentY = nextMeta + 20;
            var pnlHeader = new Panel { Name = "item_header", Location = new Point(16, currentY), Width = pnlReceiptBody.Width - 32, Height = 25 };
            var lblHItem = new Label { Text = "ITEM / QTY", Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(0, 0), AutoSize = true };
            var lblHPrice = new Label { Text = "TOTAL", Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(pnlHeader.Width - 84, 0), Size = new Size(84, 15), TextAlign = ContentAlignment.MiddleRight };
            pnlHeader.Controls.Add(lblHItem);
            pnlHeader.Controls.Add(lblHPrice);
            pnlReceiptBody.Controls.Add(pnlHeader);
            currentY += 30;

            // Items
            foreach (var item in order.Items)
            {
                var pnl = new Panel { Name = "item_" + Guid.NewGuid(), Location = new Point(16, currentY), Width = pnlReceiptBody.Width - 32, Height = 45 };
                var lblName = new Label { Text = item.ProductName, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#111827"), Location = new Point(0, 0), AutoSize = true };
                var lblQty = new Label { Text = $"x{item.Quantity} @ {item.UnitPrice:C2}", Font = new Font("Segoe UI", 8F), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(0, 20), AutoSize = true };
                var lblPrice = new Label { Text = item.LineTotal.ToString("C2"), Font = new Font("Segoe UI", 9F), ForeColor = ColorTranslator.FromHtml("#111827"), Location = new Point(pnl.Width - 84, 18), Size = new Size(84, 20), TextAlign = ContentAlignment.MiddleRight };

                pnl.Controls.Add(lblName);
                pnl.Controls.Add(lblQty);
                pnl.Controls.Add(lblPrice);

                pnlReceiptBody.Controls.Add(pnl);
                currentY += 48;
            }

            // Totals
            int totalsY = currentY + 20;
            lblReceiptSubtotalLabel.Top = totalsY;
            lblReceiptSubtotal.Top = totalsY;
            lblReceiptSubtotal.Left = pnlReceiptBody.Width - lblReceiptSubtotal.Width - 16;

            lblReceiptTotalLabel.Top = totalsY + 30;
            lblReceiptTotal.Top = totalsY + 30;
            lblReceiptTotal.Left = pnlReceiptBody.Width - lblReceiptTotal.Width - 16;

            // Cash Tendered / Change
            bool showCashDetails = order.CashTendered > 0;
            lblReceiptCashTenderedLabel.Visible = showCashDetails;
            lblReceiptCashTendered.Visible = showCashDetails;
            lblReceiptChangeLabel.Visible = showCashDetails;
            lblReceiptChange.Visible = showCashDetails;

            int cashY = totalsY + 60;
            if (showCashDetails)
            {
                lblReceiptCashTenderedLabel.Top = cashY;
                lblReceiptCashTendered.Top = cashY;
                lblReceiptCashTendered.Left = pnlReceiptBody.Width - lblReceiptCashTendered.Width - 16;
                lblReceiptCashTendered.Text = order.CashTendered.ToString("C2");

                lblReceiptChangeLabel.Top = cashY + 20;
                lblReceiptChange.Top = cashY + 20;
                lblReceiptChange.Left = pnlReceiptBody.Width - lblReceiptChange.Width - 16;
                lblReceiptChange.Text = order.ChangeGiven.ToString("C2");
                cashY += 45;
            }

            // Payment info
            string paidViaText = $"Paid via {order.PaymentMethod}";
            if (settings.ReceiptShowOrderType)
                paidViaText += $"  •  {order.OrderType}";
            if (settings.ReceiptShowCashierName && !string.IsNullOrWhiteSpace(order.CashierName))
                paidViaText += $"\nServed by {order.CashierName}";
            lblPaidVia.Text = paidViaText;
            int paidY = showCashDetails ? cashY + 5 : totalsY + 70;
            lblPaidVia.Width = pnlReceiptBody.Width - 32;
            lblPaidVia.Height = 45;
            lblPaidVia.Location = new Point(16, paidY);

            // Footer from settings
            lblThankYou.Text = settings.ReceiptFooterMessage;
            lblThankYou.Width = pnlReceiptBody.Width - 32;
            lblThankYou.Height = 40;
            lblThankYou.Location = new Point(16, paidY + 55);

            int btnY = paidY + 105;
            btnPrintReceipt.Top = btnY;
            btnExportPDF.Top = btnY;
            btnEmailReceipt.Top = btnY;
            btnPrintReceipt.Left = 16;
            btnExportPDF.Left = btnPrintReceipt.Right + 6;
            btnEmailReceipt.Left = btnExportPDF.Right + 6;

            lblReceiptSubtotal.Text = order.Subtotal.ToString("C2");
            lblReceiptTotal.Text = order.Total.ToString("C2");

            pnlReceiptBody.AutoScrollMinSize = new Size(0, btnY + 60);
            pnlReceiptBody.ResumeLayout();
            pnlReceiptBody.Invalidate();
        }

        // ══════════════════════════════════════════════════════════════
        //  PRINT & EMAIL (Reusing QuickSale Logic)
        // ══════════════════════════════════════════════════════════════

        private Order? GetSelectedOrder()
        {
            if (dgvOrders.SelectedRows.Count > 0 && dgvOrders.SelectedRows[0].DataBoundItem is Order order)
                return order;
            return null;
        }

        private void BtnPrintReceipt_Click(object? sender, EventArgs e)
        {
            var order = GetSelectedOrder();
            if (order == null) return;

            try
            {
                var doc = new PrintDocument();
                int paperWidth = Program.DataService.Settings.ReceiptPaperWidth == "58mm" ? 228 : 315;
                doc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Receipt", paperWidth, 800);
                doc.PrintPage += (ps, pe) => DrawReceipt(pe, order);

                var dlg = new PrintPreviewDialog { Document = doc, Width = 500, Height = 700 };
                dlg.ShowDialog(this.FindForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Print error: {ex.Message}", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvOrders_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var order = GetSelectedOrder();
            if (order == null) return;
            using var detail = new OrderDetailForm(order);
            detail.ShowDialog(this.FindForm());
        }

        private void BtnExportPDF_Click(object? sender, EventArgs e)
        {
            var order = GetSelectedOrder();
            if (order == null) return;

            try
            {
                QuestPDF.Settings.License = QuestInfrastructure.LicenseType.Community;

                string fileName = $"Receipt_{order.OrderId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                var settings = Program.DataService.Settings;
                ReceiptPdfGenerator.Generate(order, settings, Program.GetCurrentCashierName(), filePath);

                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting PDF: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawReceipt(PrintPageEventArgs e, Order order)
        {
            var settings = Program.DataService.Settings;
            ReceiptRenderer.Render(e.Graphics!, e.PageBounds, order, settings, Program.GetCurrentCashierName());
            e.HasMorePages = false;
        }

        private void BtnEmailReceipt_Click(object? sender, EventArgs e)
        {
            var order = GetSelectedOrder();
            if (order == null) return;

            ShowEmailPromptDialog(order);
        }

        private void ShowEmailPromptDialog(Order order)
        {
            using var dlg = new Form
            {
                Text = "Send Receipt via Email",
                Size = new Size(420, 280),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.White,
                ShowInTaskbar = false
            };

            var pnlHead = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.White };
            pnlHead.Paint += (s, pe) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 1);
                pe.Graphics.DrawLine(pen, 0, pnlHead.Height - 1, pnlHead.Width, pnlHead.Height - 1);
            };

            var lblHead = new Label
            {
                Text = "✉  Email Receipt",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#111827"),
                Location = new Point(20, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var btnX = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "✕", Size = new Size(36, 36),
                Location = new Point(374, 7),
                FillColor = Color.Transparent,
                ForeColor = ColorTranslator.FromHtml("#9CA3AF"),
                BorderThickness = 0,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnX.HoverState.FillColor = ColorTranslator.FromHtml("#FEE2E2");
            btnX.HoverState.ForeColor = ColorTranslator.FromHtml("#EF4444");
            btnX.Click += (s, ev) => dlg.Close();

            pnlHead.Controls.Add(lblHead);
            pnlHead.Controls.Add(btnX);

            var lblInfo = new Label
            {
                Text = $"Order {order.OrderId}  •  {order.Total.ToString("C2")}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                Location = new Point(20, 62),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var lblEmail = new Label
            {
                Text = "CLIENT EMAIL ADDRESS",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                Location = new Point(20, 95),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var txtEmail = new Guna.UI2.WinForms.Guna2TextBox
            {
                Text = order.CustomerEmail ?? "",
                Location = new Point(20, 118),
                Size = new Size(380, 44),
                PlaceholderText = "e.g. customer@email.com",
                Font = new Font("Segoe UI", 11F),
                BorderRadius = 8,
                BorderColor = ColorTranslator.FromHtml("#E5E7EB"),
            };
            txtEmail.FocusedState.BorderColor = ColorTranslator.FromHtml("#52B743");

            var lblStatus = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8F),
                ForeColor = ColorTranslator.FromHtml("#EF4444"),
                Location = new Point(20, 168),
                Size = new Size(380, 18),
                BackColor = Color.Transparent
            };

            var btnCancel = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Cancel",
                Location = new Point(20, 195),
                Size = new Size(180, 50),
                FillColor = ColorTranslator.FromHtml("#F3F4F6"),
                ForeColor = ColorTranslator.FromHtml("#374151"),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BorderRadius = 10,
                BorderThickness = 0
            };
            btnCancel.HoverState.FillColor = ColorTranslator.FromHtml("#E5E7EB");
            btnCancel.Click += (s, ev) => dlg.Close();

            var btnSend = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "📧  Send Receipt",
                Location = new Point(210, 195),
                Size = new Size(190, 50),
                FillColor = ColorTranslator.FromHtml("#52B743"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BorderRadius = 10,
                BorderThickness = 0
            };
            btnSend.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");

            btnSend.Click += async (s, ev) =>
            {
                string email = txtEmail.Text.Trim();

                if (string.IsNullOrWhiteSpace(email))
                {
                    lblStatus.ForeColor = ColorTranslator.FromHtml("#EF4444");
                    lblStatus.Text = "⚠ Please enter an email address.";
                    return;
                }
                if (!email.Contains('@') || !email.Contains('.'))
                {
                    lblStatus.ForeColor = ColorTranslator.FromHtml("#EF4444");
                    lblStatus.Text = "⚠ Please enter a valid email address.";
                    return;
                }

                order.CustomerEmail = email;

                btnSend.Enabled = false;
                btnCancel.Enabled = false;
                btnSend.Text = "Sending...";
                lblStatus.ForeColor = ColorTranslator.FromHtml("#6B7280");
                lblStatus.Text = "Sending receipt to " + email + "...";

                try
                {
                    await SendReceiptEmailAsync(order, email);
                    await Program.DataService.SaveOrdersAsync();

                    lblStatus.ForeColor = ColorTranslator.FromHtml("#52B743");
                    lblStatus.Text = "✓ Receipt sent successfully!";
                    btnSend.Text = "✓  Sent!";
                    btnSend.FillColor = ColorTranslator.FromHtml("#D1FAE5");
                    btnSend.ForeColor = ColorTranslator.FromHtml("#065F46");

                    var timer = new System.Windows.Forms.Timer { Interval = 1800 };
                    timer.Tick += (ts, te) => { timer.Stop(); timer.Dispose(); if (!dlg.IsDisposed) dlg.Close(); };
                    timer.Start();
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = ColorTranslator.FromHtml("#EF4444");
                    lblStatus.Text = $"⚠ Failed: {ex.Message}";
                    btnSend.Enabled = true;
                    btnCancel.Enabled = true;
                    btnSend.Text = "📧  Retry Send";
                }
            };

            dlg.Controls.Add(btnCancel);
            dlg.Controls.Add(btnSend);
            dlg.Controls.Add(lblStatus);
            dlg.Controls.Add(txtEmail);
            dlg.Controls.Add(lblEmail);
            dlg.Controls.Add(lblInfo);
            dlg.Controls.Add(pnlHead);

            dlg.Paint += (s, pe) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 2);
                pe.Graphics.DrawRectangle(pen, 0, 0, dlg.Width - 1, dlg.Height - 1);
            };

            dlg.ShowDialog(this.FindForm());
        }

        private async Task SendReceiptEmailAsync(Order order, string recipientEmail)
        {
            var settings = Program.DataService.Settings;
            var storeName = settings.StoreName;
            var senderEmail = settings.Email;
            var smtpServer = settings.SmtpServer;
            var smtpPort = settings.SmtpPort;
            var smtpPass = settings.SmtpPassword;
            
            var items = string.Join("",
                order.Items.Select(i =>
                    $"<tr><td style='padding:6px 0'>{i.ProductName}</td>" +
                    $"<td style='text-align:center;padding:6px 0'>{i.Quantity}</td>" +
                    $"<td style='text-align:right;padding:6px 0'>{i.LineTotal.ToString("C2")}</td></tr>"));

            var body = $@"
<div style='font-family:Segoe UI,Arial,sans-serif;max-width:420px;margin:auto;border:1px solid #E5E7EB;border-radius:12px;padding:28px'>
  <h2 style='color:#52B743;margin:0 0 4px'>{storeName}</h2>
  <p style='color:#6B7280;margin:0 0 16px;font-size:13px'>{Program.DataService.Settings.Address} • {Program.DataService.Settings.Phone}</p>
  <hr style='border:none;border-top:1px solid #E5E7EB;margin:0 0 16px'>
  <p style='margin:0 0 4px'><strong>Order:</strong> {order.OrderId}</p>
  <p style='margin:0 0 4px;color:#6B7280'><strong>Date:</strong> {order.Timestamp:dd/MM/yyyy HH:mm}</p>
  <p style='margin:0 0 4px'><strong>Type:</strong> {order.OrderType}</p>
  <p style='margin:0 0 16px'><strong>Customer:</strong> {order.CustomerName}</p>
  <table style='width:100%;border-collapse:collapse;margin-bottom:16px'>
    <tr style='background:#F9FAFB;font-weight:bold;font-size:12px;color:#6B7280'>
      <td style='padding:8px 0'>Item</td>
      <td style='text-align:center;padding:8px 0'>Qty</td>
      <td style='text-align:right;padding:8px 0'>Total</td>
    </tr>
    {items}
  </table>
  <hr style='border:none;border-top:1px solid #E5E7EB;margin:0 0 12px'>
  <p style='margin:0 0 4px;color:#6B7280'>Subtotal: {order.Subtotal.ToString("C2")}</p>
  <p style='margin:0 0 16px'><strong style='font-size:20px;color:#52B743'>TOTAL: {order.Total.ToString("C2")}</strong></p>
  <p style='text-align:center;color:#9CA3AF;font-size:12px'>Thank you for visiting {storeName}!</p>
</div>";

            await Task.Run(() =>
            {
                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    EnableSsl = true,
                    Timeout = 15000,
                    Credentials = new NetworkCredential(senderEmail, smtpPass)
                };

                using var mail = new MailMessage
                {
                    From = new MailAddress(senderEmail, storeName),
                    Subject = $"Your Receipt — {order.OrderId} | {storeName}",
                    Body = body,
                    IsBodyHtml = true
                };
                mail.To.Add(recipientEmail);
                client.Send(mail);
            });
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.P))
            {
                if (GetSelectedOrder() != null)
                {
                    BtnPrintReceipt_Click(this, EventArgs.Empty);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
