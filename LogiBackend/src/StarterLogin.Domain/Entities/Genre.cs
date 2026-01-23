using System;
using StarterLogin.Domain.Common;

namespace StarterLogin.Domain.Entities;

public class Genre : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    private Genre() { }

    private Genre(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public static Genre Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Genre name is required.");
        return new Genre(name, description);
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
        MarkAsUpdated();
    }
}
