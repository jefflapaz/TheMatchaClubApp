using System;
using System.Windows.Forms;
using TheMatchaClubDomain.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class CartItemRow : UserControl
    {
        private CartLine _cartLine;
        
        public event EventHandler QtyChanged;
        public event EventHandler RemoveClicked;

        public CartItemRow(CartLine cartLine)
        {
            _cartLine = cartLine;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();
            BindData();
            WireEvents();
        }

        private void BindData()
        {
            if (_cartLine == null) return;
            lblName.Text = _cartLine.Product.Name;
            lblPrice.Text = $"@ {_cartLine.Product.Price.ToString("C2")}";
            txtQty.Text = _cartLine.Qty.ToString();
            lblTotal.Text = _cartLine.Total.ToString("C2");
        }

        private void WireEvents()
        {
            btnMinus.Click += (s, e) =>
            {
                if (_cartLine.Qty > 1)
                {
                    _cartLine.Qty--;
                    OnQtyChanged();
                }
                else
                {
                    RemoveClicked?.Invoke(this, EventArgs.Empty);
                }
            };

            btnPlus.Click += (s, e) =>
            {
                _cartLine.Qty++;
                OnQtyChanged();
            };

            btnRemove.Click += (s, e) => RemoveClicked?.Invoke(this, EventArgs.Empty);

            txtQty.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    UpdateQtyFromText();
                }
            };
            txtQty.LostFocus += (s, e) => UpdateQtyFromText();
        }

        private void UpdateQtyFromText()
        {
            if (int.TryParse(txtQty.Text, out int newQty) && newQty > 0)
            {
                _cartLine.Qty = newQty;
                OnQtyChanged();
            }
            else
            {
                txtQty.Text = _cartLine.Qty.ToString();
            }
        }

        private void OnQtyChanged()
        {
            txtQty.Text = _cartLine.Qty.ToString();
            lblTotal.Text = _cartLine.Total.ToString("C2");
            QtyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
