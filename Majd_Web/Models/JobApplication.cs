using Majd_Web.Models;
using System;
namespace Majd_Web.Models
{
	public class JobApplication
	{
        public int Id { get; set; }
        public bool hasApplied { get; set; } = false;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public string JobId { get; set; }
        public Job Job { get; set; }
    }
}

