namespace LibrarySystem.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int StatusCode { get; internal set; }
        public string Message { get; internal set; } = string.Empty;
    }
}
