using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using OliveBlazor.Core.Application.Common.Interfaces;

namespace OliveBlazor.Infrastructure.Data;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction _transaction;
    public EfUnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void BeginTransaction()
    {
        _transaction = _dbContext.Database.BeginTransaction();
    }
    public void Commit()
    {
        _transaction.Commit();
    }
    public void Rollback()
    {
        _transaction.Rollback();
    }
}
