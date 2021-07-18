using Domain.DTO;
using Domain.Types;

namespace Domain.Interfaces
{
    public interface IResponseWithPaginationBuilder<T>
    {
        IResponseWithPaginationBuilder<T> AddPagination(PaginationDTO pagination);
        IResponseWithPaginationBuilder<T> AddData(T data);
        IResponseWithPaginationBuilder<T> AddHttpStatus(int statusCode, bool status);
        ResponseWithPagination<T> Build();
    }
}