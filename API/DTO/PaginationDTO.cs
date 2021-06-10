
namespace API.DTO
{
  public class PaginationDTO
  {
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
  }
}