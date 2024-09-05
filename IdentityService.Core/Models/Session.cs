namespace IdentityService.Core.Models;

public class Session
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public const int ExpiresOnDays = 60;
    public string FingerPrint { get; set; }
}