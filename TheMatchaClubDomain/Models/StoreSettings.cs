namespace TheMatchaClubDomain.Models
{
    public class StoreSettings
    {
        // ── Store Profile ────────────────────────────────────────────
        public string StoreName { get; set; } = "S.I.P.";
        public string StoreLogoPath { get; set; } = string.Empty;
        public string Email { get; set; } = "info@thematchaclub.ph";
        public string Phone { get; set; } = "+63 912 345 6789";
        public string Address { get; set; } = "Makati City, Metro Manila";
        public string ReceiptFooterMessage { get; set; } = "Thank you for your purchase!";
        public string PopupLocationName { get; set; } = string.Empty;
        public string CurrentOperatingLocation { get; set; } = string.Empty;
        public string CashierName { get; set; } = string.Empty;

        // ── Email / SMTP ─────────────────────────────────────────────
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string SmtpPassword { get; set; } = string.Empty;

        // ── Session & Cash ───────────────────────────────────────────
        public decimal DefaultStartingCash { get; set; } = 200m;
        public bool RequireCashCountOnClose { get; set; } = true;
        public bool EnableOverShortWarnings { get; set; } = true;
        public bool AutoGenerateZReport { get; set; } = true;
        public int SessionTimeoutMinutes { get; set; } = 0; // 0 = disabled
        public bool AutoLockQuickSaleIfNoSession { get; set; } = true;

        // ── Receipt Configuration ─────────────────────────────────
        public bool ReceiptShowCashierName { get; set; } = true;
        public bool ReceiptShowCustomerName { get; set; } = true;
        public bool ReceiptShowOrderType { get; set; } = true;
        public bool ReceiptShowSessionNumber { get; set; } = false;
        public string ReceiptPaperWidth { get; set; } = "80mm";


        // ── Customer Classification Thresholds ───────────────────────
        // New: 1 order, Regular: 2–7, Loyal: 8–15, Frequent: 16+ or lifetime ≥ threshold
        public int CustomerTierRegularMin { get; set; } = 2;
        public int CustomerTierLoyalMin { get; set; } = 8;
        public int CustomerTierFrequentMin { get; set; } = 16;
        public decimal CustomerTierFrequentSpend { get; set; } = 7500m;

        // ── Security Settings ───────────────────────────────────────
        public bool RequirePasswordForDeleteProduct { get; set; } = false;
        public bool RequirePasswordForDeleteOrder { get; set; } = false;
        public bool RequirePasswordForCloseSession { get; set; } = false;
        public bool RequirePasswordForSettings { get; set; } = false;
        public int AutoLockMinutes { get; set; } = 0; // 0 = Never
        public DateTime? LastPasswordChangeDate { get; set; } = null;
    }
}
