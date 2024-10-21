using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Data;

public class ApplicationDBContext : IdentityDbContext<AppUser>
{
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        List<IdentityRole> roles =
            new()
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }

    public ApplicationDBContext() { }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options) { }
}
