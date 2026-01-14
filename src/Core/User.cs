namespace Core;

using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<int>
{
    public ICollection<Game> GamesAsBlack {get; set;} = new Collection<Game>();
    public ICollection<Game> GamesAsWhite {get; set;} = new Collection<Game>();
}