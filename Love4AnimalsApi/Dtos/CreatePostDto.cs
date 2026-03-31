namespace Love4AnimalsApi.Dtos;

public record CreatePostDto (
    string Titulo,
    string Descripcion,
    string FotoUrl,
    int UsuarioId,
    int? IdCampania
);