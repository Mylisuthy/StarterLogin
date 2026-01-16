using System;
using StarterLogin.Domain.Common;

namespace StarterLogin.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; protected set; } = null!;
    public string Description { get; protected set; } = null!;

    protected Role() { } // For EF Core

    private Role(string name, string description) : base()
    {
        Name = name;
        Description = description;
    }

    public static Role Create(string name, string description = "")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty.");

        return new Role(name.ToUpper(), description);
    }

    public void UpdateDescription(string description)
    {
        Description = description;
        MarkAsUpdated();
    }
}
