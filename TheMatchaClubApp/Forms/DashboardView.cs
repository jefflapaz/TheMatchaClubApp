using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class DashboardView : UserControl
    {
        // ════════════════════════════════════════════════════════════════
        //  PUBLIC DATA PROPERTIES — MainShell pushes data via these
        // ════════════════════════════════════════════════════════════════

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string TotalSales
        {
            get => lblCard1Value.Text;
            set => lblCard1Value.Text = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string OrderCount
        {
            get => lblCard2Value.Text;
            set => lblCard2Value.Text = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string AverageOrderValue
        {
            get => lblCard3Value.Text;
            set => lblCard3Value.Text = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string CashOnHand
        {
            get => lblCard4Value.Text;
            set => lblCard4Value.Text = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Design")]
        [Description("The main chart panel placeholder.")]
        public Guna.UI2.WinForms.Guna2Panel PnlChart
        {
            get => pnlChart;
        }

        // ════════════════════════════════════════════════════════════════
        //  EVENTS
        // ════════════════════════════════════════════════════════════════

        /// <summary>Raised when the user clicks "+ New Order".</summary>
        public event EventHandler? NewOrderClicked;

        // ════════════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ════════════════════════════════════════════════════════════════
        public DashboardView()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();

            // Wire New Order click → public event
            btnNewOrder.Click += (s, e) => NewOrderClicked?.Invoke(this, EventArgs.Empty);

            // Live data updates
            Program.DataService.OrdersChanged += (s, e) =>
            {
                if (!IsDisposed) BeginInvoke(new Action(LoadDashboardData));
            };
            LoadDashboardData();
        }

        // ════════════════════════════════════════════════════════════════
        //  LIVE KPI CALCULATION
        // ════════════════════════════════════════════════════════════════
        private void LoadDashboardData()
        {
            var todayOrders = Program.DataService.Orders
                .Where(o => o.Timestamp.Date == DateTime.Today).ToList();

            decimal todaySales = todayOrders.Sum(o => o.Total);
            int todayCount = todayOrders.Count;
            decimal avgOrder = todayCount > 0 ? todaySales / todayCount : 0m;

            TotalSales = todaySales.ToString("C2");
            OrderCount = todayCount.ToString();
            AverageOrderValue = avgOrder.ToString("C2");
            CashOnHand = (todaySales + 200m).ToString("C2"); // ₱200 drawer float

            lblDate.Text = "📅 " + DateTime.Today.ToString("d");

            // Top Selling Products in chart panel
            PopulateTopSelling();
        }

        private void PopulateTopSelling()
        {
            // Use the chart panel area for top selling items
            lblChartMessage.Text = "";
            // Remove any existing top-selling controls but keep lblChartMessage
            for (int i = pnlChart.Controls.Count - 1; i >= 0; i--)
            {
                if (pnlChart.Controls[i] != lblChartMessage)
                    pnlChart.Controls.RemoveAt(i);
            }

            var topProducts = Program.DataService.Products
                .OrderByDescending(p => p.SalesCount)
                .Take(5).ToList();

            if (topProducts.Count == 0)
            {
                lblChartMessage.Text = "No sales data yet. Complete a sale to see analytics.";
                return;
            }

            var headerLbl = new Label
            {
                Text = "🏆 Top Selling Products",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(20, 10), AutoSize = true
            };
            pnlChart.Controls.Add(headerLbl);

            int y = 45;
            int rank = 1;
            foreach (var p in topProducts)
            {
                var row = new Label
                {
                    Text = $"#{rank}  {p.Name}  —  {p.SalesCount} sold  ({p.CategoryName})",
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(30, y),
                    AutoSize = true,
                    ForeColor = rank <= 3 ? ColorTranslator.FromHtml("#52B743") : ColorTranslator.FromHtml("#374151")
                };
                pnlChart.Controls.Add(row);
                y += 30;
                rank++;
            }
        }
    }
}
