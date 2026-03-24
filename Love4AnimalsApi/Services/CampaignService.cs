using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using System.Linq;
using System.Collections.Generic;

namespace Love4AnimalsApi.Services;

public class CampaignService : ICampaignService
{
    private ICampaignRepository campaignRepository;
    public CampaignService(ICampaignRepository campaignRepository)
    {
        this.campaignRepository = campaignRepository;
    }
    public List<GetCampaignDto> GetAllCampaigns()
    {
        List<Campaign> campaigns = campaignRepository.GetAllCampaigns();
        List<GetCampaignDto> response = campaigns.Select(c => new GetCampaignDto(c.IdCampania, c.Titulo, c.Descripcion, c.MetaMonto, c.MontoRecaudado, c.FechaInicio, c.FechaFin, c.Estado, c.UsuarioId)).ToList();
        return response;
    }
    public GetCampaignDto CreateCampaign(CreateCampaignDto createCampaignDto)
    {
        Campaign newCampaign = new(0, createCampaignDto.Titulo, createCampaignDto.Descripcion, createCampaignDto.MetaMonto, 0.00m, createCampaignDto.FechaInicio, createCampaignDto.FechaFin, createCampaignDto.Estado, createCampaignDto.UsuarioId);
        Campaign createdCampaign = campaignRepository.CreateCampaign(newCampaign);
        GetCampaignDto response = new(createdCampaign.IdCampania, createdCampaign.Titulo, createdCampaign.Descripcion, createdCampaign.MetaMonto, createdCampaign.MontoRecaudado, createdCampaign.FechaInicio, createdCampaign.FechaFin, createdCampaign.Estado, createdCampaign.UsuarioId);
        return response;
    }
    public GetCampaignDto UpdateCampaign(UpdateCampaignDto updateCampaignDto)
    {
        Campaign campaignToUpdate = new(updateCampaignDto.IdCampania, updateCampaignDto.Titulo, updateCampaignDto.Descripcion, updateCampaignDto.MetaMonto, updateCampaignDto.MontoRecaudado, updateCampaignDto.FechaInicio, updateCampaignDto.FechaFin, updateCampaignDto.Estado, updateCampaignDto.UsuarioId);
        Campaign updatedCampaign = campaignRepository.UpdateCampaign(campaignToUpdate);
        GetCampaignDto response = new(updatedCampaign.IdCampania, updatedCampaign.Titulo, updatedCampaign.Descripcion, updatedCampaign.MetaMonto, updatedCampaign.MontoRecaudado, updatedCampaign.FechaInicio, updatedCampaign.FechaFin, updatedCampaign.Estado, updatedCampaign.UsuarioId);
        return response;
    }
    public bool DeleteCampaign(int id)
    {
        return campaignRepository.DeleteCampaign(id);
    }
}