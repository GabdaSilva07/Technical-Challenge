using System;
using Microsoft.EntityFrameworkCore;

namespace RefactoringChallenge.Data.EF;

public abstract class DatabaseOperation : IDisposable
{
    protected readonly DbContext _dbContext;

    public DatabaseOperation(DbContext dbContext)
    {
        _dbContext = dbContext;
        if (_dbContext != null) _dbContext.Database.SetCommandTimeout(new TimeSpan(0, 5, 0));
    }

    public void Dispose()
    {
        if (_dbContext != null)
        {
            _dbContext.Database.CloseConnection();
            _dbContext.Dispose();
        }
    }
}