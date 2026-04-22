using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShadowSinServer.Data;
using ShadowSinServer.DTOs;
using ShadowSinServer.Models;
using ShadowSinServer.Services;

namespace ShadowSinServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthController(
        AppDbContext context,
        IPasswordHasher<ApplicationUser> passwordHasher,
        ITokenService tokenService
    )
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request.Password.Length < 12)
            return BadRequest(new { message = "Password must be at least 12 characters." });

        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return Conflict(new { message = "A user with this email already exists." });

        var user = new ApplicationUser { UserName = request.UserName, Email = request.Email };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var (token, expiration) = _tokenService.GenerateToken(user);
        return CreatedAtAction(
            nameof(Register),
            new AuthResponse(token, user.UserName, user.Email, expiration)
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
            return Unauthorized(new { message = "Invalid email or password." });

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Invalid email or password." });

        var (token, expiration) = _tokenService.GenerateToken(user);
        return Ok(new AuthResponse(token, user.UserName, user.Email, expiration));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (userId is null)
            return Unauthorized();

        var user = await _context.Users.FindAsync(userId);
        if (user is null)
            return Unauthorized();

        return Ok(
            new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.IsSuperuser,
            }
        );
    }
}
