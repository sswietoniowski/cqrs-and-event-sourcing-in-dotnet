using Domain;
using Domain.WriteSide;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebFrontend.Models;

namespace WebFrontend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IOrderService_WriteSide _orderServiceWriteSide;
    private readonly IOrderService_ReadSide _orderServiceReadSide;

    public HomeController(ILogger<HomeController> logger, IOrderService_WriteSide orderServiceWriteSide, IOrderService_ReadSide orderServiceReadSide)
    {
        _logger = logger;
        _orderServiceWriteSide = orderServiceWriteSide;
        _orderServiceReadSide = orderServiceReadSide;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult CreateNewOrder()
    {
        return View(new CreateOrderModel());
    }

    [HttpPost]
    public IActionResult CreateNewOrder(int CustomerId, string CustomerName)
    {
        var orderId = Guid.NewGuid();

        var createOrder = new CreateOrder()
        {
            Id = orderId,
            CustomerId = CustomerId,
            CustomerName = CustomerName
        };

        var writeSide = (IHandleCommand<CreateOrder>)_orderServiceWriteSide;

        writeSide.Handle(createOrder);

        return RedirectToAction("OrderDetails", new { id = orderId });
    }

    [HttpPost]
    public IActionResult CancelOrder(Guid OrderId)
    {
        var cancelOrder = new CancelOrder()
        {
            Id = OrderId
        };

        var writeSide = (IHandleCommand<CancelOrder>)_orderServiceWriteSide;

        writeSide.Handle(cancelOrder);

        return RedirectToAction("OrderDetails", new { id = OrderId });
    }

    [HttpPost]
    public IActionResult DeleteOrderLine(Guid orderId, Guid orderLineId)
    {
        var deleteOrderLine = new DeleteOrderLine()
        {
            Id = orderId,
            OrderLineId = orderLineId
        };

        var writeSide = (IHandleCommand<DeleteOrderLine>)_orderServiceWriteSide;

        writeSide.Handle(deleteOrderLine);

        return RedirectToAction("OrderDetails", new { id = orderId });
    }

    [HttpPost]
    public IActionResult AddOrderLine(Guid orderId, OrderLine orderLine)
    {
        var addOrderLine = new AddOrderLine()
        {
            Id = orderId,
            OrderLine = orderLine
        };

        var writeSide = (IHandleCommand<AddOrderLine>)_orderServiceWriteSide;

        writeSide.Handle(addOrderLine);

        return RedirectToAction("OrderDetails", new { id = orderId });
    }

    public IActionResult OrderDetails(Guid id)
    {
        var order = _orderServiceReadSide.LoadOrder(id);

        return View(order);
    }

    public IActionResult ListAllOrders()
    {
        var orders = _orderServiceReadSide.LoadAllOrders();

        return View(orders);
    }
}