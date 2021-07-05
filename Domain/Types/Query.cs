namespace Domain.Types
{
    public class Query<T>
    {
        public PaginationParams paginationParams { get; set; }
        public T filter { get; set; }
        public string sort { get; set; } = "";
    }
}