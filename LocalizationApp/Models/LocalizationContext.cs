using Microsoft.EntityFrameworkCore;

namespace LocalizationApp.Models
{
    public class LocalizationContext : DbContext
    {
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Resource> Resources { get; set; }

        public LocalizationContext(DbContextOptions<LocalizationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
