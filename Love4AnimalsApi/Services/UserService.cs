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
    public GetUserDto GetUser()
    {
        User getUser = userRepository.getUser();
        GetUserDto response = new(getUser.Id, getUser.Name, getUser.Email);
        return response;
    }
}
