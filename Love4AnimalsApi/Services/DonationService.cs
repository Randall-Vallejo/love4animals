using System;
using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Love4AnimalsApi.Services;

public class DonationService : IDonationService
{
    private readonly IDonationRepository donationRepository;
    private readonly IUserRepository userRepository;
    private readonly ICampaignRepository campaignRepository;
    private readonly IDistributedCache cache;
    private readonly JsonSerializerOptions jsonOptions = new() { PropertyNamingPolicy = null };

    public DonationService(IDonationRepository donationRepository, IUserRepository userRepository, ICampaignRepository campaignRepository, IDistributedCache cache)
    {
        this.donationRepository = donationRepository;
        this.userRepository = userRepository;
        this.campaignRepository = campaignRepository;
        this.cache = cache;
    }

    public GetDonationDto? GetDonationById(int id)
    {
        var key = CacheConstants.DonationItem(id);
        try
        {
            var cached = cache.GetString(key);
            if (!string.IsNullOrEmpty(cached))
            {
                var dto = JsonSerializer.Deserialize<GetDonationDto>(cached, jsonOptions);
                if (dto != null) return dto;
            }
        }
        catch { }

        Donation? donation = donationRepository.GetDonationById(id);
        if (donation == null) return null;

        var result = new GetDonationDto(
            donation.IdDonation,
            donation.Monto,
            donation.MetodoPago,
            donation.Comprobante,
            donation.Fecha,
            donation.UsuarioId,
            donation.IdCampania
        );

        try
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheConstants.DefaultTtlMinutes) };
            cache.SetString(key, JsonSerializer.Serialize(result, jsonOptions), options);
        }
        catch { }

        return result;
    }

    public IEnumerable<GetDonationDto> GetAllDonations()
    {
        const string key = "donations:all";
        var cached = cache.GetString(key);
        if (!string.IsNullOrEmpty(cached))
        {
            try
            {
                var dtoList = JsonSerializer.Deserialize<IEnumerable<GetDonationDto>>(cached, jsonOptions);
                if (dtoList != null) return dtoList;
            }
            catch { }
        }

        var donations = donationRepository.GetAllDonations();
        var result = donations.Select(d => new GetDonationDto(
            d.IdDonation,
            d.Monto,
            d.MetodoPago,
            d.Comprobante,
            d.Fecha,
            d.UsuarioId,
            d.IdCampania
        )).ToList();

        try
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheConstants.DefaultTtlMinutes) };
            cache.SetString(key, JsonSerializer.Serialize(result, jsonOptions), options);
        }
        catch { }
        return result;
    }

    public GetDonationDto CreateDonation(CreateDonationDto createDonationDto)
    {
        var user = userRepository.GetUserById(createDonationDto.UsuarioId);
        if (user == null)
            throw new ArgumentException("Usuario no encontrado");

        if (!string.Equals(user.Rol, "User", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Solo usuarios con rol User pueden realizar donaciones");

        var campaign = campaignRepository.GetCampaignById(createDonationDto.IdCampania);
        if (campaign == null)
            throw new ArgumentException("Campaña no encontrada");

        Donation newDonation = new(
            0,
            createDonationDto.Monto,
            createDonationDto.MetodoPago,
            createDonationDto.Comprobante,
            DateTime.UtcNow,
            createDonationDto.UsuarioId,
            createDonationDto.IdCampania
        );

        Donation createdDonation = donationRepository.CreateDonation(newDonation);
        try { cache.Remove(CacheConstants.DonationsAll); } catch { }
        try { cache.Remove(CacheConstants.DonationItem(createdDonation.IdDonation)); } catch { }
        return new GetDonationDto(
            createdDonation.IdDonation,
            createdDonation.Monto,
            createdDonation.MetodoPago,
            createdDonation.Comprobante,
            createdDonation.Fecha,
            createdDonation.UsuarioId,
            createdDonation.IdCampania
        );
    }

    public GetDonationDto UpdateDonation(UpdateDonationDto updateDonationDto)
    {
        var user = userRepository.GetUserById(updateDonationDto.UsuarioId);
        if (user == null)
            throw new ArgumentException("Usuario no encontrado");

        if (!string.Equals(user.Rol, "User", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Solo usuarios con rol User pueden realizar donaciones");

        var campaign = campaignRepository.GetCampaignById(updateDonationDto.IdCampania);
        if (campaign == null)
            throw new ArgumentException("Campaña no encontrada");

        DateTime fechaUtc = updateDonationDto.Fecha.ToUniversalTime();
        Donation donationToUpdate = new(
            updateDonationDto.IdDonation,
            updateDonationDto.Monto,
            updateDonationDto.MetodoPago,
            updateDonationDto.Comprobante,
            fechaUtc,
            updateDonationDto.UsuarioId,
            updateDonationDto.IdCampania
        );

        Donation updatedDonation = donationRepository.UpdateDonation(donationToUpdate);
        try { cache.Remove(CacheConstants.DonationsAll); } catch { }
        try { cache.Remove(CacheConstants.DonationItem(updatedDonation.IdDonation)); } catch { }
        return new GetDonationDto(
            updatedDonation.IdDonation,
            updatedDonation.Monto,
            updatedDonation.MetodoPago,
            updatedDonation.Comprobante,
            updatedDonation.Fecha,
            updatedDonation.UsuarioId,
            updatedDonation.IdCampania
        );
    }

    public bool DeleteDonation(int id)
    {
        var ok = donationRepository.DeleteDonation(id);
        if (ok)
        {
            try { cache.Remove(CacheConstants.DonationsAll); } catch { }
            try { cache.Remove(CacheConstants.DonationItem(id)); } catch { }
        }
        return ok;
    }
}