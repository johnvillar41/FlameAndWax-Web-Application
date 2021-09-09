namespace FlameAndWax.Services.Services.Models
{
    public class PagedServiceResult<T> : ServiceResult<T>
    {
        public int TotalProductCount { get; set; }
        public int PageNumber { get; set; }
    }
}