using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace TheMatchaClubApp.Forms
{
    /// <summary>
    /// Login form for the Matcha Club POS.
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
        }
    }
}
