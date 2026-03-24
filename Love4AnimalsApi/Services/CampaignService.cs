using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class CampaignService : ICampaignService
{
    private ICampaignRepository campaignRepository;
    
    public CampaignService(ICampaignRepository campaignRepository)
    {
        this.campaignRepository = campaignRepository;
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

    public GetCampaignDto CreateCampaign(CreateCampaignDto createCampaignDto)
    {
        Campaign newCampaign = new(0, createCampaignDto.Titulo, createCampaignDto.Descripcion, createCampaignDto.MetaMonto, 0.00m, createCampaignDto.FechaInicio, createCampaignDto.FechaFin, createCampaignDto.Estado, createCampaignDto.UsuarioId);
        Campaign createdCampaign = campaignRepository.CreateCampaign(newCampaign);
        return new GetCampaignDto(createdCampaign.IdCampania, createdCampaign.Titulo, createdCampaign.Descripcion, createdCampaign.MetaMonto, createdCampaign.MontoRecaudado, createdCampaign.FechaInicio, createdCampaign.FechaFin, createdCampaign.Estado, createdCampaign.UsuarioId);
    }

    public GetCampaignDto UpdateCampaign(UpdateCampaignDto updateCampaignDto)
    {
        Campaign campaignToUpdate = new(updateCampaignDto.IdCampania, updateCampaignDto.Titulo, updateCampaignDto.Descripcion, updateCampaignDto.MetaMonto, updateCampaignDto.MontoRecaudado, updateCampaignDto.FechaInicio, updateCampaignDto.FechaFin, updateCampaignDto.Estado, updateCampaignDto.UsuarioId);
        Campaign updatedCampaign = campaignRepository.UpdateCampaign(campaignToUpdate);
        return new GetCampaignDto(updatedCampaign.IdCampania, updatedCampaign.Titulo, updatedCampaign.Descripcion, updatedCampaign.MetaMonto, updatedCampaign.MontoRecaudado, updatedCampaign.FechaInicio, updatedCampaign.FechaFin, updatedCampaign.Estado, updatedCampaign.UsuarioId);
    }

    public bool DeleteCampaign(int id)
    {
        return campaignRepository.DeleteCampaign(id);
    }
}