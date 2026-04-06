using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    /// <summary>
    /// Controlador para la gestión de posts en Love4Animals
    /// </summary>
    [Route("v1/posts")]
    [ApiController]
    [Tags("Posts")]
    public class PostController : ControllerBase
    {
        private IPostService postService;
        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        /// <summary>
        /// Obtiene un post por su ID
        /// </summary>
        /// <param name="id">ID del post</param>
        /// <returns>Información del post</returns>
        /// <response code="200">Post encontrado</response>
        /// <response code="404">Post no encontrado</response>
        /// <summary>
        /// Obtiene un post por su ID
        /// </summary>
        /// <param name="id">ID del post</param>
        /// <returns>Información del post</returns>
        /// <response code="200">Post encontrado</response>
        /// <response code="404">Post no encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType<GetPostDto>(200)]
        [ProducesResponseType(404)]
        public ActionResult<GetPostDto> GetPostById(int id)
        {
            var post = this.postService.GetPostById(id);
            
            if (post == null)
                return NotFound(new { error = "Not Found", message = $"Post con ID {id} no encontrado", statusCode = 404 });

            return Ok(post);
        }

        /// <summary>
        /// Crea un nuevo post
        /// </summary>
        /// <param name="createPostDto">Datos del post a crear</param>
        /// <returns>Post creado</returns>
        /// <response code="201">Post creado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost("")]
        [ProducesResponseType<GetPostDto>(201)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Actualiza un post existente
        /// </summary>
        /// <param name="id">ID del post a actualizar</param>
        /// <param name="updatePostDto">Datos actualizados del post</param>
        /// <returns>Post actualizado</returns>
        /// <response code="200">Post actualizado exitosamente</response>
        /// <response code="400">Datos inválidos o ID no coincide</response>
        /// <response code="404">Post no encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType<GetPostDto>(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Elimina un post del sistema
        /// </summary>
        /// <param name="id">ID del post a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Post eliminado exitosamente</response>
        /// <response code="404">Post no encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult DeletePost(int id)
        {
            bool deleted = this.postService.DeletePost(id);
            if (!deleted)
                return NotFound(new { error = "Not Found", message = $"Post con ID {id} no encontrado", statusCode = 404 });

            return Ok(new { message = "Post eliminado exitosamente", deleted = true, statusCode = 200 });
        }
    }
}