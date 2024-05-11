using Majd_Web.Models;

namespace Majd_Web.Repository
{
    public interface IExperienceRepository
    {
        Task CreateAsync(AlumniExperience Experience);

    }
}