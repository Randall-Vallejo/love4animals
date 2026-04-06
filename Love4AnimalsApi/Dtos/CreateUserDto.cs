using System.ComponentModel.DataAnnotations;

namespace Love4AnimalsApi.Dtos;

public record CreateUserDto (
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    string Name,

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [MaxLength(255, ErrorMessage = "El email no puede exceder 255 caracteres")]
    string Email,

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    string Password,

    [Required(ErrorMessage = "El rol es obligatorio")]
    [RegularExpression("^(Admin|User|Misionero)$", ErrorMessage = "El rol debe ser Admin, User o Misionero")]
    string Rol
);
