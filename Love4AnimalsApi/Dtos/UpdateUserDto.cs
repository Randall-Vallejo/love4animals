namespace Love4AnimalsApi.Dtos;

public record UpdateUserDto (
    int Id,
    string Name,
    string Email,
    string Password,
    string Rol
);
