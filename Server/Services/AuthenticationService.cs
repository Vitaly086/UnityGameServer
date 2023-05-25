using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Extensions;
using Server.Models;
using Server.Request;

namespace Server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Settings _settings;
        private readonly GameDbContext _context;
        private readonly HeroSettingsService _heroSettingsService;

        public AuthenticationService(Settings settings, GameDbContext context, HeroSettingsService heroSettingsService)
        {
            _settings = settings;
            _context = context;
            _heroSettingsService = heroSettingsService;
        }

        public AuthenticationResult Register(AuthenticationRequest request)
        {
            var username = request.Username?.ToLower();
            var password = request.Password;
            var deviceId = request.DeviceId;

            if (_context.UserProfiles.Any(user => user.Username == username))
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Content = "Username not available."
                };
            }

            var userProfile = new UserProfile()
            {
                Username = username,
                PasswordHash = password,
                DeviceId = deviceId
            };

            userProfile.ProvideSaltAndHash();
            _context.Add(userProfile);
            _context.SaveChanges();

            _heroSettingsService.CreateDefaultHeroSettings(userProfile.Id);

            return new AuthenticationResult
            {
                Success = true,
                UserProfile = userProfile
            };
        }

        public AuthenticationResult Login(AuthenticationRequest request)
        {
            var username = request.Username?.ToLower();
            var password = request.Password;
            var deviceId = request.DeviceId;

            UserProfile userProfile;

            if (string.IsNullOrEmpty(deviceId))
            {
                userProfile = GetUserProfileByUsername(username);
                if (IsInvalidUsername(userProfile) || IsInvalidPassword(userProfile, password))
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        Content = "Invalid username or password"
                    };
                }
            }
            else
            {
                userProfile = GetUserProfileByDeviceId(deviceId);
                if (IsInvalidUsername(userProfile))
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        Content = "Invalid device id"
                    };
                }
            }

            userProfile.HeroesSettings = _heroSettingsService.LoadHeroesSettings(userProfile.Id);

            var jwtToken = GenerateJwtToken(AssembleClaimsIdentity(userProfile));
            userProfile.JwtToken = jwtToken;
            _context.SaveChanges();

            return new AuthenticationResult
            {
                Success = true,
                UserProfile = userProfile
            };
        }

        private UserProfile GetUserProfileByDeviceId(string deviceId)
        {
            return _context.UserProfiles.SingleOrDefault(user => user.DeviceId == deviceId);
        }

        private UserProfile GetUserProfileByUsername(string username)
        {
            return _context.UserProfiles.SingleOrDefault(user => user.Username == username);
        }

        private bool IsInvalidUsername(UserProfile userProfile)
        {
            return userProfile == null;
        }

        private bool IsInvalidPassword(UserProfile userProfile, string password)
        {
            return userProfile.PasswordHash != AuthenticationHelpers.ComputeHash(password, userProfile.Salt);
        }

        private ClaimsIdentity AssembleClaimsIdentity(UserProfile userProfile)
        {
            var subject = new ClaimsIdentity(new[]
            {
                new Claim("id", userProfile.Id.ToString())
            });
            return subject;
        }

        private string GenerateJwtToken(ClaimsIdentity subject)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.BearerKey);

            var symmetricSecurityKey = new SymmetricSecurityKey(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = subject,
                Expires = DateTime.Now.AddYears(1),
                SigningCredentials =
                    new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}