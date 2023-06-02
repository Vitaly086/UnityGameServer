using Server.Request;
using Server.Response;
using Server.Services.Interfaces;

namespace Server.Services;

public class UserMoneyService : IUserMoneyService
{
    private readonly GameDbContext _context;

    public UserMoneyService(GameDbContext context)
    {
        _context = context;
    }

    public bool CheckUserHasEnoughMoney(int userId, int amount)
    {
        var user = _context.UserProfiles.Find(userId);
        if (user == null)
        {
            return false;
        }

        return user.Money >= amount;
    }

    public void DeductMoneyFromUser(int userId, int amount)
    {
        var user = _context.UserProfiles.Find(userId);
        if (user == null)
        {
            return;
        }

        user.Money -= amount;
        _context.SaveChanges();
    }

    public UserMoneyResponse TryAddMoney(int userId, UserMoneyRequest request)
    {
        var user = _context.UserProfiles.Find(userId);
        if (user == null)
        {
            return new UserMoneyResponse()
            {
                Success = false,
                Content = "User not found."
            };
        }

        user.Money += request.Amount;
        _context.SaveChanges();
        
        return new UserMoneyResponse()
        {
            Success = true,
            Money = user.Money
        };;
    }
}