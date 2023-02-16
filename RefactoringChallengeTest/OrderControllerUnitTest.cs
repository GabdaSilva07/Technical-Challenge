using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefactoringChallenge.Controllers;
using RefactoringChallenge.Entities;

namespace RefactoringChallengeTest;

public class OrderControllerUnitTest
{
    private static readonly Mock<NorthwindDbContext> _mockDbContext = new();
    private static readonly Mock<IMapper> _mapper = new();
    private IServiceProvider _serviceProvider;


    // public OrderControllerUnitTest()
    // {
    //     if (_mapper == null)
    //     {
    //         var config = TypeAdapterConfig.GlobalSettings;
    //         config.NewConfig<Order, OrderResponse>().Map(dest => dest.OrderId, src => src.OrderId);
    //         _mapper = new ServiceMapper(_serviceProvider, config);
    //     }
    //     
    // }
    
    //Todo: fix test
    [Fact]
    public async Task GetOrders_Returns_The_Correct_Number()
    {
        // Arrange
        var take = 5;
        var skip = 0;
        IEnumerable<OrderResponse> orders = new List<OrderResponse>();
        var orderReturn = new List<OrderResponse>();
        var mockDbSet = new Mock<DbSet<Order>>();

        // mockDbSet.Setup(m => m.AsQueryable()).Returns(orders.AsEnumerable());
        // _mockDbContext.Setup(q => q.Orders).Returns(mockDbSet.Object);
        //
        // _mapper.Setup(m => m.From(mockDbSet.Object)).Returns(orders.AsQueryable());

        var controller = new OrdersController(_mockDbContext.Object, _mapper.Object);

        // Act
        var result = controller.Get();


        // Assert
        Assert.NotNull(result);
        
    }

    //Get by id
    [Fact]
    public async Task GetOrderById_Returns_The_Correct_Order()
    {
        // Arrange
        var orderId = 1;
        var order = new OrderResponse
        {
            OrderId = orderId
        };
        var mockDbSet = new Mock<DbSet<Order>>();
        mockDbSet.Setup(m => m.AsQueryable()).Returns(new List<Order>().AsQueryable());
        _mockDbContext.Setup(q => q.Orders).Returns(mockDbSet.Object);
        // _mapper.Setup(m => m.From(mockDbSet.Object)).Returns(new List<OrderResponse>().AsQueryable());
        var controller = new OrdersController(_mockDbContext.Object, _mapper.Object);


        // Act
        var results = controller.GetById(orderId);
        var result = (OrderResponse)((dynamic)results).Value;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
    }

    //Create order
    [Fact]
    public async Task CreateOrder_Returns_The_Correct_Order()
    {
        // Arrange
        
        List<OrderDetailRequest> orderDetails = GetOrderDetailRequest();

        List<Order> orders = GetOrders();
        
        var mockDbSet = new Mock<DbSet<Order>>();
        mockDbSet.Setup(m => m.AsQueryable()).Returns(orders.AsQueryable());
        _mockDbContext.Setup(q => q.Orders).Returns(mockDbSet.Object);
        var controller = new OrdersController(_mockDbContext.Object, _mapper.Object);


        // Act
        var results = controller.Create("test", 1, DateTime.Now, 1, null, "test", "t", "Birmingham", "AL", "BL19 4SL",
            "United Kingdom", orderDetails);
        var result = (OrderResponse)((dynamic)results).Value;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orders[0].OrderId, result.OrderId);
    }
    
    //Create add product to order
    [Fact]
    public async Task AddProductToOrder_Returns_The_Correct_Order()
    {
        // Arrange
        
        List<OrderDetailRequest> orderDetails = GetOrderDetailRequest();
        List<Order> orders = GetOrders();
        List<OrderDetail> orderDetail = GetOrderDetails(1);

        var mockDbSet = new Mock<DbSet<Order>>();
        var mockDbSetOrder = new Mock<DbSet<OrderDetail>>();
        
        mockDbSet.Setup(m => m.AsQueryable()).Returns(orders.AsQueryable());
        mockDbSetOrder.Setup(m => m.AsQueryable()).Returns(orderDetail.AsQueryable());
        
        
        // _mockDbContext.Setup(q => q.Orders).Returns(mockDbSet.Object.FirstOrDefault(o => o.OrderId == 0));
        _mockDbContext.Setup(q => q.OrderDetails).Returns(mockDbSetOrder.Object);
        var controller = new OrdersController(_mockDbContext.Object, _mapper.Object);

        // Act
        var results = controller.AddProductsToOrder(0, orderDetails);
        var result = (OrderResponse)((dynamic)results).Value;

        // Assert
        Assert.NotNull(result);
        
    }
    
    //Delete order
    [Fact]
    public async Task DeleteOrder_Returns_The_Correct_Order()
    {
        // Arrange
        var orderId = 1;
        var mockDbSet = new Mock<DbSet<Order>>();
        mockDbSet.Setup(m => m.AsQueryable()).Returns(new List<Order>().AsQueryable());
        _mockDbContext.Setup(q => q.Orders).Returns(mockDbSet.Object);
        // _mapper.Setup(m => m.From(mockDbSet.Object)).Returns(new List<OrderResponse>().AsQueryable());
        var controller = new OrdersController(_mockDbContext.Object, _mapper.Object);

        // Act
        var results = controller.Delete(orderId);

        // Assert
        Assert.NotNull(results);
    }
    
    private List<OrderDetailRequest> GetOrderDetailRequest()
    {
        List<OrderDetailRequest> orderDetails = new List<OrderDetailRequest>();
        orderDetails.Add(new OrderDetailRequest
        {
            ProductId = 1,
            Quantity = 1,
            UnitPrice = 1,
            Discount = 0.3f,
        });
        orderDetails.Add(new OrderDetailRequest
        {
            ProductId = 2,
            Quantity = 1,
            UnitPrice = 1,
            Discount = 0.3f,
        });
        return orderDetails;
    }
    
    private List<Order> GetOrders()
    {
        List<Order> orders = new List<Order>();
        orders.Add(new Order
        {
            OrderId = 0,
            CustomerId = "test",
            EmployeeId = 1,
            RequiredDate = DateTime.Now,
            ShipVia = 1,
            Freight = 1,
            ShipName = "test",
            ShipAddress = "t",
            ShipCity = "Birmingham",
            ShipRegion = "AL",
            ShipPostalCode = "BL19 4SL",
            ShipCountry = "United Kingdom",
            OrderDetails = GetOrderDetails(1)
        });
        orders.Add(new Order
        {
            OrderId = 1,
            CustomerId = "test",
            EmployeeId = 1,
            RequiredDate = DateTime.Now,
            ShipVia = 1,
            Freight = 1,
            ShipName = "test",
            ShipAddress = "t",
            ShipCity = "Birmingham",
            ShipRegion = "AL",
            ShipPostalCode = "BL19 4SL",
            ShipCountry = "United Kingdom",
            OrderDetails = GetOrderDetails(1)
        });
        return orders;
    }
    
    private List<OrderDetail> GetOrderDetails(int orderId)
    {
        List<OrderDetail> orderDetails = new List<OrderDetail>();
        orderDetails.Add(new OrderDetail
        {
            OrderId = orderId,
            ProductId = 1,
            Quantity = 1,
            UnitPrice = 1,
            Discount = 0.3f,
        });
        return orderDetails;
    }

}