namespace Love4AnimalsApi.Dtos;

public record GetDonationDto(
    int IdDonation,
    decimal Monto,
    string MetodoPago,
    string Comprobante,
    DateTime Fecha,
    int UsuarioId,
    int IdCampania
);