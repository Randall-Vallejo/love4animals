using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Repositories;
using Love4AnimalsApi.Services;
using System.IO;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
    });

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Love4Animals API",
        Version = "v1",
        Description = "API para la plataforma Love4Animals - Red social de conservación animal",
        Contact = new()
        {
            Name = "SafeWildLife ONG",
            Email = "contact@safewildlife.org"
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddOpenApi();

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
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Love4Animals API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
