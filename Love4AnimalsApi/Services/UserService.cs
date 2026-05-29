using BCrypt.Net;
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

    public GetUserDto? GetUserById(int id)
    {
        User? user = userRepository.GetUserById(id);
        if (user == null) return null;

        return new GetUserDto(user.Id, user.Name, user.Email, user.Rol);
    }

    public GetUserDto CreateUser(CreateUserDto createUserDto)
    {
        var existing = userRepository.GetUserByEmail(createUserDto.Email);
        if (existing != null)
            throw new ArgumentException("El correo ya está registrado");

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, workFactor: 12);
        User newUser = new(0, createUserDto.Name, createUserDto.Email, hashedPassword, createUserDto.Rol);
        User createdUser = userRepository.CreateUser(newUser);
        return new GetUserDto(createdUser.Id, createdUser.Name, createdUser.Email, createdUser.Rol);
    }

    public GetUserDto UpdateUser(UpdateUserDto updateUserDto)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password, workFactor: 12);
        User userToUpdate = new(updateUserDto.Id, updateUserDto.Name, updateUserDto.Email, hashedPassword, updateUserDto.Rol);
        User updatedUser = userRepository.UpdateUser(userToUpdate);
        return new GetUserDto(updatedUser.Id, updatedUser.Name, updatedUser.Email, updatedUser.Rol);
    }

    public bool DeleteUser(int id)
    {
        return userRepository.DeleteUser(id);
    }

    public LoginResponseDto? LoginUser(LoginDto loginDto)
    {
        User? user = userRepository.GetUserByEmail(loginDto.Email);
        if (user == null)
            return null;

        bool validPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!validPassword)
            return null;

        return new LoginResponseDto(user.Id, user.Name, user.Email, user.Rol, "Inicio de sesión exitoso");
    }
}