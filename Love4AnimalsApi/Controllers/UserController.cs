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

        // 1. Obtener Usuario por ID (GET v1/users/1)
        [HttpGet("{id}")]
        public ActionResult<GetUserDto> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            
            if (user == null)
                return NotFound($"Usuario con ID {id} no encontrado");

            return Ok(user);
        }

        // 2. Crear Usuario (POST v1/users)
        [HttpPost("")]
        public ActionResult<GetUserDto> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            GetUserDto newUser = _userService.CreateUser(createUserDto);
            return Created("", newUser);
        }

        // 3. Actualizar Usuario por ID (PUT v1/users/1)
        [HttpPut("{id}")]
        public ActionResult<GetUserDto> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updateUserDto.Id)
                return BadRequest("El ID de la URL no coincide con el ID del body");

            try
            {
                GetUserDto updatedUser = _userService.UpdateUser(updateUserDto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // 4. Eliminar Usuario por ID (DELETE v1/users/1)
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            bool deleted = _userService.DeleteUser(id);
            if (!deleted)
                return NotFound($"Usuario con ID {id} no encontrado");

            return NoContent();
        }
    }
}