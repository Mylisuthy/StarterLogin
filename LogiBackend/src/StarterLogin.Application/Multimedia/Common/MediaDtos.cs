using System;

namespace StarterLogin.Application.Multimedia.Common;

public record MediaResponse(
    Guid Id,
    string Title,
    string Description,
    string? ImageUrl,
    string? VideoUrl,
    TimeSpan? Duration,
    DateTime? ReleaseDate,
    string? Rating,
    Guid GenreId,
    string GenreName,
    string ContentType
);

public record CreateMediaRequest(
    string Title,
    string Description,
    Guid GenreId,
    string? ImageUrl,
    string? VideoUrl,
    TimeSpan? Duration,
    DateTime? ReleaseDate,
    string? Rating,
    string ContentType
);

public record UpdateMediaRequest(
    string Title,
    string Description,
    Guid GenreId,
    string? ImageUrl,
    string? VideoUrl,
    TimeSpan? Duration,
    DateTime? ReleaseDate,
    string? Rating
);
