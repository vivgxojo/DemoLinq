using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoLinq.Models
{
    public class LieuDBContext : IdentityDbContext
    {
        public LieuDBContext(DbContextOptions <LieuDBContext> options)
        : base(options) { }

        public DbSet<Lieu> Lieux { get; set; }
    }
}
