namespace Majd_Web.Models
{
    public class Major
    {
        public string Id { get; set; }
        public string MajorName { get; set; }
        public ICollection<ApplicationUser> Students { get; } = new List<ApplicationUser>();
    }
}
