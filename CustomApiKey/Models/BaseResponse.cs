namespace CustomApiKey.Models
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Runtime { get; set; }
        public List<Object> Data { get; set; }
        public int CountData { get; set; }
    }
}
