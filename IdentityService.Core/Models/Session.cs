namespace IdentityService.Core.Models;

public class Session
{
    public Guid Id { get; init; }
    public User User { get; init; }
    public Guid UserId { get; init; }
    public DateTime CreatedOn { get; init; }
    public const int ExpiresOnDays = 60;
    public string FingerPrint { get; init; }
}