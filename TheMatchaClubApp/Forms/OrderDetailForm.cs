using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;
using TheMatchaClubApp.Core;

namespace TheMatchaClubApp.Forms
{
    public partial class OrderDetailForm : Form
    {
        private readonly Order _order;

        public OrderDetailForm(Order order)
        {
            _order = order;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();
            SetupItemsGrid();
            WireEvents();
            LoadData();
            ShowTab("overview");
        }

        private void InitializeDesign()
        {
            var green = ColorTranslator.FromHtml("#52B743");
            var border = ColorTranslator.FromHtml("#E5E7EB");
            var bg = ColorTranslator.FromHtml("#F9FAFB");

            pnlHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(border, 1);
                e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };
            pnlTabBar.Paint += (s, e) =>
            {
                using var pen = new Pen(border, 1);
                e.Graphics.DrawLine(pen, 0, pnlTabBar.Height - 1, pnlTabBar.Width, pnlTabBar.Height - 1);
            };
            pnlFooter.Paint += (s, e) =>
            {
                using var pen = new Pen(border, 1);
                e.Graphics.DrawLine(pen, 0, 0, pnlFooter.Width, 0);
            };

            // Rounded borders for info cards
            foreach (var card in new Panel[] { pnlCustomerCard, pnlPaymentCard, pnlOrderInfoCard, pnlItemsCard })
            {
                card.Paint += (s, e) =>
                {
                    using var pen = new Pen(ColorTranslator.FromHtml("#F3F4F6"), 1);
                    e.Graphics.DrawRectangle(pen, 0, 0, ((Panel)s!).Width - 1, ((Panel)s!).Height - 1);
                };
            }

            // Items DGV styling
            dgvItems.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvItems.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#374151");
            dgvItems.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#F2FAEF");
            dgvItems.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#111827");
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvItems.EnableHeadersVisualStyles = false;

            // Close button position
            btnClose.Location = new Point(pnlHeader.Width - 60, 20);
            lblStatusBadge.Location = new Point(pnlHeader.Width - 160, 24);
            btnCloseBottom.Location = new Point(pnlFooter.Width - 122, 10);

            // Receipt preview centering
            pnlReceiptTab.Layout += (s, e) =>
            {
                int cx = (pnlReceiptTab.Width - pnlReceiptPreview.Width) / 2;
                pnlReceiptPreview.Location = new Point(Math.Max(0, cx), 20);
            };
        }

        private void SetupItemsGrid()
        {
            dgvItems.Columns.AddRange(
                new DataGridViewTextBoxColumn { HeaderText = "#", FillWeight = 6 },
                new DataGridViewTextBoxColumn { HeaderText = "Product", FillWeight = 30 },
                new DataGridViewTextBoxColumn { HeaderText = "Category", FillWeight = 18 },
                new DataGridViewTextBoxColumn { HeaderText = "Qty", FillWeight = 8, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                new DataGridViewTextBoxColumn { HeaderText = "Unit Price", FillWeight = 14, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } },
                new DataGridViewTextBoxColumn { HeaderText = "Total", FillWeight = 14, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } }
            );
        }

        private void WireEvents()
        {
            btnClose.Click += (s, e) => Close();
            btnCloseBottom.Click += (s, e) => Close();
            btnTabOverview.Click += (s, e) => ShowTab("overview");
            btnTabReceipt.Click += (s, e) => ShowTab("receipt");
            btnTabTimeline.Click += (s, e) => ShowTab("timeline");

            btnPrint.Click += (s, e) =>
            {
                try
                {
                    var doc = new PrintDocument();
                    doc.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 300, 800);
                    doc.PrintPage += (ps, pe) => DrawReceiptOnGraphics(pe!.Graphics!, pe.PageBounds);
                    var dlg = new PrintPreviewDialog { Document = doc, Width = 500, Height = 700 };
                    dlg.ShowDialog(this);
                }
                catch { }
            };

            btnExportPdf.Click += (s, e) =>
            {
                try
                {
                    string fileName = $"Receipt_{_order.OrderId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                    
                    ReceiptPdfGenerator.Generate(_order, Program.DataService.Settings, filePath);
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting PDF: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnEmail.Click += (s, e) =>
            {
                // Note: We can implement a shared Email Dialog helper if needed.
                // For now, let's keep it consistent with OrdersView if possible.
                // But since it's a new feature here, I'll add a simple prompt or 
                // ideally use the one from OrdersView if I refactor it.
                MessageBox.Show("Email receipt feature is being unified. Please use the Orders screen for now.", "Coming Soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };

            pnlReceiptPreview.Paint += PnlReceiptPreview_Paint;
        }

        private void ShowTab(string tab)
        {
            pnlOverviewTab.Visible = tab == "overview";
            pnlReceiptTab.Visible = tab == "receipt";
            pnlTimelineTab.Visible = tab == "timeline";

            StyleTab(btnTabOverview, tab == "overview");
            StyleTab(btnTabReceipt, tab == "receipt");
            StyleTab(btnTabTimeline, tab == "timeline");
        }

        private void StyleTab(Guna.UI2.WinForms.Guna2Button btn, bool active)
        {
            btn.FillColor = active ? ColorTranslator.FromHtml("#52B743") : ColorTranslator.FromHtml("#F3F4F6");
            btn.ForeColor = active ? Color.White : ColorTranslator.FromHtml("#374151");
        }

        private string Fmt(decimal v) => $"₱{v:#,##0.00}";

        // ══════════════════════════════════════════════
        //  DATA LOADING
        // ══════════════════════════════════════════════
        private void LoadData()
        {
            // Header
            lblOrderTitle.Text = $"Order — {_order.OrderId}";
            lblOrderMeta.Text = $"{_order.Timestamp:MMM dd, yyyy hh:mm tt}  •  Customer: {(_order.CustomerName ?? "Walk-In")}";
            lblStatusBadge.Text = "COMPLETED";
            lblStatusBadge.BackColor = ColorTranslator.FromHtml("#52B743");

            // KPI Cards
            flpKpiCards.Controls.Clear();
            int itemCount = _order.Items.Sum(i => i.Quantity);
            flpKpiCards.Controls.Add(CreateKpiCard("Total", Fmt(_order.Total), "#52B743"));
            flpKpiCards.Controls.Add(CreateKpiCard("Items", itemCount.ToString(), "#3B82F6"));
            flpKpiCards.Controls.Add(CreateKpiCard("Payment", _order.PaymentMethod, "#8B5CF6"));
            flpKpiCards.Controls.Add(CreateKpiCard("Type", _order.OrderType ?? "Dine-In", "#F59E0B"));

            // Customer Card
            lblCustomerName.Text = string.IsNullOrEmpty(_order.CustomerName) ? "Walk-In" : _order.CustomerName;
            lblCustomerEmail.Text = string.IsNullOrEmpty(_order.CustomerEmail) ? "No email on file" : _order.CustomerEmail;

            // Payment Card
            lblPaymentMethod.Text = _order.PaymentMethod ?? "Cash";
            lblCashTendered.Text = _order.CashTendered > 0 ? $"Tendered: {Fmt(_order.CashTendered)}" : "Tendered: —";
            lblChangeGiven.Text = _order.CashTendered > 0 ? $"Change: {Fmt(_order.ChangeGiven)}" : "Change: —";

            // Order Info Card
            lblOrderType.Text = _order.OrderType ?? "Dine-In";
            string cashier = string.IsNullOrWhiteSpace(_order.CashierName) || _order.CashierName == "Admin" 
                ? Program.GetCurrentCashierName() 
                : _order.CashierName;
            lblCashier.Text = $"Cashier: {cashier}";
            lblTimestamp.Text = _order.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");

            // Items Grid
            dgvItems.Rows.Clear();
            int rank = 1;
            foreach (var item in _order.Items)
            {
                dgvItems.Rows.Add(rank.ToString(), item.ProductName, item.CategoryName ?? "—", item.Quantity.ToString(), Fmt(item.UnitPrice), Fmt(item.LineTotal));
                rank++;
            }
        }

        private Guna.UI2.WinForms.Guna2Panel CreateKpiCard(string title, string value, string accentColor)
        {
            var pnl = new Guna.UI2.WinForms.Guna2Panel
            {
                Size = new Size(170, 84),
                BorderRadius = 10,
                FillColor = Color.White,
                BorderColor = ColorTranslator.FromHtml("#F3F4F6"),
                BorderThickness = 1,
                Margin = new Padding(0, 0, 12, 0)
            };
            pnl.ShadowDecoration.Enabled = true;
            pnl.ShadowDecoration.Depth = 4;
            pnl.ShadowDecoration.Color = Color.FromArgb(15, 0, 0, 0);

            var accent = new Panel { Size = new Size(4, 40), Location = new Point(0, 22), BackColor = ColorTranslator.FromHtml(accentColor) };
            var lblT = new Label { Text = title.ToUpper(), Font = new Font("Segoe UI", 7.5F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#9CA3AF"), Location = new Point(14, 14), AutoSize = true };
            var lblV = new Label { Text = value, Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = ColorTranslator.FromHtml("#111827"), Location = new Point(14, 38), AutoSize = true };

            pnl.Controls.AddRange(new Control[] { accent, lblT, lblV });
            return pnl;
        }

        // ══════════════════════════════════════════════
        //  RECEIPT PREVIEW (Paint)
        // ══════════════════════════════════════════════
        private void PnlReceiptPreview_Paint(object? sender, PaintEventArgs e)
        {
            DrawReceiptOnGraphics(e.Graphics, pnlReceiptPreview.ClientRectangle);
        }

        private void DrawReceiptOnGraphics(Graphics g, Rectangle bounds)
        {
            var settings = Program.DataService.Settings;
            float renderedHeight = Core.ReceiptRenderer.Render(g, bounds, _order, settings);

            // Resize preview panel to fit
            if (renderedHeight + 30 > pnlReceiptPreview.Height)
                pnlReceiptPreview.Height = (int)renderedHeight + 30;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
