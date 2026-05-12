using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using TheMatchaClubApp.Core.Services;

namespace TheMatchaClubApp
{
    internal static class Program
    {
        public static JsonDataService DataService { get; } = new JsonDataService();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var culture = new CultureInfo("en-PH");
            culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            culture.DateTimeFormat.LongDatePattern = "dd MMMM yyyy";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Forms.LoginForm());
        }
    }
}