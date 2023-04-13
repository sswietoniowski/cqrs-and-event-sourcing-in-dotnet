using Domain;
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
        _orderServiceWriteSide.CreateOrder(orderId, CustomerId, CustomerName);

        return RedirectToAction("OrderDetails", new { id = orderId });
    }

    [HttpPost]
    public IActionResult CancelOrder(Guid OrderId)
    {
        _orderServiceWriteSide.UpdateOrderState(OrderId, OrderState.cancel);

        return RedirectToAction("OrderDetails", new { id = OrderId });
    }

    [HttpPost]
    public IActionResult DeleteOrderLine(Guid orderId, Guid orderLineId)
    {
        _orderServiceWriteSide.DeleteOrderLine(orderId, orderLineId);

        return RedirectToAction("OrderDetails", new { id = orderId });
    }

    [HttpPost]
    public IActionResult AddOrderLine(Guid orderId, OrderLine orderLine)
    {
        _orderServiceWriteSide.AddOrderLine(orderId, orderLine);

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