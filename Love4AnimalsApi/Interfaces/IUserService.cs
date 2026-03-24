using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface IUserService
{
    public GetUserDto? GetUserById(int id); 
    public GetUserDto CreateUser(CreateUserDto createUserDto);
    public GetUserDto UpdateUser(UpdateUserDto updateUserDto);
    public bool DeleteUser(int id);
}