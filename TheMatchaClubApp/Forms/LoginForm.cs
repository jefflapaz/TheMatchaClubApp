using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace TheMatchaClubApp.Forms
{
    /// <summary>
    /// Login form for S.I.P. POS.
    /// Accepts an IServiceProvider to resolve Identity services for authentication.
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public LoginForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InitializeComponent();
            DoubleBuffered = true;
            InitializeDesign();

            // Keyboard shortcuts for smoother login flow
            txtEmail.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    txtPassword.Focus();
                }
            };

            txtPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    BtnSignIn_Click(btnSignIn, EventArgs.Empty);
                }
            };
        }
    }
}
