namespace infrastructure;

using Microsoft.EntityFrameworkCore;
using core;

public class ChessDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    
    public ChessDbContext(DbContextOptions<ChessDbContext> options) : base(options)
    {

    }

}