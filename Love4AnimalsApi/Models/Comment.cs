using System;

namespace Love4AnimalsApi.Models;

public class Comment
{
    public Comment(int IdComment, string Texto, DateTime Fecha, int UsuarioId, int IdPost)
    {
        this.IdComment = IdComment;
        this.Texto = Texto;
        this.Fecha = Fecha;
        this.UsuarioId = UsuarioId;
        this.IdPost = IdPost;
    }
    public int IdComment { get; set; }
    public string Texto { get; set; }
    public DateTime Fecha { get; set; }
    public int UsuarioId { get; set; }
    public int IdPost { get; set; }
}