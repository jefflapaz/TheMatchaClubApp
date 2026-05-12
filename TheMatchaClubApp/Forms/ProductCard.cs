using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;
using TheMatchaClubApp.Helpers;

namespace TheMatchaClubApp.Forms
{
    public partial class ProductCard : UserControl
    {
        private Product? _product;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Product? ProductData
        {
            get => _product;
            set
            {
                _product = value;
                if (value != null) BindData();
            }
        }

        public event EventHandler<Product>? ProductClicked;

        public ProductCard()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();
        }

        private void BindData()
        {
            if (_product == null) return;
            lblName.Text = _product.Name;
            lblCategory.Text = _product.CategoryName;
            lblPrice.Text = _product.Price.ToString("C2");

            // Stock check — disable if out of stock
            if (_product.StockLevel <= 0 || _product.IsOutOfStock)
            {
                this.Enabled = false;
                lblCategory.Text = "OUT OF STOCK";
                lblCategory.ForeColor = ColorTranslator.FromHtml("#EF4444");
            }

            // Load image or generate placeholder
            picImage.Image = ImageHelper.LoadOrPlaceholder(_product.ImagePath, _product.Name, picImage.Width);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_product != null)
                ProductClicked?.Invoke(this, _product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hoverTimer?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
