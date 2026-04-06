namespace Love4AnimalsApi.Dtos;

public record UpdateCommentDto (
    int IdComment,
    string Texto,
    DateTime Fecha,
    int UsuarioId,
    int IdPost
);