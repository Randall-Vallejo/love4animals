namespace Love4AnimalsApi.Dtos;

public record GetCommentDto (
    int IdComment,
    string Texto,
    DateTime Fecha,
    int UsuarioId,
    int IdPost
);