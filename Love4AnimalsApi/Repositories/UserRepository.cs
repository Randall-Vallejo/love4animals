using System;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Love4AnimalsApi.Repositories;

public class UserRepository : IUserRepository
{
    private List<User> Users { get; set; }

    public UserRepository()
    {
        this.Users = [];
        User newUser = new(1, "Name", "test@gmail.com");
        this.Users.Add(newUser);
    }
    public User getUser()
    {
        return this.Users.First();
    }
}
