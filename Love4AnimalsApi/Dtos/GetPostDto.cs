namespace Love4AnimalsApi.Dtos;

public record GetPostDto (
    int IdPost,
    string Titulo,
    string Descripcion,
    string FotoUrl,
    DateTime Fecha,
    int UsuarioId,
    int? IdCampania
);