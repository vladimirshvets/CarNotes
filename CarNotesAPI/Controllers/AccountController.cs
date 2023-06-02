using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers;

[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterViewModel viewModel)
    {
        var user = await _accountService.FindByEmailAsync(viewModel.Email);
        if (user != null)
        {
            return Conflict(new { Message = "A user with specified email already exists." });
        }

        user = new User
        {
            Email = viewModel.Email
        };
        user.PasswordHash = _accountService.HashPassword(user, viewModel.Password);
        User newlyCreatedUser = await _accountService.CreateAsync(user);

        return Ok(new { Token = _accountService.ProduceJWToken(newlyCreatedUser) });

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel viewModel)
    {
        var user = await _accountService.FindByEmailAsync(viewModel.Email);
        if (user == null ||
            !await _accountService.CheckPasswordAsync(user, viewModel.Password))
        {
            return Unauthorized();
        }

        return Ok(new { Token = _accountService.ProduceJWToken(user) });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var claims = identity.Claims.Where(c => c.Type != JwtRegisteredClaimNames.Jti);
            var newIdentity = new ClaimsIdentity(claims, identity.AuthenticationType);
            HttpContext.User = new ClaimsPrincipal(newIdentity);
        }

        return Ok();
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<User>> GetUserProfile()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var emailIdentifierClaim = identity?.Claims.FirstOrDefault(
            c => c.Type == ClaimTypes.Email);
        if (emailIdentifierClaim == null)
        {
            return Unauthorized();
        }

        string email = emailIdentifierClaim.Value;
        User? user = await _accountService.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [Authorize]
    [HttpPut("profile/{userId}")]
    public async Task<ActionResult<User>> UpdateProfile(
        Guid userId, [FromBody] ProfileViewModel viewModel)
    {
        var user = await _accountService.GetAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        user.UserName = viewModel.UserName;
        user.FirstName = viewModel.FirstName;
        user.LastName = viewModel.LastName;
        user = await _accountService.UpdateAsync(userId, user);

        return Ok(user);
    }
}
