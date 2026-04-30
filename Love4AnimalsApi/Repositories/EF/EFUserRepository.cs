using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Repositories.EF;

/// <summary>
/// Implementación de IUserRepository usando Entity Framework Core
/// Accede a la base de datos PostgreSQL a través del DbContext
/// </summary>
public class EFUserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public EFUserRepository(AppDbContext context)
    {
        this._context = context;
    }

    /// <summary>
    /// Obtiene un usuario por su ID desde la base de datos
    /// </summary>
    public User? GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    /// <summary>
    /// Crea un nuevo usuario en la base de datos
    /// Genera automáticamente el ID mediante auto-incremento
    /// </summary>
    public User CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    public User UpdateUser(User user)
    {
        User? existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser == null)
            throw new Exception($"Usuario con ID {user.Id} no encontrado");

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        existingUser.Rol = user.Rol;

        _context.Users.Update(existingUser);
        _context.SaveChanges();
        return existingUser;
    }

    /// <summary>
    /// Elimina un usuario de la base de datos
    /// </summary>
    public bool DeleteUser(int id)
    {
        User? userToDelete = _context.Users.FirstOrDefault(u => u.Id == id);
        if (userToDelete == null)
            return false;

        _context.Users.Remove(userToDelete);
        _context.SaveChanges();
        return true;
    }
}