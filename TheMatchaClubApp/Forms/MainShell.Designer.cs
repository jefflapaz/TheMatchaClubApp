namespace TheMatchaClubApp.Forms
{
    partial class MainShell
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges ce1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();

            pnlSidebar = new Panel();
            pnlLogoHeader = new Panel();
            pnlLogoCircle = new Panel();
            lblLogoText = new Label();
            pnlNavContainer = new Panel();
            pnlSidebarBottom = new Panel();
            btnLogout = new Guna.UI2.WinForms.Guna2Button();
            pnlFooter = new Panel();
            lblCopyright = new Label();
            lblSystemOnline = new Label();
            lblDbConnected = new Label();
            btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            btnMaximize = new Guna.UI2.WinForms.Guna2ControlBox();
            btnMinimize = new Guna.UI2.WinForms.Guna2ControlBox();
            pnlContent = new Panel();

            pnlSidebar.SuspendLayout();
            pnlLogoHeader.SuspendLayout();
            pnlSidebarBottom.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();

            // ═══════════════════════════════════════════════════════════
            // pnlSidebar
            // ═══════════════════════════════════════════════════════════
            pnlSidebar.Controls.Add(pnlNavContainer);
            pnlSidebar.Controls.Add(pnlSidebarBottom);
            pnlSidebar.Controls.Add(pnlLogoHeader);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(264, 800);
            pnlSidebar.TabIndex = 0;

            // pnlLogoHeader
            pnlLogoHeader.Controls.Add(pnlLogoCircle);
            pnlLogoHeader.Controls.Add(lblLogoText);
            pnlLogoHeader.Dock = DockStyle.Top;
            pnlLogoHeader.Location = new Point(0, 0);
            pnlLogoHeader.Name = "pnlLogoHeader";
            pnlLogoHeader.Size = new Size(264, 64);
            pnlLogoHeader.TabIndex = 0;

            // pnlLogoCircle
            pnlLogoCircle.Location = new Point(20, 18);
            pnlLogoCircle.Name = "pnlLogoCircle";
            pnlLogoCircle.Size = new Size(28, 28);
            pnlLogoCircle.TabIndex = 0;

            // lblLogoText
            lblLogoText.Location = new Point(56, 18);
            lblLogoText.Name = "lblLogoText";
            lblLogoText.Size = new Size(160, 28);
            lblLogoText.TabIndex = 1;
            lblLogoText.Text = "Matcha Café";
            lblLogoText.TextAlign = ContentAlignment.MiddleLeft;

            // pnlNavContainer
            pnlNavContainer.Dock = DockStyle.Fill;
            pnlNavContainer.Location = new Point(0, 64);
            pnlNavContainer.Name = "pnlNavContainer";
            pnlNavContainer.Padding = new Padding(16, 8, 16, 0);
            pnlNavContainer.Size = new Size(264, 560);
            pnlNavContainer.TabIndex = 1;

            // ═══════════════════════════════════════════════════════════
            // pnlSidebarBottom
            // ═══════════════════════════════════════════════════════════
            pnlSidebarBottom.Controls.Add(btnLogout);
            pnlSidebarBottom.Dock = DockStyle.Bottom;
            pnlSidebarBottom.Location = new Point(0, 680);
            pnlSidebarBottom.Name = "pnlSidebarBottom";
            pnlSidebarBottom.Size = new Size(264, 120);
            pnlSidebarBottom.TabIndex = 2;

            // btnLogout
            btnLogout.CustomizableEdges = ce1;
            btnLogout.Font = new Font("Segoe UI", 9F);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(16, 16);
            btnLogout.Name = "btnLogout";
            btnLogout.ShadowDecoration.CustomizableEdges = ce2;
            btnLogout.Size = new Size(180, 36);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "⏻  Log Out";

            // ═══════════════════════════════════════════════════════════
            // pnlFooter
            // ═══════════════════════════════════════════════════════════
            pnlFooter.Controls.Add(lblCopyright);
            pnlFooter.Controls.Add(lblSystemOnline);
            pnlFooter.Controls.Add(lblDbConnected);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(264, 752);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1016, 48);
            pnlFooter.TabIndex = 1;

            // lblCopyright
            lblCopyright.Location = new Point(16, 14);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new Size(360, 20);
            lblCopyright.TabIndex = 0;
            lblCopyright.Text = "© 2026 S.I.P. (Session Integrated POS) Version 1.0";

            // lblSystemOnline
            lblSystemOnline.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSystemOnline.Location = new Point(600, 14);
            lblSystemOnline.Name = "lblSystemOnline";
            lblSystemOnline.Size = new Size(130, 20);
            lblSystemOnline.TabIndex = 1;
            lblSystemOnline.Text = "●  System Online";
            lblSystemOnline.TextAlign = ContentAlignment.MiddleRight;

            // lblDbConnected
            lblDbConnected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDbConnected.Location = new Point(740, 14);
            lblDbConnected.Name = "lblDbConnected";
            lblDbConnected.Size = new Size(160, 20);
            lblDbConnected.TabIndex = 2;
            lblDbConnected.Text = "●  Database Connected";
            lblDbConnected.TextAlign = ContentAlignment.MiddleRight;

            // ═══════════════════════════════════════════════════════════
            // Window Control Buttons (top-right overlay)
            // ═══════════════════════════════════════════════════════════
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            btnMinimize.CustomizableEdges = ce3;
            btnMinimize.Location = new Point(1130, 0);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.ShadowDecoration.CustomizableEdges = ce4;
            btnMinimize.Size = new Size(50, 32);
            btnMinimize.TabIndex = 10;

            btnMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaximize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            btnMaximize.CustomizableEdges = ce5;
            btnMaximize.Location = new Point(1180, 0);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.ShadowDecoration.CustomizableEdges = ce6;
            btnMaximize.Size = new Size(50, 32);
            btnMaximize.TabIndex = 11;

            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(1230, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(50, 32);
            btnClose.TabIndex = 12;

            // ═══════════════════════════════════════════════════════════
            // pnlContent
            // ═══════════════════════════════════════════════════════════
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(264, 0);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1016, 752);
            pnlContent.TabIndex = 2;

            // ═══════════════════════════════════════════════════════════
            // MainShell
            // ═══════════════════════════════════════════════════════════
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 800);
            // Add order matters: last added = first docked
            Controls.Add(btnClose);
            Controls.Add(btnMaximize);
            Controls.Add(btnMinimize);
            Controls.Add(pnlContent);
            Controls.Add(pnlFooter);
            Controls.Add(pnlSidebar);
            Name = "MainShell";
            Text = "S.I.P. - Session Integrated POS";

            pnlSidebar.ResumeLayout(false);
            pnlLogoHeader.ResumeLayout(false);
            pnlSidebarBottom.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        // ── Sidebar ────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlLogoHeader;
        private System.Windows.Forms.Panel pnlLogoCircle;
        private System.Windows.Forms.Label lblLogoText;
        private System.Windows.Forms.Panel pnlNavContainer;
        private System.Windows.Forms.Panel pnlSidebarBottom;
        private Guna.UI2.WinForms.Guna2Button btnLogout;

        // ── Footer ─────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblSystemOnline;
        private System.Windows.Forms.Label lblDbConnected;

        // ── Window Controls ────────────────────────────────────────
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;
        private Guna.UI2.WinForms.Guna2ControlBox btnMaximize;
        private Guna.UI2.WinForms.Guna2ControlBox btnMinimize;

        // ── Content ────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlContent;
    }
}
