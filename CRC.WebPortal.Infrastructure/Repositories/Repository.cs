using Microsoft.EntityFrameworkCore;
using CRC.WebPortal.Domain.Interfaces;
using CRC.WebPortal.Infrastructure.Data;

namespace CRC.WebPortal.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _entities;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _entities.Update(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _entities.FindAsync(id);
    }
}