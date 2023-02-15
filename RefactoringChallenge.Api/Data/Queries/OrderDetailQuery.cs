using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Data.EF;
using RefactoringChallenge.Entities;

namespace RefactoringChallenge.Data.Queries;

public class OrderDetailQuery : DatabaseOperation, IQuery<OrderDetail>
{
    public OrderDetailQuery(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<OrderDetail>> execute(IQueryOptions<OrderDetail> queryOptions)
    {
        if (string.IsNullOrEmpty(queryOptions.SearchQuery))
        {
            var result = GetPagedResults(GetSortedResults(queryOptions.QueryValue != null
                ? await ExecuteWithQueryValue(queryOptions)
                : await ExecuteWithoutQueryValue(queryOptions), queryOptions), queryOptions).ToList();

            return await Task.FromResult(result);
        }
        else
        {
            var result = GetSearchedResults(queryOptions).ToList();

            return await Task.FromResult(result);
        }
    }

    private async Task<IQueryable<OrderDetail>> ExecuteWithQueryValue(IQueryOptions<OrderDetail> query)
    {
        if (query.AllChildren)
            return await Task.FromResult(
                ((NorthwindDbContext)_dbContext)
                .OrderDetails
                .Include(p => p.Product)
                .Where(query.QueryPredicate));
        return await Task.FromResult(
            ((NorthwindDbContext)_dbContext)
            .OrderDetails
            .Where(query.QueryPredicate));
    }

    private async Task<IQueryable<OrderDetail>> ExecuteWithoutQueryValue(IQueryOptions<OrderDetail> query)
    {
        if (query.AllChildren)
            return await Task.FromResult(
                ((NorthwindDbContext)_dbContext)
                .OrderDetails);
        return await Task.FromResult(
            ((NorthwindDbContext)_dbContext)
            .OrderDetails);
    }

    private IQueryable<OrderDetail> GetPagedResults(IQueryable<OrderDetail> results,
        IQueryOptions<OrderDetail> queryOptions)
    {
        return results.Skip(queryOptions.Skip).Take(queryOptions.Top);
    }

    private IOrderedQueryable<OrderDetail> GetSortedResults(IQueryable<OrderDetail> results,
        IQueryOptions<OrderDetail> queryOptions)
    {
        return queryOptions.SortBy switch
        {
            _ => queryOptions.SortDirection == "dec"
                ? results.OrderByDescending(x => x.OrderId)
                : results.OrderBy(x => x.OrderId)
        };
    }

    private IQueryable<OrderDetail> GetSearchedResults(IQueryOptions<OrderDetail> queryOption)
    {
        if (queryOption.AllChildren)
            return ((NorthwindDbContext)_dbContext)
                .OrderDetails
                .Include(p => p.Product)
                .ThenInclude(c => c.Category)
                .Include(s => s.Product)
                .ThenInclude(s => s.Supplier)
                .Where(x => x.OrderId == int.Parse(queryOption.SearchQuery));
        return ((NorthwindDbContext)_dbContext)
            .OrderDetails
            .Where(x => x.OrderId == int.Parse(queryOption.SearchQuery));
    }
}