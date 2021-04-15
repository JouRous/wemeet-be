
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
  public class PaginationService
  {
    public static async Task<Pagination<T>> GetPagination<T>(IQueryable<T> query, int pageNumber, int pageSize) where T : class
    {
      Pagination<T> pagination = new Pagination<T>
      {
        TotalItems = query.Count(),
        PageSize = pageSize,
        CurrentPage = pageNumber,
      };

      int skip = (pageNumber - 1) * pageSize;

      pagination.Items = await query.Skip(skip).Take(pageSize).ToListAsync();

      return pagination;
    }
  }
}