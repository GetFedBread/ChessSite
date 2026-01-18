using System.Collections.ObjectModel;

namespace Infrastructure;

public class UserDTO
{
    public required string Username;
    public List<GameDTO>? Games = [];
}