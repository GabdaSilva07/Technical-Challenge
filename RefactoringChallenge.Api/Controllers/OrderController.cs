using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Data.QueryOptions;
using RefactoringChallenge.Entities;

namespace RefactoringChallenge.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : Controller
{
    private readonly IQueryFactory<Order> _queryFactory;
    private readonly ICommandFactory<Order, Order> _commandFactory;

    public OrderController(IQueryFactory<Order> queryFactory, ICommandFactory<Order, Order> commandFactory)
    {
        _queryFactory = queryFactory;
        _commandFactory = commandFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string sortBy = "Date", string sortDirection = "dec", int top = 10,
        int skip = 0)
    {
        using var query = _queryFactory.Get();
        IQueryOptions<Order> queryOptions = new OrderQueryOptions
        {
            SortBy = sortBy,
            AllChildren = true,
            SortDirection = sortDirection,
            Top = top,
            Skip = skip
        };

        var result = await query.execute(queryOptions);

        try
        {
            return Json(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}