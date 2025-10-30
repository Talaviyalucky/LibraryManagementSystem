using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Borrow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; } = null!;

        // The user who borrowed it
        [Required]
        public string UserEmail { get; set; } = null!;

        // Optional numeric user ID if you have one
        public string? UserId { get; set; }

        [Required]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        // Null means "not yet returned"
        public DateTime? ReturnDate { get; set; }
    }
}
