using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    /// <summary>
    /// Controlador para la gestión de donaciones en Love4Animals
    /// </summary>
    [Route("v1/donations")]
    [ApiController]
    [Tags("Donaciones")]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService donationService;

        public DonationController(IDonationService donationService)
        {
            this.donationService = donationService;
        }

        /// <summary>
        /// Obtiene una donación por su ID
        /// </summary>
        /// <param name="id">ID de la donación</param>
        /// <returns>Información de la donación</returns>
        /// <response code="200">Donación encontrada</response>
        /// <response code="404">Donación no encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType<GetDonationDto>(200)]
        [ProducesResponseType(404)]
        public ActionResult<GetDonationDto> GetDonationById(int id)
        {
            var donation = this.donationService.GetDonationById(id);
            if (donation == null)
                return NotFound(new { error = "Not Found", message = $"Donación con ID {id} no encontrada", statusCode = 404 });

            return Ok(donation);
        }

        /// <summary>
        /// Crea una nueva donación
        /// </summary>
        /// <param name="createDonationDto">Datos de la donación a crear</param>
        /// <returns>Donación creada</returns>
        /// <response code="201">Donación creada exitosamente</response>
        /// <response code="400">Datos inválidos o usuario/campaña no válidos</response>
        [HttpPost("")]
        [ProducesResponseType<GetDonationDto>(201)]
        [ProducesResponseType(400)]
        public ActionResult<GetDonationDto> CreateDonation([FromBody] CreateDonationDto createDonationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            try
            {
                GetDonationDto newDonation = this.donationService.CreateDonation(createDonationDto);
                return Created("", newDonation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Bad Request", message = ex.Message, statusCode = 400 });
            }
        }

        /// <summary>
        /// Actualiza una donación existente
        /// </summary>
        /// <param name="id">ID de la donación a actualizar</param>
        /// <param name="updateDonationDto">Datos actualizados de la donación</param>
        /// <returns>Donación actualizada</returns>
        /// <response code="200">Donación actualizada exitosamente</response>
        /// <response code="400">Datos inválidos o ID no coincide</response>
        /// <response code="404">Donación no encontrada</response>
        [HttpPut("{id}")]
        [ProducesResponseType<GetDonationDto>(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<GetDonationDto> UpdateDonation(int id, [FromBody] UpdateDonationDto updateDonationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            if (id != updateDonationDto.IdDonation)
                return BadRequest(new { error = "Bad Request", message = "El ID de la URL no coincide con el ID del body", statusCode = 400 });

            try
            {
                GetDonationDto updatedDonation = this.donationService.UpdateDonation(updateDonationDto);
                return Ok(updatedDonation);
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
        /// Elimina una donación del sistema
        /// </summary>
        /// <param name="id">ID de la donación a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Donación eliminada exitosamente</response>
        /// <response code="404">Donación no encontrada</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult DeleteDonation(int id)
        {
            bool deleted = this.donationService.DeleteDonation(id);
            if (!deleted)
                return NotFound(new { error = "Not Found", message = $"Donación con ID {id} no encontrada", statusCode = 404 });

            return Ok(new { message = "Donación eliminada exitosamente", deleted = true, statusCode = 200 });
        }
    }
}