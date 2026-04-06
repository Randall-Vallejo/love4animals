using System.ComponentModel.DataAnnotations;

namespace Love4AnimalsApi.Dtos;

public record CreatePostDto (
    [Required(ErrorMessage = "El título es obligatorio")]
    [MaxLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
    string Titulo,

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [MaxLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    string Descripcion,

    [Required(ErrorMessage = "La URL de la foto es obligatoria")]
    [Url(ErrorMessage = "La URL de la foto no tiene un formato válido")]
    [MaxLength(500, ErrorMessage = "La URL no puede exceder 500 caracteres")]
    string FotoUrl,

    [Required(ErrorMessage = "El ID del usuario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor a 0")]
    int UsuarioId,

    [Range(1, int.MaxValue, ErrorMessage = "El ID de la campaña debe ser mayor a 0 si se especifica")]
    int? IdCampania
);