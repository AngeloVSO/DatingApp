using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AccountController(UserContext _context, ITokenService _tokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (registerDto.IsInvalid()) return BadRequest("Username and Password cannot be empty or whitespace.");

        if (await UserExists(registerDto.Name)) return BadRequest("Username is already registered.");

        using var hmac = new HMACSHA512();

        var user = new User
        {
            Name = registerDto.Name.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto(user.Name, _tokenService.CreateToken(user));

        return Ok(userDto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => 
            u.Name == loginDto.Name.ToLower());

        if (user == null) return Unauthorized("Invalid credentials.");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid credentials.");
        }

        var userDto = new UserDto(user.Name, _tokenService.CreateToken(user));

        return Ok(userDto);
    }

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.Name.ToLower() == username.ToLower());
    }
}
