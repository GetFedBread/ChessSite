namespace Infrastructure;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core;

public class ChessDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Game> Games { get; set; }
    
    public ChessDbContext(DbContextOptions<ChessDbContext> options) : base(options)
    {

    }

}