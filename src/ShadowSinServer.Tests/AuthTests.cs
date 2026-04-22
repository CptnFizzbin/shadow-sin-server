using ShadowSinServer.DTOs;
using ShadowSinServer.Models;

namespace ShadowSinServer.Tests;

public class ApplicationUserTests
{
    [Fact]
    public void ApplicationUser_CreatedAt_DefaultsToUtcNow()
    {
        var before = DateTime.UtcNow;
        var user = new ApplicationUser();
        var after = DateTime.UtcNow;

        Assert.InRange(user.CreatedAt, before, after);
    }
}

public class AuthDtoTests
{
    [Fact]
    public void RegisterRequest_HoldsAllFields()
    {
        var req = new RegisterRequest("runner", "runner@example.com", "P@ssw0rd1");

        Assert.Equal("runner", req.UserName);
        Assert.Equal("runner@example.com", req.Email);
        Assert.Equal("P@ssw0rd1", req.Password);
    }

    [Fact]
    public void LoginRequest_HoldsAllFields()
    {
        var req = new LoginRequest("runner@example.com", "P@ssw0rd1");

        Assert.Equal("runner@example.com", req.Email);
        Assert.Equal("P@ssw0rd1", req.Password);
    }

    [Fact]
    public void AuthResponse_HoldsAllFields()
    {
        var expiration = DateTime.UtcNow.AddHours(1);
        var response = new AuthResponse("token123", "runner", "runner@example.com", expiration);

        Assert.Equal("token123", response.Token);
        Assert.Equal("runner", response.UserName);
        Assert.Equal("runner@example.com", response.Email);
        Assert.Equal(expiration, response.Expiration);
    }
}
