namespace TheMatchaClubApp.Core.Models
{
    public class StoreSettings
    {
        public string StoreName { get; set; } = "The Matcha Club";
        public string StoreLogoPath { get; set; } = string.Empty;
        public string Email { get; set; } = "info@thematchaclub.ph";
        public string SmtpPassword { get; set; } = string.Empty;
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string Phone { get; set; } = "+63 912 345 6789";
        public string Address { get; set; } = "Makati City, Metro Manila";
        public bool IsDarkMode { get; set; } = false;
    }
}
