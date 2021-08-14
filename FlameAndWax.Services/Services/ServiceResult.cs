namespace FlameAndWax.Services.Services
{
    public class ServiceResult<T>
    {
        public T Result { get; set; }
        public bool HasError { get; set; }
        public string ErrorContent { get; set; }
    }
}
