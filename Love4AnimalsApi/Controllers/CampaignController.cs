using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/campaigns")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private ICampaignService campaignService;
        public CampaignController (ICampaignService campaignService)
        {
            this.campaignService = campaignService;
        }
        [HttpGet("")]
        public List<GetCampaignDto> GetAllCampaigns()
        {
            return this.campaignService.GetAllCampaigns();
        }
        [HttpPost("")]
        public ActionResult<GetCampaignDto> CreateCampaign([FromBody] CreateCampaignDto createCampaignDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            GetCampaignDto newCampaign = this.campaignService.CreateCampaign(createCampaignDto);
            return Created("", newCampaign);
        }
        [HttpPut("{id}")]
        public ActionResult<GetCampaignDto> UpdateCampaign(int id, [FromBody] UpdateCampaignDto updateCampaignDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updateCampaignDto.IdCampania)
                return BadRequest("El ID de la URL no coincide con el ID del body");

            try
            {
                GetCampaignDto updatedCampaign = this.campaignService.UpdateCampaign(updateCampaignDto);
                return Ok(updatedCampaign);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteCampaign(int id)
        {
            bool deleted = this.campaignService.DeleteCampaign(id);
            if (!deleted)
                return NotFound($"Campaña con ID {id} no encontrada");

            return NoContent();
        }
    }
}