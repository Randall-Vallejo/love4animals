using System;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Love4AnimalsApi.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private List<Campaign> Campaigns { get; set; }

    public CampaignRepository()
    {
        this.Campaigns = [];
        Campaign newCampaign = new(1, "Campaña de Refugio", "Ayuda a construir un refugio para animales abandonados", 5000.00m, 1250.50m, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(20), "Activa", 1);
        this.Campaigns.Add(newCampaign);
    }
    public List<Campaign> GetAllCampaigns()
    {
        return this.Campaigns;
    }
    public Campaign CreateCampaign(Campaign campaign)
    {
        // Generar un nuevo ID
        int newId = this.Campaigns.Any() ? this.Campaigns.Max(c => c.IdCampania) + 1 : 1;
        campaign.IdCampania = newId;
        this.Campaigns.Add(campaign);
        return campaign;
    }
    public Campaign UpdateCampaign(Campaign campaign)
    {
        Campaign? existingCampaign = this.Campaigns.FirstOrDefault(c => c.IdCampania == campaign.IdCampania);
        if (existingCampaign == null)
            throw new Exception($"Campaña con ID {campaign.IdCampania} no encontrada");

        existingCampaign.Titulo = campaign.Titulo;
        existingCampaign.Descripcion = campaign.Descripcion;
        existingCampaign.MetaMonto = campaign.MetaMonto;
        existingCampaign.MontoRecaudado = campaign.MontoRecaudado;
        existingCampaign.FechaInicio = campaign.FechaInicio;
        existingCampaign.FechaFin = campaign.FechaFin;
        existingCampaign.Estado = campaign.Estado;
        existingCampaign.UsuarioId = campaign.UsuarioId;
        return existingCampaign;
    }
    public bool DeleteCampaign(int id)
    {
        Campaign? campaignToDelete = this.Campaigns.FirstOrDefault(c => c.IdCampania == id);
        if (campaignToDelete == null)
            return false;

        this.Campaigns.Remove(campaignToDelete);
        return true;
    }
}