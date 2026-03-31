using System;

namespace Love4AnimalsApi.Models;

public class Post
{
    public Post(int IdPost, string Titulo, string Descripcion, string FotoUrl, DateTime Fecha, int UsuarioId, int? IdCampania)
    {
        this.IdPost = IdPost;
        this.Titulo = Titulo;
        this.Descripcion = Descripcion;
        this.FotoUrl = FotoUrl;
        this.Fecha = Fecha;
        this.UsuarioId = UsuarioId;
        this.IdCampania = IdCampania;
    }
    public int IdPost { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public string FotoUrl { get; set; }
    public DateTime Fecha { get; set; }
    public int UsuarioId { get; set; }
    public int? IdCampania { get; set; }
}