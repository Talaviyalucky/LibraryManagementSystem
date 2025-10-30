using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Book
    {
        [Key]
        [Required]
        public int Id { get; set; } // Admin will enter this manually

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        // Navigation property for Borrow records
        public ICollection<Borrow>? Borrows { get; set; }
    }
}
