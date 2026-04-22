using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShadowSinServer.Controllers;
using ShadowSinServer.Data;
using ShadowSinServer.DTOs;
using ShadowSinServer.Models;
using ShadowSinServer.Services;

namespace ShadowSinServer.Tests.Controllers;

public class AuthControllerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        IPasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
        ITokenService tokenService = new FakeTokenService();
        _controller = new AuthController(_context, hasher, tokenService);
    }

    [Fact]
    public async Task Register_PasswordTooShort_ReturnsBadRequest()
    {
        var request = new RegisterRequest("user", "user@example.com", "short");

        var result = await _controller.Register(request);

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(bad.Value);
    }

    [Fact]
    public async Task Register_PasswordExactlyTwelveChars_Succeeds()
    {
        var request = new RegisterRequest("user", "user@example.com", "123456789012");

        var result = await _controller.Register(request);

        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsConflict()
    {
        var request = new RegisterRequest("user", "dup@example.com", "longpassword1234");
        await _controller.Register(request);

        var result = await _controller.Register(request);

        Assert.IsType<ConflictObjectResult>(result);
    }

    public void Dispose() => _context.Dispose();

    private sealed class FakeTokenService : ITokenService
    {
        public (string Token, DateTime Expiration) GenerateToken(ApplicationUser user) =>
            ("fake-token", DateTime.UtcNow.AddHours(1));
    }
}
