namespace TheMatchaClubApp.Forms
{
    partial class ItemsView
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlGlobalSubHeader = new System.Windows.Forms.Panel();
            lblTotalItems = new System.Windows.Forms.Label();
            btnAddItem = new Guna.UI2.WinForms.Guna2Button();
            
            pnlBody = new System.Windows.Forms.Panel();
            pnlSidebar = new Guna.UI2.WinForms.Guna2Panel();
            lblCategoriesHeader = new System.Windows.Forms.Label();
            flpCategoryButtons = new System.Windows.Forms.FlowLayoutPanel();
            
            pnlRightArea = new System.Windows.Forms.Panel();
            pnlHeaderMain = new System.Windows.Forms.Panel();
            lblItemCount = new System.Windows.Forms.Label();
            cmbSort = new Guna.UI2.WinForms.Guna2ComboBox();
            
            flpItems = new System.Windows.Forms.FlowLayoutPanel();

            pnlGlobalSubHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlSidebar.SuspendLayout();
            pnlRightArea.SuspendLayout();
            pnlHeaderMain.SuspendLayout();
            SuspendLayout();

            // pnlGlobalSubHeader
            pnlGlobalSubHeader.Controls.Add(lblTotalItems);
            pnlGlobalSubHeader.Controls.Add(btnAddItem);
            pnlGlobalSubHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlGlobalSubHeader.Location = new System.Drawing.Point(0, 0);
            pnlGlobalSubHeader.Name = "pnlGlobalSubHeader";
            pnlGlobalSubHeader.Size = new System.Drawing.Size(1004, 56);
            pnlGlobalSubHeader.TabIndex = 0;



            // lblTotalItems
            lblTotalItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblTotalItems.Location = new System.Drawing.Point(760, 18);
            lblTotalItems.Name = "lblTotalItems";
            lblTotalItems.Size = new System.Drawing.Size(100, 20);
            lblTotalItems.Text = "Total: 124 Items";
            lblTotalItems.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // btnAddItem
            btnAddItem.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAddItem.Location = new System.Drawing.Point(880, 10);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new System.Drawing.Size(108, 36);
            btnAddItem.Text = "+ Add New";

            // pnlBody
            pnlBody.Controls.Add(pnlRightArea);
            pnlBody.Controls.Add(pnlSidebar);
            pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBody.Location = new System.Drawing.Point(0, 56);
            pnlBody.Name = "pnlBody";
            pnlBody.Size = new System.Drawing.Size(1004, 544);
            pnlBody.TabIndex = 1;

            // pnlSidebar
            pnlSidebar.Controls.Add(flpCategoryButtons);
            pnlSidebar.Controls.Add(lblCategoriesHeader);
            pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            pnlSidebar.Location = new System.Drawing.Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new System.Drawing.Size(220, 544);
            pnlSidebar.TabIndex = 0;

            // lblCategoriesHeader
            lblCategoriesHeader.Location = new System.Drawing.Point(20, 24);
            lblCategoriesHeader.Name = "lblCategoriesHeader";
            lblCategoriesHeader.Size = new System.Drawing.Size(180, 16);
            lblCategoriesHeader.Text = "Y CATEGORIES";

            // flpCategoryButtons
            flpCategoryButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flpCategoryButtons.Location = new System.Drawing.Point(12, 48);
            flpCategoryButtons.Name = "flpCategoryButtons";
            flpCategoryButtons.Padding = new System.Windows.Forms.Padding(4);
            flpCategoryButtons.Size = new System.Drawing.Size(208, 484);
            flpCategoryButtons.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            flpCategoryButtons.AutoScroll = true;
            flpCategoryButtons.WrapContents = false;

            // pnlRightArea
            pnlRightArea.Controls.Add(flpItems);
            pnlRightArea.Controls.Add(pnlHeaderMain);
            pnlRightArea.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlRightArea.Location = new System.Drawing.Point(220, 0);
            pnlRightArea.Name = "pnlRightArea";
            pnlRightArea.Size = new System.Drawing.Size(784, 544);
            pnlRightArea.TabIndex = 1;

            // pnlHeaderMain
            pnlHeaderMain.Controls.Add(lblItemCount);
            pnlHeaderMain.Controls.Add(cmbSort);
            pnlHeaderMain.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeaderMain.Location = new System.Drawing.Point(0, 0);
            pnlHeaderMain.Name = "pnlHeaderMain";
            pnlHeaderMain.Size = new System.Drawing.Size(784, 60);
            pnlHeaderMain.TabIndex = 0;

            // lblItemCount
            lblItemCount.Location = new System.Drawing.Point(20, 16);
            lblItemCount.Name = "lblItemCount";
            lblItemCount.Size = new System.Drawing.Size(200, 28);
            lblItemCount.Text = "All Items (6)";

            // cmbSort
            cmbSort.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cmbSort.Location = new System.Drawing.Point(624, 14);
            cmbSort.Name = "cmbSort";
            cmbSort.Size = new System.Drawing.Size(140, 32);


            // flpItems
            flpItems.AutoScroll = true;
            flpItems.Dock = System.Windows.Forms.DockStyle.Fill;
            flpItems.Location = new System.Drawing.Point(0, 60);
            flpItems.Name = "flpItems";
            flpItems.Padding = new System.Windows.Forms.Padding(12);
            flpItems.Size = new System.Drawing.Size(784, 484);
            flpItems.TabIndex = 1;

            // ItemsView
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlBody);
            Controls.Add(pnlGlobalSubHeader);
            Name = "ItemsView";
            Size = new System.Drawing.Size(1004, 600);

            pnlGlobalSubHeader.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            pnlSidebar.ResumeLayout(false);
            pnlRightArea.ResumeLayout(false);
            pnlHeaderMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlGlobalSubHeader;
        private System.Windows.Forms.Label lblTotalItems;
        private Guna.UI2.WinForms.Guna2Button btnAddItem;
        
        private System.Windows.Forms.Panel pnlBody;
        private Guna.UI2.WinForms.Guna2Panel pnlSidebar;
        private System.Windows.Forms.Label lblCategoriesHeader;
        private System.Windows.Forms.FlowLayoutPanel flpCategoryButtons;
        
        private System.Windows.Forms.Panel pnlRightArea;
        private System.Windows.Forms.Panel pnlHeaderMain;
        private System.Windows.Forms.Label lblItemCount;
        private Guna.UI2.WinForms.Guna2ComboBox cmbSort;
        private System.Windows.Forms.FlowLayoutPanel flpItems;
    }
}
