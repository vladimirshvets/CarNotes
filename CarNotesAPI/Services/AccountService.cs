using CarNotesAPI.Data.Api;
using CarNotesAPI.Data.Models;

namespace CarNotesAPI.Services;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;

    public AccountService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<User?> GetAsync(Guid userId)
    {
        return await _userRepository.GetAsync(userId);
    }

    public async Task<User> CreateAsync(User user)
    {
        return await _userRepository.AddAsync(user);
    }

    public async Task<User> UpdateAsync(Guid userId, User user)
    {
        return await _userRepository.UpdateAsync(userId, user);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        // ToDo: implement it.
        return true;
        //throw new NotImplementedException();
    }
}
