namespace Love4AnimalsApi.Dtos;

public record LoginResponseDto(
    int Id,
    string Name,
    string Email,
    string Rol,
    string Message
);
