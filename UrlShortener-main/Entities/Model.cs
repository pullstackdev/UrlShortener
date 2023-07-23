using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace UrlShortener.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<Url> Urls { get; set; }
        public DbSet<UrlVisit> UrlVisits { get; set; }

        public string DbPath { get; }

        public AppDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "url_shortener.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }




    public class Url
    {
        [Key]
        public int Id { get; set; }        
        public string OriginalUrl { get; set; }
        public string UrlToken { get; set; }
        public string ShortenedUrl { get; set; }
        public virtual ICollection<UrlVisit> UrlVisits { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    }

    public class UrlVisit
    {
        [Key]
        public int Id { get; set; }        
        public string? VisitorIPAddress { get; set; }
        public string? VisitorUserAgent { get; set; }
        public string? VisitorReferer { get; set; }
        public virtual Url ShortenedUrl { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
