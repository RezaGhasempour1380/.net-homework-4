using Microsoft.EntityFrameworkCore; 
using mainclass; 
namespace dbcontext {

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=db1.db");
}
}