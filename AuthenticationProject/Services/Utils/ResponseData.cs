namespace AuthenticationProject.Services.Utils
{
    public class ResponseData<T>
    {
        
        public bool Success { get; set; }
        public T? Value { get; set; }
        public string? Error { get; set; }

    }
}
