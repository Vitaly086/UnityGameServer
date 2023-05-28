namespace Server.Models;

public class AuthenticationResult
{
    public bool Success { get; init; }
    public string? Content { get; init; }
    public UserProfile? UserProfile { get; init; }
}