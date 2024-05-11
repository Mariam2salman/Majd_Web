using Majd_Web.Data;
using Majd_Web.Models;

namespace Majd_Web.Repository
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly AppDBContext _db;

        public ExperienceRepository(AppDBContext db)
        {
            _db = db;

        }
        public async Task CreateAsync(AlumniExperience Experience)
        {
            await _db.AlumniExperiences.AddAsync(Experience);
            await _db.SaveChangesAsync();

        }
    }
}