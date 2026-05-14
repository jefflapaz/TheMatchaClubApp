namespace TheMatchaClubApp.Forms
{
    partial class ProductItemCard
    {
        private System.ComponentModel.IContainer? components = null;

        private void InitializeComponent()
        {
            pnlCard = new Guna.UI2.WinForms.Guna2Panel();
            picImage = new Guna.UI2.WinForms.Guna2PictureBox();
            lblProductId = new System.Windows.Forms.Label();
            lblPrice = new System.Windows.Forms.Label();
            lblName = new System.Windows.Forms.Label();
            pnlSeparator = new System.Windows.Forms.Panel();
            btnEdit = new Guna.UI2.WinForms.Guna2Button();
            btnDelete = new Guna.UI2.WinForms.Guna2Button();

            pnlCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picImage).BeginInit();
            SuspendLayout();

            // pnlCard
            pnlCard.Controls.Add(picImage);
            pnlCard.Controls.Add(lblProductId);
            pnlCard.Controls.Add(lblPrice);
            pnlCard.Controls.Add(lblName);
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

            // pnlSeparator
            pnlSeparator.Location = new System.Drawing.Point(0, 256);
            pnlSeparator.Name = "pnlSeparator";
            pnlSeparator.Size = new System.Drawing.Size(240, 1);
            pnlSeparator.TabIndex = 8;
            pnlSeparator.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5E7EB");

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

            // ProductItemCard
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlCard);
            Name = "ProductItemCard";
            Size = new System.Drawing.Size(240, 310);

            pnlCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picImage).EndInit();
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlCard;
        private Guna.UI2.WinForms.Guna2PictureBox picImage;
        private System.Windows.Forms.Label lblProductId;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Panel pnlSeparator;
        private Guna.UI2.WinForms.Guna2Button btnEdit;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
    }
}
