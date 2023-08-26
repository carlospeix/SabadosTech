using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model1;
using System.Reflection.Metadata;

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        modelBuilder.Entity<Product>().HasKey(t => t.Id);

        modelBuilder.Entity<Customer>().HasKey(t => t.Id);
        modelBuilder.Entity<Customer>().HasOne(t => t.Country).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>().HasKey(t => t.Id);
        modelBuilder.Entity<Order>().HasMany(t => t.Items).WithOne(t => t.Order).IsRequired();
        modelBuilder.Entity<Order>().Navigation(t => t.Items).AutoInclude();
        modelBuilder.Entity<Order>().HasOne(t => t.Customer).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Order>().Navigation(t => t.Customer).AutoInclude();

        modelBuilder.Entity<OrderItem>().ToTable("OrderItems").HasKey(t => t.Id);
        modelBuilder.Entity<OrderItem>().HasOne(t => t.Product).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<OrderItem>().Navigation(t => t.Product).AutoInclude();

        modelBuilder.Entity<Country>().HasKey(t => t.Id);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Country> Countries { get; set; }
}
