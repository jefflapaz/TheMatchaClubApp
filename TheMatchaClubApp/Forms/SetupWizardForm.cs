using System;
using System.Drawing;
using System.Windows.Forms;

namespace TheMatchaClubApp.Forms
{
    public partial class SetupWizardForm : Form
    {
        private int _currentStep = 1;

        public SetupWizardForm()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            InitializeComponent();
            InitializeDesign();

            UpdateStepper();
            SwapStepContent();
        }

        private void BtnNext_Click(object? s, EventArgs e)
        {
            if (_currentStep < 4)
            {
                _currentStep++;
                UpdateStepper();
                SwapStepContent();
            }
        }

        private void BtnBack_Click(object? s, EventArgs e)
        {
            if (_currentStep > 1)
            {
                _currentStep--;
                UpdateStepper();
                SwapStepContent();
            }
        }

        private void BtnComplete_Click(object? s, EventArgs e)
        {
            this.Close();
        }

        private void SwapStepContent()
        {
            pnlStep1.Visible = _currentStep == 1;
            pnlStep2.Visible = _currentStep == 2;
            pnlStep3.Visible = _currentStep == 3;
            pnlStep4.Visible = _currentStep == 4;

            btnBack.Enabled = _currentStep > 1;
            btnNext.Visible = _currentStep < 4;
            btnComplete.Visible = _currentStep == 4;

            lblStepIndicator.Text = $"Step {_currentStep} of 4";
            lblProgressPercent.Text = $"{_currentStep * 25}%";

            // Update step header
            string[] titles = { "Store Identity", "Product Setup", "Security Configuration", "Review & Launch" };
            string[] descs = {
                "Set up your store's basic information",
                "Configure your product catalog",
                "Set up manager access controls",
                "Review your configuration"
            };
            lblStepTitle.Text = titles[_currentStep - 1];
            lblStepDesc.Text = descs[_currentStep - 1];
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }
    }
}
