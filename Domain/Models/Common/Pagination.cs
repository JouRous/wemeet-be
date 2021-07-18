using System.Collections.Generic;

namespace Domain.Models
{
    public class Pagination<T>
    {
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public List<T> Items { get; set; }
        public double TotalPages { get; set; }
    }
}