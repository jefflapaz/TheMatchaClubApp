using System;

namespace TheMatchaClubDomain.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
