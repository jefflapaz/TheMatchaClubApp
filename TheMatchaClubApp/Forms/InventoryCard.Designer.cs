namespace TheMatchaClubApp.Forms
{
    partial class InventoryCard
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlCard = new Guna.UI2.WinForms.Guna2Panel();
            picImage = new Guna.UI2.WinForms.Guna2PictureBox();
            pnlStockBadge = new Guna.UI2.WinForms.Guna2Panel();
            lblStockStatus = new System.Windows.Forms.Label();
            lblProductId = new System.Windows.Forms.Label();
            lblPrice = new System.Windows.Forms.Label();
            lblName = new System.Windows.Forms.Label();
            lblInventoryLabel = new System.Windows.Forms.Label();
            lblStock = new System.Windows.Forms.Label();
            barStock = new Guna.UI2.WinForms.Guna2ProgressBar();
            pnlSeparator = new System.Windows.Forms.Panel();
            btnEdit = new Guna.UI2.WinForms.Guna2Button();
            btnDelete = new Guna.UI2.WinForms.Guna2Button();

            pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picImage).BeginInit();
            pnlStockBadge.SuspendLayout();
            SuspendLayout();

            // pnlCard
            pnlCard.Controls.Add(pnlStockBadge); // Ensure badge is on top
            pnlCard.Controls.Add(picImage);
            pnlCard.Controls.Add(lblProductId);
            pnlCard.Controls.Add(lblPrice);
            pnlCard.Controls.Add(lblName);
            pnlCard.Controls.Add(lblInventoryLabel);
            pnlCard.Controls.Add(lblStock);
            pnlCard.Controls.Add(barStock);
            pnlCard.Controls.Add(pnlSeparator);
            pnlCard.Controls.Add(btnEdit);
            pnlCard.Controls.Add(btnDelete);
            pnlCard.Location = new System.Drawing.Point(0, 0);
            pnlCard.Name = "pnlCard";
            pnlCard.Size = new System.Drawing.Size(240, 310);
            pnlCard.TabIndex = 0;

            // picImage
            picImage.Location = new System.Drawing.Point(0, 0);
            picImage.Name = "picImage";
            picImage.Size = new System.Drawing.Size(240, 150);
            picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            picImage.TabIndex = 0;
            picImage.TabStop = false;

            // pnlStockBadge
            pnlStockBadge.Controls.Add(lblStockStatus);
            pnlStockBadge.Location = new System.Drawing.Point(150, 10);
            pnlStockBadge.Name = "pnlStockBadge";
            pnlStockBadge.Size = new System.Drawing.Size(80, 22);
            pnlStockBadge.TabIndex = 1;

            // lblStockStatus
            lblStockStatus.AutoSize = true;
            lblStockStatus.Name = "lblStockStatus";
            lblStockStatus.Text = "IN STOCK";
            lblStockStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblProductId
            lblProductId.Location = new System.Drawing.Point(16, 160);
            lblProductId.Name = "lblProductId";
            lblProductId.Size = new System.Drawing.Size(80, 18);
            lblProductId.Text = "MT-001";

            // lblPrice
            lblPrice.Location = new System.Drawing.Point(144, 160);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new System.Drawing.Size(80, 18);
            lblPrice.Text = "$6.50";
            lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // lblName
            lblName.Location = new System.Drawing.Point(16, 180);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(208, 20);
            lblName.Text = "Ceremonial Grade Matcha";

            // lblInventoryLabel
            lblInventoryLabel.Location = new System.Drawing.Point(16, 210);
            lblInventoryLabel.Name = "lblInventoryLabel";
            lblInventoryLabel.Size = new System.Drawing.Size(70, 16);
            lblInventoryLabel.Text = "Inventory";

            // lblStock
            lblStock.Location = new System.Drawing.Point(144, 210);
            lblStock.Name = "lblStock";
            lblStock.Size = new System.Drawing.Size(80, 16);
            lblStock.Text = "45 units";
            lblStock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // barStock
            barStock.Location = new System.Drawing.Point(16, 230);
            barStock.Name = "barStock";
            barStock.Size = new System.Drawing.Size(208, 6);
            barStock.TabIndex = 7;

            // pnlSeparator
            pnlSeparator.Location = new System.Drawing.Point(0, 256);
            pnlSeparator.Name = "pnlSeparator";
            pnlSeparator.Size = new System.Drawing.Size(240, 1);
            pnlSeparator.TabIndex = 8;

            // btnEdit
            btnEdit.Location = new System.Drawing.Point(16, 268);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(160, 32);
            btnEdit.TabIndex = 9;
            btnEdit.Text = "✏ Edit";

            // btnDelete
            btnDelete.Location = new System.Drawing.Point(184, 268);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(40, 32);
            btnDelete.TabIndex = 10;
            btnDelete.Text = "🗑";

            // InventoryCard
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlCard);
            Name = "InventoryCard";
            Size = new System.Drawing.Size(240, 310);

            pnlCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picImage).EndInit();
            pnlStockBadge.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlCard;
        private Guna.UI2.WinForms.Guna2PictureBox picImage;
        private Guna.UI2.WinForms.Guna2Panel pnlStockBadge;
        private System.Windows.Forms.Label lblStockStatus;
        private System.Windows.Forms.Label lblProductId;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblInventoryLabel;
        private System.Windows.Forms.Label lblStock;
        private Guna.UI2.WinForms.Guna2ProgressBar barStock;
        private System.Windows.Forms.Panel pnlSeparator;
        private Guna.UI2.WinForms.Guna2Button btnEdit;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
    }
}
