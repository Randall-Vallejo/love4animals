using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        public UserController (IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet("")]
        public List<GetUserDto> GetAllUsers()
        {
            return this.userService.GetAllUsers();
        }
        [HttpPost("")]
        public ActionResult<GetUserDto> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            GetUserDto newUser = this.userService.CreateUser(createUserDto);
            return Created("", newUser);
        }
        [HttpPut("{id}")]
        public ActionResult<GetUserDto> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (id != updateUserDto.Id)
                return BadRequest("El ID de la URL no coincide con el ID del body");
            
            GetUserDto updatedUser = this.userService.UpdateUser(updateUserDto);
            return Ok(updatedUser);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            bool deleted = this.userService.DeleteUser(id);
            if (!deleted)
                return NotFound($"Usuario con ID {id} no encontrado");
            
            return NoContent();
        }
    }
}
