using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface IDonationService
{
    public GetDonationDto? GetDonationById(int id);
    public GetDonationDto CreateDonation(CreateDonationDto createDonationDto);
    public GetDonationDto UpdateDonation(UpdateDonationDto updateDonationDto);
    public bool DeleteDonation(int id);
}