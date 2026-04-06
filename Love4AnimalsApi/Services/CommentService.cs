using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class CommentService : ICommentService
{
    private ICommentRepository commentRepository;
    private IUserRepository userRepository;
    private IPostRepository postRepository;
    
    public CommentService(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }

    public GetCommentDto? GetCommentById(int id)
    {
        Comment? comment = commentRepository.GetCommentById(id);
        if (comment == null) return null;

        return new GetCommentDto(
            comment.IdComment, comment.Texto, comment.Fecha, 
            comment.UsuarioId, comment.IdPost
        );
    }

    public GetCommentDto? GetCommentByIdAndPostId(int commentId, int postId)
    {
        Comment? comment = commentRepository.GetCommentById(commentId);
        if (comment == null || comment.IdPost != postId) return null;

        return new GetCommentDto(
            comment.IdComment, comment.Texto, comment.Fecha, 
            comment.UsuarioId, comment.IdPost
        );
    }

    public GetCommentDto CreateComment(CreateCommentDto createCommentDto)
    {
        // Validar que el usuario existe
        User? user = userRepository.GetUserById(createCommentDto.UsuarioId);
        Post? post = postRepository.GetPostById(createCommentDto.IdPost);

        // Acumular errores
        List<string> errors = new();
        if (user == null) errors.Add("Usuario no encontrado");
        if (post == null) errors.Add("Post no encontrado");

        if (errors.Any())
            throw new ArgumentException(string.Join("; ", errors));

        Comment newComment = new(0, createCommentDto.Texto, DateTime.Now, createCommentDto.UsuarioId, createCommentDto.IdPost);
        Comment createdComment = commentRepository.CreateComment(newComment);
        return new GetCommentDto(createdComment.IdComment, createdComment.Texto, createdComment.Fecha, createdComment.UsuarioId, createdComment.IdPost);
    }

    public GetCommentDto UpdateComment(UpdateCommentDto updateCommentDto)
    {
        // Validar que el usuario existe
        User? user = userRepository.GetUserById(updateCommentDto.UsuarioId);
        Post? post = postRepository.GetPostById(updateCommentDto.IdPost);

        // Acumular errores
        List<string> errors = new();
        if (user == null) errors.Add("Usuario no encontrado");
        if (post == null) errors.Add("Post no encontrado");

        if (errors.Any())
            throw new ArgumentException(string.Join("; ", errors));

        Comment commentToUpdate = new(updateCommentDto.IdComment, updateCommentDto.Texto, updateCommentDto.Fecha, updateCommentDto.UsuarioId, updateCommentDto.IdPost);
        Comment updatedComment = commentRepository.UpdateComment(commentToUpdate);
        return new GetCommentDto(updatedComment.IdComment, updatedComment.Texto, updatedComment.Fecha, updatedComment.UsuarioId, updatedComment.IdPost);
    }

    public bool DeleteComment(int id)
    {
        return commentRepository.DeleteComment(id);
    }

    public bool DeleteCommentByIdAndPostId(int commentId, int postId)
    {
        Comment? comment = commentRepository.GetCommentById(commentId);
        if (comment == null || comment.IdPost != postId)
            return false;

        return commentRepository.DeleteComment(commentId);
    }
}