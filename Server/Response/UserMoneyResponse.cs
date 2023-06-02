namespace Server.Response;

public class UserMoneyResponse
{
    public bool Success { get; set; }
    public string? Content { get; set; }
    public int Money { get; set; }
}