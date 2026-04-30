using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Repositories.EF;

/// <summary>
/// Implementación de IDonationRepository usando Entity Framework Core
/// </summary>
public class EFDonationRepository : IDonationRepository
{
    private readonly AppDbContext _context;

    public EFDonationRepository(AppDbContext context)
    {
        this._context = context;
    }

    public Donation? GetDonationById(int id)
    {
        return _context.Donations
            .Include(d => d.Usuario)
            .Include(d => d.Campania)
            .FirstOrDefault(d => d.IdDonation == id);
    }

    public Donation CreateDonation(Donation donation)
    {
        _context.Donations.Add(donation);
        _context.SaveChanges();
        return donation;
    }

    public Donation UpdateDonation(Donation donation)
    {
        Donation? existingDonation = _context.Donations.FirstOrDefault(d => d.IdDonation == donation.IdDonation);
        if (existingDonation == null)
            throw new Exception($"Donación con ID {donation.IdDonation} no encontrada");

        existingDonation.Monto = donation.Monto;
        existingDonation.MetodoPago = donation.MetodoPago;
        existingDonation.Comprobante = donation.Comprobante;
        existingDonation.Fecha = donation.Fecha;
        existingDonation.UsuarioId = donation.UsuarioId;
        existingDonation.IdCampania = donation.IdCampania;

        _context.Donations.Update(existingDonation);
        _context.SaveChanges();
        return existingDonation;
    }

    public bool DeleteDonation(int id)
    {
        Donation? donationToDelete = _context.Donations.FirstOrDefault(d => d.IdDonation == id);
        if (donationToDelete == null)
            return false;

        _context.Donations.Remove(donationToDelete);
        _context.SaveChanges();
        return true;
    }
}