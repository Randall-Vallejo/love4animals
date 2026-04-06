using System;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Love4AnimalsApi.Repositories;

public class CommentRepository : ICommentRepository
{
    private List<Comment> Comments { get; set; }

    public CommentRepository()
    {
        this.Comments = [];
        Comment newComment = new(1, "Excelente campaña, muy importante esta causa", DateTime.Now, 1, 1);
        this.Comments.Add(newComment);
    }

    public Comment? GetCommentById(int id)
    {
        return this.Comments.FirstOrDefault(c => c.IdComment == id);
    }

    public Comment CreateComment(Comment comment)
    {
        int newId = this.Comments.Any() ? this.Comments.Max(c => c.IdComment) + 1 : 1;
        comment.IdComment = newId;
        this.Comments.Add(comment);
        return comment;
    }

    public Comment UpdateComment(Comment comment)
    {
        Comment? existingComment = this.Comments.FirstOrDefault(c => c.IdComment == comment.IdComment);
        if (existingComment == null)
            throw new Exception($"Comentario con ID {comment.IdComment} no encontrado");

        existingComment.Texto = comment.Texto;
        existingComment.Fecha = comment.Fecha;
        existingComment.UsuarioId = comment.UsuarioId;
        existingComment.IdPost = comment.IdPost;
        return existingComment;
    }

    public bool DeleteComment(int id)
    {
        Comment? comment = this.Comments.FirstOrDefault(c => c.IdComment == id);
        if (comment == null)
            return false;
        this.Comments.Remove(comment);
        return true;
    }
}