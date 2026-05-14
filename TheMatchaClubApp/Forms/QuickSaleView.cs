using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
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
            txtSearch.TextChanged += (s, e) => PopulateProducts(_activeCategory);

            Program.DataService.ProductsChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(() => PopulateProducts(_activeCategory)));
            };

            Program.DataService.CategoriesChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(() => 
                {
                    if (_activeCategory != "All" && !Program.DataService.Categories.Contains(_activeCategory))
                    {
                        _activeCategory = "All";
                        PopulateProducts(_activeCategory);
                    }
                    PopulateCategories();
                }));
            };

            PopulateCategories();
            PopulateProducts("All");

            // ── Category Arrow Navigation ──
            btnCatLeft.Click += (s, e) => ScrollCategories(-150);
            btnCatRight.Click += (s, e) => ScrollCategories(150);

            // Mouse wheel horizontal scrolling on category area
            pnlCategoryScroll.MouseWheel += (s, e) => ScrollCategories(-e.Delta);
            flpCategories.MouseWheel += (s, e) =>
            {
                ScrollCategories(-e.Delta);
                ((HandledMouseEventArgs)e).Handled = true;
            };
        }

        private int _categoryScrollPos = 0;

        private void ScrollCategories(int delta)
        {
            // Calculate the total content width vs visible width
            int contentWidth = 0;
            foreach (Control c in flpCategories.Controls)
                contentWidth = Math.Max(contentWidth, c.Right + c.Margin.Right);
            contentWidth += flpCategories.Padding.Right;

            int visibleWidth = pnlCategoryScroll.ClientSize.Width;
            int maxScroll = Math.Max(0, contentWidth - visibleWidth);

            // Apply delta
            _categoryScrollPos += delta;
            _categoryScrollPos = Math.Max(0, Math.Min(_categoryScrollPos, maxScroll));

            // Move the FlowLayoutPanel
            flpCategories.Location = new Point(-_categoryScrollPos, 0);

            UpdateCategoryArrows(maxScroll);
        }

        private void UpdateCategoryArrows(int maxScroll = -1)
        {
            if (maxScroll < 0)
            {
                int contentWidth = 0;
                foreach (Control c in flpCategories.Controls)
                    contentWidth = Math.Max(contentWidth, c.Right + c.Margin.Right);
                contentWidth += flpCategories.Padding.Right;
                int visibleWidth = pnlCategoryScroll.ClientSize.Width;
                maxScroll = Math.Max(0, contentWidth - visibleWidth);
            }

            btnCatLeft.Enabled = _categoryScrollPos > 0;
            btnCatRight.Enabled = _categoryScrollPos < maxScroll;

            // Visual feedback for disabled state
            btnCatLeft.ForeColor = btnCatLeft.Enabled
                ? ColorTranslator.FromHtml("#6B7280")
                : ColorTranslator.FromHtml("#D1D5DB");
            btnCatRight.ForeColor = btnCatRight.Enabled
                ? ColorTranslator.FromHtml("#6B7280")
                : ColorTranslator.FromHtml("#D1D5DB");
        }

        private void PopulateCategories()
        {
            flpCategories.SuspendLayout();
            flpCategories.Controls.Clear();
            _categoryButtons.Clear();

            var categories = new List<string> { "All" };
            var customCats = Program.DataService.Categories
                .Where(c => c != "All Items" && c != "All") // Normalize "All" vs "All Items"
                .Distinct()
                .ToList();
            categories.AddRange(customCats);

            foreach (var cat in categories)
            {
                int textWidth = TextRenderer.MeasureText(cat, new Font("Segoe UI", 8F, FontStyle.Bold)).Width;
                int btnWidth = Math.Max(80, textWidth + 32); // 32px for padding

                var btn = new Guna.UI2.WinForms.Guna2Button
                {
                    Text = cat,
                    Tag = cat,
                    Size = new Size(btnWidth, 32),
                    Margin = new Padding(4, 0, 4, 0),
                    BorderRadius = 20,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    BorderThickness = 1,
                    Cursor = Cursors.Hand
                };
                btn.Click += CategoryFilter_Click;
                _categoryButtons.Add(btn);
                flpCategories.Controls.Add(btn);
            }

            UpdateCategoryPills();
            flpCategories.ResumeLayout();

            // Reset scroll and update arrows
            _categoryScrollPos = 0;
            flpCategories.Location = new Point(0, 0);
            UpdateCategoryArrows();
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

            string query = txtSearch.Text.Trim();
            if (!string.IsNullOrWhiteSpace(query))
            {
                filtered = filtered.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            foreach (var product in filtered)
            {
                var card = new ProductCard
                {
                    ProductData = product,
                    Size = new Size(136, 170), // Denser grid
                    Margin = new Padding(6)
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
                // Use the actual visible client width minus margin for each row
                int rowWidth = Math.Max(300, pnlCartItems.ClientSize.Width - 8);

                foreach (var line in _cart)
                {
                    var linePanel = new Panel
                    {
                        Location = new Point(4, y),
                        Size = new Size(rowWidth, 52),
                        BackColor = Color.Transparent
                    };

                    var capturedLine = line;

                    // ── Quantity Controls: [-] N [+] ──
                    var btnMinus = new Guna.UI2.WinForms.Guna2Button
                    {
                        Text = "-", Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        Size = new Size(22, 22), Location = new Point(4, 15),
                        BorderRadius = 4, FillColor = ColorTranslator.FromHtml("#E5E7EB"), ForeColor = Color.Black,
                        Cursor = Cursors.Hand
                    };
                    btnMinus.Click += (s, e) => {
                        if (capturedLine.Qty > 1) { capturedLine.Qty--; RefreshCartUI(); }
                        else { _cart.Remove(capturedLine); RefreshCartUI(); }
                    };

                    var lblQty = new Label
                    {
                        Text = capturedLine.Qty.ToString(), Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = ColorTranslator.FromHtml("#111827"),
                        Location = new Point(28, 16), Size = new Size(20, 20),
                        TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent
                    };

                    var btnPlus = new Guna.UI2.WinForms.Guna2Button
                    {
                        Text = "+", Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        Size = new Size(22, 22), Location = new Point(50, 15),
                        BorderRadius = 4, FillColor = ColorTranslator.FromHtml("#E5E7EB"), ForeColor = Color.Black,
                        Cursor = Cursors.Hand
                    };
                    btnPlus.Click += (s, e) => { capturedLine.Qty++; RefreshCartUI(); };

                    // ── Layout measurements ──
                    int nameLeft = 80;
                    int totalWidth = 80;    // right-aligned price column
                    int removeWidth = 24;   // remove button
                    int nameWidth = Math.Max(60, rowWidth - nameLeft - totalWidth - removeWidth - 4);

                    // ── Product Name (with ellipsis for long names) ──
                    var lblItemName = new Label
                    {
                        Text = line.Product.Name, Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = ColorTranslator.FromHtml("#374151"),
                        Location = new Point(nameLeft, 6), Size = new Size(nameWidth, 20),
                        BackColor = Color.Transparent, AutoEllipsis = true
                    };

                    // ── Unit Price (below name) ──
                    var lblUnitPrice = new Label
                    {
                        Text = $"@ {line.Product.Price.ToString("C2")}",
                        Font = new Font("Segoe UI", 7.5F),
                        ForeColor = ColorTranslator.FromHtml("#9CA3AF"),
                        Location = new Point(nameLeft, 28), Size = new Size(nameWidth, 16),
                        BackColor = Color.Transparent
                    };

                    // ── Remove Button ──
                    int removeX = rowWidth - totalWidth - removeWidth;
                    var btnRemove = new Label
                    {
                        Text = "✕", Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                        ForeColor = ColorTranslator.FromHtml("#EF4444"),
                        Size = new Size(removeWidth, 20), Location = new Point(removeX, 16),
                        Cursor = Cursors.Hand, BackColor = Color.Transparent,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    btnRemove.Click += (s, e) => { _cart.Remove(capturedLine); RefreshCartUI(); };

                    // ── Line Total (right-aligned) ──
                    var lblLineTotal = new Label
                    {
                        Text = line.Total.ToString("C2"),
                        Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                        ForeColor = ColorTranslator.FromHtml("#111827"),
                        TextAlign = ContentAlignment.MiddleRight,
                        Location = new Point(rowWidth - totalWidth, 0), Size = new Size(totalWidth, 52),
                        BackColor = Color.Transparent
                    };

                    linePanel.Controls.Add(btnMinus);
                    linePanel.Controls.Add(lblQty);
                    linePanel.Controls.Add(btnPlus);
                    linePanel.Controls.Add(lblItemName);
                    linePanel.Controls.Add(lblUnitPrice);
                    linePanel.Controls.Add(btnRemove);
                    linePanel.Controls.Add(lblLineTotal);

                    pnlCartItems.Controls.Add(linePanel);
                    y += 56;
                }

                btnCompleteSale.Enabled = true;
                btnCompleteSale.FillColor = ColorTranslator.FromHtml("#98D88A");
                btnCompleteSale.ForeColor = Color.White;
            }

            decimal total = _cart.Sum(c => c.Total);

            lblSubtotalValue.Text = total.ToString("C2");
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

            decimal total = _cart.Sum(c => c.Total);

            // ── Step 1: Show Checkout Dialog ──────────────────────────
            using var dlg = new CheckoutDialogForm(total);
            if (dlg.ShowDialog(this.FindForm()) != DialogResult.OK) return;

            bool isDineIn = dlg.IsDineIn;
            string orderType = dlg.SelectedOrderType;
            var customer = dlg.SelectedCustomer;
            decimal cashReceived = dlg.CashReceived;
            decimal changeDue = dlg.ChangeDue;

            // ── Step 2: Build the Order ───────────────────────────────
            var order = new Order
            {
                OrderId = $"ORD-{DateTime.Now:yyyyMMddHHmmss}",
                Timestamp = DateTime.Now,
                Subtotal = total,
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

            // ── Step 3: Increment Sales ────────────
            foreach (var line in _cart)
            {
                var dbProduct = Program.DataService.Products.FirstOrDefault(p => p.Id == line.Product.Id);
                if (dbProduct != null)
                {
                    dbProduct.SalesCount += line.Qty;
                }
            }

            // ── Step 4: Persist ──────────────────────────────────────
            Program.DataService.Orders.Add(order);
            await Program.DataService.SaveOrdersAsync();
            await Program.DataService.SaveProductsAsync();

            // ── Step 5: Success UI ───────────────────────────────────
            _lastOrder = order;
            ShowSaleCompleteDialog(order, cashReceived, changeDue);

            // ── Step 6: UI Reset ─────────────────────────────────────
            _cart.Clear();
            RefreshCartUI();
            PopulateProducts(_activeCategory);
        }

        /// <summary>
        /// Custom Sale Complete modal with Email Receipt + OK buttons.
        /// </summary>
        private void ShowSaleCompleteDialog(Order order, decimal cashReceived, decimal changeDue)
        {
            using var dlg = new Form
            {
                Text = "Sale Complete",
                Size = new Size(420, 340),
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
                Text = "✓  Sale Complete",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#52B743"),
                Location = new Point(20, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlHead.Controls.Add(lblHead);

            // ── Order Summary ────────────────────────────────────────
            var summaryText =
                $"Order {order.OrderId}\n" +
                $"Type: {order.OrderType}\n" +
                $"Customer: {order.CustomerName}\n\n" +
                $"Total: {order.Total:C2}\n" +
                $"Cash: {cashReceived:C2}\n" +
                $"Change: {changeDue:C2}";

            var lblSummary = new Label
            {
                Text = summaryText,
                Font = new Font("Segoe UI", 10F),
                ForeColor = ColorTranslator.FromHtml("#374151"),
                Location = new Point(20, 62),
                Size = new Size(380, 170),
                BackColor = Color.Transparent
            };

            // ── Buttons ──────────────────────────────────────────────
            var btnEmailReceipt = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "✉  Email Receipt",
                Location = new Point(20, 248),
                Size = new Size(190, 50),
                FillColor = ColorTranslator.FromHtml("#F3F4F6"),
                ForeColor = ColorTranslator.FromHtml("#374151"),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BorderRadius = 10,
                BorderThickness = 0
            };
            btnEmailReceipt.HoverState.FillColor = ColorTranslator.FromHtml("#E5E7EB");
            btnEmailReceipt.Click += (s, ev) =>
            {
                ShowEmailPromptDialog(order);
            };

            var btnOk = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "OK",
                Location = new Point(220, 248),
                Size = new Size(180, 50),
                FillColor = ColorTranslator.FromHtml("#52B743"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BorderRadius = 10,
                BorderThickness = 0
            };
            btnOk.HoverState.FillColor = ColorTranslator.FromHtml("#46A037");
            btnOk.Click += (s, ev) => dlg.Close();

            // ── Border paint ─────────────────────────────────────────
            dlg.Paint += (s, pe) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 2);
                pe.Graphics.DrawRectangle(pen, 0, 0, dlg.Width - 1, dlg.Height - 1);
            };

            // ── Assemble ─────────────────────────────────────────────
            dlg.Controls.Add(btnEmailReceipt);
            dlg.Controls.Add(btnOk);
            dlg.Controls.Add(lblSummary);
            dlg.Controls.Add(pnlHead);

            dlg.ShowDialog(this.FindForm());
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
            var settings = Program.DataService.Settings;
            var storeName = settings.StoreName;
            var senderEmail = settings.Email;
            var smtpServer = settings.SmtpServer;
            var smtpPort = settings.SmtpPort;
            var smtpPass = settings.SmtpPassword;
            
            var body = BuildReceiptHtml(order, storeName);

            Debug.WriteLine($"[EMAIL] Preparing to send receipt for {order.OrderId} to {recipientEmail}");
            Debug.WriteLine($"[EMAIL] SMTP Config: {smtpServer}:{smtpPort} (Sender: {senderEmail})");

            await Task.Run(() =>
            {
                try 
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
                    
                    Debug.WriteLine("[EMAIL] Sending message...");
                    client.Send(mail);
                    Debug.WriteLine("[EMAIL] Send complete.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[EMAIL] ERROR: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Debug.WriteLine($"[EMAIL] INNER ERROR: {ex.InnerException.Message}");
                    }
                    throw; // Re-throw to be caught by the UI handler
                }
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
