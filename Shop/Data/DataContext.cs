using Microsoft.EntityFrameworkCore;
using Shop.API.Models;

namespace Shop.API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<CartItem> CartItems { get; set; }
}