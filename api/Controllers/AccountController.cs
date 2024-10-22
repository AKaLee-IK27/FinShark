using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ITokenService tokenService
) : ControllerBase
{
    private readonly UserManager<AppUser> userManager = userManager;
    private readonly ITokenService tokenService = tokenService;
    private readonly SignInManager<AppUser> signInManager = signInManager;

    //* api/account/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
                return Unauthorized("Wrong username!!!");

            var result = await signInManager.CheckPasswordSignInAsync(
                user,
                loginDto.Password,
                false
            );

            if (!result.Succeeded)
                return Unauthorized("User not found or wrong password!!!");

            return Ok(
                new NewUserDto
                {
                    UserName =
                        user.UserName ?? throw new ArgumentNullException(nameof(user.UserName)),
                    Email = user.Email ?? throw new ArgumentNullException(nameof(user.Email)),
                    Token = tokenService.CreateToken(user)
                }
            );
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    //* api/account/register
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

            if (appUser.Email == null || appUser.UserName == null)
                return StatusCode(500, "User creation failed.");

            return Ok(
                new NewUserDto
                {
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Token = tokenService.CreateToken(appUser)
                }
            );
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}
