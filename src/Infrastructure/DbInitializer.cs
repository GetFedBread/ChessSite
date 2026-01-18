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

        var g1 = new Game { 
            Active = false, 
            Black = u1, 
            White = u2,
            StartDate = new DateTime(2025, 7, 5, 8, 12, 15), 
            StartingPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 
            Moves = [
                "e2e4",
                "e7e5",
                "d1h5",
                "b8c6",
                "f1c4",
                "g8f6",
                "h5f7#"
            ]
        };

        //u1.GamesAsBlack.Add(g1);
        //u2.GamesAsWhite.Add(g1);

        dbContext.AddRange([g1]);
        dbContext.AddRange([u1, u2, u3]);
        dbContext.SaveChanges();
    }
}