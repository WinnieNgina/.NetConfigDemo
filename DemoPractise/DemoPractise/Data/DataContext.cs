using DemoPractise.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoPractise.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }

}
