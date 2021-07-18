using Domain.DTO;
using Domain.Interfaces;
using Domain.Types;

namespace Application.Utils
{
    public class ResponseWithPaginationBuilder<T> : IResponseWithPaginationBuilder<T>
    {
        private T data = default(T);
        private int statusCode = 200;
        private bool status = true;
        public PaginationDTO pagination { get; set; }

        public IResponseWithPaginationBuilder<T> AddData(T data)
        {
            this.data = data;

            return this;
        }

        public IResponseWithPaginationBuilder<T> AddHttpStatus(int statusCode, bool status)
        {
            this.status = status;
            this.statusCode = statusCode;

            return this;
        }

        public IResponseWithPaginationBuilder<T> AddPagination(PaginationDTO pagination)
        {
            this.pagination = pagination;

            return this;
        }

        public ResponseWithPagination<T> Build()
        {
            return new ResponseWithPagination<T>
            {
                Data = this.data,
                pagination = this.pagination,
                status = this.statusCode,
                success = this.status
            };
        }
    }
}