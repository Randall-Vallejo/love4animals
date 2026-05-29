using System;

namespace Love4AnimalsApi.Models;

public class User
{
    public User (int Id, string Name, string Email, string PasswordHash, string Rol)
    {
        this.Id = Id;
        this.Name = Name;
        this.Email = Email;
        this.PasswordHash = PasswordHash;
        this.Rol = Rol;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Rol { get; set; }

    // Propiedades de navegación para EF Core
    public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
