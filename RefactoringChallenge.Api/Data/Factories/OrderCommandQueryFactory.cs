using RefactoringChallenge.Data.Commands;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Data.Queries;
using RefactoringChallenge.Entities;

namespace RefactoringChallenge.Data.Factories;

public class OrderCommandQueryFactory : IQueryFactory<Order>, ICommandFactory<Order, Order>
{
    private readonly string _connectionString;

    // Added Logger
    // private readonly ILogger _logger;

    public OrderCommandQueryFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IQuery<Order> Get()
    {
        return new OrderQuery(new NorthwindDbContext(_connectionString));
    }

    public ICommand<Order, Order> Update()
    {
        return new DatabaseUpdateCommand<Order>(new NorthwindDbContext(_connectionString));
    }

    public ICommand<Order, Order> Delete()
    {
        return new DatabaseDeleteCommand<Order>(new NorthwindDbContext(_connectionString));
    }

    public ICommand<Order, Order> Create()
    {
        return new DatabaseInsertCommand<Order>(new NorthwindDbContext(_connectionString));
    }
}