using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;
using TheMatchaClubApp.Helpers;

namespace TheMatchaClubApp.Forms
{
    public partial class QuickSaleView : UserControl
    {
        // ── Cart State ───────────────────────────────────────────────
        private readonly List<CartLine> _cart = new();
        private string _activeCategory = "All";

        // Last completed order — used by Print & Email buttons
        private Order? _lastOrder;

        private class CartLine
        {
            public Product Product { get; }
            public int Qty { get; set; }
            public decimal Total => Product.Price * Qty;
            public CartLine(Product product, int qty) { Product = product; Qty = qty; }
        }

        // ── Constructor ──────────────────────────────────────────────
        public QuickSaleView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();

            // Wire buttons
            btnCompleteSale.Click += BtnCompleteSale_Click;
            btnPrint.Click += BtnPrint_Click;
            btnEmail.Click += BtnEmail_Click;

            Program.DataService.ProductsChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(() => PopulateProducts(_activeCategory)));
            };

            PopulateProducts("All");
        }

        // ══════════════════════════════════════════════════════════════
        //  SECTION 1: PRODUCT GRID
        // ══════════════════════════════════════════════════════════════

        private void PopulateProducts(string category)
        {
            _activeCategory = category;
            flpProducts.SuspendLayout();
            flpProducts.Controls.Clear();

            var all = Program.DataService.Products;
            var filtered = category == "All"
                ? all
                : all.Where(p => p.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var product in filtered)
            {
                var card = new ProductCard
                {
                    ProductData = product,
                    Size = new Size(160, 190),
                    Margin = new Padding(8)
                };
                card.ProductClicked += (s, p) => AddToCart(p);
                flpProducts.Controls.Add(card);
            }
            flpProducts.ResumeLayout();
        }

        // ══════════════════════════════════════════════════════════════
        //  SECTION 2: CART OPERATIONS
        // ══════════════════════════════════════════════════════════════

        private void AddToCart(Product product)
        {
            if (product.StockLevel <= 0 || product.IsOutOfStock) return;

            var existing = _cart.FirstOrDefault(c => c.Product.Id == product.Id);
            if (existing != null) existing.Qty++;
            else _cart.Add(new CartLine(product, 1));
            RefreshCartUI();
        }

        private void RefreshCartUI()
        {
            pnlCartItems.SuspendLayout();
            pnlCartItems.Controls.Clear();

            if (_cart.Count == 0)
            {
                var emptyPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
                emptyPanel.Paint += (s, e) =>
                {
                    var g = e.Graphics;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                    using var brush = new SolidBrush(Color.FromArgb(180, 156, 163, 175));
                    using var font = new Font("Segoe UI", 10F);
                    string msg = "No items in cart";
                    var sz = g.MeasureString(msg, font);
                    g.DrawString(msg, font, brush,
                        (emptyPanel.Width - sz.Width) / 2,
                        (emptyPanel.Height - sz.Height) / 2 + 20);
                };
                pnlCartItems.Controls.Add(emptyPanel);
                btnCompleteSale.Enabled = false;
                btnCompleteSale.FillColor = ColorTranslator.FromHtml("#F3F4F6");
                btnCompleteSale.ForeColor = ColorTranslator.FromHtml("#D1D5DB");
                btnCompleteSale.Text = "₱ Complete Sale (Cash)";
            }
            else
            {
                int y = 4;
                foreach (var line in _cart)
                {
                    var linePanel = new Panel
                    {
                        Location = new Point(0, y),
                        Size = new Size(pnlCartItems.Width - 16, 44),
                        BackColor = Color.Transparent,
                        Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
                    };

                    var lblQty = new Label
                    {
                        Text = $"x{line.Qty}", Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = ColorTranslator.FromHtml("#111827"),
                        Location = new Point(8, 4), Size = new Size(28, 20),
                        BackColor = Color.Transparent
                    };

                    var lblItemName = new Label
                    {
                        Text = line.Product.Name, Font = new Font("Segoe UI", 9F),
                        ForeColor = ColorTranslator.FromHtml("#374151"),
                        Location = new Point(40, 4), Size = new Size(140, 20),
                        BackColor = Color.Transparent
                    };

                    var lblUnitPrice = new Label
                    {
                        Text = $"@ {line.Product.Price.ToString("C2")}",
                        Font = new Font("Segoe UI", 8F),
                        ForeColor = ColorTranslator.FromHtml("#9CA3AF"),
                        Location = new Point(40, 24), Size = new Size(100, 16),
                        BackColor = Color.Transparent
                    };

                    // Redo (remove) button
                    var btnRemove = new Label
                    {
                        Text = "✕", Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = ColorTranslator.FromHtml("#EF4444"),
                        Size = new Size(20, 20), Location = new Point(linePanel.Width - 90, 12),
                        Cursor = Cursors.Hand, BackColor = Color.Transparent
                    };
                    var capturedLine = line;
                    btnRemove.Click += (s, e) => { _cart.Remove(capturedLine); RefreshCartUI(); };

                    var lblLineTotal = new Label
                    {
                        Text = line.Total.ToString("C2"),
                        Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                        ForeColor = ColorTranslator.FromHtml("#111827"),
                        TextAlign = ContentAlignment.MiddleRight,
                        Dock = DockStyle.Right, Size = new Size(70, 44),
                        BackColor = Color.Transparent
                    };

                    linePanel.Controls.Add(lblQty);
                    linePanel.Controls.Add(lblItemName);
                    linePanel.Controls.Add(lblUnitPrice);
                    linePanel.Controls.Add(btnRemove);
                    linePanel.Controls.Add(lblLineTotal);
                    pnlCartItems.Controls.Add(linePanel);
                    y += 52;
                }

                btnCompleteSale.Enabled = true;
                btnCompleteSale.FillColor = ColorTranslator.FromHtml("#98D88A");
                btnCompleteSale.ForeColor = Color.White;
            }

            // 12% VAT calculation
            decimal subtotal = _cart.Sum(c => c.Total);
            decimal vat = subtotal * 0.12m;
            decimal total = subtotal + vat;

            lblSubtotalValue.Text = subtotal.ToString("C2");
            lblTaxValue.Text = vat.ToString("C2");
            lblTotalValue.Text = total.ToString("C2");
            btnCompleteSale.Text = "₱ Complete Sale (Cash)";

            pnlCartItems.ResumeLayout();
        }

        // ══════════════════════════════════════════════════════════════
        //  SECTION 3: CHECKOUT WORKFLOW
        // ══════════════════════════════════════════════════════════════

        private async void BtnCompleteSale_Click(object? sender, EventArgs e)
        {
            if (_cart.Count == 0) return;

            // ── Step 1: Show Checkout Dialog ──────────────────────────
            using var dlg = new CheckoutDialogForm();
            if (dlg.ShowDialog(this.FindForm()) != DialogResult.OK) return;

            bool isDineIn = dlg.IsDineIn;
            string orderType = dlg.SelectedOrderType;
            var customer = dlg.SelectedCustomer;

            // ── Step 2: Build the Order ───────────────────────────────
            decimal subtotal = _cart.Sum(c => c.Total);
            decimal vat = subtotal * 0.12m;
            decimal total = subtotal + vat;

            var order = new Order
            {
                OrderId = $"ORD-{DateTime.Now:yyyyMMddHHmmss}",
                Timestamp = DateTime.Now,
                Subtotal = subtotal,
                VatAmount = vat,
                Total = total,
                IsDineIn = isDineIn,
                OrderType = orderType,
                CustomerId = customer?.Id,
                CustomerName = customer?.Name ?? "Walk-In",
                CustomerEmail = customer?.Email ?? string.Empty,
                Items = _cart.Select(c => new OrderItem
                {
                    ProductId = c.Product.Id,
                    ProductName = c.Product.Name,
                    CategoryName = c.Product.CategoryName,
                    UnitPrice = c.Product.Price,
                    Quantity = c.Qty
                }).ToList()
            };

            // ── Step 3: Decrement Stock & Increment Sales ────────────
            foreach (var line in _cart)
            {
                var dbProduct = Program.DataService.Products.FirstOrDefault(p => p.Id == line.Product.Id);
                if (dbProduct != null)
                {
                    dbProduct.StockLevel = Math.Max(0, dbProduct.StockLevel - line.Qty);
                    dbProduct.SalesCount += line.Qty;
                    
                    if (dbProduct.StockLevel == 0)
                    {
                        dbProduct.IsOutOfStock = true;
                        dbProduct.CategoryName = "Out of Stock";
                    }
                }
            }

            // ── Step 4: Persist ──────────────────────────────────────
            Program.DataService.Orders.Add(order);
            await Program.DataService.SaveOrdersAsync();
            await Program.DataService.SaveProductsAsync();

            // ── Step 5: Success UI ───────────────────────────────────
            _lastOrder = order;

            var msg = new Guna.UI2.WinForms.Guna2MessageDialog();
            msg.Style = Guna.UI2.WinForms.MessageDialogStyle.Light;
            msg.Caption = "✓ Sale Complete";
            msg.Show(
                $"Order {order.OrderId}\n" +
                $"Type: {orderType}\n" +
                $"Customer: {order.CustomerName}\n" +
                $"──────────────\n" +
                $"Total: {total.ToString("C2")}\n" +
                $"VAT (12%): {vat.ToString("C2")}\n\n" +
                $"Use the Print or Email buttons for the receipt.");

            // ── Step 6: UI Reset ─────────────────────────────────────
            _cart.Clear();
            RefreshCartUI();
            PopulateProducts(_activeCategory); // Refresh stock levels on cards
        }

        // ══════════════════════════════════════════════════════════════
        //  SECTION 4: PRINT RECEIPT
        // ══════════════════════════════════════════════════════════════

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            if (_lastOrder == null)
            {
                MessageBox.Show("No recent order to print. Complete a sale first.",
                    "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var pd = new PrintDocument();
            pd.PrintPage += (s, pe) => DrawReceipt(pe, _lastOrder);

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

            // ── Store Header ─────────────────────────────────────────
            var storeName = Program.DataService.Settings.StoreName;
            var storeAddr = Program.DataService.Settings.Address;
            var storePhone = Program.DataService.Settings.Phone;

            g.DrawString(storeName, titleFont, brush, x, y);
            y += 30;
            g.DrawString(storeAddr, smallFont, grayBrush, x, y); y += 16;
            g.DrawString(storePhone, smallFont, grayBrush, x, y); y += 24;
            g.DrawLine(linePen, x, y, w, y); y += 8;

            // ── Order Info ───────────────────────────────────────────
            g.DrawString($"Order: {order.OrderId}", headerFont, brush, x, y); y += 20;
            g.DrawString($"Date: {order.Timestamp:dd/MM/yyyy HH:mm}", bodyFont, grayBrush, x, y); y += 18;
            g.DrawString($"Type: {order.OrderType}", bodyFont, brush, x, y); y += 18;
            g.DrawString($"Customer: {order.CustomerName}", bodyFont, brush, x, y); y += 24;
            g.DrawLine(linePen, x, y, w, y); y += 8;

            // ── Line Items ───────────────────────────────────────────
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

            // ── Totals ───────────────────────────────────────────────
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

        // ══════════════════════════════════════════════════════════════
        //  SECTION 5: EMAIL RECEIPT (with email prompt dialog)
        // ══════════════════════════════════════════════════════════════

        private void BtnEmail_Click(object? sender, EventArgs e)
        {
            if (_lastOrder == null)
            {
                var noOrder = new Guna.UI2.WinForms.Guna2MessageDialog();
                noOrder.Style = Guna.UI2.WinForms.MessageDialogStyle.Light;
                noOrder.Caption = "No Order";
                noOrder.Show("No recent order to email.\nComplete a sale first.");
                return;
            }

            ShowEmailPromptDialog(_lastOrder);
        }

        /// <summary>
        /// Shows a styled dialog asking for the client's email, then sends on confirm.
        /// </summary>
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

            // ── Header ───────────────────────────────────────────────
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
                ForeColor = ColorTranslator.FromHtml("#9CA3AF"), // Slightly lighter so it looks like an icon, but visible
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

            // ── Order summary ────────────────────────────────────────
            var lblInfo = new Label
            {
                Text = $"Order {order.OrderId}  •  {order.Total.ToString("C2")}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                Location = new Point(20, 62),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // ── Email Input ──────────────────────────────────────────
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

            // ── Status label ─────────────────────────────────────────
            var lblStatus = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8F),
                ForeColor = ColorTranslator.FromHtml("#EF4444"),
                Location = new Point(20, 168),
                Size = new Size(380, 18),
                BackColor = Color.Transparent
            };

            // ── Buttons ──────────────────────────────────────────────
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

                // ── Validate ─────────────────────────────────────────
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

                // ── Update order with the entered email ──────────────
                order.CustomerEmail = email;

                // ── Send ─────────────────────────────────────────────
                btnSend.Enabled = false;
                btnCancel.Enabled = false;
                btnSend.Text = "Sending...";
                lblStatus.ForeColor = ColorTranslator.FromHtml("#6B7280");
                lblStatus.Text = "Sending receipt to " + email + "...";

                try
                {
                    await SendReceiptEmailAsync(order, email);

                    // Save the updated email on the order
                    await Program.DataService.SaveOrdersAsync();

                    lblStatus.ForeColor = ColorTranslator.FromHtml("#52B743");
                    lblStatus.Text = "✓ Receipt sent successfully!";
                    btnSend.Text = "✓  Sent!";
                    btnSend.FillColor = ColorTranslator.FromHtml("#D1FAE5");
                    btnSend.ForeColor = ColorTranslator.FromHtml("#065F46");

                    // Auto-close after a moment
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

            // ── Assemble ─────────────────────────────────────────────
            dlg.Controls.Add(btnCancel);
            dlg.Controls.Add(btnSend);
            dlg.Controls.Add(lblStatus);
            dlg.Controls.Add(txtEmail);
            dlg.Controls.Add(lblEmail);
            dlg.Controls.Add(lblInfo);
            dlg.Controls.Add(pnlHead);

            // Border paint for the borderless form
            dlg.Paint += (s, pe) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 2);
                pe.Graphics.DrawRectangle(pen, 0, 0, dlg.Width - 1, dlg.Height - 1);
            };

            dlg.ShowDialog(this.FindForm());
        }

        // ── SMTP Send (background) ───────────────────────────────────
        private async Task SendReceiptEmailAsync(Order order, string recipientEmail)
        {
            var storeName = Program.DataService.Settings.StoreName;
            var senderEmail = Program.DataService.Settings.Email;
            var body = BuildReceiptHtml(order, storeName);

            await Task.Run(() =>
            {
                using var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Timeout = 15000,
                    Credentials = new NetworkCredential(senderEmail, "")
                    // NOTE: For Gmail, an App Password is required.
                    // Configure the App Password in Settings > Email.
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

        // ── HTML Receipt Builder ─────────────────────────────────────
        private string BuildReceiptHtml(Order order, string storeName)
        {
            var items = string.Join("",
                order.Items.Select(i =>
                    $"<tr><td style='padding:6px 0'>{i.ProductName}</td>" +
                    $"<td style='text-align:center;padding:6px 0'>{i.Quantity}</td>" +
                    $"<td style='text-align:right;padding:6px 0'>{i.LineTotal.ToString("C2")}</td></tr>"));

            return $@"
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
        }

        // ══════════════════════════════════════════════════════════════
        //  CATEGORY FILTER
        // ══════════════════════════════════════════════════════════════

        private void CategoryFilter_Click(object? sender, EventArgs e)
        {
            if (sender is Guna.UI2.WinForms.Guna2Button btn)
            {
                PopulateProducts(btn.Tag?.ToString() ?? "All");
                UpdateCategoryPills();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
