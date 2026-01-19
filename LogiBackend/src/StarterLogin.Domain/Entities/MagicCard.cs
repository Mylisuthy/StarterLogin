using System;
using System.ComponentModel.DataAnnotations;
using StarterLogin.Domain.Common;

namespace StarterLogin.Domain.Entities;

public class MagicCard : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public bool IsPublished { get; private set; }

    protected MagicCard() { }

    public MagicCard(string title, string description, string imageUrl, bool isPublished)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        IsPublished = isPublished;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, string imageUrl, bool isPublished)
    {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        IsPublished = isPublished;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPublished(bool isPublished)
    {
        IsPublished = isPublished;
        UpdatedAt = DateTime.UtcNow;
    }
}
