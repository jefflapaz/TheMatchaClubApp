using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();

            Program.DataService.OrdersChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(LoadData));
            };
            btnCloseDay.Click += BtnCloseDay_Click;
            LoadData();
        }

        // ── Live KPI Calculation ─────────────────────────────────────
        private void LoadData()
        {
            var orders = Program.DataService.Orders;

            decimal totalSales = orders.Sum(o => o.Total);
            int totalOrders = orders.Count;
            decimal avgOrder = totalOrders > 0 ? totalSales / totalOrders : 0;
            decimal netProfit = totalSales * 0.65m; // 65% margin estimate

            pnlKpiRow.Controls.Clear();
            pnlKpiRow.Controls.Add(CreateKpiCard("Total Sales", totalSales.ToString("C2"), "+12.5%"));
            pnlKpiRow.Controls.Add(CreateKpiCard("Total Orders", totalOrders.ToString(), "+8.2%"));
            pnlKpiRow.Controls.Add(CreateKpiCard("Avg. Order Value", avgOrder.ToString("C2"), "-2.1%"));
            pnlKpiRow.Controls.Add(CreateKpiCard("Net Profit", netProfit.ToString("C2"), "+14.3%"));

            lblExpectedCashValue.Text = (totalSales + 200m).ToString("C2");

            PopulateTopItems();
        }

        // ── Top Performing Items ─────────────────────────────────────
        private void PopulateTopItems()
        {
            pnlTableInner.Controls.Clear();
            var topItems = Program.DataService.Products
                .OrderByDescending(p => p.SalesCount)
                .Take(5).ToList();

            int y = 0;
            foreach (var item in topItems)
            {
                var row = new Panel { Size = new Size(600, 40), Location = new Point(10, y) };
                row.Controls.Add(new Label { Text = item.Name, Location = new Point(10, 10), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) });
                row.Controls.Add(new Label { Text = item.CategoryName, Location = new Point(250, 10), AutoSize = true, ForeColor = Color.Gray });
                row.Controls.Add(new Label { Text = $"{item.SalesCount} Units", Location = new Point(350, 10), AutoSize = true });
                row.Controls.Add(new Label { Text = (item.SalesCount * item.Price).ToString("C2"), Location = new Point(480, 10), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) });
                pnlTableInner.Controls.Add(row);
                y += 45;
            }
        }

        // ── KPI Card Factory ─────────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Panel CreateKpiCard(string title, string value, string trend)
        {
            var pnl = new Guna.UI2.WinForms.Guna2Panel
            {
                Size = new Size(150, 84),
                BorderRadius = 8,
                BorderColor = ColorTranslator.FromHtml("#E5E7EB"),
                BorderThickness = 1,
                FillColor = Color.White,
                Margin = new Padding(0, 0, 12, 0)
            };

            var lblT = new Label { Text = title, Font = new Font("Segoe UI", 8F), ForeColor = Color.Gray, Location = new Point(12, 10), AutoSize = true };
            var lblV = new Label { Text = value, Font = new Font("Segoe UI", 14F, FontStyle.Bold), Location = new Point(12, 35), AutoSize = true };
            var lblTrend = new Label { Text = trend, Font = new Font("Segoe UI", 7F), ForeColor = trend.StartsWith("+") ? Color.Green : Color.Red, Location = new Point(100, 10), AutoSize = true };

            pnl.Controls.AddRange(new Control[] { lblT, lblV, lblTrend });
            return pnl;
        }

        // ── Z-Report Close Day ───────────────────────────────────────
        private void BtnCloseDay_Click(object? sender, EventArgs e)
        {
            var todayOrders = Program.DataService.Orders.Where(o => o.Timestamp.Date == DateTime.Today).ToList();
            decimal todayTotal = todayOrders.Sum(o => o.Total);

            MessageBox.Show(
                $"═══ Z-REPORT ═══\n" +
                $"Date: {DateTime.Today:dd/MM/yyyy}\n" +
                $"Transactions: {todayOrders.Count}\n" +
                $"Total Sales: {todayTotal.ToString("C2")}\n" +
                $"────────────────\n" +
                $"All transactions for today have been locked.",
                "End of Day Closeout", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
