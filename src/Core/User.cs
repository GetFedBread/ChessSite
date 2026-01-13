namespace Core;

using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<int>
{
    ICollection<Game> Games {get; set;} = new Collection<Game>();
}