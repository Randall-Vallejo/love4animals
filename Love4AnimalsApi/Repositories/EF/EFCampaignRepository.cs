using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Repositories.EF;

/// <summary>
/// Implementación de ICampaignRepository usando Entity Framework Core
/// </summary>
public class EFCampaignRepository : ICampaignRepository
{
    private readonly AppDbContext _context;

    public EFCampaignRepository(AppDbContext context)
    {
        this._context = context;
    }

    public Campaign? GetCampaignById(int id)
    {
        return _context.Campaigns
            .Include(c => c.Usuario)
            .FirstOrDefault(c => c.IdCampania == id);
    }

    public IEnumerable<Campaign> GetAllCampaigns()
    {
        return _context.Campaigns
            .Include(c => c.Usuario)
            .OrderByDescending(c => c.FechaInicio)
            .ToList();
    }

    public Campaign CreateCampaign(Campaign campaign)
    {
        _context.Campaigns.Add(campaign);
        _context.SaveChanges();
        return campaign;
    }

    public Campaign UpdateCampaign(Campaign campaign)
    {
        Campaign? existingCampaign = _context.Campaigns.FirstOrDefault(c => c.IdCampania == campaign.IdCampania);
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

        _context.Campaigns.Update(existingCampaign);
        _context.SaveChanges();
        return existingCampaign;
    }

    public bool DeleteCampaign(int id)
    {
        Campaign? campaignToDelete = _context.Campaigns.FirstOrDefault(c => c.IdCampania == id);
        if (campaignToDelete == null)
            return false;

        _context.Campaigns.Remove(campaignToDelete);
        _context.SaveChanges();
        return true;
    }
}