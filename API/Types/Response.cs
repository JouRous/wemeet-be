using API.Models;

namespace API.Types
{
  public class Response<T>
  {
    public int status { get; set; }
    public T Data { get; set; }
    public Pagination<T> pagination { get; set; }
    public bool success { get; set; }
  }
}