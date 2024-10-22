using AdApi;
using Microsoft.EntityFrameworkCore;

public class AdDbContext : DbContext
{
    public AdDbContext(DbContextOptions<AdDbContext> options) : base(options) { }

    public DbSet<Ad> Ads { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Category> Categories { get; set; }
}
