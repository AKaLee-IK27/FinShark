using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IStockRepository stockRepo;

    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
        this.commentRepo = commentRepo;
        this.stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await commentRepo.GetAllAsync();

        var commentDto = comments.Select(c => c.ToCommentDto());

        return Ok(commentDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await commentRepo.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        return Ok(comment.ToCommentDto());
    }

    // POST api/comment/5
    [HttpPost("{stockId}")]
    public async Task<IActionResult> Create(
        [FromRoute] int stockId,
        [FromBody] CreateCommentDto commentDto
    )
    {
        if (!await stockRepo.StockExists(stockId))
        {
            return BadRequest("Stock does not exist");
        }

        var commentModel = commentDto.ToCommentFromCreateDTO(stockId);
        await commentRepo.CreateAsync(commentModel);

        return CreatedAtAction(
            nameof(GetById),
            new { id = commentModel.Id },
            commentModel.ToCommentDto()
        );
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateCommentDto updateDto
    )
    {
        var comment = await commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdateDTO());
        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        return Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var comment = await commentRepo.DeleteAsync(id);

        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        return NoContent();
    }
}
