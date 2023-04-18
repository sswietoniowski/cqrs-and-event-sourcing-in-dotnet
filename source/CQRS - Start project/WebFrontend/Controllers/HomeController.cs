using Domain;
using Domain.WriteSide;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Domain.WriteSide.Commands;
using WebFrontend.Models;

namespace WebFrontend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWriteService _writeService;
    private readonly IOrderServiceReadSide _orderServiceReadSide;

    public HomeController(ILogger<HomeController> logger, IWriteService writeService, IOrderServiceReadSide orderServiceReadSide)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _writeService = writeService ?? throw new ArgumentNullException(nameof(writeService));
        _orderServiceReadSide = orderServiceReadSide ?? throw new ArgumentNullException(nameof(orderServiceReadSide));
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
    public IActionResult CreateNewOrder(int customerId, string customerName)
    {
        var orderId = Guid.NewGuid();

        _writeService.HandleCommand(new CreateOrder()
        {
            Id = orderId,
            CustomerId = customerId,
            CustomerName = customerName
        });

        return RedirectToAction("OrderDetails", new { id = orderId });
    }

    [HttpPost]
    public IActionResult CancelOrder(Guid orderId)
    {
        _writeService.HandleCommand(new CancelOrder()
        {
            Id = orderId
        });

        return RedirectToAction("OrderDetails", new { id = orderId });
    }

    [HttpPost]
    public IActionResult DeleteOrderLine(Guid orderId, Guid orderLineId)
    {
        _writeService.HandleCommand(new DeleteOrderLine()
        {
            Id = orderId,
            OrderLineId = orderLineId
        });

        return RedirectToAction("OrderDetails", new { id = orderId });
    }

    [HttpPost]
    public IActionResult AddOrderLine(Guid orderId, OrderLine orderLine)
    {
        _writeService.HandleCommand(new AddOrderLine()
        {
            Id = orderId,
            OrderLine = orderLine
        });

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