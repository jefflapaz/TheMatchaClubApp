using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubDomain.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class CustomersView : UserControl
    {
        private Customer? _currentCustomer;
        private string _currentFilter = "All";
        private bool _notesEditMode = false;
        private List<Order> _currentOrders = new();

        public CustomersView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();

            splitContainerMain.SplitterDistance = 350;

            SetupDataGrid();
            WireEvents();
            
            // Initial load
            LoadDirectory();
        }

        private void SetupDataGrid()
        {
            dgvHistory.Columns.Clear();
            dgvHistory.Columns.Add("OrderId", "Order No.");
            dgvHistory.Columns.Add("Date", "Date & Time");
            dgvHistory.Columns.Add("Items", "Purchased Items");
            dgvHistory.Columns.Add("Amount", "Amount");
            dgvHistory.Columns.Add("Status", "Status");
            
            var btnCol = new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Action",
                Text = "View Receipt",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            };
            dgvHistory.Columns.Add(btnCol);
        }

        private string _sortOrder = "az"; // default A-Z

        private void WireEvents()
        {
            txtSearch.TextChanged += (s, e) => LoadDirectory();
            btnFilterAll.Click += (s, e) => { SetFilter("All"); LoadDirectory(); };
            btnFilterNew.Click += (s, e) => { SetFilter("New"); LoadDirectory(); };
            btnFilterRegular.Click += (s, e) => { SetFilter("Regular"); LoadDirectory(); };
            btnFilterLoyal.Click += (s, e) => { SetFilter("Loyal"); LoadDirectory(); };
            btnFilterFrequent.Click += (s, e) => { SetFilter("Frequent"); LoadDirectory(); };
            
            cmbSort.SelectedIndexChanged += (s, e) =>
            {
                _sortOrder = cmbSort.SelectedIndex switch
                {
                    0 => "az",
                    1 => "za",
                    2 => "newest",
                    3 => "oldest",
                    _ => "az"
                };
                LoadDirectory();
            };
            
            dgvHistory.CellContentClick += DgvHistory_CellContentClick;
            btnSaveNote.Click += BtnSaveNote_Click;
            
            btnCalendarClose.Click += (s, e) => pnlCalendarPopup.Visible = false;
            
            // History filter events
            txtHistorySearch.TextChanged += (s, e) => FilterPurchaseHistory();
            cmbDateFilter.SelectedIndexChanged += (s, e) =>
            {
                dtpCustomDate.Visible = cmbDateFilter.SelectedIndex == 4; // Custom Date
                FilterPurchaseHistory();
            };
            dtpCustomDate.ValueChanged += (s, e) => FilterPurchaseHistory();

            flpCalendarDays.Scroll += FlpCalendarDays_Scroll;
            flpCalendarDays.MouseWheel += FlpCalendarDays_Scroll;
        }

        private void SetFilter(string filter)
        {
            _currentFilter = filter;
            StyleFilterButton(btnFilterAll, filter == "All");
            StyleFilterButton(btnFilterNew, filter == "New");
            StyleFilterButton(btnFilterRegular, filter == "Regular");
            StyleFilterButton(btnFilterLoyal, filter == "Loyal");
            StyleFilterButton(btnFilterFrequent, filter == "Frequent");
        }

        /// <summary>
        /// Dynamic classification using configurable thresholds from Settings.
        /// New → Regular → Loyal → Frequent (by order count or lifetime spend).
        /// </summary>
        private string GetDynamicStatus(Customer c)
        {
            var settings = Program.DataService.Settings;
            var orders = Program.DataService.Orders.Where(o => o.CustomerId == c.Id).ToList();
            int orderCount = orders.Count;
            decimal lifetimeSpend = orders.Sum(o => o.Total);

            if (orderCount >= settings.CustomerTierFrequentMin || lifetimeSpend >= settings.CustomerTierFrequentSpend)
                return "Frequent";
            if (orderCount >= settings.CustomerTierLoyalMin)
                return "Loyal";
            if (orderCount >= settings.CustomerTierRegularMin)
                return "Regular";
            return "New";
        }

        private Color GetTierColor(string tier) => tier switch
        {
            "Frequent" => ColorTranslator.FromHtml("#52B743"),
            "Loyal" => ColorTranslator.FromHtml("#3B82F6"),
            "Regular" => ColorTranslator.FromHtml("#6B7280"),
            _ => ColorTranslator.FromHtml("#D946EF") // New
        };

        private void LoadDirectory()
        {
            flpCustomers.SuspendLayout();
            flpCustomers.Controls.Clear();

            string search = txtSearch.Text.Trim();
            
            var allCustomers = Program.DataService.Customers;

            // Compute dynamic status counts
            var tierCounts = allCustomers.GroupBy(c => GetDynamicStatus(c)).ToDictionary(g => g.Key, g => g.Count());
            
            btnFilterAll.Text = $"All ({allCustomers.Count})";
            btnFilterNew.Text = $"New ({tierCounts.GetValueOrDefault("New", 0)})";
            btnFilterRegular.Text = $"Regular ({tierCounts.GetValueOrDefault("Regular", 0)})";
            btnFilterLoyal.Text = $"Loyal ({tierCounts.GetValueOrDefault("Loyal", 0)})";
            btnFilterFrequent.Text = $"Frequent ({tierCounts.GetValueOrDefault("Frequent", 0)})";
            
            var filtered = allCustomers.AsEnumerable();
            if (_currentFilter != "All")
            {
                filtered = filtered.Where(c => string.Equals(GetDynamicStatus(c), _currentFilter, StringComparison.OrdinalIgnoreCase));
            }
            
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(c => 
                    c.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting
            var sorted = _sortOrder switch
            {
                "za" => filtered.OrderByDescending(c => c.Name),
                "newest" => filtered.OrderByDescending(c => c.MemberSince),
                "oldest" => filtered.OrderBy(c => c.MemberSince),
                _ => filtered.OrderBy(c => c.Name) // "az"
            };

            foreach (var c in sorted)
            {
                var card = CreateCustomerCard(c);
                flpCustomers.Controls.Add(card);
            }
            
            flpCustomers.ResumeLayout();
        }

        private Panel CreateCustomerCard(Customer c)
        {
            int cardWidth = Math.Max(flpCustomers.ClientSize.Width - 24, 200);
            var pnl = new Guna.UI2.WinForms.Guna2Panel
            {
                Size = new Size(cardWidth, 68),
                Margin = new Padding(0, 0, 0, 6),
                BorderRadius = 8,
                FillColor = Color.White,
                BorderColor = ColorTranslator.FromHtml("#E5E7EB"),
                BorderThickness = 1,
                Cursor = Cursors.Hand
            };
            
            pnl.Click += (s, e) => SelectCustomer(c);
            
            void BindClick(Control parent)
            {
                foreach (Control child in parent.Controls)
                {
                    child.Click += (s, e) => SelectCustomer(c);
                    BindClick(child);
                }
            }

            var pic = new Guna.UI2.WinForms.Guna2CirclePictureBox
            {
                Size = new Size(36, 36),
                Location = new Point(10, 16),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            if (!string.IsNullOrEmpty(c.ProfileImagePath) && System.IO.File.Exists(c.ProfileImagePath))
                pic.Image = Image.FromFile(c.ProfileImagePath);
            else
                pic.Image = GenerateInitialsImage(c.Name);
            pnl.Controls.Add(pic);

            var lblName = new Label
            {
                Text = c.Name,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                Location = new Point(54, 12),
                AutoSize = true,
                ForeColor = ColorTranslator.FromHtml("#111827")
            };
            pnl.Controls.Add(lblName);

            var lblEmail = new Label
            {
                Text = c.Email,
                Font = new Font("Segoe UI", 7.5F),
                Location = new Point(54, 34),
                AutoSize = true,
                ForeColor = ColorTranslator.FromHtml("#6B7280")
            };
            pnl.Controls.Add(lblEmail);
            
            var orders = Program.DataService.Orders.Where(o => o.CustomerId == c.Id).ToList();
            decimal totalSpent = orders.Sum(o => o.Total);

            var lblSpent = new Label
            {
                Text = FormatCurrency(totalSpent),
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Location = new Point(cardWidth - 100, 12),
                AutoSize = true,
                ForeColor = ColorTranslator.FromHtml("#111827")
            };
            pnl.Controls.Add(lblSpent);

            string dynamicStatus = GetDynamicStatus(c);
            var lblStatus = new Label
            {
                Text = dynamicStatus,
                Location = new Point(cardWidth - 100, 34),
                AutoSize = true,
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                ForeColor = GetTierColor(dynamicStatus),
                BackColor = Color.Transparent
            };
            pnl.Controls.Add(lblStatus);

            BindClick(pnl);
            return pnl;
        }

        private void SelectCustomer(Customer c)
        {
            _currentCustomer = c;
            pnlCalendarPopup.Visible = false;
            PopulateProfile(c);
        }

        /// <summary>Navigate to a specific customer by ID (used for cross-module navigation).</summary>
        public void SelectCustomerById(Guid customerId)
        {
            var customer = Program.DataService.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer != null)
            {
                txtSearch.Text = "";
                SetFilter("All");
                LoadDirectory();
                SelectCustomer(customer);
            }
        }

        private void PopulateProfile(Customer c)
        {
            lblProfileName.Text = c.Name;
            lblProfileEmail.Text = c.Email;
            lblProfilePhone.Text = c.Phone;
            
            if (!string.IsNullOrEmpty(c.ProfileImagePath) && System.IO.File.Exists(c.ProfileImagePath))
                picProfile.Image = Image.FromFile(c.ProfileImagePath);
            else
                picProfile.Image = GenerateInitialsImage(c.Name);

            var orders = Program.DataService.Orders.Where(o => o.CustomerId == c.Id).ToList();
            _currentOrders = orders;
            int totalVisits = orders.Count;
            decimal lifetimeValue = orders.Sum(o => o.Total);
            decimal avgOrderValue = totalVisits > 0 ? lifetimeValue / totalVisits : 0;

            flpKPIs.Controls.Clear();
            flpKPIs.Controls.Add(CreateKPICard("LIFETIME VALUE", FormatCurrency(lifetimeValue)));
            flpKPIs.Controls.Add(CreateKPICard("TOTAL VISITS", totalVisits.ToString()));
            flpKPIs.Controls.Add(CreateKPICard("AVG. ORDER VALUE", FormatCurrency(avgOrderValue)));
            flpKPIs.Controls.Add(CreateKPICard("MEMBER SINCE", c.MemberSince.ToString("MMM yyyy")));

            // Reset filters and populate history
            txtHistorySearch.Text = "";
            cmbDateFilter.SelectedIndex = 0;
            dtpCustomDate.Visible = false;
            FilterPurchaseHistory();

            // Data Intelligence
            string mostFreqCategory = "No History";
            string favoriteItem = "No History";
            string typicalTime = "No History";
            
            var allItems = orders.SelectMany(o => o.Items).ToList();
            if (allItems.Count > 0)
            {
                mostFreqCategory = allItems
                    .GroupBy(i => i.CategoryName)
                    .OrderByDescending(g => g.Sum(i => i.Quantity))
                    .First().Key;

                // Favorite Item: most purchased product by total quantity
                favoriteItem = allItems
                    .GroupBy(i => i.ProductName)
                    .OrderByDescending(g => g.Sum(i => i.Quantity))
                    .First().Key;
            }

            if (orders.Count > 0)
            {
                var mostFreqHour = orders
                    .GroupBy(o => o.Timestamp.Hour)
                    .OrderByDescending(g => g.Count())
                    .First().Key;
                
                string timePeriod = mostFreqHour < 12 ? "Morning" : (mostFreqHour < 17 ? "Afternoon" : "Evening");
                typicalTime = $"{timePeriod}, {new DateTime(2000, 1, 1, mostFreqHour, 0, 0).ToString("hh:00 tt")}";
            }

            lblFavCatValue.Text = mostFreqCategory;
            lblModValue.Text = favoriteItem;

            txtAdminNotes.Text = c.AdminNotes;

            // Set notes to read-only if there's existing content
            if (!string.IsNullOrWhiteSpace(c.AdminNotes))
            {
                SetNotesReadOnly(true);
            }
            else
            {
                SetNotesReadOnly(false);
            }
        }

        private void SetNotesReadOnly(bool readOnly)
        {
            _notesEditMode = !readOnly;
            txtAdminNotes.ReadOnly = readOnly;

            if (readOnly)
            {
                txtAdminNotes.FillColor = ColorTranslator.FromHtml("#F3F4F6");
                txtAdminNotes.ForeColor = ColorTranslator.FromHtml("#6B7280");
                btnSaveNote.Text = "Edit Note";
                btnSaveNote.FillColor = ColorTranslator.FromHtml("#374151");
            }
            else
            {
                txtAdminNotes.FillColor = Color.White;
                txtAdminNotes.ForeColor = ColorTranslator.FromHtml("#111827");
                btnSaveNote.Text = "Save Note";
                btnSaveNote.FillColor = ColorTranslator.FromHtml("#52B743");
            }
        }

        private Panel CreateKPICard(string title, string value)
        {
            var pnl = new Guna.UI2.WinForms.Guna2ShadowPanel
            {
                Size = new Size(140, 76),
                Margin = new Padding(0, 0, 10, 0),
                FillColor = Color.White,
                ShadowColor = Color.FromArgb(30, 0, 0, 0),
                ShadowDepth = 10,
                ShadowShift = 1,
                Radius = 8
            };

            var lblT = new Label 
            { 
                Text = title, 
                Font = new Font("Segoe UI", 7F, FontStyle.Bold), 
                ForeColor = ColorTranslator.FromHtml("#9CA3AF"), 
                Location = new Point(12, 12), 
                AutoSize = true 
            };
            var lblV = new Label 
            { 
                Text = value, 
                Font = new Font("Segoe UI", 13F, FontStyle.Bold), 
                ForeColor = ColorTranslator.FromHtml("#111827"),
                Location = new Point(12, 36), 
                AutoSize = true 
            };

            pnl.Controls.Add(lblT);
            pnl.Controls.Add(lblV);
            return pnl;
        }



        private void RenderCalendar(Customer c)
        {
            flpCalendarDays.SuspendLayout();
            flpCalendarDays.Controls.Clear();
            var orders = Program.DataService.Orders.Where(o => o.CustomerId == c.Id).ToList();
            
            DateTime currentDate = DateTime.Today;
            lblCalendarTitle.Text = currentDate.ToString("MMMM yyyy");
            
            // Render 6 months vertically to allow scrolling
            for (int m = 0; m < 6; m++)
            {
                DateTime monthDate = currentDate.AddMonths(-m);
                RenderMonthGrid(monthDate, orders);
            }
            flpCalendarDays.ResumeLayout();
        }

        private void RenderMonthGrid(DateTime monthDate, List<Order> orders)
        {
            var lblMonth = new Label
            {
                Text = monthDate.ToString("MMMM yyyy"),
                Font = new Font("Segoe UI Semibold", 10F),
                AutoSize = false,
                Size = new Size(flpCalendarDays.Width - 30, 30),
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(5, 15, 5, 5),
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                Tag = "MonthHeader"
            };
            flpCalendarDays.Controls.Add(lblMonth);

            string[] days = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
            foreach (var d in days)
            {
                var lblD = new Label
                {
                    Text = d,
                    Size = new Size(35, 20),
                    Margin = new Padding(2),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = ColorTranslator.FromHtml("#9CA3AF")
                };
                flpCalendarDays.Controls.Add(lblD);
            }

            DateTime firstDay = new DateTime(monthDate.Year, monthDate.Month, 1);
            int startDayOfWeek = (int)firstDay.DayOfWeek;
            
            for (int i = 0; i < startDayOfWeek; i++)
            {
                var empty = new Label { Size = new Size(35, 35), Margin = new Padding(2) };
                flpCalendarDays.Controls.Add(empty);
            }

            int daysInMonth = DateTime.DaysInMonth(monthDate.Year, monthDate.Month);
            for (int d = 1; d <= daysInMonth; d++)
            {
                DateTime day = new DateTime(monthDate.Year, monthDate.Month, d);
                var dayOrders = orders.Where(o => o.Timestamp.Date == day.Date).ToList();
                bool hasOrders = dayOrders.Count > 0;

                var btn = new Guna.UI2.WinForms.Guna2Button
                {
                    Size = new Size(35, 35),
                    Margin = new Padding(2),
                    Text = day.Day.ToString(),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    BorderRadius = 17,
                    Cursor = hasOrders ? Cursors.Hand : Cursors.Default
                };

                if (hasOrders)
                {
                    btn.FillColor = ColorTranslator.FromHtml("#52B743");
                    btn.ForeColor = Color.White;
                    btn.Click += (s, e) => ViewReceipt(dayOrders.First());
                }
                else
                {
                    btn.FillColor = ColorTranslator.FromHtml("#F3F4F6");
                    btn.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
                }
                
                flpCalendarDays.Controls.Add(btn);
            }
        }

        private void FlpCalendarDays_Scroll(object? sender, EventArgs e)
        {
            // Simple logic to find the topmost visible Month header
            foreach (Control c in flpCalendarDays.Controls)
            {
                if (c.Tag?.ToString() == "MonthHeader")
                {
                    // Check if it's within the top visible region
                    Point p = c.Parent!.PointToScreen(c.Location);
                    Point pnl = flpCalendarDays.PointToScreen(Point.Empty);
                    
                    if (p.Y >= pnl.Y && p.Y < pnl.Y + 100)
                    {
                        if (lblCalendarTitle.Text != c.Text)
                        {
                            lblCalendarTitle.Text = c.Text;
                        }
                        break;
                    }
                }
            }
        }

        private void DgvHistory_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvHistory.Columns["Action"] != null && e.ColumnIndex == dgvHistory.Columns["Action"]!.Index)
            {
                string orderId = dgvHistory.Rows[e.RowIndex].Cells["OrderId"].Value?.ToString() ?? "";
                var order = Program.DataService.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order != null)
                {
                    ViewReceipt(order);
                }
            }
        }

        private void ViewReceipt(Order order)
        {
            using var dlg = new OrderDetailForm(order);
            dlg.ShowDialog(this.FindForm());
        }

        private async void BtnSaveNote_Click(object? sender, EventArgs e)
        {
            if (_currentCustomer == null) return;

            if (_notesEditMode)
            {
                // Currently in edit mode → Save and lock
                _currentCustomer.AdminNotes = txtAdminNotes.Text;
                await Program.DataService.SaveCustomersAsync();
                SetNotesReadOnly(true);
            }
            else
            {
                // Currently read-only → Switch to edit mode
                SetNotesReadOnly(false);
                txtAdminNotes.Focus();
            }
        }


        private void FilterPurchaseHistory()
        {
            if (_currentCustomer == null) return;

            string search = txtHistorySearch.Text.ToLower();
            var filtered = _currentOrders.AsEnumerable();

            // Apply Search Filter
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(o => 
                    o.OrderId.ToLower().Contains(search) || 
                    o.Items.Any(i => i.ProductName.ToLower().Contains(search))
                );
            }

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

            // Render to DGV
            dgvHistory.Rows.Clear();
            foreach (var o in filtered.OrderByDescending(o => o.Timestamp))
            {
                var itemsStr = string.Join(", ", o.Items.Select(i => i.ProductName));
                dgvHistory.Rows.Add(
                    o.OrderId,
                    o.Timestamp.ToString("yyyy-MM-dd hh:mm tt"),
                    itemsStr,
                    FormatCurrency(o.Total),
                    "Completed"
                );
            }
        }

        private string FormatCurrency(decimal amount)
        {
            return $"₱{amount.ToString("#,##0.00")}";
        }

        private Image GenerateInitialsImage(string name)
        {
            var initials = string.Join("", name.Split(' ').Take(2).Select(s => s.Length > 0 ? s[0].ToString() : ""));
            if (string.IsNullOrEmpty(initials)) initials = "?";

            var bmp = new Bitmap(80, 80);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(ColorTranslator.FromHtml("#E5E7EB"));
            
            using var brush = new SolidBrush(ColorTranslator.FromHtml("#6B7280"));
            using var font = new Font("Segoe UI", 24F, FontStyle.Bold);
            
            var size = g.MeasureString(initials, font);
            g.DrawString(initials, font, brush, (80 - size.Width) / 2, (80 - size.Height) / 2);
            
            return bmp;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
