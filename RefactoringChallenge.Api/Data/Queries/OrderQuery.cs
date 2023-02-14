using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Data.EF;
using RefactoringChallenge.Entities;

namespace RefactoringChallenge.Data.Queries
{
    public class OrderQuery : DatabaseOperation, IQuery<Order>
    {
        public OrderQuery(NorthwindDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> execute(IQueryOptions<Order> queryOptions)
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

        private async Task<IQueryable<Order>> ExecuteWithQueryValue(IQueryOptions<Order> query)
        {
            if (query.AllChildren)
            {
                return await Task.FromResult(
                    ((NorthwindDbContext) this._dbContext)
                    .Orders
                    .Include(j => j.Customer)
                    .Where(query.QueryPredicate));
            }
            else
            {
                return await Task.FromResult(
                    ((NorthwindDbContext) this._dbContext)
                    .Orders
                    .Where(query.QueryPredicate));
            }
        }
        
        private async Task<IQueryable<Order>> ExecuteWithoutQueryValue(IQueryOptions<Order> query)
        {
            if (query.AllChildren)
            {
                return await Task.FromResult(
                    ((NorthwindDbContext) this._dbContext)
                    .Orders
                    .Include(j => j.Customer));
            }
            else
            {
                return await Task.FromResult(
                    ((NorthwindDbContext) this._dbContext)
                    .Orders);
            }
        }

        private IQueryable<Order> GetPagedResults(IQueryable<Order> results, IQueryOptions<Order> queryOptions)
        {
            return results.Skip(queryOptions.Skip).Take(queryOptions.Top);
        }

        private IOrderedQueryable<Order> GetSortedResults(IQueryable<Order> results, IQueryOptions<Order> queryOptions)
        {
            return queryOptions.SortBy switch
            {
                _ => queryOptions.SortDirection == "dec"
                    ? results.OrderByDescending(x => x.OrderDate)
                    : results.OrderBy(x => x.OrderId)
            };
        }

        private IQueryable<Order> GetSearchedResults(IQueryOptions<Order> queryOption)
        {
            if (queryOption.AllChildren)
                return ((NorthwindDbContext)_dbContext)
                    .Orders
                    .Include(c => c.Customer)
                    .Include(E => E.Employee)
                    .Include(od => od.OrderDetails)
                    .Where(x => x.OrderId == int.Parse(queryOption.SearchQuery));
            return ((NorthwindDbContext)_dbContext)
                .Orders
                .Where(x => x.OrderId == int.Parse(queryOption.SearchQuery));
        }


    }
}

