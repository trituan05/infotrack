using InfoTrack.DataAccess.DbContexts;
using InfoTrack.DataAccess.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace InfoTrack.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRankingRepository Rankings { get; }
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task DisposeTransactionAsync();
        void Dispose();
    }

    internal class UnitOfWork: IUnitOfWork, IDisposable
    {
        private bool _IsDisposed;
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public IRankingRepository Rankings { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Rankings = new RankingRepository(_dbContext);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null) throw new ArgumentException("Transaction is null");
            await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null) throw new ArgumentException("Transaction is null");
            await _transaction.RollbackAsync(cancellationToken);
        }

        public async Task DisposeTransactionAsync()
        {
            if (_transaction == null) throw new ArgumentException("Transaction is null");
            await _transaction.DisposeAsync();
            _IsDisposed = true;
        }

        public async void Dispose()
        {
            await Dispose(true);
        }

        protected async virtual Task Dispose(bool disposing)
        {
            if (!_IsDisposed && disposing && _transaction is not null)
            {
                await DisposeTransactionAsync();
            }
        }
    }
}
