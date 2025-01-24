namespace ShortherUrlCore.Models
{
    public class ShortUrl
    {
        public required string OriginalUrl { get; set; }
        public required string ShortnedUrl { get; set; }
    }
}
