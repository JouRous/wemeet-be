using Domain.DTO;

namespace Domain.Types
{
    public class ResponseWithPagination<T> : Response<T>
    {
        public PaginationDTO pagination { get; set; }
    }
}