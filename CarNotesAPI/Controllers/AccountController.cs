using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarNotesAPI.Data.Api;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        var user = await _accountService.FindByEmailAsync(model.Email);
        if (user == null || !await _accountService.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(
            _configuration["ApplicationSettings:JwtSecret"] ?? throw new ArgumentNullException("JWT Secret must be specified."));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["ApplicationSettings:WebServerUrl"],

            Audience = _configuration["ApplicationSettings:WebClientUrl"],

            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                // ToDo: Add additional claims here if needed.
            }),

            Expires = DateTime.UtcNow.AddHours(1),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new { Token = tokenHandler.WriteToken(token) });
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
