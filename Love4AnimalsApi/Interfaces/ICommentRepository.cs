using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface ICommentRepository
{
    public Comment? GetCommentById(int id); 
    public Comment CreateComment(Comment comment);
    public Comment UpdateComment(Comment comment);
    public bool DeleteComment(int id);
}