using Microsoft.AspNetCore.Mvc;
using UrlShortener.Dto;
using System.Text.RegularExpressions;
using UrlShortener.Entities;

namespace UrlShortener.Controllers;

[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private static string BaseUrl = "http://www.samplesite.com";

    public UrlShortenerController(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }


    private string GenerateToken()
    {
        Random random = new Random();
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string uniqueString = new string(Enumerable.Repeat(characters, 6)
                                                    .Select(s => s[random.Next(s.Length)])
                                                    .ToArray());
        return uniqueString;
    }


    private bool isValidURL(string str)
    {
        string strRegex = @"((http|https)://)(www.)?" +
            "[a-zA-Z0-9@:%._\\+~#?&//=]" +
            "{2,256}\\.[a-z]" +
            "{2,6}\\b([-a-zA-Z0-9@:%" +
            "._\\+~#?&//=]*)";
        Regex re = new Regex(strRegex);
        if (re.IsMatch(str))
            return true;
        else
            return false;
    }


    [HttpPost]
    [Route("/shorten")]
    public ActionResult<UrlShortenResponseDto> Shorten([FromBody] UrlShortenRequestDto dto)
    {
        if (!this.isValidURL(dto.Url))
        {
            return BadRequest(new { Error = "Invalid URL" });
        }

        // check if url already shortened
        var existingUrl = _dbContext.Urls.FirstOrDefault(r => r.OriginalUrl == dto.Url);
        if (existingUrl != null)
        {
            return new UrlShortenResponseDto()
            {
                ShortenedUrl = existingUrl.ShortenedUrl
            };
        }


        string shortenedUrl , token;

        var existingTokenObj = _dbContext.Urls.Where(r => r.UrlToken == dto.CustomToken).FirstOrDefault();
        if (!string.IsNullOrEmpty(dto.CustomToken) && dto.CustomToken.Length <= 6 && existingTokenObj == null)
        {
            shortenedUrl = $"{BaseUrl}/{dto.CustomToken}";
            token = dto.CustomToken;
            if (!this.isValidURL(shortenedUrl))
            {
                token = this.GenerateToken();
                shortenedUrl = $"{BaseUrl}/{token}";
            }
        } else {
            token = this.GenerateToken();
            shortenedUrl = $"{BaseUrl}/{token}";
        }

        Url urlRecord = new Url(){
            OriginalUrl = dto.Url,
            ShortenedUrl = shortenedUrl,
            UrlToken = token
        };

        UrlShortenResponseDto response = new UrlShortenResponseDto(){
            ShortenedUrl = shortenedUrl
        };

        _dbContext.Urls.Add(urlRecord);
        _dbContext.SaveChanges();
        return response;
    }


    [HttpGet]
    [Route("/{token?}")]
    public ActionResult RedirectToOriginalUrl(string? token)
    {
        var ClientIPAddr = HttpContext.Connection.RemoteIpAddress?.ToString();
        var urlObj = _dbContext.Urls.FirstOrDefault(r => r.UrlToken == token);
        if (urlObj == null)
        {
            return BadRequest(new { Error = "Invalid URL" });
        }

        UrlVisit urlVisit = new UrlVisit()
        {
            ShortenedUrl = urlObj,
            VisitorIPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            VisitorUserAgent = Request.Headers["User-Agent"],
            VisitorReferer = Request.Headers["Referer"],
        };
        _dbContext.UrlVisits.Add(urlVisit);
        _dbContext.SaveChanges();

        return Redirect(urlObj.OriginalUrl);
    }
}