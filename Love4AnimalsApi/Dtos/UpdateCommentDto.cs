using System.ComponentModel.DataAnnotations;

namespace Love4AnimalsApi.Dtos;

public record UpdateCommentDto (
    [Required(ErrorMessage = "El ID del comentario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del comentario debe ser mayor a 0")]
    int IdComment,

    [Required(ErrorMessage = "El texto del comentario es obligatorio")]
    [MaxLength(1000, ErrorMessage = "El comentario no puede exceder 1000 caracteres")]
    string Texto,

    [Required(ErrorMessage = "La fecha es obligatoria")]
    DateTime Fecha,

    [Required(ErrorMessage = "El ID del usuario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor a 0")]
    int UsuarioId,

    [Required(ErrorMessage = "El ID del post es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del post debe ser mayor a 0")]
    int IdPost
);