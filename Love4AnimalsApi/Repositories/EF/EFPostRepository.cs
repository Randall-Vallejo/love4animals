using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Repositories.EF;

/// <summary>
/// Implementación de IPostRepository usando Entity Framework Core
/// </summary>
public class EFPostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public EFPostRepository(AppDbContext context)
    {
        this._context = context;
    }

    public Post? GetPostById(int id)
    {
        return _context.Posts
            .Include(p => p.Usuario)
            .Include(p => p.Campania)
            .FirstOrDefault(p => p.IdPost == id);
    }

    public Post CreatePost(Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();
        return post;
    }

    public Post UpdatePost(Post post)
    {
        Post? existingPost = _context.Posts.FirstOrDefault(p => p.IdPost == post.IdPost);
        if (existingPost == null)
            throw new Exception($"Post con ID {post.IdPost} no encontrado");

        existingPost.Titulo = post.Titulo;
        existingPost.Descripcion = post.Descripcion;
        existingPost.FotoUrl = post.FotoUrl;
        existingPost.Fecha = post.Fecha;
        existingPost.UsuarioId = post.UsuarioId;
        existingPost.IdCampania = post.IdCampania;

        _context.Posts.Update(existingPost);
        _context.SaveChanges();
        return existingPost;
    }

    public bool DeletePost(int id)
    {
        Post? postToDelete = _context.Posts.FirstOrDefault(p => p.IdPost == id);
        if (postToDelete == null)
            return false;

        _context.Posts.Remove(postToDelete);
        _context.SaveChanges();
        return true;
    }
}