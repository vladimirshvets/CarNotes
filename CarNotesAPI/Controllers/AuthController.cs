using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotesAPI.Controllers;

[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;

    private readonly IAuthService _authService;

    public AuthController(
        IAccountService accountService,
        IAuthService authService)
    {
        _accountService = accountService;
        _authService = authService;
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
        user.PasswordHash = _authService.HashPassword(user, viewModel.Password);
        User newlyCreatedUser = await _accountService.CreateAsync(user);

        return Ok(new { Token = _authService.GenerateTokenString(newlyCreatedUser) });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel viewModel)
    {
        var user = await _accountService.FindByEmailAsync(viewModel.Email);
        if (user == null ||
            !await _authService.CheckPasswordAsync(user, viewModel.Password))
        {
            return Unauthorized();
        }

        return Ok(new { Token = _authService.GenerateTokenString(user) });
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
}
