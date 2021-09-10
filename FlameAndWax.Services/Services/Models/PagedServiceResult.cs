namespace FlameAndWax.Services.Services.Models
{
    public class PagedServiceResult<T> : ServiceResult<T>
    {
        /// <summary>
        /// Fetches the total product count of a certain generic type model
        /// then the total product count will then be divided by the Math.Ceiling of total number of products 
        /// in a page you want display.
        /// </summary>
        /// <value></value>
        public int TotalProductCount { get; set; }
        public int PageNumber { get; set; }
    }
}