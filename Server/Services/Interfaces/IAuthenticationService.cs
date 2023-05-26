using Server.Models;
using Server.Request;

namespace Server.Services.Interfaces;

public interface IAuthenticationService
{
    public AuthenticationResult Register(AuthenticationRequest authenticationRequest);
    public AuthenticationResult Login(AuthenticationRequest authenticationRequest);
}