using System;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Love4AnimalsApi.Repositories;

public class PostRepository : IPostRepository
{
    private List<Post> Posts { get; set; }

    public PostRepository()
    {
        this.Posts = [];
        Post newPost = new(1, "Post de ejemplo", "Descripción de un post de ejemplo", "https://example.com/foto.jpg", DateTime.Now, 1, 1);
        this.Posts.Add(newPost);
    }

    public Post? GetPostById(int id)
    {
        return this.Posts.FirstOrDefault(p => p.IdPost == id);
    }

    public Post CreatePost(Post post)
    {
        int newId = this.Posts.Any() ? this.Posts.Max(p => p.IdPost) + 1 : 1;
        post.IdPost = newId;
        this.Posts.Add(post);
        return post;
    }

    public Post UpdatePost(Post post)
    {
        Post? existingPost = this.Posts.FirstOrDefault(p => p.IdPost == post.IdPost);
        if (existingPost == null)
            throw new Exception($"Post con ID {post.IdPost} no encontrado");

        existingPost.Titulo = post.Titulo;
        existingPost.Descripcion = post.Descripcion;
        existingPost.FotoUrl = post.FotoUrl;
        existingPost.Fecha = post.Fecha;
        existingPost.UsuarioId = post.UsuarioId;
        existingPost.IdCampania = post.IdCampania;
        return existingPost;
    }

    public bool DeletePost(int id)
    {
        Post? post = this.Posts.FirstOrDefault(p => p.IdPost == id);
        if (post == null)
            return false;
        this.Posts.Remove(post);
        return true;
    }
}