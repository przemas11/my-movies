using System.ComponentModel.DataAnnotations;

namespace my_movies_backend.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required"), MaxLength(200, ErrorMessage = "Value for {0} must be less than {1} characters.")]
        public string Title { get; set; }

        [Range(1900, 2100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? ReleaseDate { get; set; }
    }
}
