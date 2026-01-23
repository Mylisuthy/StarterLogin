using System;
using StarterLogin.Domain.Common;

namespace StarterLogin.Domain.Entities;

public class UserFavorite : BaseEntity
{
    public Guid UserId { get; private set; }
    public virtual User User { get; private set; }
    
    public Guid MediaContentId { get; private set; }
    public virtual MediaContent MediaContent { get; private set; }

    private UserFavorite() { }

    public UserFavorite(Guid userId, Guid mediaContentId)
    {
        UserId = userId;
        MediaContentId = mediaContentId;
    }
}
