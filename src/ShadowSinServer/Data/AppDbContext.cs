using Microsoft.EntityFrameworkCore;
using ShadowSinServer.Models;

namespace ShadowSinServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
}
