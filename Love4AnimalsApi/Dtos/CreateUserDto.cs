namespace Love4AnimalsApi.Dtos;

public record CreateUserDto (
    string Name,
    string Email,
    string Password,
    string Rol
);
