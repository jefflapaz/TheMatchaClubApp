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
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class OrdersView : UserControl
    {
        private BindingList<Order> _boundOrders = new();
        private List<Order> _allOrders = new();
        private string _activeFilter = "All"; // "All", "Dine-In", "Take-Out"

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
            btnFilterTakeaway.Click += (s, e) => ApplyFilter("Take-Out");
            txtSearch.TextChanged += (s, e) => ApplyFilter(_activeFilter);

            // Wire up actions
            btnReprint.Click += BtnReprint_Click;
            btnEmailReceipt.Click += BtnEmailReceipt_Click;

            // Bind data service events
            Program.DataService.OrdersChanged += (s, e) => LoadOrdersAsync();

            LoadOrdersAsync();
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
        //  FILTERING
        // ══════════════════════════════════════════════════════════════

        private void ApplyFilter(string filterType)
        {
            _activeFilter = filterType;
            string searchText = txtSearch.Text.Trim().ToLower();

            // Update button visual states
            StyleFilterPill(btnFilterAll, filterType == "All");
            StyleFilterPill(btnFilterDineIn, filterType == "Dine-In");
            StyleFilterPill(btnFilterTakeaway, filterType == "Take-Out");

            _boundOrders.Clear();
            var filtered = _allOrders.AsEnumerable();
            
            if (filterType != "All")
                filtered = filtered.Where(o => o.OrderType == filterType);

            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(o => 
                    (o.OrderId != null && o.OrderId.ToLower().Contains(searchText)) || 
                    (o.CustomerName != null && o.CustomerName.ToLower().Contains(searchText)));
            }

            foreach (var o in filtered)
            {
                _boundOrders.Add(o);
            }

            lblPaginationInfo.Text = $"Showing {_boundOrders.Count} results";

            // If no items, clear receipt
            if (_boundOrders.Count == 0)
            {
                ClearReceipt();
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
            lblReceiptTax.Text = "₱0.00";
            lblReceiptTotal.Text = "₱0.00";
        }

        private void RenderReceipt(Order order)
        {
            lblReceiptItems.Visible = false;
            lblStoreName.Text = Program.DataService.Settings.StoreName;
            lblStoreAddress.Text = $"{Program.DataService.Settings.Address}\n{Program.DataService.Settings.Phone}\n{Program.DataService.Settings.Email}";

            lblStoreName.Location = new Point(16, 75);
            lblStoreName.Width = 288;
            lblStoreName.TextAlign = ContentAlignment.MiddleCenter;
            
            lblStoreAddress.Location = new Point(16, 100);
            lblStoreAddress.Width = 288;
            lblStoreAddress.Height = 60;
            lblStoreAddress.TextAlign = ContentAlignment.MiddleCenter;

            lblReceiptOrderIdLabel.Top = 175;
            lblReceiptOrderId.Top = 175;
            lblReceiptDateLabel.Top = 195;
            lblReceiptDate.Top = 195;
            lblReceiptCustomerLabel.Top = 215;
            lblReceiptCustomer.Top = 215;

            lblReceiptOrderId.Text = order.OrderId;
            lblReceiptDate.Text = order.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            lblReceiptCustomer.Text = string.IsNullOrEmpty(order.CustomerName) ? "Walk-In" : order.CustomerName;

            // Remove old item panels
            var oldItems = pnlReceiptBody.Controls.OfType<Panel>().Where(p => p.Name.StartsWith("item_")).ToList();
            foreach (var p in oldItems) pnlReceiptBody.Controls.Remove(p);

            // Add Header
            int currentY = 250;
            var pnlHeader = new Panel { Name = "item_header", Location = new Point(16, currentY), Width = 284, Height = 25 };
            var lblHItem = new Label { Text = "ITEM / QTY", Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(0, 0), AutoSize = true };
            var lblHPrice = new Label { Text = "PRICE", Font = new Font("Segoe UI", 8F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(200, 0), Size = new Size(84, 15), TextAlign = ContentAlignment.MiddleRight };
            pnlHeader.Controls.Add(lblHItem);
            pnlHeader.Controls.Add(lblHPrice);
            pnlReceiptBody.Controls.Add(pnlHeader);
            currentY += 30;

            // Add Items
            foreach (var item in order.Items)
            {
                var pnl = new Panel { Name = "item_" + Guid.NewGuid(), Location = new Point(16, currentY), Width = 284, Height = 40 };
                var lblName = new Label { Text = item.ProductName, Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#111827"), Location = new Point(0, 0), AutoSize = true };
                var lblQty = new Label { Text = $"x{item.Quantity} @ {item.UnitPrice:C2}", Font = new Font("Segoe UI", 8F), ForeColor = ColorTranslator.FromHtml("#6B7280"), Location = new Point(0, 20), AutoSize = true };
                var lblPrice = new Label { Text = item.LineTotal.ToString("C2"), Font = new Font("Segoe UI", 9F), ForeColor = ColorTranslator.FromHtml("#111827"), Location = new Point(200, 20), Size = new Size(84, 20), TextAlign = ContentAlignment.MiddleRight };
                
                pnl.Controls.Add(lblName);
                pnl.Controls.Add(lblQty);
                pnl.Controls.Add(lblPrice);
                
                pnlReceiptBody.Controls.Add(pnl);
                currentY += 48; // Give items some breathing room
            }

            // Adjust positions of totals
            int totalsY = currentY + 20;
            lblReceiptSubtotalLabel.Top = totalsY;
            lblReceiptSubtotal.Top = totalsY;
            lblReceiptTaxLabel.Top = totalsY + 20;
            lblReceiptTax.Top = totalsY + 20;
            lblReceiptTotalLabel.Top = totalsY + 50;
            lblReceiptTotal.Top = totalsY + 50;
            
            lblPaidVia.Top = totalsY + 95;
            lblPaidVia.Width = 288;
            lblPaidVia.Location = new Point(16, lblPaidVia.Top);
            
            lblThankYou.Top = totalsY + 130;
            lblThankYou.Width = 288;
            lblThankYou.Location = new Point(16, lblThankYou.Top);
            
            btnReprint.Top = totalsY + 165;
            btnEmailReceipt.Top = totalsY + 165;

            lblReceiptSubtotal.Text = order.Subtotal.ToString("C2");
            lblReceiptTax.Text = order.VatAmount.ToString("C2");
            lblReceiptTotal.Text = order.Total.ToString("C2");

            pnlReceiptBody.Invalidate(); // trigger dashed line repaint
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

        private void BtnReprint_Click(object? sender, EventArgs e)
        {
            var order = GetSelectedOrder();
            if (order == null) return;

            var pd = new PrintDocument();
            pd.PrintPage += (s, pe) => DrawReceipt(pe, order);

            using var preview = new PrintPreviewDialog { Document = pd, Width = 400, Height = 600 };
            preview.ShowDialog();
        }

        private void DrawReceipt(PrintPageEventArgs e, Order order)
        {
            var g = e.Graphics!;
            float x = 20, y = 20;
            float w = e.PageBounds.Width - 40;

            using var titleFont = new Font("Segoe UI", 14F, FontStyle.Bold);
            using var headerFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var bodyFont = new Font("Segoe UI", 9F);
            using var smallFont = new Font("Segoe UI", 8F);
            using var brush = new SolidBrush(Color.Black);
            using var grayBrush = new SolidBrush(Color.Gray);
            using var linePen = new Pen(Color.LightGray, 1);

            var storeName = Program.DataService.Settings.StoreName;
            var storeAddr = Program.DataService.Settings.Address;
            var storePhone = Program.DataService.Settings.Phone;

            g.DrawString(storeName, titleFont, brush, x, y); y += 30;
            g.DrawString(storeAddr, smallFont, grayBrush, x, y); y += 16;
            g.DrawString(storePhone, smallFont, grayBrush, x, y); y += 24;
            g.DrawLine(linePen, x, y, w, y); y += 8;

            g.DrawString($"Order: {order.OrderId}", headerFont, brush, x, y); y += 20;
            g.DrawString($"Date: {order.Timestamp:dd/MM/yyyy HH:mm}", bodyFont, grayBrush, x, y); y += 18;
            g.DrawString($"Type: {order.OrderType}", bodyFont, brush, x, y); y += 18;
            g.DrawString($"Customer: {order.CustomerName}", bodyFont, brush, x, y); y += 24;
            g.DrawLine(linePen, x, y, w, y); y += 8;

            g.DrawString("ITEM", headerFont, brush, x, y);
            g.DrawString("QTY", headerFont, brush, x + 200, y);
            g.DrawString("TOTAL", headerFont, brush, x + 280, y);
            y += 22;

            foreach (var item in order.Items)
            {
                g.DrawString(item.ProductName, bodyFont, brush, x, y);
                g.DrawString(item.Quantity.ToString(), bodyFont, brush, x + 210, y);
                g.DrawString(item.LineTotal.ToString("C2"), bodyFont, brush, x + 270, y);
                y += 18;
            }

            y += 8;
            g.DrawLine(linePen, x, y, w, y); y += 8;

            g.DrawString("Subtotal:", bodyFont, grayBrush, x, y);
            g.DrawString(order.Subtotal.ToString("C2"), bodyFont, brush, x + 270, y); y += 18;
            g.DrawString("VAT (12%):", bodyFont, grayBrush, x, y);
            g.DrawString(order.VatAmount.ToString("C2"), bodyFont, brush, x + 270, y); y += 22;
            g.DrawString("TOTAL:", headerFont, brush, x, y);
            g.DrawString(order.Total.ToString("C2"), headerFont, brush, x + 260, y); y += 30;

            g.DrawLine(linePen, x, y, w, y); y += 12;
            g.DrawString("Thank you for visiting!", bodyFont, grayBrush, x + 60, y);

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
            var storeName = Program.DataService.Settings.StoreName;
            var senderEmail = Program.DataService.Settings.Email;
            
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
  <p style='margin:0 0 8px;color:#6B7280'>VAT (12%): {order.VatAmount.ToString("C2")}</p>
  <p style='margin:0 0 16px'><strong style='font-size:20px;color:#52B743'>TOTAL: {order.Total.ToString("C2")}</strong></p>
  <p style='text-align:center;color:#9CA3AF;font-size:12px'>Thank you for visiting {storeName}!</p>
</div>";

            await Task.Run(() =>
            {
                using var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Timeout = 15000,
                    Credentials = new NetworkCredential(senderEmail, "")
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

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
