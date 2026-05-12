using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TheMatchaClubApp.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class CustomOrderTable : UserControl
    {
        private List<MockOrder> _orders = new();
        private int _selectedIndex = -1;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public List<MockOrder> Orders
        {
            get => _orders;
            set { _orders = value ?? new(); _selectedIndex = _orders.Count > 0 ? 0 : -1; Invalidate(); }
        }

        public MockOrder? SelectedOrder =>
            _selectedIndex >= 0 && _selectedIndex < _orders.Count ? _orders[_selectedIndex] : null;

        public event EventHandler<MockOrder>? OrderSelected;

        public CustomOrderTable()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            int headerH = 40;
            int rowH = 56;
            if (e.Y < headerH) return;

            int idx = (e.Y - headerH) / rowH;
            if (idx >= 0 && idx < _orders.Count)
            {
                _selectedIndex = idx;
                Invalidate();
                OrderSelected?.Invoke(this, _orders[idx]);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
