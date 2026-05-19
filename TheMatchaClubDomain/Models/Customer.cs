using System;
using System.Collections.Generic;

namespace TheMatchaClubDomain.Models
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty; // +63 format
        public DateTime MemberSince { get; set; } = DateTime.Now;
        public string Status { get; set; } = "New"; // Regular, New
        public string AdminNotes { get; set; } = string.Empty;
        public string ProfileImagePath { get; set; } = string.Empty;
    }
}
