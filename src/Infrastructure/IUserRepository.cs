namespace Infrastructure;

public interface IUserRepository
{
    public Task<UserDTO?> GetUserByName(string username);
}