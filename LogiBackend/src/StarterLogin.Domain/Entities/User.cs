using System;
using System.Collections.Generic;
using StarterLogin.Domain.Common;
using StarterLogin.Domain.ValueObjects;

namespace StarterLogin.Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; protected set; } = null!;
    public Email Email { get; protected set; } = null!;
    public PasswordHash PasswordHash { get; protected set; } = null!;
    public bool IsActive { get; protected set; }
    public DateTime? BirthDate { get; protected set; }
    public string? Sex { get; protected set; }
    
    // DDD: Using a list for roles
    private readonly List<Role> _roles = new();
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    protected User() { } // For EF Core

    private User(string userName, Email email, PasswordHash passwordHash) : base()
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
    }

    public static User Create(string userName, Email email, PasswordHash passwordHash)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("Username cannot be empty.");

        return new User(userName, email, passwordHash);
    }

    public void UpdateProfile(DateTime? birthDate, string? sex)
    {
        BirthDate = birthDate;
        Sex = sex;
        MarkAsUpdated();
    }

    public void AddRole(Role role)
    {
        if (!_roles.Contains(role))
        {
            _roles.Add(role);
            MarkAsUpdated();
        }
    }

    public void RemoveRole(Role role)
    {
        if (_roles.Remove(role))
        {
            MarkAsUpdated();
        }
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        MarkAsUpdated();
    }

    public void UpdatePassword(PasswordHash newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        MarkAsUpdated();
    }
}
