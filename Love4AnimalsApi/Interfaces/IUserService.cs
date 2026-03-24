using Love4AnimalsApi.Dtos;
using System.Collections.Generic;

namespace Love4AnimalsApi.Interfaces;

public interface IUserService
{
    public List<GetUserDto> GetAllUsers();
    public GetUserDto CreateUser(CreateUserDto createUserDto);
    public GetUserDto UpdateUser(UpdateUserDto updateUserDto);
    public bool DeleteUser(int id);
}
