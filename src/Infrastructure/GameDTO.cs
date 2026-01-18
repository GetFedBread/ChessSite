namespace Infrastructure;

public class GameDTO
{
    public UserDTO? Black;
    public UserDTO? White;
    public required int GameId;
    public required bool Active;
    public required DateTime StartDate;
    public required string StartingPosition;
    public required ICollection<string> Moves;
}