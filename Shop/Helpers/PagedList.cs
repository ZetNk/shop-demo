using Microsoft.EntityFrameworkCore;

namespace Shop.API.Helpers;

public class PagedList<T> : List<T>
{
    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Totalcount = count;
        Pagesize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int) Math.Ceiling(count / (double) pageSize);
        AddRange(items);
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int Pagesize { get; set; }
    public int Totalcount { get; set; }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}