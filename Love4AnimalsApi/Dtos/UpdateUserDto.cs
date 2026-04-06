using System.ComponentModel.DataAnnotations;

namespace Love4AnimalsApi.Dtos;

public record UpdateUserDto (
    [Required(ErrorMessage = "El ID es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser mayor a 0")]
    int Id,

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    string Name,

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    [MaxLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
    string Email,

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    [MaxLength(100, ErrorMessage = "La contraseña no puede exceder 100 caracteres")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "La contraseña debe contener al menos una letra minúscula, una mayúscula, un número y un carácter especial")]
    string Password,

    [Required(ErrorMessage = "El rol es obligatorio")]
    [RegularExpression("^(Admin|User)$", ErrorMessage = "El rol debe ser 'Admin' o 'User'")]
    string Rol
);
