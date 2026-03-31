using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface IPostService
{
    public GetPostDto? GetPostById(int id); 
    public GetPostDto CreatePost(CreatePostDto createPostDto);
    public GetPostDto UpdatePost(UpdatePostDto updatePostDto);
    public bool DeletePost(int id);
}