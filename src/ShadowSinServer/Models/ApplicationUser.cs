using Microsoft.AspNetCore.Identity;

namespace ShadowSinServer.Models;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
