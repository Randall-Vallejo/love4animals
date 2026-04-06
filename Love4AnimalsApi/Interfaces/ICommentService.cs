using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface ICommentService
{
    public GetCommentDto? GetCommentById(int id); 
    public GetCommentDto CreateComment(CreateCommentDto createCommentDto);
    public GetCommentDto UpdateComment(UpdateCommentDto updateCommentDto);
    public bool DeleteComment(int id);
}