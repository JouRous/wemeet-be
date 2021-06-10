
using API.DTO;
using API.Types;

namespace API.Interfaces
{
  public interface IResponseBuilder<T>
  {
    IResponseBuilder<T> AddData(T data);
    IResponseBuilder<T> AddPagination(PaginationDTO pagination);
    IResponseBuilder<T> AddHttpStatus(int statusCode, bool status);
    Response<T> Build();
  }
}