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
        private static readonly Color SidebarBg = ColorTranslator.FromHtml("#52B743");
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
            pnlSidebar.Padding = new Padding(0, 32, 0, 0); // Balance with content padding
            pnlSidebar.Paint += PnlSidebar_Paint;
            pnlSidebar.SizeChanged += (s, e) => ApplySidebarRounding();

            // ── Logo header ────────────────────────────────────────
            pnlLogoHeader.Height = 80;
            pnlLogoHeader.BackColor = SidebarBg;
            pnlLogoHeader.Paint += PnlLogoHeader_Paint;
            pnlLogoHeader.MouseDown += Drag_MouseDown;
            pnlLogoHeader.MouseMove += Drag_MouseMove;
            pnlLogoHeader.MouseUp += Drag_MouseUp;
            pnlLogoHeader.DoubleClick += LogoHeader_DoubleClick;
            pnlLogoHeader.SizeChanged += (s, e) => ApplySidebarRounding();

            pnlLogoCircle.Size = new Size(40, 40);
            pnlLogoCircle.Location = new Point(20, 20);
            pnlLogoCircle.BackColor = Color.Transparent;
            pnlLogoCircle.Paint += PnlLogoCircle_Paint;

            lblLogoText.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblLogoText.Size = new Size(110, 40);
            lblLogoText.Location = new Point(68, 20);
            lblLogoText.ForeColor = Color.White;
            lblLogoText.BackColor = Color.Transparent;

            // ── Nav container ──────────────────────────────────────
            pnlNavContainer.BackColor = SidebarBg;
            CreateNavItems();

            ApplySidebarRounding();

            // ── Sidebar bottom ─────────────────────────────────────
            pnlSidebarBottom.BackColor = SidebarBg;
            pnlSidebarBottom.Paint += PnlSidebarBottom_Paint;

            btnLogout.FillColor = Color.Transparent;
            btnLogout.ForeColor = Color.White;
            btnLogout.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnLogout.BorderThickness = 0;
            btnLogout.HoverState.FillColor = Color.White;
            btnLogout.HoverState.ForeColor = Red;
            btnLogout.TextAlign = HorizontalAlignment.Left;
            btnLogout.Click += BtnLogout_Click;

            // ── Content panel ──────────────────────────────────────
            pnlContent.BackColor = ContentBg;
            pnlContent.Padding = new Padding(0, 32, 0, 0); // Push content down to clear window buttons

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
            using var pen = new Pen(Color.FromArgb(40, Color.White), 1);
            int y = pnlLogoHeader.Height - 1;
            e.Graphics.DrawLine(pen, 0, y, pnlLogoHeader.Width, y);
        }

        private void PnlLogoCircle_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int w = pnlLogoCircle.Width;
            int h = pnlLogoCircle.Height;

            // Draw a white outline circle border
            using (var borderPen = new Pen(Color.White, 2f))
            {
                g.DrawEllipse(borderPen, 2, 2, w - 5, h - 5);
            }

            // Draw a beautiful white vector cup icon (matcha bowl with steam) inside
            using (var pen = new Pen(Color.White, 2f))
            using (var brush = new SolidBrush(Color.White))
            {
                float cx = w / 2f;
                float cy = h / 2f;

                float bowlW = w * 0.45f;
                float bowlH = h * 0.30f;
                float bowlX = cx - bowlW / 2f;
                float bowlY = cy - bowlH / 2f + 4; // space for steam

                // Draw matcha bowl path
                using (var path = new GraphicsPath())
                {
                    float r = bowlH * 0.4f; // bottom corner radius
                    path.AddLine(bowlX, bowlY, bowlX + bowlW, bowlY);
                    path.AddArc(bowlX + bowlW - r, bowlY + bowlH - r, r, r, 0, 90);
                    path.AddLine(bowlX + bowlW - r, bowlY + bowlH, bowlX + r, bowlY + bowlH);
                    path.AddArc(bowlX, bowlY + bowlH - r, r, r, 90, 90);
                    path.CloseFigure();
                    g.FillPath(brush, path);
                }

                // Draw base
                float baseW = bowlW * 0.4f;
                float baseH = bowlH * 0.15f;
                g.FillRectangle(brush, cx - baseW / 2f, bowlY + bowlH, baseW, baseH);

                // Draw steam lines
                // Steam 1
                g.DrawBezier(pen, 
                    cx - 3, bowlY - 3,
                    cx - 5, bowlY - 7,
                    cx - 1, bowlY - 11,
                    cx - 3, bowlY - 15);
                // Steam 2
                g.DrawBezier(pen, 
                    cx + 3, bowlY - 3,
                    cx + 1, bowlY - 7,
                    cx + 5, bowlY - 11,
                    cx + 3, bowlY - 15);
            }
        }

        /// <summary>Separator line at top of sidebar bottom.</summary>
        private void PnlSidebarBottom_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(40, Color.White), 1);
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

        // ────────────────────────────────────────────────────────────
        //  SIDEBAR ROUNDING
        // ────────────────────────────────────────────────────────────
        private void ApplySidebarRounding()
        {
            int radius = 80; // Radius of rounding for top-right corner
            RoundControlCorner(pnlSidebar, radius);
            RoundControlCorner(pnlLogoHeader, radius);
        }

        private void RoundControlCorner(Control ctrl, int radius)
        {
            if (ctrl.Width <= 0 || ctrl.Height <= 0) return;

            using var path = new GraphicsPath();
            int w = ctrl.Width;
            int h = ctrl.Height;

            // Top-left corner (sharp)
            path.AddLine(0, 0, 0, 0);

            // Top-right corner (rounded)
            path.AddArc(w - radius, 0, radius, radius, 270, 90);

            // Bottom-right corner (sharp)
            path.AddLine(w, h, w, h);

            // Bottom-left corner (sharp)
            path.AddLine(0, h, 0, h);

            path.CloseAllFigures();
            ctrl.Region = new Region(path);
        }
    }
}
