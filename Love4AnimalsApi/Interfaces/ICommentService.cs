using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface ICommentService
{
    public GetCommentDto? GetCommentById(int id);
    public GetCommentDto? GetCommentByIdAndPostId(int commentId, int postId);
    public GetCommentDto CreateComment(CreateCommentDto createCommentDto);
    public GetCommentDto UpdateComment(UpdateCommentDto updateCommentDto);
    public bool DeleteComment(int id);
    public bool DeleteCommentByIdAndPostId(int commentId, int postId);
}