using Microsoft.AspNetCore.Mvc;
using Moq;
using RefactoringChallenge.Controllers;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Entities;

namespace RefactoringChallengeTest;

public class RefactoredOrderControllerTest
{
    private readonly Mock<IQueryFactory<Order>> _orderQueryFactory;
    private readonly Mock<IQueryFactory<OrderDetail>> _orderDetailQueryFactory;
    private readonly Mock<ICommandFactory<OrderDetail, OrderDetail>> _orderDetailCommandFactory;
    private readonly Mock<ICommandFactory<Order, Order>> _orderCommandFactory;
    private readonly Mock<IQuery<Order>> _orderQuery;
    private readonly Mock<IQuery<OrderDetail>> _orderDetailQuery;
    private readonly Mock<ICommand<OrderDetail, OrderDetail>> _orderDetailCommand;
    private readonly Mock<ICommand<Order, Order>> _orderCommand;
    private readonly List<Order> _orders = GetTestOrders();

    public RefactoredOrderControllerTest()
    {
        _orderQueryFactory = new Mock<IQueryFactory<Order>>();
        _orderDetailQueryFactory = new Mock<IQueryFactory<OrderDetail>>();
        _orderDetailCommandFactory = new Mock<ICommandFactory<OrderDetail, OrderDetail>>();
        _orderCommandFactory = new Mock<ICommandFactory<Order, Order>>();
        _orderQuery = new Mock<IQuery<Order>>();
        _orderDetailQuery = new Mock<IQuery<OrderDetail>>();
        _orderDetailCommand = new Mock<ICommand<OrderDetail, OrderDetail>>();
        _orderCommand = new Mock<ICommand<Order, Order>>();
    }

    [Fact]
    public async Task Get_Orders()
    {
        //Arrange
        _orderQuery.Setup(q => q.execute(It.IsAny<IQueryOptions<Order>>()))
            .Returns(Task.Factory.StartNew(() => (IEnumerable<Order>)_orders));
        _orderQueryFactory.Setup(qf => qf.Get()).Returns(_orderQuery.Object);

        var orderController = new OrderController(_orderQueryFactory.Object, _orderCommandFactory.Object,
            _orderDetailQueryFactory.Object, _orderDetailCommandFactory.Object);

        //Act
        var results = await orderController.Get();
        var result = results as JsonResult;
        var orders = result.Value as List<Order>;


        //Assert
        Assert.IsType<JsonResult>(result);
        Assert.IsType<List<Order>>(orders);
        Assert.Equal(2, orders.Count);
    }

    [Fact]
    public async Task Get_Order_By_Id()
    {
        //Arrange
        var order = GetOrderById(1);

        _orderQuery.Setup(q => q.execute(It.IsAny<IQueryOptions<Order>>()))
            .Returns(Task.Factory.StartNew(() => (IEnumerable<Order>)new List<Order> { order }));
        _orderQueryFactory.Setup(qf => qf.Get()).Returns(_orderQuery.Object);

        var orderController = new OrderController(_orderQueryFactory.Object, _orderCommandFactory.Object,
            _orderDetailQueryFactory.Object, _orderDetailCommandFactory.Object);

        //Act
        var results = await orderController.Get();
        var result = results as JsonResult;
        var orders = result.Value as List<Order>;


        //Assert
        Assert.IsType<JsonResult>(result);
        Assert.IsType<List<Order>>(orders);
        Assert.Single(orders);
    }

    [Fact]
    public async Task Post_Order()
    {
        //Arrange
        Order order = GetOrderById(1);
        _orderCommand.Setup(c => c.Execute(order)).Returns(Task.Factory.StartNew(() => order));
        _orderCommandFactory.Setup(cf => cf.Create()).Returns(_orderCommand.Object);
        
        var orderController = new OrderController(_orderQueryFactory.Object, _orderCommandFactory.Object,
            _orderDetailQueryFactory.Object, _orderDetailCommandFactory.Object);
        
        //Act
        var results = await orderController.Post(order);
        var result = results as JsonResult;
        var postedOrder = result.Value as Order;
        
        //Assert
        Assert.IsType<JsonResult>(result);
        Assert.IsType<Order>(postedOrder);
        Assert.Equal(order, postedOrder);
        
    }

    [Fact]
    public async Task Update_Order()
    {
        //Arrange
        Order order = GetOrderById(1);
        Order newOrder = GetOrderById(1);
        string newShippingName = "New Company";
        newOrder.ShipName = newShippingName;
        _orderCommand.Setup(c => c.Execute(order)).Returns(Task.Factory.StartNew(() => newOrder));
        _orderCommandFactory.Setup(cf => cf.Update()).Returns(_orderCommand.Object);
        
        var orderController = new OrderController(_orderQueryFactory.Object, _orderCommandFactory.Object,
            _orderDetailQueryFactory.Object, _orderDetailCommandFactory.Object);
        
        //Act
        var results = await orderController.Put(order);
        var result = results as JsonResult;
        var updatedOrder = result.Value as Order;
        
        //Assert
        Assert.IsType<JsonResult>(result);
        Assert.IsType<Order>(updatedOrder);
        Assert.Equal(newShippingName, updatedOrder.ShipName);
    }

    [Fact]
    public async Task Get_Order_Details()
    {
        //Arrange
        var orderDetails = GetOrderDetails();
        _orderDetailQuery.Setup(q => q.execute(It.IsAny<IQueryOptions<OrderDetail>>()))
            .Returns(Task.Factory.StartNew(() => (IEnumerable<OrderDetail>)orderDetails));
        _orderDetailQueryFactory.Setup(qf => qf.Get()).Returns(_orderDetailQuery.Object);
        
        var orderController = new OrderController(_orderQueryFactory.Object, _orderCommandFactory.Object,
            _orderDetailQueryFactory.Object, _orderDetailCommandFactory.Object);
        
        //Act
        var results = await orderController.GetOrderDetails();
        var result = results as JsonResult;
        var orderDetailsResult = result.Value as List<OrderDetail>;
        
        //Assert
        Assert.IsType<JsonResult>(result);
        Assert.IsType<List<OrderDetail>>(orderDetailsResult);
        Assert.Equal(4, orderDetailsResult.Count);
    }

    [Fact]
    public async Task Get_Order_Detail_By_Id()
    {
        OrderDetail orderDetail = GetOrderDetailById(1, 1);
        _orderDetailQuery.Setup(q => q.execute(It.IsAny<IQueryOptions<OrderDetail>>()))
            .Returns(Task.Factory.StartNew(() => (IEnumerable<OrderDetail>)new List<OrderDetail> { orderDetail }));
        
        _orderDetailQueryFactory.Setup(qf => qf.Get()).Returns(_orderDetailQuery.Object);
        
        var orderController = new OrderController(_orderQueryFactory.Object, _orderCommandFactory.Object,
            _orderDetailQueryFactory.Object, _orderDetailCommandFactory.Object);
        
        //Act
        var results = await orderController.GetOrderDetailId("1","1");
        var result = results as JsonResult;
        var orderDetailResult = result.Value as List<OrderDetail>;
        
        
        //Assert
        Assert.IsType<JsonResult>(result);
        Assert.IsType<List<OrderDetail>>(orderDetailResult);
        Assert.Single(orderDetailResult);
    }

    public static List<Order> GetTestOrders()
    {
        var orders = new List<Order>
        {
            new()
            {
                OrderId = 1,
                CustomerId = "ALFKI",
                EmployeeId = 1,
                OrderDate = new DateTime(2022, 10, 1),
                RequiredDate = new DateTime(2022, 10, 15),
                ShippedDate = new DateTime(2022, 10, 10),
                ShipVia = 1,
                Freight = 12.34M,
                ShipName = "Some Company",
                ShipAddress = "123 Main St.",
                ShipCity = "Anytown",
                ShipRegion = "Region",
                ShipPostalCode = "12345",
                ShipCountry = "USA",
                Customer = new Customer { CustomerId = "ALFKI", CompanyName = "Alfreds Futterkiste" },
                Employee = new Employee { EmployeeId = 1, LastName = "Doe", FirstName = "John" },
                ShipViaNavigation = new Shipper { ShipperId = 1, CompanyName = "Speedy Express" },
                OrderDetails = new List<OrderDetail>
                {
                    new() { OrderId = 1, ProductId = 1, UnitPrice = 10.00M, Quantity = 5 },
                    new() { OrderId = 1, ProductId = 2, UnitPrice = 20.00M, Quantity = 10 }
                }
            },
            new()
            {
                OrderId = 2,
                CustomerId = "ANATR",
                EmployeeId = 2,
                OrderDate = new DateTime(2022, 11, 1),
                RequiredDate = new DateTime(2022, 11, 15),
                ShippedDate = new DateTime(2022, 11, 10),
                ShipVia = 2,
                Freight = 23.45M,
                ShipName = "Another Company",
                ShipAddress = "456 Elm St.",
                ShipCity = "Othertown",
                ShipRegion = "Region",
                ShipPostalCode = "67890",
                ShipCountry = "USA",
                Customer = new Customer { CustomerId = "ANATR", CompanyName = "Ana Trujillo Emparedados y helados" },
                Employee = new Employee { EmployeeId = 2, LastName = "Smith", FirstName = "Jane" },
                ShipViaNavigation = new Shipper { ShipperId = 2, CompanyName = "United Package" },
                OrderDetails = new List<OrderDetail>
                {
                    new() { OrderId = 2, ProductId = 3, UnitPrice = 30.00M, Quantity = 5 },
                    new() { OrderId = 2, ProductId = 4, UnitPrice = 40.00M, Quantity = 10 }
                }
            }
        };
        return orders;
    }

    public Order GetOrderById(int orderId)
    {
        var order = new Order
        {
            OrderId = orderId,
            CustomerId = "ALFKI",
            EmployeeId = 1,
            OrderDate = new DateTime(2022, 10, 1),
            RequiredDate = new DateTime(2022, 10, 15),
            ShippedDate = new DateTime(2022, 10, 10),
            ShipVia = 1,
            Freight = 12.34M,
            ShipName = "Some Company",
            ShipAddress = "123 Main St.",
            ShipCity = "Anytown",
            ShipRegion = "Region",
            ShipPostalCode = "12345",
            ShipCountry = "USA",
            Customer = new Customer { CustomerId = "ALFKI", CompanyName = "Alfreds Futterkiste" },
            Employee = new Employee { EmployeeId = 1, LastName = "Doe", FirstName = "John" },
            ShipViaNavigation = new Shipper { ShipperId = 1, CompanyName = "Speedy Express" },
            OrderDetails = new List<OrderDetail>
            {
                new() { OrderId = orderId, ProductId = 1, UnitPrice = 10.00M, Quantity = 5 },
                new() { OrderId = orderId, ProductId = 2, UnitPrice = 20.00M, Quantity = 10 }
            }
        };
        return order;
    }
    
    public List<OrderDetail> GetOrderDetails()
    {
        List<OrderDetail> orderDetails = new List<OrderDetail>
        {
            new OrderDetail { OrderId = 1, ProductId = 1, UnitPrice = 10.00M, Quantity = 5, Discount = 0.0F },
            new OrderDetail { OrderId = 1, ProductId = 2, UnitPrice = 20.00M, Quantity = 10, Discount = 0.1F },
            new OrderDetail { OrderId = 2, ProductId = 3, UnitPrice = 30.00M, Quantity = 15, Discount = 0.2F },
            new OrderDetail { OrderId = 2, ProductId = 4, UnitPrice = 40.00M, Quantity = 20, Discount = 0.3F }
        };
        return orderDetails;
    }
    
    public OrderDetail GetOrderDetailById(int orderId, int productId)
    {
        OrderDetail orderDetail = new OrderDetail
        {
            OrderId = orderId,
            ProductId = productId,
            UnitPrice = 10.00M,
            Quantity = 5,
            Discount = 0.0F,
            Order = new Order { OrderId = orderId, CustomerId = "ALFKI", EmployeeId = 1 },
            Product = new Product { ProductId = productId, ProductName = "Chai", CategoryId = 1 }
        };
        return orderDetail;
    }


}