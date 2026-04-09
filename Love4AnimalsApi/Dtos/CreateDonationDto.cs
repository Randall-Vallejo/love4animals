using System.ComponentModel.DataAnnotations;

namespace Love4AnimalsApi.Dtos;

public record CreateDonationDto(
    [Required(ErrorMessage = "El monto es obligatorio")]
    [Range(0.01, 1000000, ErrorMessage = "El monto debe ser mayor a 0")]
    decimal Monto,

    [Required(ErrorMessage = "El método de pago es obligatorio")]
    [MaxLength(100, ErrorMessage = "El método de pago no puede exceder 100 caracteres")]
    string MetodoPago,

    [Required(ErrorMessage = "El comprobante es obligatorio")]
    [MaxLength(50000, ErrorMessage = "El comprobante no puede exceder 50,000 caracteres")]
    string Comprobante,

    [Required(ErrorMessage = "La fecha es obligatoria")]
    DateTime Fecha,

    [Required(ErrorMessage = "El ID del usuario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser mayor a 0")]
    int UsuarioId,

    [Required(ErrorMessage = "El ID de la campaña es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de la campaña debe ser mayor a 0")]
    int IdCampania
);