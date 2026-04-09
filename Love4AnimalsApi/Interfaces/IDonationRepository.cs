using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface IDonationRepository
{
    public Donation? GetDonationById(int id);
    public Donation CreateDonation(Donation donation);
    public Donation UpdateDonation(Donation donation);
    public bool DeleteDonation(int id);
}