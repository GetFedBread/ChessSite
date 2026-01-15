namespace Infrastructure;

public interface IUserRepository
{
    public Task<UserDTO?> GetUserByName(string username);
    public Task<List<UserDTO>> GetUsers(int count, int offset, string? search);
}