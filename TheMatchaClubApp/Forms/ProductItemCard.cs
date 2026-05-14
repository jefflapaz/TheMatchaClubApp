using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;
using TheMatchaClubApp.Helpers;

namespace TheMatchaClubApp.Forms
{
    public partial class ProductItemCard : UserControl
    {
        private Product? _product;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Product? Product
        {
            get => _product;
            set
            {
                _product = value;
                if (value != null) BindData();
            }
        }

        public event EventHandler? EditClicked;
        public event EventHandler? DeleteClicked;

        public ProductItemCard()
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
            lblProductId.Text = _product.Id.ToString()[..8].ToUpper();
            lblPrice.Text = _product.Price.ToString("C2");
            lblName.Text = _product.Name;

            lblProductId.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
            lblPrice.ForeColor = ColorTranslator.FromHtml("#111827");
            lblName.ForeColor = ColorTranslator.FromHtml("#111827");

            // Async Image Load
            picImage.Image = null;
            string path = _product.ImagePath;
            string name = _product.Name;

            // Ensure we have dimensions. If not yet laid out, use designer defaults.
            int w = picImage.Width > 0 ? picImage.Width : 240;
            int h = picImage.Height > 0 ? picImage.Height : 150;

            System.Threading.Tasks.Task.Run(() =>
            {
                var img = ImageHelper.LoadOrPlaceholder(path, name, w, h, false);
                
                // Use a safe way to update the UI
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() => { picImage.Image = img; }));
                }
                else
                {
                    // Handle not created yet, wait for it
                    this.HandleCreated += (s, e) =>
                    {
                        this.BeginInvoke(new Action(() => { picImage.Image = img; }));
                    };
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
