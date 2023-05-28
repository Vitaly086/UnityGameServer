using Server.Models;

namespace Server.Response;

public class AuthenticationResponse
{
    public UserProfile? UserProfile { get; set; }
}