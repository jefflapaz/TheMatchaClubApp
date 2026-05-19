using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubDomain.Models;
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

            Program.DataService.CategoriesChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(() => 
                {
                    // If active category was deleted, reset to All Items
                    if (_activeCategory != "All Items" && !Program.DataService.Categories.Any(c => c.Name == _activeCategory))
                    {
                        _activeCategory = "All Items";
                        PopulateItems(_activeCategory);
                    }
                    PopulateCategories();
                }));
            };

            flpCategoryButtons.AllowDrop = true;
            flpCategoryButtons.DragEnter += (s, e) => { if (e.Data!.GetDataPresent(typeof(CategoryNavItem))) e.Effect = DragDropEffects.Move; };
            flpCategoryButtons.DragOver += (s, e) =>
            {
                if (e.Data!.GetDataPresent(typeof(CategoryNavItem)))
                {
                    var source = (CategoryNavItem)e.Data.GetData(typeof(CategoryNavItem))!;
                    var pt = flpCategoryButtons.PointToClient(new Point(e.X, e.Y));
                    var target = flpCategoryButtons.GetChildAtPoint(pt);
                    if (target != null && target != source && target is CategoryNavItem tNav && !tNav.IsProtected)
                    {
                        int targetIdx = flpCategoryButtons.Controls.GetChildIndex(target);
                        flpCategoryButtons.Controls.SetChildIndex(source, targetIdx);
                    }
                }
            };
            flpCategoryButtons.DragDrop += async (s, e) =>
            {
                var ordered = flpCategoryButtons.Controls.OfType<CategoryNavItem>()
                    .Where(n => !n.IsProtected)
                    .Select(n => n.Category)
                    .ToList();
                await Program.DataService.UpdateCategoryOrderAsync(ordered);
            };

            PopulateCategories();
            PopulateItems("All Items");
        }

        private void PopulateCategories()
        {
            flpCategoryButtons.SuspendLayout();
            flpCategoryButtons.Controls.Clear();

            // "All Items" is virtual
            var allItemsCat = new Category { Name = "All Items", DisplayOrder = -1 };
            var navAll = new CategoryNavItem(allItemsCat, true);
            navAll.CategoryClicked += (s, e) =>
            {
                _activeCategory = "All Items";
                UpdateCategoryButtons();
                PopulateItems("All Items");
            };
            flpCategoryButtons.Controls.Add(navAll);

            var categories = Program.DataService.Categories
                .Where(c => c.Name != "All Items")
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            foreach (var cat in categories)
            {
                var navItem = new CategoryNavItem(cat, false);
                
                navItem.CategoryClicked += (s, e) => 
                {
                    _activeCategory = cat.Name;
                    UpdateCategoryButtons();
                    PopulateItems(cat.Name);
                };

                navItem.EditClicked += async (s, e) =>
                {
                    string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new name for category:", "Rename Category", cat.Name);
                    if (!string.IsNullOrWhiteSpace(newName) && newName != cat.Name)
                    {
                        if (Program.DataService.Categories.Any(c => string.Equals(c.Name, newName, StringComparison.OrdinalIgnoreCase)))
                        {
                            var msg = new Guna.UI2.WinForms.Guna2MessageDialog { Parent = this.FindForm(), Caption = "Error", Text = "Category already exists.", Style = Guna.UI2.WinForms.MessageDialogStyle.Light, Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, Icon = Guna.UI2.WinForms.MessageDialogIcon.Error };
                            msg.Show();
                            return;
                        }
                        await Program.DataService.RenameCategoryAsync(cat, newName);
                        PopulateCategories();
                        if (_activeCategory == cat.Name) PopulateItems(newName);
                    }
                };

                navItem.DeleteClicked += async (s, e) =>
                {
                    // Check if category is empty
                    bool hasProducts = Program.DataService.Products.Any(p => string.Equals(p.CategoryName, cat.Name, StringComparison.OrdinalIgnoreCase));
                    if (hasProducts)
                    {
                        var msg = new Guna.UI2.WinForms.Guna2MessageDialog { Parent = this.FindForm(), Caption = "Cannot Delete", Text = $"You cannot delete '{cat.Name}' because there are still products assigned to it. Please remove or reassign all products first.", Style = Guna.UI2.WinForms.MessageDialogStyle.Light, Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK, Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning };
                        msg.Show();
                    }
                    else
                    {
                        var confirmMsg = new Guna.UI2.WinForms.Guna2MessageDialog { Parent = this.FindForm(), Caption = "Confirm Delete", Text = $"Are you sure you want to delete the empty category '{cat.Name}'?", Style = Guna.UI2.WinForms.MessageDialogStyle.Light, Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo, Icon = Guna.UI2.WinForms.MessageDialogIcon.Question };
                        var confirm = confirmMsg.Show();
                        if (confirm == DialogResult.Yes)
                        {
                            Program.DataService.Categories.Remove(cat);
                            await Program.DataService.SaveCategoriesAsync();
                            if (_activeCategory == cat.Name)
                            {
                                _activeCategory = "All Items";
                            }
                            PopulateCategories();
                            PopulateItems(_activeCategory);
                        }
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

            if (category == "All Items")
            {
                filtered = all;
            }
            else
            {
                filtered = all.Where(p => 
                    p.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // Apply Sort
            string sortOpt = cmbSort.SelectedItem?.ToString() ?? "Newest";
            if (sortOpt == "Oldest")
            {
                filtered = filtered.OrderBy(p => p.CreatedAt).ToList();
            }
            else if (sortOpt == "A → Z")
            {
                filtered = filtered.OrderBy(p => p.Name ?? "", StringComparer.OrdinalIgnoreCase).ToList();
            }
            else if (sortOpt == "Z → A")
            {
                filtered = filtered.OrderByDescending(p => p.Name ?? "", StringComparer.OrdinalIgnoreCase).ToList();
            }
            else
            {
                filtered = filtered.OrderByDescending(p => p.CreatedAt).ToList();
            }

            foreach (var p in filtered)
            {
                var card = new ProductItemCard { Product = p };
                card.EditClicked += (s, e) => ShowEditDialog(p);
                card.DeleteClicked += (s, e) => SmartDelete(p);
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
            lblTotalItems.Text = $"Total: {all.Count} Items";

            flpItems.ResumeLayout();
            UpdateCategoryButtons();
        }

        // ── Smart Delete ─────────────────────────────────────────────
        private async void SmartDelete(Product product)
        {
            // Hard delete confirmation
            var result = MessageBox.Show(
                $"Permanently delete '{product.Name}'?\nThis action cannot be undone.",
                "Delete Product",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (Program.DataService.Settings.RequirePasswordForDeleteProduct)
                {
                    using var authDialog = new PasswordPromptDialog("Enter password to delete this product.");
                    if (authDialog.ShowDialog(this) != DialogResult.OK) return;
                }
                Program.DataService.Products.Remove(product);
                await Program.DataService.SaveProductsAsync();
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
                Size = new Size(450, 480),
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
            
            var categories = Program.DataService.Categories
                .Where(c => c.Name != "All Items")
                .OrderBy(c => c.DisplayOrder)
                .Select(c => c.Name)
                .ToList();
            cmbCat.Items.AddRange(categories.ToArray());
            if (!isNew && categories.Contains(product.CategoryName)) cmbCat.SelectedItem = product.CategoryName;

            var btnAddCat = new Guna.UI2.WinForms.Guna2Button { Text = "+", Location = new Point(390, 152), Size = new Size(40, 40), FillColor = ColorTranslator.FromHtml("#52B743"), ForeColor = Color.White, Font = new Font("Segoe UI", 14F, FontStyle.Bold), BorderRadius = 6 };

            // ── Inline New Category Panel ──
            var pnlNewCategory = new Guna.UI2.WinForms.Guna2Panel
            {
                Location = new Point(20, 200),
                Size = new Size(410, 80),
                FillColor = ColorTranslator.FromHtml("#F9FAFB"),
                BorderColor = ColorTranslator.FromHtml("#E5E7EB"),
                BorderThickness = 1,
                BorderRadius = 8,
                Visible = false
            };
            
            var txtNewCat = new Guna.UI2.WinForms.Guna2TextBox
            {
                PlaceholderText = "New Category Name",
                Location = new Point(10, 20),
                Size = new Size(250, 40),
                BorderRadius = 6
            };
            
            var btnSaveCat = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Add",
                Location = new Point(270, 20),
                Size = new Size(60, 40),
                FillColor = ColorTranslator.FromHtml("#52B743"),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BorderRadius = 6
            };
            
            var btnCancelCat = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "X",
                Location = new Point(340, 20),
                Size = new Size(60, 40),
                FillColor = ColorTranslator.FromHtml("#F3F4F6"),
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BorderRadius = 6
            };
            
            var lblCatError = new Label { Text = "", ForeColor = ColorTranslator.FromHtml("#EF4444"), Font = new Font("Segoe UI", 8F), Location = new Point(10, 62), AutoSize = true, Visible = false, BackColor = Color.Transparent };

            pnlNewCategory.Controls.AddRange(new Control[] { txtNewCat, btnSaveCat, btnCancelCat, lblCatError });

            var lblPrice = new Label { Text = "Price (₱)", Location = new Point(20, 200), AutoSize = true };
            var txtPrice = new Guna.UI2.WinForms.Guna2TextBox { Text = isNew ? "" : product.Price.ToString("F2"), Location = new Point(20, 222), Size = new Size(410, 40) };

            var lblImage = new Label { Text = "Image", Location = new Point(20, 275), AutoSize = true };
            var picPreview = new PictureBox { Location = new Point(20, 297), Size = new Size(100, 100), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
            picPreview.Image = ImageHelper.LoadOrPlaceholder(product.ImagePath, product.Name, picPreview.Width);

            var txtImagePath = new Guna.UI2.WinForms.Guna2TextBox { Text = Program.DataService.GetFullImagePath(product.ImagePath), Location = new Point(130, 297), Size = new Size(200, 40), ReadOnly = true };
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

            var btnCancel = new Guna.UI2.WinForms.Guna2Button { Text = "Cancel", Location = new Point(20, 410), Size = new Size(195, 45), FillColor = ColorTranslator.FromHtml("#F3F4F6"), ForeColor = ColorTranslator.FromHtml("#374151"), Font = new Font("Segoe UI", 10F, FontStyle.Bold), BorderRadius = 8 };
            btnCancel.Click += (s, e) => dlg.Close();

            // Shift elements logic for inline category
            int shiftAmount = 90;
            var shiftableControls = new Control[] { lblPrice, txtPrice, lblImage, picPreview, txtImagePath, btnBrowse, btnCancel };

            void ToggleNewCategoryPanel(bool show)
            {
                pnlNewCategory.Visible = show;
                btnAddCat.Enabled = !show;
                int shift = show ? shiftAmount : -shiftAmount;
                dlg.Height += shift;
                foreach (var ctrl in shiftableControls)
                {
                    ctrl.Top += shift;
                }
                if (show)
                {
                    txtNewCat.Text = "";
                    lblCatError.Visible = false;
                    txtNewCat.Focus();
                }
            }

            btnAddCat.Click += (s, e) => ToggleNewCategoryPanel(true);
            btnCancelCat.Click += (s, e) => ToggleNewCategoryPanel(false);

            Action saveCategory = async () =>
            {
                string newCat = txtNewCat.Text.Trim();
                if (string.IsNullOrWhiteSpace(newCat))
                {
                    lblCatError.Text = "Category name cannot be empty.";
                    lblCatError.Visible = true;
                    return;
                }
                if (Program.DataService.Categories.Any(c => string.Equals(c.Name, newCat, StringComparison.OrdinalIgnoreCase)))
                {
                    lblCatError.Text = "Category already exists.";
                    lblCatError.Visible = true;
                    return;
                }

                var catObj = new Category { Name = newCat, DisplayOrder = Program.DataService.Categories.Count };
                Program.DataService.Categories.Add(catObj);
                await Program.DataService.SaveCategoriesAsync();
                
                cmbCat.Items.Add(newCat);
                cmbCat.SelectedItem = newCat;
                PopulateCategories();
                
                ToggleNewCategoryPanel(false);
            };

            btnSaveCat.Click += (s, e) => saveCategory();
            txtNewCat.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; saveCategory(); } };

            var btnSave = new Guna.UI2.WinForms.Guna2Button { Text = "Save", Location = new Point(235, 410), Size = new Size(195, 45), FillColor = ColorTranslator.FromHtml("#52B743"), ForeColor = Color.White, Font = new Font("Segoe UI", 10F, FontStyle.Bold), BorderRadius = 8 };
            
            // Add btnSave to the shiftable controls
            var tempShiftable = new List<Control>(shiftableControls) { btnSave };
            shiftableControls = tempShiftable.ToArray();

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

                // Copy image to local storage if needed
                if (!string.IsNullOrWhiteSpace(txtImagePath.Text))
                {
                    product.ImagePath = Program.DataService.CopyImageToLocal(txtImagePath.Text);
                }

                if (isNew) Program.DataService.Products.Add(product);
                await Program.DataService.SaveProductsAsync();
                
                dlg.Close();
            };

            dlg.Controls.AddRange(new Control[] { pnlHead, lblName, txtName, lblCat, cmbCat, btnAddCat, pnlNewCategory, lblPrice, txtPrice, lblImage, picPreview, txtImagePath, btnBrowse, btnCancel, btnSave });
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
