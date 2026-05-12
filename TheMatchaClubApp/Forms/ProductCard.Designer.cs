namespace TheMatchaClubApp.Forms
{
    partial class ProductCard
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlCard = new Guna.UI2.WinForms.Guna2Panel();
            picImage = new Guna.UI2.WinForms.Guna2PictureBox();
            pnlPriceBadge = new Guna.UI2.WinForms.Guna2Panel();
            lblPrice = new System.Windows.Forms.Label();
            lblName = new System.Windows.Forms.Label();
            lblCategory = new System.Windows.Forms.Label();

            pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picImage).BeginInit();
            pnlPriceBadge.SuspendLayout();
            SuspendLayout();

            // pnlCard
            pnlCard.Controls.Add(picImage);
            pnlCard.Controls.Add(pnlPriceBadge);
            pnlCard.Controls.Add(lblName);
            pnlCard.Controls.Add(lblCategory);
            pnlCard.Location = new System.Drawing.Point(0, 0);
            pnlCard.Name = "pnlCard";
            pnlCard.Size = new System.Drawing.Size(160, 190);
            pnlCard.TabIndex = 0;

            // picImage
            picImage.Location = new System.Drawing.Point(0, 0);
            picImage.Name = "picImage";
            picImage.Size = new System.Drawing.Size(160, 120);
            picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            picImage.TabIndex = 0;
            picImage.TabStop = false;

            // pnlPriceBadge
            pnlPriceBadge.Controls.Add(lblPrice);
            pnlPriceBadge.Location = new System.Drawing.Point(104, 6);
            pnlPriceBadge.Name = "pnlPriceBadge";
            pnlPriceBadge.Size = new System.Drawing.Size(52, 22);
            pnlPriceBadge.TabIndex = 1;

            // lblPrice
            lblPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            lblPrice.Location = new System.Drawing.Point(0, 0);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new System.Drawing.Size(52, 22);
            lblPrice.TabIndex = 0;
            lblPrice.Text = "$0.00";
            lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblName
            lblName.Location = new System.Drawing.Point(8, 126);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(144, 32);
            lblName.TabIndex = 2;
            lblName.Text = "Product Name";

            // lblCategory
            lblCategory.Location = new System.Drawing.Point(8, 158);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new System.Drawing.Size(144, 18);
            lblCategory.TabIndex = 3;
            lblCategory.Text = "CATEGORY";

            // ProductCard
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlCard);
            Name = "ProductCard";
            Size = new System.Drawing.Size(160, 190);

            pnlCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picImage).EndInit();
            pnlPriceBadge.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlCard;
        private Guna.UI2.WinForms.Guna2PictureBox picImage;
        private Guna.UI2.WinForms.Guna2Panel pnlPriceBadge;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCategory;
    }
}
