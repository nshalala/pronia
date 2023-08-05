using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.DataAccess
{
    public class ProniaDbContext:DbContext
    {
        public ProniaDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

    }
}
