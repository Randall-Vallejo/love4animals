using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using System.Linq;

namespace Love4AnimalsApi.Services;

public class UserService : IUserService
{
    private IUserRepository userRepository;
    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    public List<GetUserDto> GetAllUsers()
    {
        List<User> users = userRepository.GetAllUsers();
        List<GetUserDto> response = users.Select(u => new GetUserDto(u.Id, u.Name, u.Email, u.Password, u.Rol)).ToList();
        return response;
    }
    public GetUserDto CreateUser(CreateUserDto createUserDto)
    {
        User newUser = new(0, createUserDto.Name, createUserDto.Email, createUserDto.Password, createUserDto.Rol);
        User createdUser = userRepository.CreateUser(newUser);
        GetUserDto response = new(createdUser.Id, createdUser.Name, createdUser.Email, createdUser.Password, createdUser.Rol);
        return response;
    }
    public GetUserDto UpdateUser(UpdateUserDto updateUserDto)
    {
        User userToUpdate = new(updateUserDto.Id, updateUserDto.Name, updateUserDto.Email, updateUserDto.Password, updateUserDto.Rol);
        User updatedUser = userRepository.UpdateUser(userToUpdate);
        GetUserDto response = new(updatedUser.Id, updatedUser.Name, updatedUser.Email, updatedUser.Password, updatedUser.Rol);
        return response;
    }
    public bool DeleteUser(int id)
    {
        return userRepository.DeleteUser(id);
    }
}
