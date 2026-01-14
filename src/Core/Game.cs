using System.Collections.ObjectModel;

namespace Core;

public class Game
{
    public int GameId {get; set;}
    public string StartingPosition {get; set;} = "";
    public ICollection<string> Moves {get; set;} = new Collection<string>();

    public int WhiteId {set; get;}
    public User? White {set; get;}
    public int BlackId {set; get;}
    public User? Black {set; get;}
}