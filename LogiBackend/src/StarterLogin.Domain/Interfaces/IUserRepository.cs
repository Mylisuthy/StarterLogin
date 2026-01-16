using System;
using System.Threading.Tasks;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.ValueObjects;

namespace StarterLogin.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(Email email);
    Task<User?> GetByUserNameAsync(string userName);
    Task AddAsync(User user);
    void Update(User user);
}
