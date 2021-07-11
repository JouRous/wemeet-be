
using Domain.DTO;
using Domain.Types;

namespace Domain.Interfaces
{
    public interface IResponseBuilder<T>
    {
        IResponseBuilder<T> AddData(T data);
        IResponseBuilder<T> AddHttpStatus(int statusCode, bool status);
        IResponseBuilder<T> AddMessage(string message);
        Response<T> Build();
    }
}