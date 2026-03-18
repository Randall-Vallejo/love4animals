using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public GetUserDto GetUser(int Id)
        {
            return this.userService.GetUser();
        } 
    }
}
