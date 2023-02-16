using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactoringChallenge.Data.CQRS;

public interface IQuery<T> : IDisposable
{
    Task<IEnumerable<T>> execute(IQueryOptions<T> queryOptions);
}