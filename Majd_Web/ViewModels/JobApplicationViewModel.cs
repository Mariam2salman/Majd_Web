using Majd_Web.Models;

namespace Majd_Web.ViewModels
{
    public class JobApplicationViewModel
    {
        public IQueryable<JobApplication> JobApplications { get; internal set; }

        public Job Job { get; set; }
    }
}
