using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using TheMatchaClubApp.Core.Models;

namespace TheMatchaClubApp.Forms
{
    public partial class CategoryNavItem : UserControl
    {
        private bool _isActive;
        public Category Category { get; private set; }
        public string CategoryName => Category.Name;
        public bool IsProtected { get; private set; }

        public event EventHandler? CategoryClicked;
        public event EventHandler? EditClicked;

        private Point _mouseDownLocation;
        private bool _isSwiping;

        public CategoryNavItem(Category category, bool isProtected = false)
        {
            InitializeComponent();
            Category = category;
            IsProtected = isProtected;

            btnCategory.Text = Category.Name;
            StyleControls();
            
            btnCategory.MouseDown += BtnCategory_MouseDown;
            btnCategory.MouseMove += BtnCategory_MouseMove;
            btnCategory.MouseUp += BtnCategory_MouseUp;

            btnCancel.Click += (s, e) => HideActions();
            btnEdit.Click += (s, e) => EditClicked?.Invoke(this, EventArgs.Empty);
        }

        private void StyleControls()
        {
            this.BackColor = Color.Transparent;

            btnCategory.BorderRadius = 4;
            btnCategory.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            btnCategory.Cursor = Cursors.Hand;
            btnCategory.TextAlign = HorizontalAlignment.Left;
            btnCategory.TextOffset = new Point(8, 0);
            btnCategory.BorderThickness = 0;
            btnCategory.FillColor = Color.Transparent;
            btnCategory.ForeColor = ColorTranslator.FromHtml("#6B7280");
            btnCategory.Image = CreateChevronImage(ColorTranslator.FromHtml("#6B7280"));
            btnCategory.ImageAlign = HorizontalAlignment.Right;
            btnCategory.ImageOffset = new Point(8, 0);

            // Action buttons
            btnCancel.BorderRadius = 4;
            btnCancel.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnCancel.FillColor = ColorTranslator.FromHtml("#94A3B8"); // Slate
            btnCancel.ForeColor = Color.White;
            btnCancel.Cursor = Cursors.Hand;

            btnEdit.BorderRadius = 4;
            btnEdit.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnEdit.FillColor = ColorTranslator.FromHtml("#3B82F6"); // Blue
            btnEdit.ForeColor = Color.White;
            btnEdit.Cursor = Cursors.Hand;
            
            pnlActions.BackColor = Color.Transparent;
        }

        public void SetActive(bool active)
        {
            _isActive = active;
            if (active)
            {
                btnCategory.FillColor = ColorTranslator.FromHtml("#52B743"); // Green
                btnCategory.ForeColor = Color.White;
                btnCategory.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                btnCategory.Image = CreateChevronImage(Color.White);
            }
            else
            {
                btnCategory.FillColor = Color.Transparent;
                btnCategory.ForeColor = ColorTranslator.FromHtml("#6B7280");
                btnCategory.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                btnCategory.Image = CreateChevronImage(ColorTranslator.FromHtml("#6B7280"));
                HideActions(); // hide if deselected
            }
        }

        private void BtnCategory_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseDownLocation = e.Location;
                _isSwiping = false;
            }
        }

        private void BtnCategory_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = Math.Abs(e.X - _mouseDownLocation.X);
                int dy = Math.Abs(e.Y - _mouseDownLocation.Y);

                if (!_isSwiping && (dx > 8 || dy > 8))
                {
                    if (dx > dy)
                    {
                        _isSwiping = true;
                    }
                    else if (!IsProtected)
                    {
                        // Start Drag
                        this.DoDragDrop(this, DragDropEffects.Move);
                    }
                }
            }
        }

        private void BtnCategory_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_isSwiping)
            {
                // Swipe detected
                if (e.X < _mouseDownLocation.X - 20)
                {
                    // Swipe Left -> Show Actions
                    ShowActions();
                }
                else if (e.X > _mouseDownLocation.X + 20)
                {
                    // Swipe Right -> Hide Actions
                    HideActions();
                }
            }
            else
            {
                // Normal click
                CategoryClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ShowActions()
        {
            if (IsProtected) return;
            pnlActions.Visible = true;
            // Hide the chevron when actions are shown
            btnCategory.Image = null;
        }

        public void HideActions()
        {
            pnlActions.Visible = false;
            // Restore chevron
            btnCategory.Image = CreateChevronImage(_isActive ? Color.White : ColorTranslator.FromHtml("#6B7280"));
        }

        private Image CreateChevronImage(Color color)
        {
            var bmp = new Bitmap(12, 12);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(color, 1.5f);
                g.DrawLine(pen, 2, 4, 6, 8);
                g.DrawLine(pen, 6, 8, 10, 4);
            }
            return bmp;
        }
    }
}
