using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Love4AnimalsApi.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Love4AnimalsApi.Tests;

public class CacheServiceTests
{
    private static readonly DateTime Timestamp = new(2026, 5, 31, 12, 0, 0, DateTimeKind.Utc);
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = null };

    [Fact]
    public void GetAllPosts_CacheMiss_LoadsRepositoryAndStoresResult()
    {
        var cache = new RecordingDistributedCache();
        var repository = new PostRepositoryStub
        {
            Posts = [CreatePost()]
        };
        var service = CreatePostService(repository, cache);

        var result = service.GetAllPosts().ToList();

        Assert.Single(result);
        Assert.Equal(1, repository.GetAllCalls);
        Assert.True(cache.Entries.ContainsKey(CacheConstants.PostsAll));
        Assert.Equal(
            TimeSpan.FromMinutes(CacheConstants.DefaultTtlMinutes),
            cache.LastOptions?.AbsoluteExpirationRelativeToNow);
    }

    [Fact]
    public void GetAllPosts_CacheHit_DoesNotQueryRepository()
    {
        var cachedPosts = new[]
        {
            new GetPostDto(7, "Desde Redis", "Descripcion", "https://example.com/post.jpg", Timestamp, 1, null)
        };
        var cache = new RecordingDistributedCache();
        cache.Seed(CacheConstants.PostsAll, cachedPosts);
        var repository = new PostRepositoryStub();
        var service = CreatePostService(repository, cache);

        var result = service.GetAllPosts().ToList();

        Assert.Single(result);
        Assert.Equal("Desde Redis", result[0].Titulo);
        Assert.Equal(0, repository.GetAllCalls);
    }

    [Fact]
    public void GetAllPosts_WhenRedisFails_FallsBackToRepository()
    {
        var cache = new RecordingDistributedCache { ThrowOnAccess = true };
        var repository = new PostRepositoryStub
        {
            Posts = [CreatePost()]
        };
        var service = CreatePostService(repository, cache);

        var result = service.GetAllPosts().ToList();

        Assert.Single(result);
        Assert.Equal(1, repository.GetAllCalls);
    }

    [Fact]
    public void DeletePost_InvalidatesCollectionAndItemCacheEntries()
    {
        var cache = new RecordingDistributedCache();
        cache.Seed(CacheConstants.PostsAll, Array.Empty<GetPostDto>());
        cache.Seed(CacheConstants.PostItem(1), CreatePostDto());
        var repository = new PostRepositoryStub { DeleteResult = true };
        var service = CreatePostService(repository, cache);

        var result = service.DeletePost(1);

        Assert.True(result);
        Assert.False(cache.Entries.ContainsKey(CacheConstants.PostsAll));
        Assert.False(cache.Entries.ContainsKey(CacheConstants.PostItem(1)));
    }

    [Fact]
    public void GetAllDonations_CacheHit_DoesNotQueryRepository()
    {
        var cache = new RecordingDistributedCache();
        cache.Seed(
            CacheConstants.DonationsAll,
            new[] { new GetDonationDto(1, 100, "QR", "receipt", Timestamp, 1, 1) });
        var repository = new DonationRepositoryStub();
        var service = new DonationService(
            repository,
            new UserRepositoryStub(),
            new CampaignRepositoryStub(),
            cache,
            NullLogger<DonationService>.Instance);

        var result = service.GetAllDonations().ToList();

        Assert.Single(result);
        Assert.Equal(0, repository.GetAllCalls);
    }

    [Fact]
    public void GetAllCampaigns_CacheHit_DoesNotQueryRepository()
    {
        var cache = new RecordingDistributedCache();
        cache.Seed(
            CacheConstants.CampaignsAll,
            new[] { new GetCampaignDto(1, "Campania", "Descripcion", 500, 100, Timestamp, Timestamp.AddDays(10), "Activa", 1) });
        var repository = new CampaignRepositoryStub();
        var service = new CampaignService(
            repository,
            new UserRepositoryStub(),
            cache,
            NullLogger<CampaignService>.Instance);

        var result = service.GetAllCampaigns().ToList();

        Assert.Single(result);
        Assert.Equal(0, repository.GetAllCalls);
    }

    private static PostService CreatePostService(PostRepositoryStub repository, IDistributedCache cache)
    {
        return new PostService(
            repository,
            new UserRepositoryStub(),
            new CampaignRepositoryStub(),
            cache,
            NullLogger<PostService>.Instance);
    }

    private static Post CreatePost()
    {
        return new Post(1, "Post", "Descripcion", "https://example.com/post.jpg", Timestamp, 1, null);
    }

    private static GetPostDto CreatePostDto()
    {
        return new GetPostDto(1, "Post", "Descripcion", "https://example.com/post.jpg", Timestamp, 1, null);
    }

    private sealed class RecordingDistributedCache : IDistributedCache
    {
        public Dictionary<string, byte[]> Entries { get; } = [];
        public DistributedCacheEntryOptions? LastOptions { get; private set; }
        public bool ThrowOnAccess { get; init; }

        public byte[]? Get(string key)
        {
            ThrowIfUnavailable();
            return Entries.GetValueOrDefault(key);
        }

        public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
        {
            return Task.FromResult(Get(key));
        }

        public void Refresh(string key)
        {
            ThrowIfUnavailable();
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            Refresh(key);
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            ThrowIfUnavailable();
            Entries.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            ThrowIfUnavailable();
            Entries[key] = value;
            LastOptions = options;
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            Set(key, value, options);
            return Task.CompletedTask;
        }

        public void Seed<T>(string key, T value)
        {
            Entries[key] = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, JsonOptions));
        }

        private void ThrowIfUnavailable()
        {
            if (ThrowOnAccess)
                throw new InvalidOperationException("Redis no disponible");
        }
    }

    private sealed class PostRepositoryStub : IPostRepository
    {
        public IEnumerable<Post> Posts { get; init; } = [];
        public int GetAllCalls { get; private set; }
        public bool DeleteResult { get; init; }

        public Post? GetPostById(int id) => Posts.FirstOrDefault(post => post.IdPost == id);

        public IEnumerable<Post> GetAllPosts()
        {
            GetAllCalls++;
            return Posts;
        }

        public Post CreatePost(Post post) => post;
        public Post UpdatePost(Post post) => post;
        public bool DeletePost(int id) => DeleteResult;
    }

    private sealed class DonationRepositoryStub : IDonationRepository
    {
        public int GetAllCalls { get; private set; }

        public Donation? GetDonationById(int id) => null;

        public IEnumerable<Donation> GetAllDonations()
        {
            GetAllCalls++;
            return [];
        }

        public Donation CreateDonation(Donation donation) => donation;
        public Donation UpdateDonation(Donation donation) => donation;
        public bool DeleteDonation(int id) => false;
    }

    private sealed class CampaignRepositoryStub : ICampaignRepository
    {
        public int GetAllCalls { get; private set; }

        public Campaign? GetCampaignById(int id) => null;

        public IEnumerable<Campaign> GetAllCampaigns()
        {
            GetAllCalls++;
            return [];
        }

        public Campaign CreateCampaign(Campaign campaign) => campaign;
        public Campaign UpdateCampaign(Campaign campaign) => campaign;
        public bool DeleteCampaign(int id) => false;
    }

    private sealed class UserRepositoryStub : IUserRepository
    {
        public User? GetUserById(int id) => null;
        public User? GetUserByEmail(string email) => null;
        public User CreateUser(User user) => user;
        public User UpdateUser(User user) => user;
        public bool DeleteUser(int id) => false;
    }
}
