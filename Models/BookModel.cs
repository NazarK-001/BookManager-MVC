using System.ComponentModel.DataAnnotations;

namespace BookManager.Models
{
    public class BookModel
    {
        [Key]  // Marks this property as the primary key
        public int Id { get; set; }  // Primary key column, automatically incremented

        [Required(ErrorMessage = "Title  is required.")]
        public string? Title { get; set; }

        public string? Author { get; set; }
        public string? Genre { get; set; }

    }
}
