using Server.Models;
using Server.Request;

namespace Server.Services;

public interface IAuthenticationService
{
    public (bool success, string content) Register(AuthenticationRequest authenticationRequest);
    public (bool success, string content, UserProfile userProfile) Login(AuthenticationRequest authenticationRequest);
}