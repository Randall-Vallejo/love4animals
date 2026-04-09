using System;
using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class DonationService : IDonationService
{
    private readonly IDonationRepository donationRepository;
    private readonly IUserRepository userRepository;
    private readonly ICampaignRepository campaignRepository;

    public DonationService(IDonationRepository donationRepository, IUserRepository userRepository, ICampaignRepository campaignRepository)
    {
        this.donationRepository = donationRepository;
        this.userRepository = userRepository;
        this.campaignRepository = campaignRepository;
    }

    public GetDonationDto? GetDonationById(int id)
    {
        Donation? donation = donationRepository.GetDonationById(id);
        if (donation == null) return null;

        return new GetDonationDto(
            donation.IdDonation,
            donation.Monto,
            donation.MetodoPago,
            donation.Comprobante,
            donation.Fecha,
            donation.UsuarioId,
            donation.IdCampania
        );
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
            createDonationDto.Fecha,
            createDonationDto.UsuarioId,
            createDonationDto.IdCampania
        );

        Donation createdDonation = donationRepository.CreateDonation(newDonation);
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

        Donation donationToUpdate = new(
            updateDonationDto.IdDonation,
            updateDonationDto.Monto,
            updateDonationDto.MetodoPago,
            updateDonationDto.Comprobante,
            updateDonationDto.Fecha,
            updateDonationDto.UsuarioId,
            updateDonationDto.IdCampania
        );

        Donation updatedDonation = donationRepository.UpdateDonation(donationToUpdate);
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
        return donationRepository.DeleteDonation(id);
    }
}