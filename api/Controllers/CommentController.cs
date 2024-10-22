using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController(
    ICommentRepository commentRepo,
    IStockRepository stockRepo,
    UserManager<AppUser> userManager,
    IFMPService fmpService
) : ControllerBase
{
    private readonly ICommentRepository commentRepo = commentRepo;
    private readonly IStockRepository stockRepo = stockRepo;
    private readonly UserManager<AppUser> userManager = userManager;
    private readonly IFMPService fmpService = fmpService;

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
    [HttpPost("{symbol:alpha}")]
    public async Task<IActionResult> Create(
        [FromRoute] string symbol,
        [FromBody] CreateCommentDto commentDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stock = await stockRepo.GetBySymbolAsync(symbol);
        if (stock == null)
        {
            stock = await fmpService.FindStockBySymbolAsync(symbol);
            if (stock == null)
                return BadRequest("Stock does not exist");
            else
            {
                await stockRepo.CreateAsync(stock);
            }
        }

        var userName = User.GetUsername();
        if (string.IsNullOrEmpty(userName))
            return Unauthorized("Username not found in claims");

        var appUser = await userManager.FindByNameAsync(userName);
        if (appUser == null)
            return Unauthorized("User not found");

        var commentModel = commentDto.ToCommentFromCreateDTO(stock.Id);
        commentModel.AppUserId = appUser.Id;
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
