using Love4AnimalsApi.Dtos;
using System.Collections.Generic;

namespace Love4AnimalsApi.Interfaces;

public interface ICampaignService
{
    public List<GetCampaignDto> GetAllCampaigns();
    public GetCampaignDto CreateCampaign(CreateCampaignDto createCampaignDto);
    public GetCampaignDto UpdateCampaign(UpdateCampaignDto updateCampaignDto);
    public bool DeleteCampaign(int id);
}