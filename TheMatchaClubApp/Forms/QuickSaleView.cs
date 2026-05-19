using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using TheMatchaClubDomain.Models;
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
            btnQuickOpenSession.Click += BtnQuickOpenSession_Click;
            btnClearCart.Click += BtnClearCart_Click;
            btnEndSession.Click += BtnEndSession_Click;
            txtSearch.TextChanged += (s, e) => PopulateProducts(_activeCategory);
            
            pnlSessionOverlay.Resize += (s, e) => CenterOverlayControls();
            
            flpProducts.Layout += (s, e) => 
            {
                // flpProducts has 32px horizontal padding, header has 12px horizontal margin = 44px total
                int targetWidth = flpProducts.ClientSize.Width - 50;
                if (targetWidth > 10)
                {
                    foreach (Control c in flpProducts.Controls)
                    {
                        if (c is Label lbl && lbl.Tag?.ToString() == "CategoryHeader" && lbl.Width != targetWidth)
                        {
                            lbl.Width = targetWidth;
                        }
                    }
                }
            };
            
            Program.SessionService.SessionOpened += (s, e) => { if (!IsDisposed) BeginInvoke(new Action(UpdateSessionState)); };
            Program.SessionService.SessionClosed += (s, e) => { if (!IsDisposed) BeginInvoke(new Action(UpdateSessionState)); };
            Program.DataService.DataLoaded += (s, e) => { if (!IsDisposed) BeginInvoke(new Action(() => { UpdateSessionState(); PopulateCategories(); PopulateProducts(_activeCategory); })); };
            
            UpdateSessionState();

            Program.DataService.ProductsChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(() => PopulateProducts(_activeCategory)));
            };

            Program.DataService.CategoriesChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(() => 
                {
                    if (_activeCategory != "All" && !Program.DataService.Categories.Any(c => c.Name == _activeCategory))
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

        /// <summary>
        /// Focus the product search bar for fast cashier input.
        /// </summary>
        public void FocusSearch()
        {
            if (txtSearch != null && !txtSearch.IsDisposed)
                BeginInvoke(new Action(() => txtSearch.Focus()));
        }

        private void UpdateSessionState()
        {
            bool isActive = Program.SessionService.HasActiveSession();
            bool lockIfNoSession = Program.DataService.Settings.AutoLockQuickSaleIfNoSession;

            btnEndSession.Visible = isActive;

            if (lockIfNoSession)
            {
                pnlSessionOverlay.Visible = !isActive;
                pnlProductGrid.Enabled = isActive;
                pnlCategoryRow.Enabled = isActive;
                btnCompleteSale.Enabled = isActive;

                if (!isActive)
                {
                    pnlSessionOverlay.BringToFront();
                    CenterOverlayControls();
                }
            }
            else
            {
                // Allow sales without session
                pnlSessionOverlay.Visible = false;
                pnlProductGrid.Enabled = true;
                pnlCategoryRow.Enabled = true;
                btnCompleteSale.Enabled = true;
            }
        }

        private void CenterOverlayControls()
        {
            int centerX = pnlSessionOverlay.Width / 2;
            int centerY = pnlSessionOverlay.Height / 2;
            
            lblSessionWarning.Location = new Point(centerX - (lblSessionWarning.Width / 2), centerY - 60);
            btnQuickOpenSession.Location = new Point(centerX - (btnQuickOpenSession.Width / 2), centerY);
        }

        private async void BtnQuickOpenSession_Click(object? sender, EventArgs e)
        {
            if (Program.SessionService.HasActiveSession()) return;
            string cashierName = Program.GetCurrentCashierName();
            decimal defaultCash = Program.DataService.Settings.DefaultStartingCash;

            using var openDialog = new OpenSessionDialogForm(cashierName, defaultCash);
            
            Form bg = new Form();
            bg.StartPosition = FormStartPosition.Manual;
            bg.FormBorderStyle = FormBorderStyle.None;
            bg.Opacity = 0.50d;
            bg.BackColor = Color.Black;
            bg.WindowState = FormWindowState.Maximized;
            bg.TopMost = false;
            bg.Location = this.FindForm()!.Location;
            bg.ShowInTaskbar = false;
            bg.Show();

            openDialog.Owner = bg;
            var result = openDialog.ShowDialog();
            
            bg.Dispose();

            if (result != DialogResult.OK) return;

            decimal startingCash = openDialog.StartingCash;

            btnQuickOpenSession.Enabled = false;
            try 
            { 
                await Program.SessionService.OpenSessionAsync(cashierName, startingCash); 
                UpdateSessionState();
            }
            catch (InvalidOperationException ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { btnQuickOpenSession.Enabled = true; }
        }

        private async void BtnEndSession_Click(object? sender, EventArgs e)
        {
            var activeSession = Program.SessionService.GetActiveSession();
            if (activeSession == null) 
            { 
                MessageBox.Show("No active session.", "Session", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                return; 
            }
            
            var settings = Program.DataService.Settings;
            decimal actualCash = 0;

            if (Program.DataService.Settings.RequirePasswordForCloseSession)
            {
                using var authDialog = new PasswordPromptDialog("Enter password to close the session.");
                if (authDialog.ShowDialog(this) != DialogResult.OK) return;
            }

            if (settings.RequireCashCountOnClose)
            {
                using var closeDialog = new CloseSessionDialogForm(activeSession);
                
                // Dim background
                Form bg = new Form();
                bg.StartPosition = FormStartPosition.Manual;
                bg.FormBorderStyle = FormBorderStyle.None;
                bg.Opacity = 0.50d;
                bg.BackColor = Color.Black;
                bg.WindowState = FormWindowState.Maximized;
                bg.TopMost = false;
                bg.Location = this.FindForm()!.Location;
                bg.ShowInTaskbar = false;
                bg.Show();

                closeDialog.Owner = bg;
                var result = closeDialog.ShowDialog();
                
                bg.Dispose();

                if (result != DialogResult.OK) return; // User canceled

                actualCash = closeDialog.ActualCashCounted;
            }
            else
            {
                Program.SessionService.ComputeSessionTotals(activeSession);
                actualCash = activeSession.StartingCash + activeSession.TotalRevenue;
            }

            btnEndSession.Enabled = false;
            try 
            {
                var closed = await Program.SessionService.CloseSessionAsync(actualCash, Program.GetCurrentCashierName());
                decimal overShort = closed.ActualCash - closed.ExpectedCash;
                
                MessageBox.Show(
                    $"Session closed successfully.\n\nTransactions: {closed.TotalTransactions}\nRevenue: ₱{closed.TotalRevenue:#,##0.00}",
                    "Session Closed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                UpdateSessionState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to close session: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnEndSession.Enabled = true;
            }
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
                .Where(c => c.Name != "All Items" && c.Name != "All")
                .OrderBy(c => c.DisplayOrder)
                .Select(c => c.Name)
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
            string query = txtSearch.Text.Trim();
            bool isSearching = !string.IsNullOrWhiteSpace(query);

            var filtered = all;

            if (isSearching)
            {
                filtered = filtered.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (category == "All" && !isSearching)
            {
                // Find Top 8 Items sold
                var topSellingProductIds = Program.DataService.Orders
                    .SelectMany(o => o.Items)
                    .GroupBy(i => i.ProductId)
                    .OrderByDescending(g => g.Sum(i => i.Quantity))
                    .Take(8)
                    .Select(g => g.Key)
                    .ToList();

                var topProducts = all.Where(p => topSellingProductIds.Contains(p.Id)).ToList();
                if (topProducts.Any())
                {
                    AddCategoryHeader("✨ The Usuals");
                    foreach (var p in topProducts) flpProducts.Controls.Add(CreateProductCard(p));
                }

                // Group all items by category
                var categoryOrder = Program.DataService.Categories.ToDictionary(c => c.Name, c => c.DisplayOrder);
                var allGroups = all.GroupBy(p => p.CategoryName)
                                   .OrderBy(g => categoryOrder.TryGetValue(g.Key, out int order) ? order : int.MaxValue)
                                   .ThenBy(g => g.Key);

                foreach (var group in allGroups)
                {
                    AddCategoryHeader(group.Key);
                    foreach (var p in group.OrderBy(x => x.Name)) flpProducts.Controls.Add(CreateProductCard(p));
                }
            }
            else
            {
                if (category != "All")
                {
                    filtered = filtered.Where(p => p.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                foreach (var p in filtered.OrderBy(x => x.Name))
                {
                    flpProducts.Controls.Add(CreateProductCard(p));
                }
            }

            flpProducts.ResumeLayout();
        }

        private void AddCategoryHeader(string title)
        {
            var header = new Label
            {
                Text = title,
                Tag = "CategoryHeader",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = false,
                Width = flpProducts.ClientSize.Width - 50 > 10 ? flpProducts.ClientSize.Width - 50 : 500,
                Height = 35,
                TextAlign = ContentAlignment.BottomLeft,
                Margin = new Padding(6, 12, 6, 4)
            };

            flpProducts.Controls.Add(header);
        }

        private ProductCard CreateProductCard(Product product)
        {
            var card = new ProductCard
            {
                ProductData = product,
                Size = new Size(136, 170), // Denser grid
                Margin = new Padding(6)
            };
            card.ProductClicked += (s, p) => AddToCart(p);
            return card;
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
            
            // Update Order Number Placeholder
            string nextOrderNum = Program.DataService.GenerateOrderNumber();
            lblOrderMeta.Text = $"Order {nextOrderNum} \u2022 Cashier: {Program.GetCurrentCashierName()}";

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
                btnCompleteSale.Text = "\u20B1 Complete Sale (Cash)";
            }
            else
            {
                int y = 4;
                foreach (var line in _cart)
                {
                    var row = new CartItemRow(line)
                    {
                        Location = new Point(4, y),
                        Width = pnlCartItems.Width - 24, // Account for scrollbar
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                    };

                    var capturedLine = line;
                    row.QtyChanged += (s, e) => UpdateCartTotals();
                    row.RemoveClicked += (s, e) => { _cart.Remove(capturedLine); RefreshCartUI(); };

                    pnlCartItems.Controls.Add(row);
                    y += row.Height + 4;
                }

                btnCompleteSale.Enabled = true;
                btnCompleteSale.FillColor = ColorTranslator.FromHtml("#98D88A");
                btnCompleteSale.ForeColor = Color.White;
            }

            UpdateCartTotals();
            pnlCartItems.ResumeLayout();
        }

        private void UpdateCartTotals()
        {
            decimal total = _cart.Sum(c => c.Total);
            string formattedTotal = "\u20B1" + total.ToString("#,##0.00");
            lblSubtotalValue.Text = formattedTotal;
            lblTotalValue.Text = formattedTotal;
            btnCompleteSale.Text = $"\u20B1 Complete Sale ({formattedTotal})";
        }

        // ══════════════════════════════════════════════════════════════
        //  SECTION 3: CHECKOUT WORKFLOW
        // ══════════════════════════════════════════════════════════════

        private void BtnClearCart_Click(object? sender, EventArgs e)
        {
            if (_cart.Count == 0) return;
            if (MessageBox.Show("Are you sure you want to clear the current cart?", "Clear Cart", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _cart.Clear();
                RefreshCartUI();
            }
        }

        private bool _isProcessingSale; // Prevent duplicate sales

        private async void BtnCompleteSale_Click(object? sender, EventArgs e)
        {
            if (_isProcessingSale) return; // Guard against rapid clicks
            if (_cart.Count == 0) return;

            // ── Session Gate: block checkout if no active session ────
            if (!Program.SessionService.HasActiveSession())
            {
                MessageBox.Show(
                    "No active store session.\n\nPlease open a store session before processing orders.",
                    "Store Session Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            decimal total = _cart.Sum(c => c.Total);

            // ── Step 1: Show Checkout Dialog ──────────────────────────
            using var dlg = new CheckoutDialogForm(total);
            if (dlg.ShowDialog(this.FindForm()) != DialogResult.OK) return;

            // Lock processing
            _isProcessingSale = true;
            btnCompleteSale.Enabled = false;
            btnCompleteSale.Text = "Processing...";

            try
            {
                bool isDineIn = dlg.IsDineIn;
                string orderType = dlg.SelectedOrderType;
                var customer = dlg.SelectedCustomer;
                decimal cashReceived = dlg.CashReceived;
                decimal changeDue = dlg.ChangeDue;

                // ── Step 2: Build the Order ───────────────────────────────
                string cashierName = Program.GetCurrentCashierName();
                var order = new Order
                {
                    OrderId = Program.DataService.GenerateOrderNumber(),
                    Timestamp = DateTime.Now,
                    Subtotal = total,
                    Total = total,
                    IsDineIn = isDineIn,
                    OrderType = orderType,
                    CustomerId = customer?.Id,
                    CustomerName = customer?.Name ?? "Walk-In",
                    CustomerEmail = customer?.Email ?? string.Empty,
                    PaymentMethod = "Cash",
                    CashierName = cashierName,
                    CashTendered = cashReceived,
                    ChangeGiven = changeDue,
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

                // ── Step 4: Link to active session & Persist ─────────────
                Program.SessionService.AttachOrderToSession(order);
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing sale:\n{ex.Message}", "Sale Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isProcessingSale = false;
                btnCompleteSale.Enabled = true;
                btnCompleteSale.Text = "Complete Sale";
            }
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
