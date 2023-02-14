using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
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
        int skip = 0, bool allChildern = true)
    {
        using var query = _queryFactory.Get();
        IQueryOptions<Order> queryOptions = new OrderQueryOptions
        {
            SortBy = sortBy,
            AllChildren = allChildern,
            SortDirection = sortDirection,
            Top = top,
            Skip = skip
        };
        
        try
        {
            var result = await query.execute(queryOptions);
            return Json(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetById([FromRoute] string orderId)
    {
        using var query = _queryFactory.Get();
        IQueryOptions<Order> queryOptions = new OrderQueryOptions
        {
            SearchQuery = orderId,
            AllChildren = true
        };

        try
        {
            var result = await query.execute(queryOptions);

            //Added Mapper
            var OrderDetails = result.Select(x => x.Adapt<OrderResponse>()).ToList();

            return Json(OrderDetails);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        using var command = _commandFactory.Create();
        try
        {
            var result = await command.Execute(order);
            return Json(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut]
    [Route("{orderId}")]
    public async Task<IActionResult> Put([FromBody] Order order)
    {
        using var command = _commandFactory.Update();
        try
        {
            var result = await command.Execute(order);
            return Json(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    //Change ModelBuilder to enable deleting order with order details
    [HttpDelete]
    [Route("{orderId}")]
    public async Task<IActionResult> Delete([FromBody] Order order)
    {
        using var command = _commandFactory.Delete();
        try
        {
            var result = await command.Execute(order);
            return Json(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}