using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface IUserRepository
{
    // Cambiado: Busca un usuario por ID
    public User? GetUserById(int id); 
    public User CreateUser(User user);
    public User UpdateUser(User user);
    public bool DeleteUser(int id);
}