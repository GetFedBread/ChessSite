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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Game>(entity =>
        {
            entity.HasKey(g => new { g.BlackId, g.WhiteId });

            entity.HasOne(g => g.Black)
                .WithMany(u => u.GamesAsBlack)
                .HasForeignKey(g => g.BlackId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(g => g.White)
                .WithMany(u => u.GamesAsWhite)
                .HasForeignKey(g => g.WhiteId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}