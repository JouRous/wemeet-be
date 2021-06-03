using System.Collections.Generic;

namespace API.Models
{
  public class Pagination<T>
  {
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public List<T> Items { get; set; }
  }
}