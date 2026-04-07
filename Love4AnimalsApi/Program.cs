using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Repositories;
using Love4AnimalsApi.Services;
using Scalar.AspNetCore;
using System.IO;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
    });

// Configurar Scalar/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Love4Animals API",
            Version = "v1",
            Description = "API para la plataforma Love4Animals - Red social de conservación animal",
            Contact = new()
            {
                Name = "SafeWildLife ONG",
                Email = "contact@safewildlife.org"
            }
        };
        return Task.CompletedTask;
    });
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddSingleton<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddSingleton<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddSingleton<ICommentRepository, CommentRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Love4Animals API";
        options.Theme = ScalarTheme.Mars;
        options.DefaultOpenAllTags = true;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
