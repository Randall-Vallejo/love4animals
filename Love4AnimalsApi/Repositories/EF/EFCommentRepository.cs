using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Repositories.EF;

/// <summary>
/// Implementación de ICommentRepository usando Entity Framework Core
/// </summary>
public class EFCommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;

    public EFCommentRepository(AppDbContext context)
    {
        this._context = context;
    }

    public Comment? GetCommentById(int id)
    {
        return _context.Comments
            .Include(c => c.Usuario)
            .Include(c => c.Post)
            .FirstOrDefault(c => c.IdComment == id);
    }

    public Comment CreateComment(Comment comment)
    {
        _context.Comments.Add(comment);
        _context.SaveChanges();
        return comment;
    }

    public Comment UpdateComment(Comment comment)
    {
        Comment? existingComment = _context.Comments.FirstOrDefault(c => c.IdComment == comment.IdComment);
        if (existingComment == null)
            throw new Exception($"Comentario con ID {comment.IdComment} no encontrado");

        existingComment.Texto = comment.Texto;
        existingComment.Fecha = comment.Fecha;
        existingComment.UsuarioId = comment.UsuarioId;
        existingComment.IdPost = comment.IdPost;

        _context.Comments.Update(existingComment);
        _context.SaveChanges();
        return existingComment;
    }

    public bool DeleteComment(int id)
    {
        Comment? commentToDelete = _context.Comments.FirstOrDefault(c => c.IdComment == id);
        if (commentToDelete == null)
            return false;

        _context.Comments.Remove(commentToDelete);
        _context.SaveChanges();
        return true;
    }
}