namespace Majd_Web.Models
{
    public class News
    {
        public int newsId { get; set; }
        public string newsContent { get; set; }
        public string UserId { get; set; } // Required foreign key property
        public ApplicationUser User { get; set; } = null!; // Req reference!d
    }
}
