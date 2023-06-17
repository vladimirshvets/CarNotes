using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CarNotesAPI.Services;

public class AuthService : IAuthService
{
    private readonly IAccountService _accountService;

    private readonly IConfiguration _configuration;

    public AuthService(
        IAccountService accountService,
        IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
    }

    public string GenerateTokenString(User user)
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

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        PasswordVerificationResult result =
            passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        switch (result)
        {
            case PasswordVerificationResult.Failed:
                return false;

            case PasswordVerificationResult.Success:
                return true;

            case PasswordVerificationResult.SuccessRehashNeeded:
                // Update the stored password hash
                // to use the latest hashing algorithm.
                user.PasswordHash = HashPassword(user, password);
                await _accountService.UpdateAsync(user.Id, user);
                return true;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public string HashPassword(User user, string password)
        => new PasswordHasher<User>().HashPassword(user, password);
}
