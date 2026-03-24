using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class UserService : IUserService
{
    private IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    // NUEVA LÓGICA: Obtener usuario por ID
    public GetUserDto? GetUserById(int id)
    {
        User? user = userRepository.GetUserById(id);
        if (user == null) return null;

        return new GetUserDto(user.Id, user.Name, user.Email, user.Password, user.Rol);
    }

    public GetUserDto CreateUser(CreateUserDto createUserDto)
    {
        User newUser = new(0, createUserDto.Name, createUserDto.Email, createUserDto.Password, createUserDto.Rol);
        User createdUser = userRepository.CreateUser(newUser);
        return new GetUserDto(createdUser.Id, createdUser.Name, createdUser.Email, createdUser.Password, createdUser.Rol);
    }

    public GetUserDto UpdateUser(UpdateUserDto updateUserDto)
    {
        User userToUpdate = new(updateUserDto.Id, updateUserDto.Name, updateUserDto.Email, updateUserDto.Password, updateUserDto.Rol);
        User updatedUser = userRepository.UpdateUser(userToUpdate);
        return new GetUserDto(updatedUser.Id, updatedUser.Name, updatedUser.Email, updatedUser.Password, updatedUser.Rol);
    }

    public bool DeleteUser(int id)
    {
        return userRepository.DeleteUser(id);
    }
}