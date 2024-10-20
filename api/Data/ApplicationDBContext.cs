using api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Data;

public class ApplicationDBContext : DbContext
{
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public ApplicationDBContext() { }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options) { }
}
