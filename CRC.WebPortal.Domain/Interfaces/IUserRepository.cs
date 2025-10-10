using CRC.WebPortal.Domain.Entities;

namespace CRC.WebPortal.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetByEmailAsync(string email);
}

public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
}