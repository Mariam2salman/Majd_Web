using Majd_Web.Models;

namespace Majd_Web.ViewModels
{
    public class AlumniPageViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public Post Post { get; set; }
    }
}
