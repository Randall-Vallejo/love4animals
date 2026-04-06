namespace Love4AnimalsApi.Dtos;

public record CreateCommentDto (
    string Texto,
    int UsuarioId,
    int IdPost
);