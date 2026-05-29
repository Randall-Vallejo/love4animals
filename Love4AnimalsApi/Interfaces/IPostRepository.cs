using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface IPostRepository
{
    public Post? GetPostById(int id); 
    public IEnumerable<Post> GetAllPosts();
    public Post CreatePost(Post post);
    public Post UpdatePost(Post post);
    public bool DeletePost(int id);
}