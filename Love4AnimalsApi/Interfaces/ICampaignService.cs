using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface ICampaignService
{
    public GetCampaignDto? GetCampaignById(int id); 
    public GetCampaignDto CreateCampaign(CreateCampaignDto createCampaignDto);
    public GetCampaignDto UpdateCampaign(UpdateCampaignDto updateCampaignDto);
    public bool DeleteCampaign(int id);
}