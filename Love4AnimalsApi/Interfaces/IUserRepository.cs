using System;
using Love4AnimalsApi.Models;
using System.Collections.Generic;

namespace Love4AnimalsApi.Interfaces;

public interface IUserRepository
{
    public List<User> GetAllUsers();
    public User CreateUser(User user);
    public User UpdateUser(User user);
    public bool DeleteUser(int id);
}
