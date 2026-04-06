using System.ComponentModel.DataAnnotations;

namespace Love4AnimalsApi.Dtos;

public record UpdateCampaignDto (
    [Required(ErrorMessage = "El ID de la campaña es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de la campaña debe ser mayor a 0")]
    int IdCampania,

    [Required(ErrorMessage = "El título es obligatorio")]
    [MaxLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
    string Titulo,

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [MaxLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    string Descripcion,

    [Required(ErrorMessage = "La meta de monto es obligatoria")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La meta de monto debe ser mayor a 0")]
    decimal MetaMonto,

    [Required(ErrorMessage = "El monto recaudado es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El monto recaudado no puede ser negativo")]
    decimal MontoRecaudado,

    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    DateTime FechaInicio,

    [Required(ErrorMessage = "La fecha de fin es obligatoria")]
    DateTime FechaFin,

    [Required(ErrorMessage = "El estado es obligatorio")]
    [RegularExpression("^(Activa|Inactiva|Completada)$", ErrorMessage = "El estado debe ser 'Activa', 'Inactiva' o 'Completada'")]
    string Estado,

    [Required(ErrorMessage = "El ID del usuario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor a 0")]
    int UsuarioId
);