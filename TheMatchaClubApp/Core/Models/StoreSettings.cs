namespace TheMatchaClubApp.Core.Models
{
    public class StoreSettings
    {
        public string StoreName { get; set; } = "The Matcha Club";
        public string StoreLogoPath { get; set; } = string.Empty;
        public string Email { get; set; } = "info@thematchaclub.ph";
        public string Phone { get; set; } = "+63 912 345 6789";
        public string Address { get; set; } = "Makati City, Metro Manila";
        public bool IsDarkMode { get; set; } = false;
    }
}
