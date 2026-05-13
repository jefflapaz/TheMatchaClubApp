namespace TheMatchaClubApp.Forms
{
    partial class AdminSetupForm
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges ce1 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce2 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce3 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce4 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce5 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce6 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce7 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce8 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce9 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce10 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce11 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce12 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce13 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce14 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce15 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce16 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce17 = new();
            Guna.UI2.WinForms.Suite.CustomizableEdges ce18 = new();

            // Title bar
            pnlTitleBar = new Panel();
            btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            btnMinimize = new Guna.UI2.WinForms.Guna2ControlBox();

            // Card
            pnlCard = new Guna.UI2.WinForms.Guna2Panel();

            // Logo & headings
            picLogo = new PictureBox();
            pnlBadge = new Guna.UI2.WinForms.Guna2Panel();
            lblBadge = new Label();
            lblTitle = new Label();
            lblSubtitle = new Label();

            // Fields
            lblFullName = new Label();
            txtFullName = new Guna.UI2.WinForms.Guna2TextBox();
            lblUsername = new Label();
            txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            lblEmail = new Label();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            lblPassword = new Label();
            txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new Guna.UI2.WinForms.Guna2TextBox();

            // Button
            btnCreateAccount = new Guna.UI2.WinForms.Guna2Button();

            // Error label
            lblError = new Label();

            pnlTitleBar.SuspendLayout();
            pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlBadge.SuspendLayout();
            SuspendLayout();

            // ─── pnlTitleBar ────────────────────────────────────────
            pnlTitleBar.Controls.Add(btnMinimize);
            pnlTitleBar.Controls.Add(btnClose);
            pnlTitleBar.Dock = DockStyle.Top;
            pnlTitleBar.Location = new Point(0, 0);
            pnlTitleBar.Name = "pnlTitleBar";
            pnlTitleBar.Size = new Size(693, 49);
            pnlTitleBar.TabIndex = 0;

            // btnMinimize
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            btnMinimize.CustomizableEdges = ce1;
            btnMinimize.FillColor = Color.FromArgb(139, 152, 166);
            btnMinimize.IconColor = Color.White;
            btnMinimize.Location = new Point(573, 0);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.ShadowDecoration.CustomizableEdges = ce2;
            btnMinimize.Size = new Size(60, 49);
            btnMinimize.TabIndex = 0;

            // btnClose
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.CustomizableEdges = ce3;
            btnClose.FillColor = Color.FromArgb(139, 152, 166);
            btnClose.IconColor = Color.White;
            btnClose.Location = new Point(633, 0);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = ce4;
            btnClose.Size = new Size(60, 49);
            btnClose.TabIndex = 1;

            // ─── pnlCard ────────────────────────────────────────────
            pnlCard.Controls.Add(lblError);
            pnlCard.Controls.Add(btnCreateAccount);
            pnlCard.Controls.Add(txtConfirmPassword);
            pnlCard.Controls.Add(lblConfirmPassword);
            pnlCard.Controls.Add(txtPassword);
            pnlCard.Controls.Add(lblPassword);
            pnlCard.Controls.Add(txtEmail);
            pnlCard.Controls.Add(lblEmail);
            pnlCard.Controls.Add(txtUsername);
            pnlCard.Controls.Add(lblUsername);
            pnlCard.Controls.Add(txtFullName);
            pnlCard.Controls.Add(lblFullName);
            pnlCard.Controls.Add(lblSubtitle);
            pnlCard.Controls.Add(lblTitle);
            pnlCard.Controls.Add(picLogo);
            pnlCard.Controls.Add(pnlBadge);
            pnlCard.CustomizableEdges = ce5;
            pnlCard.Location = new Point(53, 60);
            pnlCard.Name = "pnlCard";
            pnlCard.ShadowDecoration.CustomizableEdges = ce6;
            pnlCard.Size = new Size(587, 940);
            pnlCard.TabIndex = 1;

            // pnlBadge
            pnlBadge.Controls.Add(lblBadge);
            pnlBadge.CustomizableEdges = ce7;
            pnlBadge.Location = new Point(200, 20);
            pnlBadge.Name = "pnlBadge";
            pnlBadge.ShadowDecoration.CustomizableEdges = ce8;
            pnlBadge.Size = new Size(187, 37);
            pnlBadge.TabIndex = 0;

            // lblBadge
            lblBadge.Location = new Point(0, 0);
            lblBadge.Name = "lblBadge";
            lblBadge.Size = new Size(187, 37);
            lblBadge.TabIndex = 0;
            lblBadge.Text = "FIRST TIME SETUP";

            // picLogo
            picLogo.Location = new Point(259, 68);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(69, 80);
            picLogo.TabIndex = 1;
            picLogo.TabStop = false;

            // lblTitle
            lblTitle.Location = new Point(0, 158);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(587, 46);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Create Admin Account";

            // lblSubtitle
            lblSubtitle.Location = new Point(0, 204);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(587, 31);
            lblSubtitle.TabIndex = 3;
            lblSubtitle.Text = "Set up your first administrator account to get started.";

            // lblFullName
            lblFullName.Location = new Point(32, 252);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(523, 25);
            lblFullName.TabIndex = 4;
            lblFullName.Text = "FULL NAME";

            // txtFullName
            txtFullName.CustomizableEdges = ce9;
            txtFullName.DefaultText = "";
            txtFullName.Font = new Font("Segoe UI", 9F);
            txtFullName.Location = new Point(32, 277);
            txtFullName.Name = "txtFullName";
            txtFullName.PlaceholderText = "e.g. Juan Dela Cruz";
            txtFullName.SelectedText = "";
            txtFullName.ShadowDecoration.CustomizableEdges = ce10;
            txtFullName.Size = new Size(523, 50);
            txtFullName.TabIndex = 5;

            // lblUsername
            lblUsername.Location = new Point(32, 337);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(250, 25);
            lblUsername.TabIndex = 6;
            lblUsername.Text = "USERNAME";

            // txtUsername
            txtUsername.CustomizableEdges = ce11;
            txtUsername.DefaultText = "";
            txtUsername.Font = new Font("Segoe UI", 9F);
            txtUsername.Location = new Point(32, 362);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "e.g. admin";
            txtUsername.SelectedText = "";
            txtUsername.ShadowDecoration.CustomizableEdges = ce12;
            txtUsername.Size = new Size(250, 50);
            txtUsername.TabIndex = 7;

            // lblEmail
            lblEmail.Location = new Point(305, 337);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(250, 25);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "EMAIL";

            // txtEmail
            txtEmail.CustomizableEdges = ce13;
            txtEmail.DefaultText = "";
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.Location = new Point(305, 362);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "e.g. admin@matchacafe.pos";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = ce14;
            txtEmail.Size = new Size(250, 50);
            txtEmail.TabIndex = 9;

            // lblPassword
            lblPassword.Location = new Point(32, 422);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(250, 25);
            lblPassword.TabIndex = 10;
            lblPassword.Text = "PASSWORD";

            // txtPassword
            txtPassword.CustomizableEdges = ce15;
            txtPassword.DefaultText = "";
            txtPassword.Font = new Font("Segoe UI", 9F);
            txtPassword.Location = new Point(32, 447);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Min 6 characters";
            txtPassword.SelectedText = "";
            txtPassword.ShadowDecoration.CustomizableEdges = ce16;
            txtPassword.Size = new Size(250, 50);
            txtPassword.TabIndex = 11;

            // lblConfirmPassword
            lblConfirmPassword.Location = new Point(305, 422);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(250, 25);
            lblConfirmPassword.TabIndex = 12;
            lblConfirmPassword.Text = "CONFIRM PASSWORD";

            // txtConfirmPassword
            txtConfirmPassword.CustomizableEdges = ce17;
            txtConfirmPassword.DefaultText = "";
            txtConfirmPassword.Font = new Font("Segoe UI", 9F);
            txtConfirmPassword.Location = new Point(305, 447);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PlaceholderText = "Re-enter password";
            txtConfirmPassword.SelectedText = "";
            txtConfirmPassword.ShadowDecoration.CustomizableEdges = ce18;
            txtConfirmPassword.Size = new Size(250, 50);
            txtConfirmPassword.TabIndex = 13;

            // lblError
            lblError.Location = new Point(32, 510);
            lblError.Name = "lblError";
            lblError.Size = new Size(523, 40);
            lblError.TabIndex = 14;
            lblError.Text = "";
            lblError.Visible = false;

            // btnCreateAccount
            btnCreateAccount.Font = new Font("Segoe UI", 9F);
            btnCreateAccount.ForeColor = Color.White;
            btnCreateAccount.Location = new Point(32, 560);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(523, 65);
            btnCreateAccount.TabIndex = 15;
            btnCreateAccount.Text = "Create Admin Account & Continue";

            // ─── AdminSetupForm ────────────────────────────────────
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(693, 1046);
            Controls.Add(pnlCard);
            Controls.Add(pnlTitleBar);
            Name = "AdminSetupForm";
            Text = "Admin Setup";
            Resize += AdminSetupForm_Resize;

            pnlTitleBar.ResumeLayout(false);
            pnlCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlBadge.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel pnlTitleBar;
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;
        private Guna.UI2.WinForms.Guna2ControlBox btnMinimize;
        private Guna.UI2.WinForms.Guna2Panel pnlCard;
        private PictureBox picLogo;
        private Guna.UI2.WinForms.Guna2Panel pnlBadge;
        private Label lblBadge;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblFullName;
        private Guna.UI2.WinForms.Guna2TextBox txtFullName;
        private Label lblUsername;
        private Guna.UI2.WinForms.Guna2TextBox txtUsername;
        private Label lblEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Label lblPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Label lblConfirmPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmPassword;
        private Guna.UI2.WinForms.Guna2Button btnCreateAccount;
        private Label lblError;
    }
}
