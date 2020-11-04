using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tes3mp_verifier.Data.Queries
{
  public class Paginate<T>
  {
    private int Page;
    private int PageSize;
    private int Overflow = 0;

    public Paginate(int page, int pageSize)
    {
      Page = page;
      PageSize = pageSize;
    }

    public Paginate(int page, int pageSize, int overflow)
    {
      Page = page;
      PageSize = pageSize;
      Overflow = overflow;
    }

    public async Task<List<T>> ToListAsync(IQueryable<T> source)
    {
      return await source
        .Skip((Page - 1) * PageSize)
        .Take(PageSize + Overflow)
        .ToListAsync();
    }
  }
}
