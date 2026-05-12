using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class NavItem : UserControl
    {
        private string _iconKey = "";
        private bool _isActive;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string IconKey
        {
            get => _iconKey;
            set { _iconKey = value; pnlIcon.Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string LabelText
        {
            get => lblText.Text;
            set => lblText.Text = value;
        }

        public bool IsActive => _isActive;

        public event EventHandler? NavClicked;

        public NavItem()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            InitializeComponent();
            InitializeDesign();
        }

        public void SetActive(bool active)
        {
            _isActive = active;
            ApplyState();
        }

        internal void RaiseClick() => NavClicked?.Invoke(this, EventArgs.Empty);
    }
}
