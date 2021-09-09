namespace FlameAndWax.Services.Services.Models
{
    public class PagedServiceResult<T> : ServiceResult<T>
    {
        public int PerPage { get; set; }
        public int PageNumber { get; set; }
    }
}