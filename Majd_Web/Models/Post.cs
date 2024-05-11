using System.ComponentModel.DataAnnotations;

namespace Majd_Web.Models
{
    public class Post
    {
        
        public string Id { get; set; }
        [Required(ErrorMessage = "Post content is required.")]
        [StringLength(2000, ErrorMessage = "Post content must be within 2000 characters.")]
        public string PostContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime PostDate { get; set; } = DateTime.Now;
        public int LikeCount { get; set; } = 0;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
