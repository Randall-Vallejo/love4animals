using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/users/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private IPostService postService;
        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet("{id}")]
        public ActionResult<GetPostDto> GetPostById(int id)
        {
            var post = this.postService.GetPostById(id);
            
            if (post == null)
                return NotFound(new { error = "Not Found", message = $"Post con ID {id} no encontrado", statusCode = 404 });

            return Ok(post);
        }

        [HttpPost("")]
        public ActionResult<GetPostDto> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            try
            {
                GetPostDto newPost = this.postService.CreatePost(createPostDto);
                return Created("", newPost);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Bad Request", message = ex.Message, statusCode = 400 });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<GetPostDto> UpdatePost(int id, [FromBody] UpdatePostDto updatePostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            if (id != updatePostDto.IdPost)
                return BadRequest(new { error = "Bad Request", message = "El ID de la URL no coincide con el ID del body", statusCode = 400 });

            try
            {
                GetPostDto updatedPost = this.postService.UpdatePost(updatePostDto);
                return Ok(updatedPost);
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
        public ActionResult DeletePost(int id)
        {
            bool deleted = this.postService.DeletePost(id);
            if (!deleted)
                return NotFound(new { error = "Not Found", message = $"Post con ID {id} no encontrado", statusCode = 404 });

            return Ok(new { message = "Post eliminado exitosamente", deleted = true, statusCode = 200 });
        }
    }
}