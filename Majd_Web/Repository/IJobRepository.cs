using Majd_Web.Models;

namespace Majd_Web.Repository
{
    public interface IJobRepository
    {

        Task<Job> GetByIdAsync(string id);
        IQueryable<Job> GetAllAsync();
        Task CreateAsync(Job job);
        Task DeleteAsync(string id);
    }
}
