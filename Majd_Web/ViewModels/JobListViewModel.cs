using Majd_Web.Models;

namespace Majd_Web.ViewModels
{
    public class JobListViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<Job> Jobs { get; set; }

    }
}
