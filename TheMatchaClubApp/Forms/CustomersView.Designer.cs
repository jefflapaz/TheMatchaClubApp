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
            btnEmail = new Guna.UI2.WinForms.Guna2Button();
            btnEditProfile = new Guna.UI2.WinForms.Guna2Button();
            btnExport = new Guna.UI2.WinForms.Guna2Button();
            
            flpKPIs = new System.Windows.Forms.FlowLayoutPanel();
            
            pnlHistoryHeader = new System.Windows.Forms.Panel();
            lblHistoryTitle = new System.Windows.Forms.Label();
            pnlHistoryFilters = new System.Windows.Forms.Panel();
            txtHistorySearch = new Guna.UI2.WinForms.Guna2TextBox();
            cmbDateFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            dtpCustomDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            
            dgvHistory = new Guna.UI2.WinForms.Guna2DataGridView();
            
            pnlDataIntelligence = new System.Windows.Forms.Panel();
            pnlPreferences = new Guna.UI2.WinForms.Guna2Panel();
            lblPrefTitle = new System.Windows.Forms.Label();
            lblFavCatLabel = new System.Windows.Forms.Label();
            lblFavCatValue = new System.Windows.Forms.Label();
            lblModLabel = new System.Windows.Forms.Label();
            lblModValue = new System.Windows.Forms.Label();
            lblTimeLabel = new System.Windows.Forms.Label();
            lblTimeValue = new System.Windows.Forms.Label();
            cmbSort = new Guna.UI2.WinForms.Guna2ComboBox();
            pnlSearchRow = new System.Windows.Forms.Panel();
            
            pnlAdminNotes = new Guna.UI2.WinForms.Guna2Panel();
            lblNotesTitle = new System.Windows.Forms.Label();
            txtAdminNotes = new Guna.UI2.WinForms.Guna2TextBox();
            btnSaveNote = new Guna.UI2.WinForms.Guna2Button();
            pnlNotesBtnRow = new System.Windows.Forms.Panel();
            
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
            pnlTopHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTopHeader.Size = new System.Drawing.Size(1200, 64);

            lblChevron.Location = new System.Drawing.Point(16, 20);
            lblChevron.Size = new System.Drawing.Size(16, 24);
            lblChevron.Text = "\u25B6";

            lblViewName.Location = new System.Drawing.Point(34, 18);
            lblViewName.Size = new System.Drawing.Size(120, 28);
            lblViewName.Text = "Customers";

            // Split Container
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.SplitterDistance = 350;
            splitContainerMain.SplitterWidth = 2;
            splitContainerMain.Panel1.Controls.Add(pnlDirectory);
            splitContainerMain.Panel2.Controls.Add(pnlProfile);

            // Directory Panel
            pnlDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlDirectory.Padding = new System.Windows.Forms.Padding(12);
            pnlDirectory.Controls.Add(flpCustomers);
            pnlDirectory.Controls.Add(pnlFilters);
            pnlDirectory.Controls.Add(pnlSearchRow);

            // Search Row (search + sort dropdown side by side)
            pnlSearchRow.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSearchRow.Height = 40;
            pnlSearchRow.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);

            txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSearch.Height = 36;
            txtSearch.PlaceholderText = "Search by name, email or phone...";

            cmbSort.Dock = System.Windows.Forms.DockStyle.Right;
            cmbSort.Width = 100;
            cmbSort.Items.AddRange(new object[] { "A \u2192 Z", "Z \u2192 A", "Newest", "Oldest" });
            cmbSort.SelectedIndex = 0;
            cmbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            pnlSearchRow.Controls.Add(txtSearch);
            pnlSearchRow.Controls.Add(cmbSort);

            pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            pnlFilters.Height = 36;
            pnlFilters.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            pnlFilters.Controls.Add(btnFilterNew);
            pnlFilters.Controls.Add(btnFilterRegular);
            pnlFilters.Controls.Add(btnFilterAll);

            btnFilterAll.Width = 80;
            btnFilterAll.Dock = System.Windows.Forms.DockStyle.Left;
            btnFilterAll.Text = "All";
            
            btnFilterRegular.Width = 100;
            btnFilterRegular.Dock = System.Windows.Forms.DockStyle.Left;
            btnFilterRegular.Text = "Regular";
            btnFilterRegular.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);

            btnFilterNew.Width = 80;
            btnFilterNew.Dock = System.Windows.Forms.DockStyle.Left;
            btnFilterNew.Text = "New";
            btnFilterNew.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);

            flpCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            flpCustomers.AutoScroll = true;
            flpCustomers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpCustomers.WrapContents = false;

            // Profile Panel
            pnlProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlProfile.Padding = new System.Windows.Forms.Padding(24, 24, 24, 16);
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
            flpKPIs.Height = 110;
            flpKPIs.Padding = new System.Windows.Forms.Padding(0, 10, 0, 6);
            flpKPIs.WrapContents = true;

            // History Header
            pnlHistoryHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHistoryHeader.Height = 80;
            pnlHistoryHeader.Controls.Add(pnlHistoryFilters);
            pnlHistoryHeader.Controls.Add(lblHistoryTitle);

            lblHistoryTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblHistoryTitle.Height = 30;
            lblHistoryTitle.Text = "Purchase History";
            lblHistoryTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

            pnlHistoryFilters.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHistoryFilters.Height = 40;
            pnlHistoryFilters.Padding = new System.Windows.Forms.Padding(0, 6, 0, 4);
            pnlHistoryFilters.Controls.Add(txtHistorySearch);
            pnlHistoryFilters.Controls.Add(dtpCustomDate);
            pnlHistoryFilters.Controls.Add(cmbDateFilter);

            txtHistorySearch.Dock = System.Windows.Forms.DockStyle.Fill;
            txtHistorySearch.Height = 32;
            txtHistorySearch.PlaceholderText = "Search items or order ID...";

            cmbDateFilter.Dock = System.Windows.Forms.DockStyle.Right;
            cmbDateFilter.Width = 140;
            cmbDateFilter.Items.AddRange(new object[] { "All Time", "Today", "This Week", "This Month", "Custom Date" });
            cmbDateFilter.SelectedIndex = 0;
            cmbDateFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            dtpCustomDate.Dock = System.Windows.Forms.DockStyle.Right;
            dtpCustomDate.Width = 150;
            dtpCustomDate.Visible = false;
            dtpCustomDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // History DataGrid
            dgvHistory.Dock = System.Windows.Forms.DockStyle.Top;
            dgvHistory.Height = 200;
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;

            // Data Intelligence
            pnlDataIntelligence.Dock = System.Windows.Forms.DockStyle.Top;
            pnlDataIntelligence.Height = 200;
            pnlDataIntelligence.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            pnlDataIntelligence.Controls.Add(pnlAdminNotes);
            pnlDataIntelligence.Controls.Add(pnlPreferences);

            pnlPreferences.Dock = System.Windows.Forms.DockStyle.Left;
            pnlPreferences.Width = 320;
            pnlPreferences.Controls.Add(lblPrefTitle);
            pnlPreferences.Controls.Add(lblFavCatLabel);
            pnlPreferences.Controls.Add(lblFavCatValue);
            pnlPreferences.Controls.Add(lblModLabel);
            pnlPreferences.Controls.Add(lblModValue);
            pnlPreferences.Controls.Add(lblTimeLabel);
            pnlPreferences.Controls.Add(lblTimeValue);

            lblPrefTitle.Location = new System.Drawing.Point(20, 15);
            lblPrefTitle.Text = "CUSTOMER INSIGHTS";
            lblPrefTitle.AutoSize = true;

            lblFavCatLabel.Location = new System.Drawing.Point(20, 50);
            lblFavCatLabel.Text = "Favorite Category";
            lblFavCatLabel.AutoSize = true;
            lblFavCatValue.Location = new System.Drawing.Point(160, 50);
            lblFavCatValue.AutoSize = true;

            lblModLabel.Location = new System.Drawing.Point(20, 80);
            lblModLabel.Text = "Favorite Item";
            lblModLabel.AutoSize = true;
            lblModValue.Location = new System.Drawing.Point(160, 80);
            lblModValue.AutoSize = true;

            lblTimeLabel.Location = new System.Drawing.Point(20, 110);
            lblTimeLabel.Text = "Typical Visit Time";
            lblTimeLabel.AutoSize = true;
            lblTimeValue.Location = new System.Drawing.Point(160, 110);
            lblTimeValue.AutoSize = true;

            pnlAdminNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAdminNotes.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            pnlAdminNotes.Controls.Add(txtAdminNotes);
            pnlAdminNotes.Controls.Add(pnlNotesBtnRow);
            pnlAdminNotes.Controls.Add(lblNotesTitle);

            lblNotesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            lblNotesTitle.Height = 28;
            lblNotesTitle.Text = "ADMIN NOTES";
            lblNotesTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;

            pnlNotesBtnRow.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlNotesBtnRow.Height = 40;
            pnlNotesBtnRow.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            pnlNotesBtnRow.Controls.Add(btnSaveNote);

            btnSaveNote.Dock = System.Windows.Forms.DockStyle.Right;
            btnSaveNote.Width = 100;
            btnSaveNote.Text = "Save Note";

            txtAdminNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            txtAdminNotes.Multiline = true;

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
        private Guna.UI2.WinForms.Guna2Button btnEmail;
        private Guna.UI2.WinForms.Guna2Button btnEditProfile;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        
        private System.Windows.Forms.FlowLayoutPanel flpKPIs;
        
        private System.Windows.Forms.Panel pnlHistoryHeader;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.Panel pnlHistoryFilters;
        private Guna.UI2.WinForms.Guna2TextBox txtHistorySearch;
        private Guna.UI2.WinForms.Guna2ComboBox cmbDateFilter;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpCustomDate;
        
        private Guna.UI2.WinForms.Guna2DataGridView dgvHistory;
        
        private System.Windows.Forms.Panel pnlDataIntelligence;
        private Guna.UI2.WinForms.Guna2Panel pnlPreferences;
        private System.Windows.Forms.Label lblPrefTitle;
        private System.Windows.Forms.Label lblFavCatLabel;
        private System.Windows.Forms.Label lblFavCatValue;
        private System.Windows.Forms.Label lblModLabel;
        private System.Windows.Forms.Label lblModValue;
        private System.Windows.Forms.Label lblTimeLabel;
        private System.Windows.Forms.Label lblTimeValue;
        private Guna.UI2.WinForms.Guna2ComboBox cmbSort;
        private System.Windows.Forms.Panel pnlSearchRow;
        
        private Guna.UI2.WinForms.Guna2Panel pnlAdminNotes;
        private System.Windows.Forms.Label lblNotesTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtAdminNotes;
        private Guna.UI2.WinForms.Guna2Button btnSaveNote;
        private System.Windows.Forms.Panel pnlNotesBtnRow;
        
        private Guna.UI2.WinForms.Guna2Panel pnlCalendarPopup;
        private System.Windows.Forms.FlowLayoutPanel flpCalendarDays;
        private System.Windows.Forms.Label lblCalendarTitle;
        private Guna.UI2.WinForms.Guna2Button btnCalendarClose;
    }
}
