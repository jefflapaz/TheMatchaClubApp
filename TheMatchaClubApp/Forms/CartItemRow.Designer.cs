namespace TheMatchaClubApp.Forms
{
    partial class CartItemRow
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
            pnlContainer = new Guna.UI2.WinForms.Guna2Panel();
            btnMinus = new Guna.UI2.WinForms.Guna2Button();
            txtQty = new Guna.UI2.WinForms.Guna2TextBox();
            btnPlus = new Guna.UI2.WinForms.Guna2Button();
            lblName = new System.Windows.Forms.Label();
            lblPrice = new System.Windows.Forms.Label();
            lblTotal = new System.Windows.Forms.Label();
            btnRemove = new Guna.UI2.WinForms.Guna2Button();
            pnlContainer.SuspendLayout();
            SuspendLayout();

            // pnlContainer
            pnlContainer.Controls.Add(btnMinus);
            pnlContainer.Controls.Add(txtQty);
            pnlContainer.Controls.Add(btnPlus);
            pnlContainer.Controls.Add(lblName);
            pnlContainer.Controls.Add(lblPrice);
            pnlContainer.Controls.Add(lblTotal);
            pnlContainer.Controls.Add(btnRemove);
            pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlContainer.Location = new System.Drawing.Point(0, 0);
            pnlContainer.Name = "pnlContainer";
            pnlContainer.Size = new System.Drawing.Size(380, 52);
            pnlContainer.TabIndex = 0;

            // btnMinus
            btnMinus.Location = new System.Drawing.Point(8, 11);
            btnMinus.Name = "btnMinus";
            btnMinus.Size = new System.Drawing.Size(30, 30);
            btnMinus.Text = "\u2212";

            // txtQty
            txtQty.Location = new System.Drawing.Point(38, 11);
            txtQty.Name = "txtQty";
            txtQty.Size = new System.Drawing.Size(48, 30);
            txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            // btnPlus
            btnPlus.Location = new System.Drawing.Point(86, 11);
            btnPlus.Name = "btnPlus";
            btnPlus.Size = new System.Drawing.Size(30, 30);
            btnPlus.Text = "+";

            // lblName
            lblName.Location = new System.Drawing.Point(120, 6);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(140, 20);
            lblName.Text = "Product Name";

            // lblPrice
            lblPrice.Location = new System.Drawing.Point(120, 28);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new System.Drawing.Size(140, 16);
            lblPrice.Text = "@ \u20B10.00";

            // btnRemove
            btnRemove.Location = new System.Drawing.Point(265, 11);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new System.Drawing.Size(30, 30);
            btnRemove.Text = "\u2715";

            // lblTotal
            lblTotal.Location = new System.Drawing.Point(295, 0);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new System.Drawing.Size(80, 52);
            lblTotal.Text = "\u20B10.00";
            lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // CartItemRow
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pnlContainer);
            Name = "CartItemRow";
            Size = new System.Drawing.Size(380, 52);
            pnlContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlContainer;
        private Guna.UI2.WinForms.Guna2Button btnMinus;
        private Guna.UI2.WinForms.Guna2TextBox txtQty;
        private Guna.UI2.WinForms.Guna2Button btnPlus;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblTotal;
        private Guna.UI2.WinForms.Guna2Button btnRemove;
    }
}
