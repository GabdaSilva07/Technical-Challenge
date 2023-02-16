using System;
using System.Linq.Expressions;

namespace RefactoringChallenge.Data.CQRS;

public interface IQueryOptions<T>
{
    string QueryValue { get; set; }
    string SortBy { get; set; }
    string SortDirection { get; set; }
    int Top { get; set; }
    int Skip { get; set; }
    bool AllChildren { get; set; }
    string SearchQuery { get; set; }
    Expression<Func<T, bool>> QueryPredicate { get; set; }
}