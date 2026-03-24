using System;
using Love4AnimalsApi.Models;
using System.Collections.Generic;

namespace Love4AnimalsApi.Interfaces;

public interface ICampaignRepository
{
    public List<Campaign> GetAllCampaigns();
    public Campaign CreateCampaign(Campaign campaign);
    public Campaign UpdateCampaign(Campaign campaign);
    public bool DeleteCampaign(int id);
}