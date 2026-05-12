namespace TheMatchaClubApp.Forms
{
    partial class NavItem
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

            pnlContainer = new Guna.UI2.WinForms.Guna2Panel();
            pnlIcon = new Panel();
            lblText = new Label();

            pnlContainer.SuspendLayout();
            SuspendLayout();

            // pnlContainer
            pnlContainer.Controls.Add(lblText);
            pnlContainer.Controls.Add(pnlIcon);
            pnlContainer.CustomizableEdges = ce1;
            pnlContainer.Dock = DockStyle.Fill;
            pnlContainer.Location = new Point(0, 0);
            pnlContainer.Name = "pnlContainer";
            pnlContainer.ShadowDecoration.CustomizableEdges = ce2;
            pnlContainer.Size = new Size(232, 40);
            pnlContainer.TabIndex = 0;

            // pnlIcon
            pnlIcon.Location = new Point(12, 10);
            pnlIcon.Name = "pnlIcon";
            pnlIcon.Size = new Size(20, 20);
            pnlIcon.TabIndex = 0;

            // lblText
            lblText.Location = new Point(44, 0);
            lblText.Name = "lblText";
            lblText.Size = new Size(180, 40);
            lblText.TabIndex = 1;
            lblText.Text = "Nav Item";
            lblText.TextAlign = ContentAlignment.MiddleLeft;

            // NavItem
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlContainer);
            Name = "NavItem";
            Size = new Size(232, 40);

            pnlContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlContainer;
        private System.Windows.Forms.Panel pnlIcon;
        private System.Windows.Forms.Label lblText;
    }
}
