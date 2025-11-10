using Microsoft.EntityFrameworkCore.Storage;
using TaskManagement.Application.Data;

namespace TaskManagement.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly TaskManagementDbContext _context;
    private IDbContextTransaction? _transaction;
    
    public UnitOfWork(TaskManagementDbContext context)
    {
        _context = context;
    }
    
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }
    
    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    
    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
