using RefactoringChallenge.Data.Commands;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Data.Queries;
using RefactoringChallenge.Entities;

namespace RefactoringChallenge.Data.Factories;

public class OrderDetailCommandQueryFactory : IQueryFactory<OrderDetail>, ICommandFactory<OrderDetail, OrderDetail>
{
    private readonly string _connectionString;

    public OrderDetailCommandQueryFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IQuery<OrderDetail> Get()
    {
        return new OrderDetailQuery(new NorthwindDbContext(_connectionString));
    }

    public ICommand<OrderDetail, OrderDetail> Update()
    {
        return new DatabaseUpdateCommand<OrderDetail>(new NorthwindDbContext(_connectionString));
    }

    public ICommand<OrderDetail, OrderDetail> Delete()
    {
        return new DatabaseDeleteCommand<OrderDetail>(new NorthwindDbContext(_connectionString));
    }

    public ICommand<OrderDetail, OrderDetail> Create()
    {
        return new DatabaseInsertCommand<OrderDetail>(new NorthwindDbContext(_connectionString));
    }
}