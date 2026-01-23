using System;
using StarterLogin.Domain.Common;

namespace StarterLogin.Domain.Entities;

public class UserMediaHistory : BaseEntity
{
    public Guid UserId { get; private set; }
    public virtual User User { get; private set; }
    
    public Guid MediaContentId { get; private set; }
    public virtual MediaContent MediaContent { get; private set; }
    
    public TimeSpan WatchedTime { get; private set; }
    public bool IsFinished { get; private set; }

    private UserMediaHistory() { }

    public UserMediaHistory(Guid userId, Guid mediaContentId, TimeSpan watchedTime, bool isFinished)
    {
        UserId = userId;
        MediaContentId = mediaContentId;
        WatchedTime = watchedTime;
        IsFinished = isFinished;
    }

    public void UpdateProgress(TimeSpan watchedTime, bool isFinished)
    {
        WatchedTime = watchedTime;
        IsFinished = isFinished;
        MarkAsUpdated();
    }
}
