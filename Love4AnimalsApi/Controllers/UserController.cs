using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public ActionResult<GetUserDto> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            
            if (user == null)
                return NotFound(new { error = "Not Found", message = $"Usuario con ID {id} no encontrado", statusCode = 404 });

            return Ok(user);
        }

        [HttpPost("")]
        public ActionResult<GetUserDto> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            GetUserDto newUser = _userService.CreateUser(createUserDto);
            return Created("", newUser);
        }

        [HttpPut("{id}")]
        public ActionResult<GetUserDto> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            if (id != updateUserDto.Id)
                return BadRequest(new { error = "Bad Request", message = "El ID de la URL no coincide con el ID del body", statusCode = 400 });

            try
            {
                GetUserDto updatedUser = _userService.UpdateUser(updateUserDto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = "Not Found", message = ex.Message, statusCode = 404 });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            bool deleted = _userService.DeleteUser(id);
            if (!deleted)
                return NotFound(new { error = "Not Found", message = $"Usuario con ID {id} no encontrado", statusCode = 404 });

            return NoContent();
        }
    }
}