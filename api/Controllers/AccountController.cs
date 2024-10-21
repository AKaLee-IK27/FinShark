using api.Dtos.Account;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> userManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        this.userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };

            if (string.IsNullOrEmpty(registerDto.Password))
                return BadRequest("Password cannot be null or empty.");

            var createdUser = await userManager.CreateAsync(appUser, registerDto.Password);

            if (!createdUser.Succeeded)
                return StatusCode(500, createdUser.Errors);

            var roleResult = await userManager.AddToRoleAsync(appUser, "User");

            if (!roleResult.Succeeded)
                return StatusCode(500, roleResult.Errors);

            return Ok("User created successfully");
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}
