namespace Love4AnimalsApi.Dtos;

public record GetCampaignDto (
    int IdCampania,
    string Titulo,
    string Descripcion,
    decimal MetaMonto,
    decimal MontoRecaudado,
    DateTime FechaInicio,
    DateTime FechaFin,
    string Estado,
    int UsuarioId
);