using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TheMatchaClubDomain.Models;
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

            // Load image or generate placeholder (Async)
            picImage.Image = null;
            string path = _product.ImagePath;
            string name = _product.Name;
            int w = picImage.Width > 0 ? picImage.Width : 200;
            int h = picImage.Height > 0 ? picImage.Height : 140;

            System.Threading.Tasks.Task.Run(() =>
            {
                var img = ImageHelper.LoadOrPlaceholder(path, name, w, h, false);
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() => { picImage.Image = img; }));
                }
                else
                {
                    this.HandleCreated += (s, e) =>
                    {
                        this.BeginInvoke(new Action(() => { picImage.Image = img; }));
                    };
                }
            });
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
