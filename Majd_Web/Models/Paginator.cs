using Microsoft.EntityFrameworkCore;

namespace Majd_Web.Models
{
    public class Paginator<T> : List<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; private set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }


        public Paginator(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public bool PreviousPage => (PageIndex > 1);
        public bool NextPage => (PageIndex < TotalPages);


        public static async Task<Paginator<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync(); //total number of record in the DB.      
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Paginator<T>(items, count, pageIndex, pageSize);

        }
    }
}
