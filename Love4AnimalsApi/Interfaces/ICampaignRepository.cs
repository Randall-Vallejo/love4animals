using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface ICampaignRepository
{
    public Campaign? GetCampaignById(int id); 
    public Campaign CreateCampaign(Campaign campaign);
    public Campaign UpdateCampaign(Campaign campaign);
    public bool DeleteCampaign(int id);
}