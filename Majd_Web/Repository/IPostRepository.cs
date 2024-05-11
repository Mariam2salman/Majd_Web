using Majd_Web.Models;

namespace Majd_Web.Repository
{
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(string id);
        Task CreateAsync(Post post);
        Task DeleteAsync(string id);
    }
}
