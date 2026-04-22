using ShadowSinServer.DTOs;

namespace ShadowSinServer.Tests.DTOs;

public class AuthDtoTests
{
    [Fact]
    public void RegisterRequest_HoldsAllFields()
    {
        var req = new RegisterRequest("runner", "runner@example.com", "P@ssw0rd1Secret");

        Assert.Equal("runner", req.UserName);
        Assert.Equal("runner@example.com", req.Email);
        Assert.Equal("P@ssw0rd1Secret", req.Password);
    }

    [Fact]
    public void LoginRequest_HoldsAllFields()
    {
        var req = new LoginRequest("runner@example.com", "P@ssw0rd1Secret");

        Assert.Equal("runner@example.com", req.Email);
        Assert.Equal("P@ssw0rd1Secret", req.Password);
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
