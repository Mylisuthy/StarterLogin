using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StarterLogin.Domain.Common;

namespace StarterLogin.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public bool IsRevoked { get; private set; }
    public Guid UserId { get; private set; }
    
    [ForeignKey("UserId")]
    public virtual User User { get; private set; }

    // Constructor required for EF Core
    protected RefreshToken() { }

    public RefreshToken(Guid userId, string token, DateTime expiryDate)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
        ExpiryDate = expiryDate;
        IsRevoked = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Revoke()
    {
        IsRevoked = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
