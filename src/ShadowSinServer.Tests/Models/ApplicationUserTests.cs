using ShadowSinServer.Models;

namespace ShadowSinServer.Tests.Models;

public class ApplicationUserTests
{
    [Fact]
    public void ApplicationUser_Id_DefaultsToNonEmptyGuid()
    {
        var user = new ApplicationUser();

        Assert.False(string.IsNullOrEmpty(user.Id));
        Assert.True(Guid.TryParse(user.Id, out _));
    }

    [Fact]
    public void ApplicationUser_IsSuperuser_DefaultsFalse()
    {
        var user = new ApplicationUser();

        Assert.False(user.IsSuperuser);
    }
}
