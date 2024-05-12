using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShorteningService.Models
{
    public class UrlMap
    {
            [Key]
            public int Id { get; set; }

            [Required]
            [Url]
            public string OriginalUrl { get; set; }

            [Required]
            public string ShortUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    


}
