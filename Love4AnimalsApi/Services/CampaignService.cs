using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Love4AnimalsApi.Services;

public class CampaignService : ICampaignService
{
    private ICampaignRepository campaignRepository;
    private IUserRepository userRepository;
    private readonly IDistributedCache cache;
    private readonly ILogger<CampaignService> logger;
    private readonly JsonSerializerOptions jsonOptions = new() { PropertyNamingPolicy = null };
    
    public CampaignService(ICampaignRepository campaignRepository, IUserRepository userRepository, IDistributedCache cache, ILogger<CampaignService> logger)
    {
        this.campaignRepository = campaignRepository;
        this.userRepository = userRepository;
        this.cache = cache;
        this.logger = logger;
    }

    public GetCampaignDto? GetCampaignById(int id)
    {
        var key = CacheConstants.CampaignItem(id);
        var cached = CacheOperations.TryGet<GetCampaignDto>(cache, key, jsonOptions, logger);
        if (cached != null) return cached;

        Campaign? campaign = campaignRepository.GetCampaignById(id);
        if (campaign == null) return null;

        var result = new GetCampaignDto(
            campaign.IdCampania, campaign.Titulo, campaign.Descripcion,
            campaign.MetaMonto, campaign.MontoRecaudado, campaign.FechaInicio,
            campaign.FechaFin, campaign.Estado, campaign.UsuarioId
        );

        CacheOperations.TrySet(cache, key, result, jsonOptions, logger);

        return result;
    }

    public IEnumerable<GetCampaignDto> GetAllCampaigns()
    {
        const string key = CacheConstants.CampaignsAll;
        var cached = CacheOperations.TryGet<IEnumerable<GetCampaignDto>>(cache, key, jsonOptions, logger);
        if (cached != null) return cached;

        var campaigns = campaignRepository.GetAllCampaigns();
        var result = campaigns.Select(c => new GetCampaignDto(
            c.IdCampania, c.Titulo, c.Descripcion,
            c.MetaMonto, c.MontoRecaudado, c.FechaInicio,
            c.FechaFin, c.Estado, c.UsuarioId
        )).ToList();

        CacheOperations.TrySet(cache, key, result, jsonOptions, logger);
        return result;
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
        CacheOperations.TryRemove(cache, CacheConstants.CampaignsAll, logger);
        CacheOperations.TryRemove(cache, CacheConstants.CampaignItem(createdCampaign.IdCampania), logger);
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
        CacheOperations.TryRemove(cache, CacheConstants.CampaignsAll, logger);
        CacheOperations.TryRemove(cache, CacheConstants.CampaignItem(updatedCampaign.IdCampania), logger);
        return new GetCampaignDto(updatedCampaign.IdCampania, updatedCampaign.Titulo, updatedCampaign.Descripcion, updatedCampaign.MetaMonto, updatedCampaign.MontoRecaudado, updatedCampaign.FechaInicio, updatedCampaign.FechaFin, updatedCampaign.Estado, updatedCampaign.UsuarioId);
    }

    public bool DeleteCampaign(int id)
    {
        var ok = campaignRepository.DeleteCampaign(id);
        if (ok)
        {
            CacheOperations.TryRemove(cache, CacheConstants.CampaignsAll, logger);
            CacheOperations.TryRemove(cache, CacheConstants.CampaignItem(id), logger);
        }
        return ok;
    }
}
