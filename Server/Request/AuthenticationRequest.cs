namespace Server.Request;

public class AuthenticationRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? DeviceId { get; set; }
}