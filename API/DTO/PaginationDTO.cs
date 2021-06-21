
namespace API.DTO
{
  public class PaginationDTO
  {
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public double TotalPage { get; set; }
    public int Count { get; set; }
  }
}