using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CarNotesAPI.Services;

public class AccountService : IAccountService
{
    private readonly IConfiguration _configuration;

    private readonly IUserRepository _userRepository;

    public AccountService(
        IConfiguration configuration,
        IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<User?> FindByEmailAsync(string email)
        => await _userRepository.GetByEmailAsync(email);

    public async Task<User?> GetAsync(Guid userId)
        => await _userRepository.GetAsync(userId);

    public async Task<User> CreateAsync(User user)
        => await _userRepository.AddAsync(user);

    public async Task<User> UpdateAsync(Guid userId, User user)
        => await _userRepository.UpdateAsync(userId, user);

    public string ProduceJWToken(User user)
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

    public string HashPassword(User user, string password)
        => new PasswordHasher<User>().HashPassword(user, password);

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
                await UpdateAsync(user.Id, user);
                return true;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
