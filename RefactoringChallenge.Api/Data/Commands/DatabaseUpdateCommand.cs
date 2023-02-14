using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Data.EF;

namespace RefactoringChallenge.Data.Commands;

public class DatabaseUpdateCommand<T> : DatabaseOperation, ICommand<T, T> where T : class
{
    public DatabaseUpdateCommand(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<T> Execute(T data)
    {
        _dbContext.Update(data);
        await _dbContext.SaveChangesAsync();
        return await Task.FromResult(data);
    }
}