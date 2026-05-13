using Microsoft.AspNetCore.Identity;

namespace TheMatchaClubApp.Core.Models
{
    /// <summary>
    /// Extended Identity user for the Matcha Club POS system.
    /// Adds FullName and DateCreated on top of the standard IdentityUser fields
    /// (UserName, Email, PasswordHash, etc.).
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Display name shown in the POS UI (e.g. sidebar, receipts).
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// UTC timestamp of when this account was created.
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
