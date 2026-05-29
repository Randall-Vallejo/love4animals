using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface IUserRepository
{
    public User? GetUserById(int id);
    public User? GetUserByEmail(string email);
    public User CreateUser(User user);
    public User UpdateUser(User user);
    public bool DeleteUser(int id);
}