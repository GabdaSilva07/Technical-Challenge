using Moq;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Entities;

namespace RefactoringChallengeTest;

public class RefactoredOrderController
{
    private readonly Mock<IQueryFactory<Order>> _orderQueryFactory;
    private readonly Mock<IQueryFactory<OrderDetail>> _orderDetailQueryFactory;
    private readonly Mock<ICommandFactory<OrderDetail, OrderDetail>> _orderDetailCommandFactory;
    private readonly Mock<ICommandFactory<Order, Order>> _orderCommandFactory;
}