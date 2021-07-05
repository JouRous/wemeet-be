
using Domain.DTO;
using Domain.Types;

namespace Domain.Interfaces
{
    public interface IResponseBuilder<T>
    {
        IResponseBuilder<T> AddData(T data);
        IResponseBuilder<T> AddHttpStatus(int statusCode, bool status);
        Response<T> Build();
    }
}