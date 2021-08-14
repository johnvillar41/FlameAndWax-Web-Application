namespace FlameAndWax.Services.Services
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public bool HasError { get; set; }
        public string ErrorContent { get; set; }
    }
}
