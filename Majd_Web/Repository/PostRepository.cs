using Majd_Web.Data;
using Majd_Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Majd_Web.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDBContext _db;

        public PostRepository(AppDBContext db)
        {
               _db = db;    
        }

       public async Task CreateAsync(Post post)
       {
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
           
       }

       public async  Task DeleteAsync(string id)
        {
            var result = await _db.Posts.FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _db.Posts.Remove(result);
                await _db.SaveChangesAsync();
            }
        }

       public async Task<Post> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
