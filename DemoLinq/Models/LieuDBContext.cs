using Microsoft.EntityFrameworkCore;

namespace DemoLinq.Models
{
    public class LieuDBContext : DbContext
    {
        public LieuDBContext(DbContextOptions <LieuDBContext> options)
        : base(options) { }

        public DbSet<Lieu> Lieux { get; set; }
    }
}
