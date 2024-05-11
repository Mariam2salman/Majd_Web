using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Majd_Web.Models
{
    public class AlumniExperience
    {
        public string Id { get; set; }
        [Required]
        public string ExperienceName { get; set; }
        [Required]
        public string ExperienceYear { get; set; }
        public string Descreption { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;




    }
}