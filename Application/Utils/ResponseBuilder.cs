using Domain.DTO;
using Domain.Interfaces;
using Domain.Types;


namespace Application.Utils
{
    public class ResponseBuilder<T> : IResponseBuilder<T>
    {
        private T data = default(T);
        private int statusCode = 200;
        private bool status = true;
        private string message = "Ok";

        public IResponseBuilder<T> AddData(T data)
        {
            this.data = data;
            return this;
        }

        public IResponseBuilder<T> AddHttpStatus(int statusCode, bool status)
        {
            this.status = status;
            this.statusCode = statusCode;
            return this;
        }

        public IResponseBuilder<T> AddMessage(string message)
        {
            this.message = message;
            return this;
        }

        public Response<T> Build()
        {
            return new Response<T>
            {
                Data = this.data,
                status = this.statusCode,
                success = this.status,
                Message = this.message
            };
        }


    }
}