using Core;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure;

public class DbInitializer
{
    public static void SeedDatabase(ChessDbContext dbContext)
    {
        if (dbContext.Users.Any()) return;
        
        var u1 = new User { UserName = "John Doe", Email = "JohnDoe@mail.com" };
        var u2 = new User { UserName = "Jane Doe", Email = "JaneDoe@mail.com" };
        var u3 = new User { UserName = "Michelangelo", Email = "Michelangelo@mail.com" };
        dbContext.AddRange([u1, u2, u3]);
        dbContext.SaveChanges();
    }
}