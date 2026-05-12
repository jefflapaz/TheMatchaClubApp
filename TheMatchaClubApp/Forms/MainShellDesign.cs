using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class MainShell
    {
        // ── Palette ────────────────────────────────────────────────
        private static readonly Color SidebarBg = ColorTranslator.FromHtml("#F8F9F8");
        private static readonly Color ContentBg = ColorTranslator.FromHtml("#FAFAFA");
        private static readonly Color BorderLine = ColorTranslator.FromHtml("#E5E7EB");
        private static readonly Color GreenAccent = ColorTranslator.FromHtml("#52B743");
        private static readonly Color TextDark = ColorTranslator.FromHtml("#111827");
        private static readonly Color TextGray = ColorTranslator.FromHtml("#6B7280");
        private static readonly Color TextMuted = ColorTranslator.FromHtml("#9CA3AF");
        private static readonly Color Red = ColorTranslator.FromHtml("#EF4444");

        // ── Drag state ─────────────────────────────────────────────
        private bool _dragging;
        private Point _dragCursor;
        private Point _dragForm;

        // ── Nav items ──────────────────────────────────────────────
        private NavItem navDashboard = null!;
        private NavItem navQuickSale = null!;
        private NavItem navOrders = null!;
        private NavItem navItems = null!;
        private NavItem navCustomers = null!;
        private NavItem navReports = null!;
        private NavItem navSettings = null!;
        private NavItem[] _navItems = null!;

        private void InitializeDesign()
        {
            // ── Form ───────────────────────────────────────────────
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = ContentBg;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1024, 640);

            // ── Window controls ────────────────────────────────────
            btnClose.FillColor = Color.Transparent;
            btnClose.IconColor = TextGray;
            btnClose.HoverState.FillColor = Color.FromArgb(232, 17, 35);
            btnClose.HoverState.IconColor = Color.White;

            btnMaximize.FillColor = Color.Transparent;
            btnMaximize.IconColor = TextGray;
            btnMaximize.HoverState.FillColor = Color.LightGray;
            btnMaximize.HoverState.IconColor = Color.Black;

            btnMinimize.FillColor = Color.Transparent;
            btnMinimize.IconColor = TextGray;
            btnMinimize.HoverState.FillColor = Color.LightGray;
            btnMinimize.HoverState.IconColor = Color.Black;

            // Bring controls to front so they overlay content
            btnClose.BringToFront();
            btnMaximize.BringToFront();
            btnMinimize.BringToFront();

            // ── Sidebar ────────────────────────────────────────────
            pnlSidebar.BackColor = SidebarBg;
            pnlSidebar.Paint += PnlSidebar_Paint;

            // ── Logo header ────────────────────────────────────────
            pnlLogoHeader.BackColor = SidebarBg;
            pnlLogoHeader.Paint += PnlLogoHeader_Paint;
            pnlLogoHeader.MouseDown += Drag_MouseDown;
            pnlLogoHeader.MouseMove += Drag_MouseMove;
            pnlLogoHeader.MouseUp += Drag_MouseUp;
            pnlLogoHeader.DoubleClick += LogoHeader_DoubleClick;

            pnlLogoCircle.BackColor = Color.Transparent;
            pnlLogoCircle.Paint += PnlLogoCircle_Paint;

            lblLogoText.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblLogoText.ForeColor = TextDark;
            lblLogoText.BackColor = Color.Transparent;

            // ── Nav container ──────────────────────────────────────
            pnlNavContainer.BackColor = SidebarBg;
            CreateNavItems();

            // ── Sidebar bottom ─────────────────────────────────────
            pnlSidebarBottom.BackColor = SidebarBg;
            pnlSidebarBottom.Paint += PnlSidebarBottom_Paint;

            btnLogout.FillColor = Color.Transparent;
            btnLogout.ForeColor = Red;
            btnLogout.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnLogout.BorderThickness = 0;
            btnLogout.HoverState.FillColor = Color.FromArgb(254, 242, 242);
            btnLogout.HoverState.ForeColor = Red;
            btnLogout.TextAlign = HorizontalAlignment.Left;
            btnLogout.Click += BtnLogout_Click;

            lblStation.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            lblStation.ForeColor = TextMuted;
            lblStation.BackColor = Color.Transparent;

            lblStationName.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            lblStationName.ForeColor = TextDark;
            lblStationName.BackColor = Color.Transparent;

            // ── Content panel ──────────────────────────────────────
            pnlContent.BackColor = ContentBg;

            // ── Footer ─────────────────────────────────────────────
            pnlFooter.BackColor = Color.White;
            pnlFooter.Paint += PnlFooter_Paint;

            lblCopyright.Font = new Font("Segoe UI", 8F);
            lblCopyright.ForeColor = TextMuted;
            lblCopyright.BackColor = Color.Transparent;

            lblSystemOnline.Font = new Font("Segoe UI", 8F);
            lblSystemOnline.ForeColor = TextGray;
            lblSystemOnline.BackColor = Color.Transparent;

            lblDbConnected.Font = new Font("Segoe UI", 8F);
            lblDbConnected.ForeColor = TextGray;
            lblDbConnected.BackColor = Color.Transparent;

            lnkSupport.Font = new Font("Segoe UI", 8F);
            lnkSupport.LinkColor = GreenAccent;
            lnkSupport.ActiveLinkColor = ColorTranslator.FromHtml("#46A037");
            lnkSupport.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkSupport.BackColor = Color.Transparent;
        }

        // ────────────────────────────────────────────────────────────
        //  NAV ITEM CREATION
        // ────────────────────────────────────────────────────────────
        private void CreateNavItems()
        {
            navDashboard = MakeNav("dashboard", "Dashboard");
            navQuickSale = MakeNav("quicksale", "Quick Sale");
            navOrders = MakeNav("orders", "Orders");
            navItems = MakeNav("items", "Items");
            navCustomers = MakeNav("customers", "Customers");
            navReports = MakeNav("reports", "Reports");
            navSettings = MakeNav("settings", "Settings");

            _navItems = new[] { navDashboard, navQuickSale, navOrders,
                                navItems, navCustomers, navReports, navSettings };

            int y = 8;
            foreach (var nav in _navItems)
            {
                nav.Location = new Point(16, y);
                pnlNavContainer.Controls.Add(nav);
                y += 44; // 40px height + 4px gap
            }

            // Wire click events
            navDashboard.NavClicked += NavDashboard_Click;
            navQuickSale.NavClicked += NavQuickSale_Click;
            navOrders.NavClicked += NavOrders_Click;
            navItems.NavClicked += NavItems_Click;
            navCustomers.NavClicked += NavCustomers_Click;
            navReports.NavClicked += NavReports_Click;
            navSettings.NavClicked += NavSettings_Click;
        }

        private NavItem MakeNav(string iconKey, string label)
        {
            var nav = new NavItem
            {
                IconKey = iconKey,
                LabelText = label,
                Size = new Size(232, 40)
            };
            return nav;
        }

        // ────────────────────────────────────────────────────────────
        //  PAINT HANDLERS
        // ────────────────────────────────────────────────────────────

        /// <summary>Right border on sidebar.</summary>
        private void PnlSidebar_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(BorderLine, 1);
            int x = pnlSidebar.Width - 1;
            e.Graphics.DrawLine(pen, x, 0, x, pnlSidebar.Height);
        }

        /// <summary>Bottom border on logo header.</summary>
        private void PnlLogoHeader_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(BorderLine, 1);
            int y = pnlLogoHeader.Height - 1;
            e.Graphics.DrawLine(pen, 0, y, pnlLogoHeader.Width, y);
        }

        /// <summary>Green circle with leaf icon.</summary>
        private void PnlLogoCircle_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using var bgBrush = new SolidBrush(GreenAccent);
            g.FillEllipse(bgBrush, 0, 0, 27, 27);

            using var font = new Font("Segoe UI", 14F);
            using var textBrush = new SolidBrush(Color.White);
            string leaf = "🍵";
            var sz = g.MeasureString(leaf, font);
            g.DrawString(leaf, font, textBrush, (28 - sz.Width) / 2, (28 - sz.Height) / 2);
        }

        /// <summary>Separator line at top of sidebar bottom.</summary>
        private void PnlSidebarBottom_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(BorderLine, 1);
            e.Graphics.DrawLine(pen, 16, 0, pnlSidebarBottom.Width - 16, 0);
        }

        /// <summary>Top border on footer.</summary>
        private void PnlFooter_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(BorderLine, 1);
            e.Graphics.DrawLine(pen, 0, 0, pnlFooter.Width, 0);
        }

        // ────────────────────────────────────────────────────────────
        //  TITLE BAR DRAG
        // ────────────────────────────────────────────────────────────
        private void Drag_MouseDown(object? s, MouseEventArgs e)
        {
            _dragging = true;
            _dragCursor = Cursor.Position;
            _dragForm = this.Location;
        }

        private void Drag_MouseMove(object? s, MouseEventArgs e)
        {
            if (_dragging)
            {
                var diff = Point.Subtract(Cursor.Position, new Size(_dragCursor));
                this.Location = Point.Add(_dragForm, new Size(diff));
            }
        }

        private void Drag_MouseUp(object? s, MouseEventArgs e) => _dragging = false;

        private void LogoHeader_DoubleClick(object? s, EventArgs e)
        {
            WindowState = WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
        }

        // ────────────────────────────────────────────────────────────
        //  LOGOUT
        // ────────────────────────────────────────────────────────────
        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            this.Close(); // FormClosed event in LoginDesign shows LoginForm
        }
    }
}
