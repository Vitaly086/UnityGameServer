using Server.Models;

namespace Server.Services;

public class AuthenticationResult
{
    public bool Success { get; init; }
    public string Content { get; init; }
    public UserProfile UserProfile { get; init; }
}