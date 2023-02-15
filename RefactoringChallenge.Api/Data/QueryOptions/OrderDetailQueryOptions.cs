using System;
using System.Linq.Expressions;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Entities;

namespace RefactoringChallenge.Data.QueryOptions;

public class OrderDetailQueryOptions : IQueryOptions<OrderDetail>
{
    public OrderDetailQueryOptions()
    {
        Top = 10000;
    }

    public string QueryValue { get; set; }
    public string SortBy { get; set; }
    public string SortDirection { get; set; }
    public int Top { get; set; }
    public int Skip { get; set; }
    public bool AllChildren { get; set; }
    public string SearchQuery { get; set; }

    public Expression<Func<OrderDetail, bool>> QueryPredicate
    {
        get { return o => o.OrderId == int.Parse(QueryValue); }
        set => throw new NotImplementedException();
    }
}