using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefactoringChallenge.Controllers;
using RefactoringChallenge.Entities;

namespace RefactoringChallengeTest;

public class OrderControllerUnitTest
{
    private static readonly Mock<NorthwindDbContext> _mockDbContext = new();
    private static readonly Mock<IMapper> _mockMapper = new();
    
    //Todo: fix test
    [Fact]
    public async Task GetOrders_Returns_The_Correct_Number()
    {
        // Arrange
        var take = 5;
        var skip = 0;
        IEnumerable<Order> orders = new List<Order>();
        var mockDbSet = new Mock<DbSet<Order>>();
        mockDbSet.Setup(m => m.AsQueryable()).Returns(orders.AsQueryable());
        _mockDbContext.Setup(q => q.Orders).Returns(mockDbSet.Object);
        
        _mockMapper.Setup(m => m.Map<IEnumerable<OrderResponse>>(It.IsAny<object>())).
            Returns(new List<OrderResponse>());
        
        var controller = new OrdersController(_mockDbContext.Object, _mockMapper.Object);

        // Act
        var result = controller.Get();

        // Assert
        var okResult = result as OkObjectResult;
        var returnValue = okResult.Value as IEnumerable<OrderResponse>;
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(take, returnValue.Count());
    }
}