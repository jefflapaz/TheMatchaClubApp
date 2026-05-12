using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;
using TheMatchaClubApp.Helpers;

namespace TheMatchaClubApp.Forms
{
    public partial class ItemsView : UserControl
    {
        private string _activeCategory = "All Items";

        public ItemsView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();

            Program.DataService.ProductsChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(() => PopulateItems(_activeCategory)));
            };

            PopulateCategories();
            PopulateItems("All Items");
        }

        private void PopulateCategories()
        {
            flpCategoryButtons.SuspendLayout();
            flpCategoryButtons.Controls.Clear();

            var categories = new List<string> { "All Items" };
            var customCats = Program.DataService.Categories
                .Where(c => c != "All Items" && c != "Out of stock")
                .Distinct()
                .ToList();
            categories.AddRange(customCats);
            categories.Add("Out of stock");

            foreach (var cat in categories)
            {
                bool isProtected = cat == "All Items" || cat == "Out of stock";
                var navItem = new CategoryNavItem(cat, isProtected);
                
                navItem.CategoryClicked += (s, e) => 
                {
                    _activeCategory = cat;
                    UpdateCategoryButtons();
                    PopulateItems(cat);
                };

                navItem.DeleteClicked += async (s, e) =>
                {
                    if (isProtected) return;
                    
                    bool inUse = Program.DataService.Products.Any(p => string.Equals(p.CategoryName, cat, StringComparison.OrdinalIgnoreCase));
                    if (inUse)
                    {
                        var msg = new Guna.UI2.WinForms.Guna2MessageDialog 
                        { 
                            Caption = "Error", 
                            Text = "Cannot delete. Move or delete existing items first.", 
                            Style = Guna.UI2.WinForms.MessageDialogStyle.Light, 
                            Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, 
                            Icon = Guna.UI2.WinForms.MessageDialogIcon.Error 
                        };
                        msg.Show();
                    }
                    else
                    {
                        Program.DataService.Categories.Remove(cat);
                        await Program.DataService.SaveCategoriesAsync();
                        if (_activeCategory == cat) _activeCategory = "All Items";
                        PopulateCategories();
                        PopulateItems(_activeCategory);
                    }
                };

                flpCategoryButtons.Controls.Add(navItem);
            }
            
            UpdateCategoryButtons();
            flpCategoryButtons.ResumeLayout();
        }

        private void UpdateCategoryButtons()
        {
            foreach (Control ctrl in flpCategoryButtons.Controls)
            {
                if (ctrl is CategoryNavItem navItem)
                {
                    navItem.SetActive(navItem.CategoryName == _activeCategory);
                }
            }
        }

        // ── Grid Population ──────────────────────────────────────────
        private void PopulateItems(string category)
        {
            _activeCategory = category;
            flpItems.SuspendLayout();
            flpItems.Controls.Clear();

            var all = Program.DataService.Products.ToList();
            List<Product> filtered;

            if (category == "Out of stock")
            {
                filtered = all.Where(p => p.StockLevel == 0 || p.IsOutOfStock).ToList();
            }
            else if (category == "All Items")
            {
                filtered = all; // Show everything, out-of-stock will be dimmed
            }
            else
            {
                // Normal category: Exclude out-of-stock items
                filtered = all.Where(p => 
                    p.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase) && 
                    p.StockLevel > 0 && 
                    !p.IsOutOfStock
                ).ToList();
            }

            // Apply Sort
            if (cmbSort.SelectedItem?.ToString() == "Oldest")
            {
                filtered = filtered.OrderBy(p => p.CreatedAt).ToList();
            }
            else
            {
                filtered = filtered.OrderByDescending(p => p.CreatedAt).ToList();
            }

            foreach (var item in filtered)
            {
                var card = new InventoryCard
                {
                    Product = item,
                    Size = new Size(240, 310),
                    Margin = new Padding(8)
                };
                card.EditClicked += (s, e) => ShowEditDialog(item);
                card.DeleteClicked += (s, e) => SmartDelete(item);
                flpItems.Controls.Add(card);
            }

            // End of List footer
            var pnlEnd = new Panel
            {
                Size = new Size(flpItems.Width - 40, 120),
                Margin = new Padding(10, 40, 10, 40)
            };
            
            // Hexagon/Box icon background
            var pnlIconBg = new Guna.UI2.WinForms.Guna2Panel
            {
                Size = new Size(48, 48),
                BorderRadius = 12, // Hexagon-ish rounded square
                FillColor = ColorTranslator.FromHtml("#F3F4F6"),
                Location = new Point(pnlEnd.Width / 2 - 24, 10)
            };
            
            var lblBox = new Label
            {
                Text = "📦", // Box emoji
                Font = new Font("Segoe UI", 16F),
                AutoSize = false,
                Size = new Size(48, 48),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                Location = new Point(0, 0)
            };
            pnlIconBg.Controls.Add(lblBox);
            
            var lblEndTitle = new Label
            {
                Text = "End of List",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#111827"),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(200, 24),
                Location = new Point(pnlEnd.Width / 2 - 100, 66)
            };
            var lblEndDesc = new Label
            {
                Text = $"Showing all {filtered.Count} products in {_activeCategory}.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(300, 20),
                Location = new Point(pnlEnd.Width / 2 - 150, 90)
            };
            pnlEnd.Controls.Add(pnlIconBg);
            pnlEnd.Controls.Add(lblEndTitle);
            pnlEnd.Controls.Add(lblEndDesc);
            flpItems.Controls.Add(pnlEnd);

            lblItemCount.Text = $"{category} ({filtered.Count})";
            int lowStockCount = filtered.Count(p => p.StockLevel <= 5);
            lblLowStock.Text = lowStockCount > 0 ? $"⚠ {lowStockCount} Low Stock Items" : "";
            lblTotalItems.Text = $"Total: {all.Count} Items";

            flpItems.ResumeLayout();
            UpdateCategoryButtons();
        }

        // ── Smart Delete ─────────────────────────────────────────────
        private async void SmartDelete(Product product)
        {
            bool hasHistory = Program.DataService.Orders.Any(o => o.Items.Any(i => i.ProductId == product.Id));

            if (hasHistory)
            {
                // Cannot hard-delete — offer "Move to Out of Stock"
                var result = MessageBox.Show(
                    $"'{product.Name}' has recorded sales in order history.\n\n" +
                    "It cannot be permanently deleted to preserve historical data.\n" +
                    "Would you like to move it to 'Out of Stock' instead?",
                    "Smart Delete Guard",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    product.IsOutOfStock = true;
                    product.CategoryName = "Out of Stock";
                    product.StockLevel = 0;
                    await Program.DataService.SaveProductsAsync();
                }
            }
            else
            {
                // Hard delete allowed
                var result = MessageBox.Show(
                    $"Permanently delete '{product.Name}'?\nThis action cannot be undone.",
                    "Delete Product",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Program.DataService.Products.Remove(product);
                    await Program.DataService.SaveProductsAsync();
                }
            }
        }

        // ── Add / Edit Dialog ────────────────────────────────────────
        private void ShowAddDialog()
        {
            ShowEditDialog(null);
        }

        private async void ShowEditDialog(Product? existing)
        {
            bool isNew = (existing == null);
            var product = existing ?? new Product();

            using var dlg = new Form
            {
                Text = isNew ? "Add Product" : "Edit Product",
                Size = new Size(450, 560),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.White
            };

            dlg.Paint += (s, e) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 2);
                e.Graphics.DrawRectangle(pen, 0, 0, dlg.Width - 1, dlg.Height - 1);
            };

            var pnlHead = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.White };
            var lblTitle = new Label { Text = isNew ? "Add Product" : "Edit Product", Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(20, 12), AutoSize = true };
            var btnX = new Guna.UI2.WinForms.Guna2Button { Text = "✕", Size = new Size(36, 36), Location = new Point(dlg.Width - 46, 7), FillColor = Color.Transparent, ForeColor = ColorTranslator.FromHtml("#9CA3AF"), Font = new Font("Segoe UI", 14F, FontStyle.Bold), Cursor = Cursors.Hand };
            btnX.Click += (s, e) => dlg.Close();
            pnlHead.Controls.Add(lblTitle);
            pnlHead.Controls.Add(btnX);

            var lblName = new Label { Text = "Product Name", Location = new Point(20, 60), AutoSize = true };
            var txtName = new Guna.UI2.WinForms.Guna2TextBox { Text = product.Name, Location = new Point(20, 82), Size = new Size(410, 40) };

            var lblCat = new Label { Text = "Category", Location = new Point(20, 130), AutoSize = true };
            var cmbCat = new Guna.UI2.WinForms.Guna2ComboBox { Location = new Point(20, 152), Size = new Size(360, 40), BorderColor = ColorTranslator.FromHtml("#52B743") };
            
            var categories = Program.DataService.Categories.Where(c => c != "All Items" && c != "Out of Stock").Distinct().ToList();
            cmbCat.Items.AddRange(categories.ToArray());
            if (!isNew && categories.Contains(product.CategoryName)) cmbCat.SelectedItem = product.CategoryName;

            var btnAddCat = new Guna.UI2.WinForms.Guna2Button { Text = "+", Location = new Point(390, 152), Size = new Size(40, 40), FillColor = ColorTranslator.FromHtml("#52B743"), ForeColor = Color.White, Font = new Font("Segoe UI", 14F, FontStyle.Bold), BorderRadius = 6 };
            btnAddCat.Click += async (s, e) =>
            {
                string newCat = Microsoft.VisualBasic.Interaction.InputBox("Enter new category name:", "New Category", "");
                if (string.IsNullOrWhiteSpace(newCat)) return;
                
                if (Program.DataService.Categories.Contains(newCat, StringComparer.OrdinalIgnoreCase))
                {
                    var msg = new Guna.UI2.WinForms.Guna2MessageDialog { Caption = "Error", Text = "Error: This category already exists.", Style = Guna.UI2.WinForms.MessageDialogStyle.Light, Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, Icon = Guna.UI2.WinForms.MessageDialogIcon.Error };
                    msg.Show();
                }
                else
                {
                    Program.DataService.Categories.Add(newCat);
                    await Program.DataService.SaveCategoriesAsync();
                    cmbCat.Items.Add(newCat);
                    cmbCat.SelectedItem = newCat;
                }
            };

            var lblPrice = new Label { Text = "Price (₱)", Location = new Point(20, 200), AutoSize = true };
            var txtPrice = new Guna.UI2.WinForms.Guna2TextBox { Text = isNew ? "" : product.Price.ToString("F2"), Location = new Point(20, 222), Size = new Size(195, 40) };

            var lblStock = new Label { Text = "Units/Stock", Location = new Point(235, 200), AutoSize = true };
            var txtStock = new Guna.UI2.WinForms.Guna2TextBox { Text = isNew ? "" : product.StockLevel.ToString(), Location = new Point(235, 222), Size = new Size(195, 40) };

            var lblImage = new Label { Text = "Image", Location = new Point(20, 275), AutoSize = true };
            var picPreview = new PictureBox { Location = new Point(20, 297), Size = new Size(100, 100), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
            picPreview.Image = ImageHelper.LoadOrPlaceholder(product.ImagePath, product.Name, picPreview.Width);

            var txtImagePath = new Guna.UI2.WinForms.Guna2TextBox { Text = product.ImagePath, Location = new Point(130, 297), Size = new Size(200, 40), ReadOnly = true };
            var btnBrowse = new Guna.UI2.WinForms.Guna2Button { Text = "Browse...", Location = new Point(340, 297), Size = new Size(90, 40), FillColor = ColorTranslator.FromHtml("#F3F4F6"), ForeColor = ColorTranslator.FromHtml("#374151") };
            btnBrowse.Click += (s, e) =>
            {
                using var ofd = new OpenFileDialog { Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtImagePath.Text = ofd.FileName;
                    picPreview.ImageLocation = ofd.FileName;
                }
            };

            var btnCancel = new Guna.UI2.WinForms.Guna2Button { Text = "Cancel", Location = new Point(20, 480), Size = new Size(195, 45), FillColor = ColorTranslator.FromHtml("#F3F4F6"), ForeColor = ColorTranslator.FromHtml("#374151"), Font = new Font("Segoe UI", 10F, FontStyle.Bold), BorderRadius = 8 };
            btnCancel.Click += (s, e) => dlg.Close();

            var btnSave = new Guna.UI2.WinForms.Guna2Button { Text = "Save", Location = new Point(235, 480), Size = new Size(195, 45), FillColor = ColorTranslator.FromHtml("#52B743"), ForeColor = Color.White, Font = new Font("Segoe UI", 10F, FontStyle.Bold), BorderRadius = 8 };

            btnSave.Click += async (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || cmbCat.SelectedItem == null)
                {
                    MessageBox.Show("Product name and Category are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                product.Name = txtName.Text.Trim();
                product.CategoryName = cmbCat.SelectedItem.ToString() ?? "";
                if (decimal.TryParse(txtPrice.Text, out decimal price)) product.Price = price;
                if (int.TryParse(txtStock.Text, out int stock)) 
                {
                    product.StockLevel = stock;
                    if (stock > 0) product.IsOutOfStock = false;
                }

                // Copy image to local storage
                if (!string.IsNullOrWhiteSpace(txtImagePath.Text) && txtImagePath.Text != product.ImagePath)
                {
                    product.ImagePath = Program.DataService.CopyImageToLocal(txtImagePath.Text);
                }

                if (isNew) Program.DataService.Products.Add(product);
                await Program.DataService.SaveProductsAsync();
                
                dlg.Close();
            };

            dlg.Controls.AddRange(new Control[] { pnlHead, lblName, txtName, lblCat, cmbCat, btnAddCat, lblPrice, txtPrice, lblStock, txtStock, lblImage, picPreview, txtImagePath, btnBrowse, btnCancel, btnSave });
            dlg.ShowDialog();
        }

        // ── Category Filter ──────────────────────────────────────────
        private void CategoryBtn_Click(object? sender, EventArgs e)
        {
            if (sender is Guna.UI2.WinForms.Guna2Button btn)
            {
                PopulateItems(btn.Text);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
