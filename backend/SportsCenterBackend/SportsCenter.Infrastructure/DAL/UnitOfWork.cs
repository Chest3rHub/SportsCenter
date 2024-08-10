using System.Threading;
using System.Threading.Tasks;
using SportsCenter.Application.Abstractions;
using SportsCenter.Infrastructure.Abstractions;

namespace SportsCenter.Infrastructure.DAL;

internal class UnitOfWork : IUnitOfWork
{
    private readonly SportsCenterDbContext _context;

    public UnitOfWork(SportsCenterDbContext context)
    {
        _context = context;
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken)
    {
        return _context.Database.RollbackTransactionAsync(cancellationToken);
    }
}