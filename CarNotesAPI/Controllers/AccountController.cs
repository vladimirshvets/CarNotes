using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using CarNotesAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CarNotesAPI.Controllers;

[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    private readonly IConfiguration _configuration;

    public AccountController(
        IAccountService accountService,
        IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
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
            Email = viewModel.Email,
            // ToDo: implement password hasher
            PasswordHash = viewModel.Password
        };
        User newlyCreatedUser = await _accountService.CreateAsync(user);

        return Ok(new { Token = ProduceJWToken(newlyCreatedUser) });

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

        return Ok(new { Token = ProduceJWToken(user) });
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
        var emailIdentifierClaim = identity.Claims.FirstOrDefault(
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

    // ToDo: move to service
    private string ProduceJWToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(
            _configuration["ApplicationSettings:JwtSecret"]
            ?? throw new ArgumentNullException("JWT Secret must be specified."));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["ApplicationSettings:WebServerUrl"],

            Audience = _configuration["ApplicationSettings:WebClientUrl"],

            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email)
            }),

            Expires = DateTime.UtcNow.AddHours(1),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
