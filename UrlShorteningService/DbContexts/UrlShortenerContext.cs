using Microsoft.EntityFrameworkCore;
using UrlShorteningService.Models;

namespace UrlShorteningService.DbContexts
{
    public class UrlShortenerContext : DbContext
    {
            public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options)
                : base(options)
            {
            }

            public DbSet<UrlMap> UrlMaps { get; set; }
        }
}
