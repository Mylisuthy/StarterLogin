using System;

namespace StarterLogin.Application.Cards.Common;

public record CardResponse(
    Guid Id,
    string Title,
    string ImageUrl,
    string Description,
    int HP,
    int Attack,
    int Defense,
    bool IsPublished,
    DateTime CreatedAt
);

public record CreateCardRequest(
    string Title,
    string ImageUrl,
    string Description,
    int HP,
    int Attack,
    int Defense
);

public record UpdateCardRequest(
    string Title,
    string ImageUrl,
    string Description,
    int HP,
    int Attack,
    int Defense,
    bool IsPublished
);
