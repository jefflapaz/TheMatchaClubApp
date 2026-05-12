namespace TheMatchaClubApp.Forms
{
    partial class CustomersView
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomersView));
            
            components = new System.ComponentModel.Container();
            pnlTopHeader = new System.Windows.Forms.Panel();
            lblChevron = new System.Windows.Forms.Label();
            lblViewName = new System.Windows.Forms.Label();
            btnAddCustomer = new Guna.UI2.WinForms.Guna2Button();
            
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            pnlDirectory = new System.Windows.Forms.Panel();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            pnlFilters = new System.Windows.Forms.Panel();
            btnFilterAll = new Guna.UI2.WinForms.Guna2Button();
            btnFilterRegular = new Guna.UI2.WinForms.Guna2Button();
            btnFilterNew = new Guna.UI2.WinForms.Guna2Button();
            flpCustomers = new System.Windows.Forms.FlowLayoutPanel();
            
            pnlProfile = new System.Windows.Forms.Panel();
            pnlProfileHeader = new System.Windows.Forms.Panel();
            picProfile = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            lblProfileName = new System.Windows.Forms.Label();
            lblProfileEmail = new System.Windows.Forms.Label();
            lblProfilePhone = new System.Windows.Forms.Label();
            chipStatus = new Guna.UI2.WinForms.Guna2Chip();
            btnEmail = new Guna.UI2.WinForms.Guna2Button();
            btnEditProfile = new Guna.UI2.WinForms.Guna2Button();
            btnExport = new Guna.UI2.WinForms.Guna2Button();
            
            flpKPIs = new System.Windows.Forms.FlowLayoutPanel();
            
            pnlHistoryHeader = new System.Windows.Forms.Panel();
            lblHistoryTitle = new System.Windows.Forms.Label();
            btnViewOrders = new Guna.UI2.WinForms.Guna2Button();
            
            dgvHistory = new Guna.UI2.WinForms.Guna2DataGridView();
            
            pnlDataIntelligence = new System.Windows.Forms.Panel();
            pnlPreferences = new Guna.UI2.WinForms.Guna2Panel();
            lblPrefTitle = new System.Windows.Forms.Label();
            lblFavCatLabel = new System.Windows.Forms.Label();
            lblFavCatValue = new Guna.UI2.WinForms.Guna2Chip();
            lblModLabel = new System.Windows.Forms.Label();
            lblModValue = new Guna.UI2.WinForms.Guna2Chip();
            lblTimeLabel = new System.Windows.Forms.Label();
            lblTimeValue = new Guna.UI2.WinForms.Guna2Chip();
            
            pnlAdminNotes = new Guna.UI2.WinForms.Guna2Panel();
            lblNotesTitle = new System.Windows.Forms.Label();
            txtAdminNotes = new Guna.UI2.WinForms.Guna2TextBox();
            btnSaveNote = new Guna.UI2.WinForms.Guna2Button();
            
            pnlCalendarPopup = new Guna.UI2.WinForms.Guna2Panel();
            flpCalendarDays = new System.Windows.Forms.FlowLayoutPanel();
            lblCalendarTitle = new System.Windows.Forms.Label();
            btnCalendarClose = new Guna.UI2.WinForms.Guna2Button();

            pnlTopHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            pnlDirectory.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlProfile.SuspendLayout();
            pnlProfileHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picProfile).BeginInit();
            pnlHistoryHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            pnlDataIntelligence.SuspendLayout();
            pnlPreferences.SuspendLayout();
            pnlAdminNotes.SuspendLayout();
            pnlCalendarPopup.SuspendLayout();
            SuspendLayout();

            // Top Header
            pnlTopHeader.Controls.Add(lblChevron);
            pnlTopHeader.Controls.Add(lblViewName);
            pnlTopHeader.Controls.Add(btnAddCustomer);
            pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopHeader.Size = new System.Drawing.Size(1200, 64);

            lblChevron.Location = new System.Drawing.Point(16, 20);
            lblChevron.Size = new System.Drawing.Size(16, 24);
            lblChevron.Text = "\u25B6";

            lblViewName.Location = new System.Drawing.Point(34, 18);
            lblViewName.Size = new System.Drawing.Size(120, 28);
            lblViewName.Text = "Customers";

            btnAddCustomer.Location = new System.Drawing.Point(1050, 16);
            btnAddCustomer.Size = new System.Drawing.Size(128, 32);
            btnAddCustomer.Text = "+ Add Customer";
            btnAddCustomer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // Split Container
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.SplitterDistance = 350;
            splitContainerMain.SplitterWidth = 2;
            splitContainerMain.Panel1.Controls.Add(pnlDirectory);
            splitContainerMain.Panel2.Controls.Add(pnlProfile);

            // Directory Panel
            pnlDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlDirectory.Padding = new System.Windows.Forms.Padding(16);
            pnlDirectory.Controls.Add(flpCustomers);
            pnlDirectory.Controls.Add(pnlFilters);
            pnlDirectory.Controls.Add(txtSearch);

            txtSearch.Dock = System.Windows.Forms.DockStyle.Top;
            txtSearch.Height = 40;
            txtSearch.PlaceholderText = "Search by name, email or phone...";
            
            pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            pnlFilters.Height = 50;
            pnlFilters.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            pnlFilters.Controls.Add(btnFilterNew);
            pnlFilters.Controls.Add(btnFilterRegular);
            pnlFilters.Controls.Add(btnFilterAll);

            btnFilterAll.Width = 80;
            btnFilterAll.Dock = System.Windows.Forms.DockStyle.Left;
            btnFilterAll.Text = "All";
            
            btnFilterRegular.Width = 100;
            btnFilterRegular.Dock = System.Windows.Forms.DockStyle.Left;
            btnFilterRegular.Text = "Regular";
            btnFilterRegular.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);

            btnFilterNew.Width = 80;
            btnFilterNew.Dock = System.Windows.Forms.DockStyle.Left;
            btnFilterNew.Text = "New";
            btnFilterNew.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);

            flpCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            flpCustomers.AutoScroll = true;
            flpCustomers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpCustomers.WrapContents = false;

            // Profile Panel
            pnlProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlProfile.Padding = new System.Windows.Forms.Padding(32);
            pnlProfile.AutoScroll = true;
            pnlProfile.Controls.Add(pnlCalendarPopup); // Added here so it overlays
            pnlProfile.Controls.Add(pnlDataIntelligence);
            pnlProfile.Controls.Add(dgvHistory);
            pnlProfile.Controls.Add(pnlHistoryHeader);
            pnlProfile.Controls.Add(flpKPIs);
            pnlProfile.Controls.Add(pnlProfileHeader);

            // Profile Header
            pnlProfileHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlProfileHeader.Height = 100;
            pnlProfileHeader.Controls.Add(picProfile);
            pnlProfileHeader.Controls.Add(lblProfileName);
            pnlProfileHeader.Controls.Add(chipStatus);
            pnlProfileHeader.Controls.Add(lblProfileEmail);
            pnlProfileHeader.Controls.Add(lblProfilePhone);
            pnlProfileHeader.Controls.Add(btnExport);
            pnlProfileHeader.Controls.Add(btnEditProfile);
            pnlProfileHeader.Controls.Add(btnEmail);

            picProfile.Location = new System.Drawing.Point(0, 0);
            picProfile.Size = new System.Drawing.Size(80, 80);
            picProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            lblProfileName.Location = new System.Drawing.Point(100, 10);
            lblProfileName.AutoSize = true;

            chipStatus.Location = new System.Drawing.Point(300, 10);
            chipStatus.Size = new System.Drawing.Size(80, 24);

            lblProfileEmail.Location = new System.Drawing.Point(100, 45);
            lblProfileEmail.AutoSize = true;
            lblProfileEmail.BackColor = System.Drawing.Color.Transparent;

            lblProfilePhone.Location = new System.Drawing.Point(100, 65);
            lblProfilePhone.AutoSize = true;
            lblProfilePhone.BackColor = System.Drawing.Color.Transparent;

            btnExport.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnExport.Location = new System.Drawing.Point(700, 20);
            btnExport.Size = new System.Drawing.Size(80, 36);
            btnExport.Text = "Export";

            btnEditProfile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnEditProfile.Location = new System.Drawing.Point(600, 20);
            btnEditProfile.Size = new System.Drawing.Size(90, 36);
            btnEditProfile.Text = "Edit Profile";

            btnEmail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnEmail.Location = new System.Drawing.Point(510, 20);
            btnEmail.Size = new System.Drawing.Size(80, 36);
            btnEmail.Text = "Email";

            // KPIs
            flpKPIs.Dock = System.Windows.Forms.DockStyle.Top;
            flpKPIs.Height = 120;
            flpKPIs.Padding = new System.Windows.Forms.Padding(0, 20, 0, 20);
            flpKPIs.WrapContents = false;

            // History Header
            pnlHistoryHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHistoryHeader.Height = 40;
            pnlHistoryHeader.Controls.Add(lblHistoryTitle);
            pnlHistoryHeader.Controls.Add(btnViewOrders);

            lblHistoryTitle.Dock = System.Windows.Forms.DockStyle.Left;
            lblHistoryTitle.Text = "Purchase History";
            lblHistoryTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            lblHistoryTitle.AutoSize = true;

            btnViewOrders.Dock = System.Windows.Forms.DockStyle.Right;
            btnViewOrders.Width = 150;
            btnViewOrders.Text = "View All Orders";

            // History DataGrid
            dgvHistory.Dock = System.Windows.Forms.DockStyle.Top;
            dgvHistory.Height = 250;
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;

            // Data Intelligence
            pnlDataIntelligence.Dock = System.Windows.Forms.DockStyle.Top;
            pnlDataIntelligence.Height = 250;
            pnlDataIntelligence.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            pnlDataIntelligence.Controls.Add(pnlAdminNotes);
            pnlDataIntelligence.Controls.Add(pnlPreferences);

            pnlPreferences.Dock = System.Windows.Forms.DockStyle.Left;
            pnlPreferences.Width = 380;
            pnlPreferences.Controls.Add(lblPrefTitle);
            pnlPreferences.Controls.Add(lblFavCatLabel);
            pnlPreferences.Controls.Add(lblFavCatValue);
            pnlPreferences.Controls.Add(lblModLabel);
            pnlPreferences.Controls.Add(lblModValue);
            pnlPreferences.Controls.Add(lblTimeLabel);
            pnlPreferences.Controls.Add(lblTimeValue);

            lblPrefTitle.Location = new System.Drawing.Point(20, 20);
            lblPrefTitle.Text = "REGULAR PREFERENCES";
            lblPrefTitle.AutoSize = true;

            lblFavCatLabel.Location = new System.Drawing.Point(20, 60);
            lblFavCatLabel.Text = "Favorite Category";
            lblFavCatLabel.AutoSize = true;
            lblFavCatValue.Location = new System.Drawing.Point(150, 55);
            lblFavCatValue.Size = new System.Drawing.Size(200, 30);

            lblModLabel.Location = new System.Drawing.Point(20, 110);
            lblModLabel.Text = "Modification Style";
            lblModLabel.AutoSize = true;
            lblModValue.Location = new System.Drawing.Point(150, 105);
            lblModValue.Size = new System.Drawing.Size(200, 30);

            lblTimeLabel.Location = new System.Drawing.Point(20, 160);
            lblTimeLabel.Text = "Typical Visit Time";
            lblTimeLabel.AutoSize = true;
            lblTimeValue.Location = new System.Drawing.Point(150, 155);
            lblTimeValue.Size = new System.Drawing.Size(200, 30);

            pnlAdminNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAdminNotes.Controls.Add(lblNotesTitle);
            pnlAdminNotes.Controls.Add(txtAdminNotes);
            pnlAdminNotes.Controls.Add(btnSaveNote);

            lblNotesTitle.Location = new System.Drawing.Point(20, 20);
            lblNotesTitle.Text = "ADMIN NOTES";
            lblNotesTitle.AutoSize = true;

            txtAdminNotes.Location = new System.Drawing.Point(20, 50);
            txtAdminNotes.Size = new System.Drawing.Size(350, 100);
            txtAdminNotes.Multiline = true;
            txtAdminNotes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            btnSaveNote.Location = new System.Drawing.Point(270, 160);
            btnSaveNote.Size = new System.Drawing.Size(100, 36);
            btnSaveNote.Text = "Save Note";
            btnSaveNote.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            // Calendar Popup
            pnlCalendarPopup.Size = new System.Drawing.Size(320, 300);
            pnlCalendarPopup.Location = new System.Drawing.Point(400, 180);
            pnlCalendarPopup.Visible = false;
            pnlCalendarPopup.Controls.Add(flpCalendarDays);
            pnlCalendarPopup.Controls.Add(lblCalendarTitle);
            pnlCalendarPopup.Controls.Add(btnCalendarClose);

            lblCalendarTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblCalendarTitle.Height = 40;
            lblCalendarTitle.Text = "Purchase Calendar";
            lblCalendarTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            btnCalendarClose.Size = new System.Drawing.Size(30, 30);
            btnCalendarClose.Location = new System.Drawing.Point(280, 5);
            btnCalendarClose.Text = "X";

            flpCalendarDays.Dock = System.Windows.Forms.DockStyle.Fill;
            flpCalendarDays.Padding = new System.Windows.Forms.Padding(10);
            flpCalendarDays.AutoScroll = true;

            // CustomersView
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainerMain);
            Controls.Add(pnlTopHeader);
            Name = "CustomersView";
            Size = new System.Drawing.Size(1200, 800);

            pnlTopHeader.ResumeLayout(false);
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            pnlDirectory.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            pnlProfile.ResumeLayout(false);
            pnlProfileHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picProfile).EndInit();
            pnlHistoryHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            pnlDataIntelligence.ResumeLayout(false);
            pnlPreferences.ResumeLayout(false);
            pnlAdminNotes.ResumeLayout(false);
            pnlCalendarPopup.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTopHeader;
        private System.Windows.Forms.Label lblChevron;
        private System.Windows.Forms.Label lblViewName;
        private Guna.UI2.WinForms.Guna2Button btnAddCustomer;
        
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel pnlDirectory;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private System.Windows.Forms.Panel pnlFilters;
        private Guna.UI2.WinForms.Guna2Button btnFilterAll;
        private Guna.UI2.WinForms.Guna2Button btnFilterRegular;
        private Guna.UI2.WinForms.Guna2Button btnFilterNew;
        private System.Windows.Forms.FlowLayoutPanel flpCustomers;
        
        private System.Windows.Forms.Panel pnlProfile;
        private System.Windows.Forms.Panel pnlProfileHeader;
        private Guna.UI2.WinForms.Guna2CirclePictureBox picProfile;
        private System.Windows.Forms.Label lblProfileName;
        private System.Windows.Forms.Label lblProfileEmail;
        private System.Windows.Forms.Label lblProfilePhone;
        private Guna.UI2.WinForms.Guna2Chip chipStatus;
        private Guna.UI2.WinForms.Guna2Button btnEmail;
        private Guna.UI2.WinForms.Guna2Button btnEditProfile;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        
        private System.Windows.Forms.FlowLayoutPanel flpKPIs;
        
        private System.Windows.Forms.Panel pnlHistoryHeader;
        private System.Windows.Forms.Label lblHistoryTitle;
        private Guna.UI2.WinForms.Guna2Button btnViewOrders;
        
        private Guna.UI2.WinForms.Guna2DataGridView dgvHistory;
        
        private System.Windows.Forms.Panel pnlDataIntelligence;
        private Guna.UI2.WinForms.Guna2Panel pnlPreferences;
        private System.Windows.Forms.Label lblPrefTitle;
        private System.Windows.Forms.Label lblFavCatLabel;
        private Guna.UI2.WinForms.Guna2Chip lblFavCatValue;
        private System.Windows.Forms.Label lblModLabel;
        private Guna.UI2.WinForms.Guna2Chip lblModValue;
        private System.Windows.Forms.Label lblTimeLabel;
        private Guna.UI2.WinForms.Guna2Chip lblTimeValue;
        
        private Guna.UI2.WinForms.Guna2Panel pnlAdminNotes;
        private System.Windows.Forms.Label lblNotesTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtAdminNotes;
        private Guna.UI2.WinForms.Guna2Button btnSaveNote;
        
        private Guna.UI2.WinForms.Guna2Panel pnlCalendarPopup;
        private System.Windows.Forms.FlowLayoutPanel flpCalendarDays;
        private System.Windows.Forms.Label lblCalendarTitle;
        private Guna.UI2.WinForms.Guna2Button btnCalendarClose;
    }
}
