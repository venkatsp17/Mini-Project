using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Services.Interfaces;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;

namespace ShoppingAppAPI.Services.Classes
{
    public class UnitOfWorkServices : IUnitOfWork
    {
        private readonly ShoppingAppContext _context;
        private  IDbContextTransaction _transaction;
        private bool _disposed;

        public UnitOfWorkServices(ShoppingAppContext context)
        {
            _context = context;
        }

        public IDbContextTransaction BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
            return _transaction;
        }

        public async Task Rollback()
        {
           await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                _disposed = true;
            }
        }

        public async Task Commit()
        {
            await _transaction.CommitAsync();
        }
    }
}
