using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class CampaignService : ICampaignService
{
    private ICampaignRepository campaignRepository;
    private IUserRepository userRepository;
    
    public CampaignService(ICampaignRepository campaignRepository, IUserRepository userRepository)
    {
        this.campaignRepository = campaignRepository;
        this.userRepository = userRepository;
    }

    public GetCampaignDto? GetCampaignById(int id)
    {
        Campaign? campaign = campaignRepository.GetCampaignById(id);
        if (campaign == null) return null;

        return new GetCampaignDto(
            campaign.IdCampania, campaign.Titulo, campaign.Descripcion, 
            campaign.MetaMonto, campaign.MontoRecaudado, campaign.FechaInicio, 
            campaign.FechaFin, campaign.Estado, campaign.UsuarioId
        );
    }

    public IEnumerable<GetCampaignDto> GetAllCampaigns()
    {
        var campaigns = campaignRepository.GetAllCampaigns();
        return campaigns.Select(c => new GetCampaignDto(
            c.IdCampania, c.Titulo, c.Descripcion,
            c.MetaMonto, c.MontoRecaudado, c.FechaInicio,
            c.FechaFin, c.Estado, c.UsuarioId
        ));
    }

    public GetCampaignDto CreateCampaign(CreateCampaignDto createCampaignDto)
    {
        User? user = userRepository.GetUserById(createCampaignDto.UsuarioId);
        if (user == null)
            throw new ArgumentException("Usuario no encontrado");

        DateTime fechaInicioUtc = DateTime.UtcNow;
        DateTime fechaFinUtc = createCampaignDto.FechaFin.ToUniversalTime();
        Campaign newCampaign = new(0, createCampaignDto.Titulo, createCampaignDto.Descripcion, createCampaignDto.MetaMonto, 0.00m, fechaInicioUtc, fechaFinUtc, createCampaignDto.Estado, createCampaignDto.UsuarioId);
        Campaign createdCampaign = campaignRepository.CreateCampaign(newCampaign);
        return new GetCampaignDto(createdCampaign.IdCampania, createdCampaign.Titulo, createdCampaign.Descripcion, createdCampaign.MetaMonto, createdCampaign.MontoRecaudado, createdCampaign.FechaInicio, createdCampaign.FechaFin, createdCampaign.Estado, createdCampaign.UsuarioId);
    }

    public GetCampaignDto UpdateCampaign(UpdateCampaignDto updateCampaignDto)
    {
        User? user = userRepository.GetUserById(updateCampaignDto.UsuarioId);
        if (user == null)
            throw new ArgumentException("Usuario no encontrado");

        DateTime fechaInicioUtc = updateCampaignDto.FechaInicio.ToUniversalTime();
        DateTime fechaFinUtc = updateCampaignDto.FechaFin.ToUniversalTime();
        Campaign campaignToUpdate = new(updateCampaignDto.IdCampania, updateCampaignDto.Titulo, updateCampaignDto.Descripcion, updateCampaignDto.MetaMonto, updateCampaignDto.MontoRecaudado, fechaInicioUtc, fechaFinUtc, updateCampaignDto.Estado, updateCampaignDto.UsuarioId);
        Campaign updatedCampaign = campaignRepository.UpdateCampaign(campaignToUpdate);
        return new GetCampaignDto(updatedCampaign.IdCampania, updatedCampaign.Titulo, updatedCampaign.Descripcion, updatedCampaign.MetaMonto, updatedCampaign.MontoRecaudado, updatedCampaign.FechaInicio, updatedCampaign.FechaFin, updatedCampaign.Estado, updatedCampaign.UsuarioId);
    }

    public bool DeleteCampaign(int id)
    {
        return campaignRepository.DeleteCampaign(id);
    }
}