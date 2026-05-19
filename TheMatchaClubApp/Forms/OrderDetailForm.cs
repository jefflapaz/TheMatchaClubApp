using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TheMatchaClubDomain.Models;
using TheMatchaClub.Services;

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

            btnPrint.Click += (s, e) =>
            {
                try
                {
                    var doc = new PrintDocument();
                    int paperWidth = Program.DataService.Settings.ReceiptPaperWidth == "58mm" ? 228 : 315;
                    doc.DefaultPageSettings.PaperSize = new PaperSize("Receipt", paperWidth, 800);
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
                    
                    ReceiptPdfGenerator.Generate(_order, Program.DataService.Settings, Program.GetCurrentCashierName(), filePath);
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting PDF: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnEmail.Click += (s, e) =>
            {
                ShowEmailPromptDialog(_order);
            };

            btnDeleteOrder.Click += async (s, e) =>
            {
                if (Program.DataService.Settings.RequirePasswordForDeleteOrder)
                {
                    using var authDialog = new PasswordPromptDialog("Enter password to delete this order.");
                    if (authDialog.ShowDialog(this) != DialogResult.OK) return;
                }

                var result = MessageBox.Show(
                    $"Permanently delete order '{_order.OrderId}'?\nThis action cannot be undone and will affect historical reporting.",
                    "Delete Order",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Program.DataService.Orders.Remove(_order);
                    await Program.DataService.SaveOrdersAsync();
                    
                    // Note: Ideally, we should also trigger an event to refresh OrdersView if needed,
                    // but the DataService.OrdersChanged event will fire and refresh the background UI.
                    this.Close();
                }
            };

            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };

            pnlReceiptPreview.Paint += PnlReceiptPreview_Paint;
        }

        private void ShowTab(string tab)
        {
            pnlOverviewTab.Visible = tab == "overview";
            pnlReceiptTab.Visible = tab == "receipt";

            StyleTab(btnTabOverview, tab == "overview");
            StyleTab(btnTabReceipt, tab == "receipt");
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
            float renderedHeight = ReceiptRenderer.Render(g, bounds, _order, settings, Program.GetCurrentCashierName());

            // Resize preview panel to fit
            if (renderedHeight + 30 > pnlReceiptPreview.Height)
                pnlReceiptPreview.Height = (int)renderedHeight + 30;
        }

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
                ForeColor = ColorTranslator.FromHtml("#9CA3AF"),
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

            var lblInfo = new Label
            {
                Text = $"Order {order.OrderId}  •  {order.Total.ToString("C2")}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = ColorTranslator.FromHtml("#6B7280"),
                Location = new Point(20, 62),
                AutoSize = true,
                BackColor = Color.Transparent
            };

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

            var lblStatus = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8F),
                ForeColor = ColorTranslator.FromHtml("#EF4444"),
                Location = new Point(20, 168),
                Size = new Size(380, 18),
                BackColor = Color.Transparent
            };

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

                order.CustomerEmail = email;

                btnSend.Enabled = false;
                btnCancel.Enabled = false;
                btnSend.Text = "Sending...";
                lblStatus.ForeColor = ColorTranslator.FromHtml("#6B7280");
                lblStatus.Text = "Sending receipt to " + email + "...";

                try
                {
                    await SendReceiptEmailAsync(order, email);
                    await Program.DataService.SaveOrdersAsync();

                    lblStatus.ForeColor = ColorTranslator.FromHtml("#52B743");
                    lblStatus.Text = "✓ Receipt sent successfully!";
                    btnSend.Text = "✓  Sent!";
                    btnSend.FillColor = ColorTranslator.FromHtml("#D1FAE5");
                    btnSend.ForeColor = ColorTranslator.FromHtml("#065F46");

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

            dlg.Controls.Add(btnCancel);
            dlg.Controls.Add(btnSend);
            dlg.Controls.Add(lblStatus);
            dlg.Controls.Add(txtEmail);
            dlg.Controls.Add(lblEmail);
            dlg.Controls.Add(lblInfo);
            dlg.Controls.Add(pnlHead);

            dlg.Paint += (s, pe) =>
            {
                using var pen = new Pen(ColorTranslator.FromHtml("#E5E7EB"), 2);
                pe.Graphics.DrawRectangle(pen, 0, 0, dlg.Width - 1, dlg.Height - 1);
            };

            dlg.ShowDialog(this);
        }

        private async Task SendReceiptEmailAsync(Order order, string recipientEmail)
        {
            var settings = Program.DataService.Settings;
            var storeName = settings.StoreName;
            var senderEmail = settings.Email;
            var smtpServer = settings.SmtpServer;
            var smtpPort = settings.SmtpPort;
            var smtpPass = settings.SmtpPassword;
            
            var items = string.Join("",
                order.Items.Select(i =>
                    $"<tr><td style='padding:6px 0'>{i.ProductName}</td>" +
                    $"<td style='text-align:center;padding:6px 0'>{i.Quantity}</td>" +
                    $"<td style='text-align:right;padding:6px 0'>{i.LineTotal.ToString("C2")}</td></tr>"));

            var body = $@"
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

            await Task.Run(() =>
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
                client.Send(mail);
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
