using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model1;

namespace Mapping1;

public class ApplicationContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Source\SabadosTech\Data\Mappings.mdf;Integrated Security=True")
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(t => t.Id);

        modelBuilder.Entity<Customer>().HasKey(t => t.Id);

        modelBuilder.Entity<Order>().HasKey(t => t.Id);
        modelBuilder.Entity<Order>().HasMany(t => t.Items).WithOne(t => t.Order).IsRequired();
        modelBuilder.Entity<Order>().HasOne(t => t.Customer).WithMany().IsRequired();

        modelBuilder.Entity<OrderItem>().ToTable("OrderItems").HasKey(t => t.Id);
        modelBuilder.Entity<OrderItem>().HasOne(t => t.Product).WithMany().IsRequired();
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
}
