using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShorteningService.DbContexts;
using UrlShorteningService.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace UrlShorteningService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlsController : ControllerBase
    {

            private readonly UrlShortenerContext _context;

        public UrlsController(UrlShortenerContext context, IConfiguration configuration)
        {
                _context = context;

        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] string originalUrl)
        {
            if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out Uri validatedUrl))
            {
                return BadRequest("Invalid URL format.");
            }

            string baseUrl = ExtractBaseUrl(validatedUrl);

            var existingMap = await _context.UrlMaps
                .Where(x => x.OriginalUrl == originalUrl)
                .Select(x => x.ShortUrl)
                .FirstOrDefaultAsync();

            if (existingMap != null)
            {
                return Ok(new { ShortUrl = $"{baseUrl}/{existingMap}" });
            }

            string shortCode = GenerateShortUrl();
            string newShortUrl = $"{baseUrl}/{shortCode}";

            _context.UrlMaps.Add(new UrlMap { OriginalUrl = originalUrl, ShortUrl = shortCode });
            await _context.SaveChangesAsync();

            return Created(newShortUrl, new { ShortUrl = newShortUrl });
        }

        private string GenerateShortUrl()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());  
        }
        private string ExtractBaseUrl(Uri url)
        {
            var host = url.Host; 
            int index = host.LastIndexOf(".com");
            if (index > 0)
            {
                return $"https://{host.Substring(0, index )}"; 
            }
            return $"https://{host}";
        }
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
        {
            var urlMap = await _context.UrlMaps.FirstOrDefaultAsync(u => u.ShortUrl == shortCode);

            if (urlMap == null)
            {
                return NotFound("Short URL not found.");
            }

            return Ok(urlMap.OriginalUrl);
        }
        [HttpPost("custom")]
        public async Task<IActionResult> CreateCustomShortUrl([FromBody] CustomerUrlRequest request)
        {
            if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out Uri validatedUrl))
            {
                return BadRequest("Invalid URL format.");
            }

            if (await _context.UrlMaps.AnyAsync(u => u.ShortUrl == request.CustomShortUrl))
            {
                return Conflict("This custom short URL is already in use.");
            }

            _context.UrlMaps.Add(new UrlMap { OriginalUrl = request.OriginalUrl, ShortUrl = request.CustomShortUrl });
            await _context.SaveChangesAsync();

            string newShortUrl = $"{Request.Scheme}://{Request.Host}/{request.CustomShortUrl}";
            return Created(newShortUrl, new { ShortUrl = newShortUrl });
        }
    }
}
