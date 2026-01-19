using System;

namespace StarterLogin.Application.MagicCards.Queries;

public record MagicCardDto(
    Guid Id,
    string Title,
    string Description,
    string ImageUrl,
    bool IsPublished,
    DateTime CreatedAt
);
