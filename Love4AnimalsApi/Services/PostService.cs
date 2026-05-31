using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Love4AnimalsApi.Services;

public class PostService : IPostService
{
    private IPostRepository postRepository;
    private IUserRepository userRepository;
    private ICampaignRepository campaignRepository;
    private readonly IDistributedCache cache;
    private readonly JsonSerializerOptions jsonOptions = new() { PropertyNamingPolicy = null };

    public PostService(IPostRepository postRepository, IUserRepository userRepository, ICampaignRepository campaignRepository, IDistributedCache cache)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.campaignRepository = campaignRepository;
        this.cache = cache;
    }

    public GetPostDto? GetPostById(int id)
    {
        var key = CacheConstants.PostItem(id);
        try
        {
            var cached = cache.GetString(key);
            if (!string.IsNullOrEmpty(cached))
            {
                var dto = JsonSerializer.Deserialize<GetPostDto>(cached, jsonOptions);
                if (dto != null) return dto;
            }
        }
        catch { }

        Post? post = postRepository.GetPostById(id);
        if (post == null) return null;

        var result = new GetPostDto(
            post.IdPost, post.Titulo, post.Descripcion,
            post.FotoUrl, post.Fecha, post.UsuarioId, post.IdCampania
        );

        try
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheConstants.DefaultTtlMinutes) };
            cache.SetString(key, JsonSerializer.Serialize(result, jsonOptions), options);
        }
        catch { }

        return result;
    }

    public IEnumerable<GetPostDto> GetAllPosts()
    {
        const string key = "posts:all";
        var cached = cache.GetString(key);
        if (!string.IsNullOrEmpty(cached))
        {
            try
            {
                var dtoList = JsonSerializer.Deserialize<IEnumerable<GetPostDto>>(cached, jsonOptions);
                if (dtoList != null) return dtoList;
            }
            catch { /* fallthrough to reload cache */ }
        }

        var posts = postRepository.GetAllPosts();
        var result = posts.Select(post => new GetPostDto(
            post.IdPost, post.Titulo, post.Descripcion,
            post.FotoUrl, post.Fecha, post.UsuarioId, post.IdCampania
        )).ToList();

        try
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheConstants.DefaultTtlMinutes) };
            cache.SetString(key, JsonSerializer.Serialize(result, jsonOptions), options);
        }
        catch { }
        return result;
    }

    public GetPostDto CreatePost(CreatePostDto createPostDto)
    {
        User? user = userRepository.GetUserById(createPostDto.UsuarioId);
        Campaign? campaign = null;
        if (createPostDto.IdCampania.HasValue)
        {
            campaign = campaignRepository.GetCampaignById(createPostDto.IdCampania.Value);
        }

        List<string> errors = new();
        if (user == null) errors.Add("Usuario no encontrado");
        if (createPostDto.IdCampania.HasValue && campaign == null) errors.Add("Campaña no encontrada");

        if (errors.Any())
            throw new ArgumentException(string.Join("; ", errors));

        Post newPost = new(0, createPostDto.Titulo, createPostDto.Descripcion, createPostDto.FotoUrl, DateTime.UtcNow, createPostDto.UsuarioId, createPostDto.IdCampania);
        Post createdPost = postRepository.CreatePost(newPost);
        // Invalidate cache
        try { cache.Remove(CacheConstants.PostsAll); } catch { }
        try { cache.Remove(CacheConstants.PostItem(createdPost.IdPost)); } catch { }
        return new GetPostDto(createdPost.IdPost, createdPost.Titulo, createdPost.Descripcion, createdPost.FotoUrl, createdPost.Fecha, createdPost.UsuarioId, createdPost.IdCampania);
    }

    public GetPostDto UpdatePost(UpdatePostDto updatePostDto)
    {
        User? user = userRepository.GetUserById(updatePostDto.UsuarioId);
        Campaign? campaign = null;
        if (updatePostDto.IdCampania.HasValue)
        {
            campaign = campaignRepository.GetCampaignById(updatePostDto.IdCampania.Value);
        }

        List<string> errors = new();
        if (user == null) errors.Add("Usuario no encontrado");
        if (updatePostDto.IdCampania.HasValue && campaign == null) errors.Add("Campaña no encontrada");

        if (errors.Any())
            throw new ArgumentException(string.Join("; ", errors));

        DateTime fechaUtc = updatePostDto.Fecha.ToUniversalTime();
        Post postToUpdate = new(updatePostDto.IdPost, updatePostDto.Titulo, updatePostDto.Descripcion, updatePostDto.FotoUrl, fechaUtc, updatePostDto.UsuarioId, updatePostDto.IdCampania);
        Post updatedPost = postRepository.UpdatePost(postToUpdate);
        try { cache.Remove(CacheConstants.PostsAll); } catch { }
        try { cache.Remove(CacheConstants.PostItem(updatedPost.IdPost)); } catch { }
        return new GetPostDto(updatedPost.IdPost, updatedPost.Titulo, updatedPost.Descripcion, updatedPost.FotoUrl, updatedPost.Fecha, updatedPost.UsuarioId, updatedPost.IdCampania);
    }

    public bool DeletePost(int id)
    {
        var ok = postRepository.DeletePost(id);
        if (ok)
        {
            try { cache.Remove(CacheConstants.PostsAll); } catch { }
            try { cache.Remove(CacheConstants.PostItem(id)); } catch { }
        }
        return ok;
    }
}