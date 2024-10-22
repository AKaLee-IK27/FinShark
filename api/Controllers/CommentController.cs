using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController(ICommentRepository commentRepo, IStockRepository stockRepo) : ControllerBase
{
    private readonly ICommentRepository commentRepo = commentRepo;
    private readonly IStockRepository stockRepo = stockRepo;

    //* GET api/comment

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comments = await commentRepo.GetAllAsync();

        var commentDto = comments.Select(c => c.ToCommentDto());

        return Ok(commentDto);
    }

    //* GET api/comment/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await commentRepo.GetByIdAsync(id);

        if (comment == null)
            return NotFound("Comment not found");

        return Ok(comment.ToCommentDto());
    }

    //* POST api/comment/{stockId}
    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create(
        [FromRoute] int stockId,
        [FromBody] CreateCommentDto commentDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await stockRepo.StockExists(stockId))
            return BadRequest("Stock does not exist");

        var commentModel = commentDto.ToCommentFromCreateDTO(stockId);
        await commentRepo.CreateAsync(commentModel);

        return CreatedAtAction(
            nameof(GetById),
            new { id = commentModel.Id },
            commentModel.ToCommentDto()
        );
    }

    //* PUT api/comment/{id}
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateCommentDto updateDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdateDTO());
        if (comment == null)
            return NotFound("Comment not found");

        return Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await commentRepo.DeleteAsync(id);

        if (comment == null)
            return NotFound("Comment not found");

        return NoContent();
    }
}
