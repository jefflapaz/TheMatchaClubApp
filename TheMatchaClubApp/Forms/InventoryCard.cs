using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Models;
using TheMatchaClubApp.Helpers;

namespace TheMatchaClubApp.Forms
{
    public partial class InventoryCard : UserControl
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

        public InventoryCard()
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
            lblStock.Text = $"{_product.StockLevel} units";
            barStock.Value = Math.Min(100, _product.StockLevel);

            bool outOfStock = _product.StockLevel == 0 || _product.IsOutOfStock;
            bool lowStock = !outOfStock && _product.StockLevel <= 5;

            // Dim text if out of stock
            if (outOfStock)
            {
                lblProductId.ForeColor = ColorTranslator.FromHtml("#D1D5DB");
                lblPrice.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
                lblName.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
                lblInventoryLabel.ForeColor = ColorTranslator.FromHtml("#D1D5DB");
            }
            else
            {
                lblProductId.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
                lblPrice.ForeColor = ColorTranslator.FromHtml("#111827");
                lblName.ForeColor = ColorTranslator.FromHtml("#111827");
                lblInventoryLabel.ForeColor = ColorTranslator.FromHtml("#9CA3AF");
            }

            if (outOfStock)
            {
                lblStockStatus.Text = "OUT OF STOCK";
                pnlStockBadge.FillColor = ColorTranslator.FromHtml("#FEE2E2");
                lblStockStatus.ForeColor = ColorTranslator.FromHtml("#DC2626");
                barStock.FillColor = ColorTranslator.FromHtml("#EF4444");
                lblStock.ForeColor = ColorTranslator.FromHtml("#EF4444");
            }
            else if (lowStock)
            {
                lblStockStatus.Text = "LOW STOCK";
                pnlStockBadge.FillColor = ColorTranslator.FromHtml("#FEEBC8");
                lblStockStatus.ForeColor = ColorTranslator.FromHtml("#D97706");
                barStock.FillColor = ColorTranslator.FromHtml("#F59E0B");
                lblStock.ForeColor = ColorTranslator.FromHtml("#F59E0B");
            }
            else
            {
                lblStockStatus.Text = "IN STOCK";
                pnlStockBadge.FillColor = ColorTranslator.FromHtml("#52B743");
                lblStockStatus.ForeColor = Color.White;
                barStock.FillColor = ColorTranslator.FromHtml("#52B743");
                lblStock.ForeColor = ColorTranslator.FromHtml("#6B7280");
            }

            // Dynamic Badge Width
            int paddingX = 8;
            int paddingY = 4;
            pnlStockBadge.Width = lblStockStatus.PreferredWidth + (paddingX * 2);
            pnlStockBadge.Height = lblStockStatus.PreferredHeight + (paddingY * 2);
            pnlStockBadge.BorderRadius = pnlStockBadge.Height / 2; // Perfect Pill
            
            // Center text inside badge
            lblStockStatus.Location = new Point(paddingX, paddingY);
            
            // Align badge to the top-right of the card image
            pnlStockBadge.Location = new Point(this.Width - pnlStockBadge.Width - 10, 10);

            // Async Image Load
            picImage.Image = null; // Clear old image
            int w = picImage.Width;
            int h = picImage.Height;
            string path = _product.ImagePath;
            string name = _product.Name;
            bool dimImage = outOfStock;

            System.Threading.Tasks.Task.Run(() =>
            {
                var img = ImageHelper.LoadOrPlaceholder(path, name, w, h, dimImage);
                if (!this.IsDisposed && this.IsHandleCreated)
                {
                    this.Invoke(new Action(() => { picImage.Image = img; }));
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
