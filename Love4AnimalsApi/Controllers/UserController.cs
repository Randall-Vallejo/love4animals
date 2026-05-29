using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    /// <summary>
    /// Controlador para la gestión de usuarios en Love4Animals
    /// </summary>
    [Route("v1/users")]
    [ApiController]
    [Tags("Usuarios")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Información del usuario</returns>
        /// <response code="200">Usuario encontrado</response>
        /// <response code="404">Usuario no encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType<GetUserDto>(200)]
        [ProducesResponseType(404)]
        public ActionResult<GetUserDto> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            
            if (user == null)
                return NotFound(new { error = "Not Found", message = $"Usuario con ID {id} no encontrado", statusCode = 404 });

            return Ok(user);
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        /// <param name="createUserDto">Datos del usuario a registrar</param>
        /// <returns>Usuario registrado</returns>
        /// <response code="201">Usuario registrado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost("register")]
        [ProducesResponseType<GetUserDto>(201)]
        [ProducesResponseType(400)]
        public ActionResult<GetUserDto> RegisterUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            GetUserDto newUser = _userService.CreateUser(createUserDto);
            return Created("", newUser);
        }

        /// <summary>
        /// Inicia sesión con email y contraseña
        /// </summary>
        /// <param name="loginDto">Credenciales de acceso</param>
        /// <returns>Resultado del login</returns>
        /// <response code="200">Inicio de sesión exitoso</response>
        /// <response code="401">Credenciales incorrectas</response>
        [HttpPost("login")]
        [ProducesResponseType<LoginResponseDto>(200)]
        [ProducesResponseType(401)]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            LoginResponseDto? result = _userService.LoginUser(loginDto);
            if (result == null)
                return Unauthorized(new { error = "Unauthorized", message = "Email o contraseña incorrectos", statusCode = 401 });

            return Ok(result);
        }

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        /// <param name="id">ID del usuario a actualizar</param>
        /// <param name="updateUserDto">Datos actualizados del usuario</param>
        /// <returns>Usuario actualizado</returns>
        /// <response code="200">Usuario actualizado exitosamente</response>
        /// <response code="400">Datos inválidos o ID no coincide</response>
        /// <response code="404">Usuario no encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType<GetUserDto>(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Elimina un usuario del sistema
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Usuario eliminado exitosamente</response>
        /// <response code="404">Usuario no encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult DeleteUser(int id)
        {
            bool deleted = _userService.DeleteUser(id);
            if (!deleted)
                return NotFound(new { error = "Not Found", message = $"Usuario con ID {id} no encontrado", statusCode = 404 });

            return Ok(new { message = "Usuario eliminado exitosamente", deleted = true, statusCode = 200 });
        }
    }
}