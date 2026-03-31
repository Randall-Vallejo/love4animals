using System;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;

namespace Love4AnimalsApi.Repositories;

public class UserRepository : IUserRepository
{
    private List<User> Users { get; set; }

    public UserRepository()
    {
        this.Users = [];
        User newUser = new(1, "Name", "test@gmail.com", "password123", "Admin");
        this.Users.Add(newUser);
    }

    public User? GetUserById(int id)
    {
        return this.Users.FirstOrDefault(u => u.Id == id);
    }

    public User CreateUser(User user)
    {
        int newId = this.Users.Any() ? this.Users.Max(u => u.Id) + 1 : 1;
        user.Id = newId;
        this.Users.Add(user);
        return user;
    }

    public User UpdateUser(User user)
    {
        User? existingUser = this.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser == null)
            throw new Exception($"Usuario con ID {user.Id} no encontrado");
        
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        existingUser.Rol = user.Rol;
        return existingUser;
    }

    public bool DeleteUser(int id)
    {
        User? userToDelete = this.Users.FirstOrDefault(u => u.Id == id);
        if (userToDelete == null)
            return false;
        
        this.Users.Remove(userToDelete);
        return true;
    }
}