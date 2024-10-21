using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Data;

public class ApplicationDBContext : IdentityDbContext<AppUser>
{
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public ApplicationDBContext() { }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options) { }
}
