namespace TheMatchaClubApp.Forms
{
    partial class CategoryNavItem
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnCategory = new Guna.UI2.WinForms.Guna2Button();
            pnlActions = new System.Windows.Forms.Panel();
            btnEdit = new Guna.UI2.WinForms.Guna2Button();
            btnCancel = new Guna.UI2.WinForms.Guna2Button();
            pnlActions.SuspendLayout();
            SuspendLayout();
            // 
            // btnCategory
            // 
            btnCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCategory.Location = new System.Drawing.Point(0, 0);
            btnCategory.Name = "btnCategory";
            btnCategory.Size = new System.Drawing.Size(196, 40);
            btnCategory.TabIndex = 0;
            btnCategory.Text = "Category Name";
            // 
            // pnlActions
            // 
            pnlActions.Controls.Add(btnCancel);
            pnlActions.Controls.Add(btnEdit);
            pnlActions.Dock = System.Windows.Forms.DockStyle.Right;
            pnlActions.Location = new System.Drawing.Point(100, 0);
            pnlActions.Name = "pnlActions";
            pnlActions.Size = new System.Drawing.Size(96, 40);
            pnlActions.TabIndex = 1;
            pnlActions.Visible = false;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(4, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(42, 32);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "✕";
            // 
            // btnEdit
            // 
            btnEdit.Location = new System.Drawing.Point(50, 4);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(42, 32);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "✎";
            // 
            // CategoryNavItem
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlActions);
            Controls.Add(btnCategory);
            Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            Name = "CategoryNavItem";
            Size = new System.Drawing.Size(196, 40);
            pnlActions.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Button btnCategory;
        private System.Windows.Forms.Panel pnlActions;
        private Guna.UI2.WinForms.Guna2Button btnEdit;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
    }
}
