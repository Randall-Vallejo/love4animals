using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/campaigns")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private ICampaignService campaignService;
        public CampaignController(ICampaignService campaignService)
        {
            this.campaignService = campaignService;
        }

        // NUEVO: Ahora pide el ID por la URL -> GET v1/campaigns/1
        [HttpGet("{id}")]
        public ActionResult<GetCampaignDto> GetCampaignById(int id)
        {
            var campaign = this.campaignService.GetCampaignById(id);
            
            if (campaign == null)
                return NotFound($"Campaña con ID {id} no encontrada");

            return Ok(campaign);
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