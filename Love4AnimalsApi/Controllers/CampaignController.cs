using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    /// <summary>
    /// Controlador para la gestión de campañas de recaudación en Love4Animals
    /// </summary>
    [Route("v1/campaigns")]
    [ApiController]
    [Tags("Campañas")]
    public class CampaignController : ControllerBase
    {
        private ICampaignService campaignService;
        public CampaignController(ICampaignService campaignService)
        {
            this.campaignService = campaignService;
        }

        /// <summary>
        /// Obtiene una campaña por su ID
        /// </summary>
        /// <param name="id">ID de la campaña</param>
        /// <returns>Información de la campaña</returns>
        /// <response code="200">Campaña encontrada</response>
        /// <response code="404">Campaña no encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType<GetCampaignDto>(200)]
        [ProducesResponseType(404)]
        public ActionResult<GetCampaignDto> GetCampaignById(int id)
        {
            var campaign = this.campaignService.GetCampaignById(id);
            
            if (campaign == null)
                return NotFound(new { error = "Not Found", message = $"Campaña con ID {id} no encontrada", statusCode = 404 });

            return Ok(campaign);
        }

        /// <summary>
        /// Crea una nueva campaña de recaudación
        /// </summary>
        /// <param name="createCampaignDto">Datos de la campaña a crear</param>
        /// <returns>Campaña creada</returns>
        /// <response code="201">Campaña creada exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost("")]
        [ProducesResponseType<GetCampaignDto>(201)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Actualiza una campaña existente
        /// </summary>
        /// <param name="id">ID de la campaña a actualizar</param>
        /// <param name="updateCampaignDto">Datos actualizados de la campaña</param>
        /// <returns>Campaña actualizada</returns>
        /// <response code="200">Campaña actualizada exitosamente</response>
        /// <response code="400">Datos inválidos o ID no coincide</response>
        /// <response code="404">Campaña no encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType<GetCampaignDto>(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Elimina una campaña del sistema
        /// </summary>
        /// <param name="id">ID de la campaña a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Campaña eliminada exitosamente</response>
        /// <response code="404">Campaña no encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult DeleteCampaign(int id)
        {
            bool deleted = this.campaignService.DeleteCampaign(id);
            if (!deleted)
                return NotFound(new { error = "Not Found", message = $"Campaña con ID {id} no encontrada", statusCode = 404 });

            return Ok(new { message = "Campaña eliminada exitosamente", deleted = true, statusCode = 200 });
        }
    }
}