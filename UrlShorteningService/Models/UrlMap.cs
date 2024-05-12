using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShorteningService.Models
{
    public class UrlMap
    {
            [Key]
            public int Id { get; set; } // Primary key

            [Required]
            [Url]
            public string OriginalUrl { get; set; } // Store the original URL

            [Required]
            public string ShortUrl { get; set; } // Store the shortened URL

            // Optionally, add a creation date
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    


}
