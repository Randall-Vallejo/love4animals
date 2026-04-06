using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/posts/{postId}/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet("{id}")]
        public ActionResult<GetCommentDto> GetCommentById(int id)
        {
            var comment = this.commentService.GetCommentById(id);
            
            if (comment == null)
                return NotFound(new { error = "Not Found", message = $"Comentario con ID {id} no encontrado", statusCode = 404 });

            return Ok(comment);
        }

        [HttpPost("")]
        public ActionResult<GetCommentDto> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

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

        [HttpPut("{id}")]
        public ActionResult<GetCommentDto> UpdateComment(int id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Bad Request", message = "Datos inválidos", details = ModelState, statusCode = 400 });

            if (id != updateCommentDto.IdComment)
                return BadRequest(new { error = "Bad Request", message = "El ID de la URL no coincide con el ID del body", statusCode = 400 });

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

        [HttpDelete("{id}")]
        public ActionResult DeleteComment(int id)
        {
            bool deleted = this.commentService.DeleteComment(id);
            if (!deleted)
                return NotFound(new { error = "Not Found", message = $"Comentario con ID {id} no encontrado", statusCode = 404 });

            return Ok(new { message = "Comentario eliminado exitosamente", deleted = true, statusCode = 200 });
        }
    }
}