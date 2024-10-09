using DA.Entites;
using Microsoft.EntityFrameworkCore;

namespace DA;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions<AppContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}