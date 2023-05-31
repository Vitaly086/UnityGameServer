using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server;
using Server.Extensions;
using Server.Models;
using Server.Services;
using Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);

var services = builder.Services;
services.AddSingleton(settings);
AuthenticationHelpers.Initialize(settings.BearerKey);

services.AddDbContext<GameDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("GameDatabase");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    options.UseMySql(connectionString, serverVersion);
});

services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<HeroesService>();
services.AddScoped<IInventoryService, InventoryService>();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
    };
});


var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();