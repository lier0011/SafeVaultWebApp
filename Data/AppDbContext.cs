using Microsoft.EntityFrameworkCore;
using SafeVaultWebApp.Models;

namespace SafeVaultWebApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
