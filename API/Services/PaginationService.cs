
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
      var totalPages = (double)query.Count() / (pageSize == 0 ? 10 : pageSize);
      Pagination<T> pagination = new Pagination<T>
      {
        Total = query.Count(),
        PerPage = pageSize == 0 ? 10 : pageSize,
        CurrentPage = pageNumber == 0 ? 1 : pageNumber,
        TotalPages = Math.Ceiling(totalPages)
      };


      int skip = (pagination.CurrentPage - 1) * pagination.PerPage;

      var items = await query.Skip(skip).Take(pagination.PerPage).ToListAsync();
      pagination.Items = items;
      pagination.Count = items.Count;

      return pagination;
    }
  }
}