using System;

namespace Love4AnimalsApi.Models;

public class User
{
    public User (int Id, string Name, string Email, string Password, string Rol)
    {
        this.Id = Id;
        this.Name = Name;
        this.Email = Email;
        this.Password = Password;
        this.Rol = Rol;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Rol { get; set; }
}
