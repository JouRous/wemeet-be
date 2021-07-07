using Domain.DTO;
using Domain.Models;

namespace Domain.Types
{
    public class Response<T>
    {
        public int status { get; set; }
        public T Data { get; set; }
        public bool success { get; set; }
        public string Message { get; set; }
    }
}