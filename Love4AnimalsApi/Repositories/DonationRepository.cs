using System;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Love4AnimalsApi.Repositories;

public class DonationRepository : IDonationRepository
{
    private List<Donation> Donations { get; set; }

    public DonationRepository()
    {
        this.Donations = [];
        Donation newDonation = new(1, 150.00m, "Tarjeta de crédito", "COMPROBANTE-12345", DateTime.Now, 1, 1);
        this.Donations.Add(newDonation);
    }

    public Donation? GetDonationById(int id)
    {
        return this.Donations.FirstOrDefault(d => d.IdDonation == id);
    }

    public Donation CreateDonation(Donation donation)
    {
        int newId = this.Donations.Any() ? this.Donations.Max(d => d.IdDonation) + 1 : 1;
        donation.IdDonation = newId;
        this.Donations.Add(donation);
        return donation;
    }

    public Donation UpdateDonation(Donation donation)
    {
        Donation? existingDonation = this.Donations.FirstOrDefault(d => d.IdDonation == donation.IdDonation);
        if (existingDonation == null)
            throw new Exception($"Donación con ID {donation.IdDonation} no encontrada");

        existingDonation.Monto = donation.Monto;
        existingDonation.MetodoPago = donation.MetodoPago;
        existingDonation.Comprobante = donation.Comprobante;
        existingDonation.Fecha = donation.Fecha;
        existingDonation.UsuarioId = donation.UsuarioId;
        existingDonation.IdCampania = donation.IdCampania;
        return existingDonation;
    }

    public bool DeleteDonation(int id)
    {
        Donation? donation = this.Donations.FirstOrDefault(d => d.IdDonation == id);
        if (donation == null)
            return false;

        this.Donations.Remove(donation);
        return true;
    }
}