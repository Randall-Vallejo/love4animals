using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class PostService : IPostService
{
    private IPostRepository postRepository;
    private IUserRepository userRepository;
    private ICampaignRepository campaignRepository;
    
    public PostService(IPostRepository postRepository, IUserRepository userRepository, ICampaignRepository campaignRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.campaignRepository = campaignRepository;
    }

    public GetPostDto? GetPostById(int id)
    {
        Post? post = postRepository.GetPostById(id);
        if (post == null) return null;

        return new GetPostDto(
            post.IdPost, post.Titulo, post.Descripcion, 
            post.FotoUrl, post.Fecha, post.UsuarioId, post.IdCampania
        );
    }

    public GetPostDto CreatePost(CreatePostDto createPostDto)
    {
        // Validar que el usuario existe
        User? user = userRepository.GetUserById(createPostDto.UsuarioId);
        Campaign? campaign = null;
        if (createPostDto.IdCampania.HasValue)
        {
            campaign = campaignRepository.GetCampaignById(createPostDto.IdCampania.Value);
        }

        // Acumular errores
        List<string> errors = new();
        if (user == null) errors.Add("Usuario no encontrado");
        if (createPostDto.IdCampania.HasValue && campaign == null) errors.Add("Campaña no encontrada");

        if (errors.Any())
            throw new ArgumentException(string.Join("; ", errors));

        Post newPost = new(0, createPostDto.Titulo, createPostDto.Descripcion, createPostDto.FotoUrl, DateTime.Now, createPostDto.UsuarioId, createPostDto.IdCampania);
        Post createdPost = postRepository.CreatePost(newPost);
        return new GetPostDto(createdPost.IdPost, createdPost.Titulo, createdPost.Descripcion, createdPost.FotoUrl, createdPost.Fecha, createdPost.UsuarioId, createdPost.IdCampania);
    }

    public GetPostDto UpdatePost(UpdatePostDto updatePostDto)
    {
        // Validar que el usuario existe
        User? user = userRepository.GetUserById(updatePostDto.UsuarioId);
        Campaign? campaign = null;
        if (updatePostDto.IdCampania.HasValue)
        {
            campaign = campaignRepository.GetCampaignById(updatePostDto.IdCampania.Value);
        }

        // Acumular errores
        List<string> errors = new();
        if (user == null) errors.Add("Usuario no encontrado");
        if (updatePostDto.IdCampania.HasValue && campaign == null) errors.Add("Campaña no encontrada");

        if (errors.Any())
            throw new ArgumentException(string.Join("; ", errors));

        Post postToUpdate = new(updatePostDto.IdPost, updatePostDto.Titulo, updatePostDto.Descripcion, updatePostDto.FotoUrl, updatePostDto.Fecha, updatePostDto.UsuarioId, updatePostDto.IdCampania);
        Post updatedPost = postRepository.UpdatePost(postToUpdate);
        return new GetPostDto(updatedPost.IdPost, updatedPost.Titulo, updatedPost.Descripcion, updatedPost.FotoUrl, updatedPost.Fecha, updatedPost.UsuarioId, updatedPost.IdCampania);
    }

    public bool DeletePost(int id)
    {
        return postRepository.DeletePost(id);
    }
}