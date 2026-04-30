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

    // Propiedades de navegación para EF Core
    public ICollection<Campaign> Campaigns { get; set; } = [];
    public ICollection<Post> Posts { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<Donation> Donations { get; set; } = [];
}
