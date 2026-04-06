using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    /// <summary>
    /// Controlador para la gestión de comentarios en posts de Love4Animals
    /// </summary>
    [Route("v1/posts/{postId}/comments")]
    [ApiController]
    [Tags("Comentarios")]
    public class CommentController : ControllerBase
    {
        private ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        /// <summary>
        /// Obtiene un comentario por su ID dentro de un post específico
        /// </summary>
        /// <param name="postId">ID del post</param>
        /// <param name="id">ID del comentario</param>
        /// <returns>Información del comentario</returns>
        /// <response code="200">Comentario encontrado</response>
        /// <response code="404">Comentario no encontrado o no pertenece al post especificado</response>
        [HttpGet("{id}")]
        [ProducesResponseType<GetCommentDto>(200)]
        [ProducesResponseType(404)]
        public ActionResult<GetCommentDto> GetCommentById(int postId, int id)
        {
            var comment = this.commentService.GetCommentByIdAndPostId(id, postId);
            
            if (comment == null)
                return NotFound(new { error = "Not Found", message = $"Comentario con ID {id} no encontrado en el post {postId}", statusCode = 404 });

            return Ok(comment);
        }

        /// <summary>
        /// Crea un nuevo comentario en un post específico
        /// </summary>
        /// <param name="postId">ID del post donde crear el comentario</param>
        /// <param name="createCommentDto">Datos del comentario a crear</param>
        /// <returns>Comentario creado</returns>
        /// <response code="201">Comentario creado exitosamente</response>
        /// <response code="400">Datos inválidos o ID del post no coincide</response>
        [HttpPost("")]
        [ProducesResponseType<GetCommentDto>(201)]
        [ProducesResponseType(400)]
        public ActionResult<GetCommentDto> CreateComment(int postId, [FromBody] CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            // Validar que el IdPost del comentario coincida con el postId de la URL
            if (createCommentDto.IdPost != postId)
                return BadRequest(new { error = "Bad Request", message = $"El comentario debe pertenecer al post {postId}", statusCode = 400 });

            try
            {
                GetCommentDto newComment = this.commentService.CreateComment(createCommentDto);
                return Created("", newComment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Bad Request", message = ex.Message, statusCode = 400 });
            }
        }

        /// <summary>
        /// Actualiza un comentario existente dentro de un post específico
        /// </summary>
        /// <param name="postId">ID del post</param>
        /// <param name="id">ID del comentario a actualizar</param>
        /// <param name="updateCommentDto">Datos actualizados del comentario</param>
        /// <returns>Comentario actualizado</returns>
        /// <response code="200">Comentario actualizado exitosamente</response>
        /// <response code="400">Datos inválidos o ID no coincide</response>
        /// <response code="404">Comentario no encontrado o no pertenece al post</response>
        [HttpPut("{id}")]
        [ProducesResponseType<GetCommentDto>(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<GetCommentDto> UpdateComment(int postId, int id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            if (id != updateCommentDto.IdComment)
                return BadRequest(new { error = "Bad Request", message = "El ID de la URL no coincide con el ID del body", statusCode = 400 });

            // Validar que el comentario pertenece al post especificado
            if (updateCommentDto.IdPost != postId)
                return BadRequest(new { error = "Bad Request", message = $"El comentario no pertenece al post {postId}", statusCode = 400 });

            try
            {
                GetCommentDto updatedComment = this.commentService.UpdateComment(updateCommentDto);
                return Ok(updatedComment);
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
        /// Elimina un comentario de un post específico
        /// </summary>
        /// <param name="postId">ID del post</param>
        /// <param name="id">ID del comentario a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Comentario eliminado exitosamente</response>
        /// <response code="404">Comentario no encontrado o no pertenece al post</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult DeleteComment(int postId, int id)
        {
            bool deleted = this.commentService.DeleteCommentByIdAndPostId(id, postId);
            if (!deleted)
                return NotFound(new { error = "Not Found", message = $"Comentario con ID {id} no encontrado en el post {postId}", statusCode = 404 });

            return Ok(new { message = "Comentario eliminado exitosamente", deleted = true, statusCode = 200 });
        }
    }
}