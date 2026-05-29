using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Repositories;
using Love4AnimalsApi.Repositories.EF;
using Love4AnimalsApi.Services;
using Scalar.AspNetCore;
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//5234
// ═══════════════════════════════════════════════════════════════
// Configuración de Entity Framework Core con PostgreSQL
// ═══════════════════════════════════════════════════════════════
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
    });

// Redis distributed cache (removed)

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

// ═══════════════════════════════════════════════════════════════
// OPCIÓN 1: Registrar Repositorios CON Entity Framework Core
// ═══════════════════════════════════════════════════════════════
// Descomenta las líneas a continuación para usar EF Core
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();

builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<ICampaignRepository, EFCampaignRepository>();

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostRepository, EFPostRepository>();

builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentRepository, EFCommentRepository>();

builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IDonationRepository, EFDonationRepository>();

// ═══════════════════════════════════════════════════════════════
// OPCIÓN 2: Registrar Repositorios EN MEMORIA (comentado)
// ═══════════════════════════════════════════════════════════════
// Descomenta las líneas a continuación para usar listas en memoria
/*
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddSingleton<ICampaignRepository, CampaignRepository>();

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddSingleton<IPostRepository, PostRepository>();

builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddSingleton<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddSingleton<IDonationRepository, DonationRepository>();
*/

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

