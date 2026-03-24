namespace Love4AnimalsApi.Dtos;

public record CreateCampaignDto (
    string Titulo,
    string Descripcion,
    decimal MetaMonto,
    DateTime FechaInicio,
    DateTime FechaFin,
    string Estado,
    int UsuarioId
);