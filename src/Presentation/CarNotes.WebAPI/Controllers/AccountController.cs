using System.Security.Claims;
using CarNotes.Domain.Interfaces.Services;
using CarNotes.Domain.Models;
using CarNotes.WebAPI.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

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

    [HttpPut("profile/{userId:guid}")]
    public async Task<ActionResult<User>> UpdateProfile(
        Guid userId, [FromBody] ProfileDto dto)
    {
        var user = await _accountService.GetAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        user.UserName = dto.UserName;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user = await _accountService.UpdateAsync(userId, user);

        return Ok(user);
    }
}
