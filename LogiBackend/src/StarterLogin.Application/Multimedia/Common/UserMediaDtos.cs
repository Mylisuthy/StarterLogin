using System;

namespace StarterLogin.Application.Multimedia.Common;

public record UserHistoryResponse(
    Guid MediaId,
    string Title,
    string? ImageUrl,
    TimeSpan WatchedTime,
    bool IsFinished,
    DateTime LastWatched
);

public record UpdateHistoryRequest(
    Guid MediaId,
    TimeSpan WatchedTime,
    bool IsFinished
);

public record FavoriteResponse(
    Guid MediaId,
    string Title,
    string? ImageUrl,
    string GenreName
);
