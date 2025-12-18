namespace INFORCE_.NET_TASK.Models.ViewModels
{
    public class ShortUrlViewModel
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string CreatedByUsername { get; set; } = string.Empty;
        public bool CanDelete { get; set; }
    }
}
