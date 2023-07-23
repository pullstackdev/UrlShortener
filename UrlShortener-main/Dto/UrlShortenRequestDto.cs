namespace UrlShortener.Dto
{
    public class UrlShortenRequestDto
    {
        public string Url { get; set; }
        public string? CustomToken { get; set; }
    }
}
