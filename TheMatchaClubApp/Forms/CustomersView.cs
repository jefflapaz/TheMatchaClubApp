using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class CustomersView : UserControl
    {
        private Customer? _currentCustomer;
        private string _currentFilter = "All";

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

        private void WireEvents()
        {
            txtSearch.TextChanged += (s, e) => LoadDirectory();
            btnFilterAll.Click += (s, e) => { SetFilter("All"); LoadDirectory(); };
            btnFilterRegular.Click += (s, e) => { SetFilter("Regular"); LoadDirectory(); };
            btnFilterNew.Click += (s, e) => { SetFilter("New"); LoadDirectory(); };
            
            dgvHistory.CellContentClick += DgvHistory_CellContentClick;
            btnSaveNote.Click += BtnSaveNote_Click;
            btnAddCustomer.Click += BtnAddCustomer_Click;
            
            btnViewOrders.Click += BtnViewOrders_Click;
            btnCalendarClose.Click += (s, e) => pnlCalendarPopup.Visible = false;
            
            flpCalendarDays.Scroll += FlpCalendarDays_Scroll;
            flpCalendarDays.MouseWheel += FlpCalendarDays_Scroll;
        }

        private void SetFilter(string filter)
        {
            _currentFilter = filter;
            StyleFilterButton(btnFilterAll, filter == "All");
            StyleFilterButton(btnFilterRegular, filter == "Regular");
            StyleFilterButton(btnFilterNew, filter == "New");
        }

        private void LoadDirectory()
        {
            flpCustomers.SuspendLayout();
            flpCustomers.Controls.Clear();

            string search = txtSearch.Text.Trim();
            
            var allCustomers = Program.DataService.Customers;
            
            btnFilterAll.Text = $"All ({allCustomers.Count})";
            btnFilterRegular.Text = $"Regular ({allCustomers.Count(c => c.Status == "Regular")})";
            btnFilterNew.Text = $"New ({allCustomers.Count(c => c.Status == "New")})";
            
            var filtered = allCustomers.AsEnumerable();
            if (_currentFilter != "All")
            {
                filtered = filtered.Where(c => string.Equals(c.Status, _currentFilter, StringComparison.OrdinalIgnoreCase));
            }
            
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(c => 
                    c.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var c in filtered)
            {
                var card = CreateCustomerCard(c);
                flpCustomers.Controls.Add(card);
            }
            
            flpCustomers.ResumeLayout();
        }

        private Panel CreateCustomerCard(Customer c)
        {
            var pnl = new Guna.UI2.WinForms.Guna2Panel
            {
                Size = new Size(310, 80),
                Margin = new Padding(0, 0, 0, 10),
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
                Size = new Size(40, 40),
                Location = new Point(10, 20),
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
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Location = new Point(60, 15),
                AutoSize = true,
                ForeColor = ColorTranslator.FromHtml("#111827")
            };
            pnl.Controls.Add(lblName);

            var lblEmail = new Label
            {
                Text = c.Email,
                Font = new Font("Segoe UI", 8F),
                Location = new Point(60, 40),
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
                Location = new Point(220, 15),
                AutoSize = true,
                ForeColor = ColorTranslator.FromHtml("#111827")
            };
            pnl.Controls.Add(lblSpent);

            var chip = new Guna.UI2.WinForms.Guna2Chip
            {
                Text = c.Status,
                Location = new Point(220, 35),
                Size = new Size(70, 20),
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                FillColor = c.Status == "Regular" ? ColorTranslator.FromHtml("#EFF6FF") : ColorTranslator.FromHtml("#FDF4FF"),
                ForeColor = c.Status == "Regular" ? ColorTranslator.FromHtml("#3B82F6") : ColorTranslator.FromHtml("#D946EF"),
                BorderThickness = 0
            };
            pnl.Controls.Add(chip);

            BindClick(pnl);
            return pnl;
        }

        private void SelectCustomer(Customer c)
        {
            _currentCustomer = c;
            pnlCalendarPopup.Visible = false;
            PopulateProfile(c);
        }

        private void PopulateProfile(Customer c)
        {
            lblProfileName.Text = c.Name;
            lblProfileEmail.Text = c.Email;
            lblProfilePhone.Text = c.Phone;
            chipStatus.Text = c.Status;
            
            if (!string.IsNullOrEmpty(c.ProfileImagePath) && System.IO.File.Exists(c.ProfileImagePath))
                picProfile.Image = Image.FromFile(c.ProfileImagePath);
            else
                picProfile.Image = GenerateInitialsImage(c.Name);

            var orders = Program.DataService.Orders.Where(o => o.CustomerId == c.Id).ToList();
            int totalVisits = orders.Count;
            decimal lifetimeValue = orders.Sum(o => o.Total);
            decimal avgOrderValue = totalVisits > 0 ? lifetimeValue / totalVisits : 0;

            flpKPIs.Controls.Clear();
            flpKPIs.Controls.Add(CreateKPICard("LIFETIME VALUE", FormatCurrency(lifetimeValue)));
            flpKPIs.Controls.Add(CreateKPICard("TOTAL VISITS", totalVisits.ToString()));
            flpKPIs.Controls.Add(CreateKPICard("AVG. ORDER VALUE", FormatCurrency(avgOrderValue)));
            flpKPIs.Controls.Add(CreateKPICard("MEMBER SINCE", c.MemberSince.ToString("MMM yyyy")));

            dgvHistory.Rows.Clear();
            foreach (var o in orders.OrderByDescending(o => o.Timestamp))
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

            // Data Intelligence
            string mostFreqCategory = "No History Found";
            string typicalTime = "No History Found";
            string modStyle = "No History Found";
            
            var allItems = orders.SelectMany(o => o.Items).ToList();
            if (allItems.Count > 0)
            {
                mostFreqCategory = allItems
                    .GroupBy(i => i.CategoryName)
                    .OrderByDescending(g => g.Sum(i => i.Quantity))
                    .First().Key;
                    
                modStyle = "Oat Milk / No Sugar"; // Simulated default
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
            lblModValue.Text = modStyle;
            lblTimeValue.Text = typicalTime;

            txtAdminNotes.Text = c.AdminNotes;
        }

        private Panel CreateKPICard(string title, string value)
        {
            var pnl = new Guna.UI2.WinForms.Guna2ShadowPanel
            {
                Size = new Size(160, 90),
                Margin = new Padding(0, 0, 15, 0),
                FillColor = Color.White,
                ShadowColor = Color.Black,
                ShadowDepth = 20,
                ShadowShift = 2,
                Radius = 6
            };

            var lblT = new Label 
            { 
                Text = title, 
                Font = new Font("Segoe UI", 7F, FontStyle.Bold), 
                ForeColor = ColorTranslator.FromHtml("#9CA3AF"), 
                Location = new Point(15, 15), 
                AutoSize = true 
            };
            var lblV = new Label 
            { 
                Text = value, 
                Font = new Font("Segoe UI", 12F, FontStyle.Bold), 
                ForeColor = ColorTranslator.FromHtml("#111827"),
                Location = new Point(15, 45), 
                AutoSize = true 
            };

            pnl.Controls.Add(lblT);
            pnl.Controls.Add(lblV);
            return pnl;
        }

        private void BtnViewOrders_Click(object? sender, EventArgs e)
        {
            if (_currentCustomer == null) return;
            RenderCalendar(_currentCustomer);
            pnlCalendarPopup.Visible = true;
            pnlCalendarPopup.BringToFront();
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
                    Point p = c.Parent.PointToScreen(c.Location);
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
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvHistory.Columns["Action"].Index)
            {
                string orderId = dgvHistory.Rows[e.RowIndex].Cells["OrderId"].Value.ToString() ?? "";
                var order = Program.DataService.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order != null)
                {
                    ViewReceipt(order);
                }
            }
        }

        private void ViewReceipt(Order order)
        {
            var items = string.Join("\n", order.Items.Select(i => $"  {i.Quantity}x {i.ProductName} — {FormatCurrency(i.LineTotal)}"));
            MessageBox.Show(
                $"═══ VIRTUAL RECEIPT ═══\n" +
                $"Order: {order.OrderId}\n" +
                $"Date:  {order.Timestamp:dd/MM/yyyy hh:mm tt}\n" +
                $"Type:  {(order.IsDineIn ? "Dine-In" : "Take-Out")}\n" +
                $"───────────────────\n" +
                $"{items}\n" +
                $"───────────────────\n" +
                $"Subtotal: {FormatCurrency(order.Subtotal)}\n" +
                $"VAT 12%:  {FormatCurrency(order.VatAmount)}\n" +
                $"TOTAL:    {FormatCurrency(order.Total)}",
                "Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void BtnSaveNote_Click(object? sender, EventArgs e)
        {
            if (_currentCustomer != null)
            {
                _currentCustomer.AdminNotes = txtAdminNotes.Text;
                await Program.DataService.SaveCustomersAsync();
                MessageBox.Show("Notes saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddCustomer_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Add Customer dialog not implemented.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
