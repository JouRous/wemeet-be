
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
        PageSize = pageSize == 0 ? 10 : pageSize,
        CurrentPage = pageNumber == 0 ? 1 : pageNumber,
      };

      int skip = (pagination.CurrentPage - 1) * pagination.PageSize;

      pagination.Items = await query.Skip(skip).Take(pagination.PageSize).ToListAsync();

      return pagination;
    }
  }
}