using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Extensions;
using Server.Models;
using Server.Request;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Settings _settings;
        private readonly GameDbContext _context;
        private readonly HeroesService _heroesService;

        public AuthenticationService(Settings settings, GameDbContext context, HeroesService heroesService)
        {
            _settings = settings;
            _context = context;
            _heroesService = heroesService;
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
            var defaultHero = _context.DefaultHeroes.FirstOrDefault(hero => hero.IsSelected);
            var hero = CreateHeroSettings(userProfile, defaultHero);
            
            userProfile.HeroesSettings.Add(hero);
            _context.Add(userProfile);
            _context.SaveChanges();

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
                userProfile = _context.UserProfiles.SingleOrDefault(user => user.Username == username);
                if (userProfile == null || IsInvalidPassword(userProfile, password))
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
                userProfile = _context.UserProfiles.SingleOrDefault(user => user.DeviceId == deviceId);

                if (userProfile == null)
                {
                    return new AuthenticationResult
                    {
                        Success = false,
                        Content = "Invalid device id"
                    };
                }
            }

            userProfile.HeroesSettings = _heroesService.GetUserHeroes(userProfile.Id);

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("id", userProfile.Id.ToString())
            });

            var jwtToken = GenerateJwtToken(claimsIdentity);
            userProfile.JwtToken = jwtToken;
            _context.SaveChanges();

            return new AuthenticationResult
            {
                Success = true,
                UserProfile = userProfile
            };
        }


        private HeroesSettings CreateHeroSettings(UserProfile userProfile, DefaultHeroes? defaultHero)
        {
            var hero = new HeroesSettings
            {
                UserId = userProfile.Id,
                PrefabId = defaultHero.PrefabId,
                Name = defaultHero.Name,
                Level = defaultHero.Level,
                Experience = defaultHero.Experience,
                Description = defaultHero.Description,
                Health = defaultHero.Health,
                Attack = defaultHero.Attack,
                Defense = defaultHero.Defense,
                Speed = defaultHero.Speed,
                Type = defaultHero.Type,
                WasBought = defaultHero.WasBought,
                Price = defaultHero.Price,
                IsSelected = defaultHero.IsSelected
            };
            return hero;
        }

        private bool IsInvalidPassword(UserProfile userProfile, string password)
        {
            return userProfile.PasswordHash != AuthenticationHelpers.ComputeHash(password, userProfile.Salt);
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