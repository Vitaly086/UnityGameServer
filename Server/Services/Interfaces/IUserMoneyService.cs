using Server.Request;
using Server.Response;

namespace Server.Services.Interfaces;

public interface IUserMoneyService
{
    public bool CheckUserHasEnoughMoney(int userId, int amount);
    public void DeductMoneyFromUser(int userId, int amount);
    public UserMoneyResponse TryAddMoney(int userId, UserMoneyRequest request);
}