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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        modelBuilder.Entity<Product>().HasKey(t => t.Id);
        modelBuilder.Entity<Product>().Property(t => t.Price).HasColumnType("decimal").HasPrecision(18, 2);
        modelBuilder.Entity<Product>().HasOne(t => t.Category).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Customer>().HasKey(t => t.Id);
        modelBuilder.Entity<Customer>().HasOne(t => t.Country).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>().HasKey(t => t.Id);
        modelBuilder.Entity<Order>().Property(t => t.ItemsTotal).HasColumnType("decimal").HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(t => t.Discount).HasColumnType("decimal").HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(t => t.Total).HasColumnType("decimal").HasPrecision(18, 2);
        modelBuilder.Entity<Order>().HasMany(t => t.Items).WithOne(t => t.Order).IsRequired();
        modelBuilder.Entity<Order>().Navigation(t => t.Items).AutoInclude();
        modelBuilder.Entity<Order>().HasOne(t => t.Customer).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Order>().Navigation(t => t.Customer).AutoInclude();

        modelBuilder.Entity<OrderItem>().ToTable("OrderItems").HasKey(t => t.Id);
        modelBuilder.Entity<OrderItem>().Property(t => t.ProductPriceWhenOrdered).HasColumnType("decimal").HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(t => t.Total).HasColumnType("decimal").HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().HasOne(t => t.Product).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<OrderItem>().Navigation(t => t.Product).AutoInclude();

        modelBuilder.Entity<Country>().HasKey(t => t.Id);

        modelBuilder.Entity<Discount>().HasKey(t => t.Id);
        modelBuilder.Entity<Discount>().Property<string>("Discriminator");
        modelBuilder.Entity<Discount>().HasIndex("Discriminator");
        modelBuilder.Entity<Discount>().Property(t => t.Percentage).HasColumnType("decimal").HasPrecision(18, 2);
        modelBuilder.Entity<DiscountForCountry>().HasOne(t => t.Country).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction); ;
        modelBuilder.Entity<DiscountForCategory>().HasOne(t => t.Category).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Discount> Discounts { get; set; }
}
