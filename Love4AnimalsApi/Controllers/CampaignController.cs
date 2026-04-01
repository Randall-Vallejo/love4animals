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

        [HttpGet("{id}")]
        public ActionResult<GetCampaignDto> GetCampaignById(int id)
        {
            var campaign = this.campaignService.GetCampaignById(id);
            
            if (campaign == null)
                return NotFound(new { error = "Not Found", message = $"Campaña con ID {id} no encontrada", statusCode = 404 });

            return Ok(campaign);
        }

        [HttpPost("")]
        public ActionResult<GetCampaignDto> CreateCampaign([FromBody] CreateCampaignDto createCampaignDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            try
            {
                GetCampaignDto newCampaign = this.campaignService.CreateCampaign(createCampaignDto);
                return Created("", newCampaign);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Bad Request", message = ex.Message, statusCode = 400 });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<GetCampaignDto> UpdateCampaign(int id, [FromBody] UpdateCampaignDto updateCampaignDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            if (id != updateCampaignDto.IdCampania)
                return BadRequest(new { error = "Bad Request", message = "El ID de la URL no coincide con el ID del body", statusCode = 400 });

            try
            {
                GetCampaignDto updatedCampaign = this.campaignService.UpdateCampaign(updateCampaignDto);
                return Ok(updatedCampaign);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Bad Request", message = ex.Message, statusCode = 400 });
            }
            catch (Exception ex)
            {
                return NotFound(new { error = "Not Found", message = ex.Message, statusCode = 404 });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCampaign(int id)
        {
            bool deleted = this.campaignService.DeleteCampaign(id);
            if (!deleted)
                return NotFound(new { error = "Not Found", message = $"Campaña con ID {id} no encontrada", statusCode = 404 });

            return Ok(new { message = "Campaña eliminada exitosamente", deleted = true });
        }
    }
}