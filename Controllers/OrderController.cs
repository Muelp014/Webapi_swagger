using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

namespace Northwind.MySQL.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly NorthwindContext _context;

    public OrderController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        var Orders = await _context.Orders
            .ToListAsync();
       
        return Orders;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
    var Order = await _context.Orders.FindAsync(id);

    if (Order == null)
    {
        return NotFound(); // Return 404 Not Found if the Order with the specified ID is not found
    }

    return Order;
    }
   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
    var Order = await _context.Orders.FindAsync(id);

    if (Order == null)
        {
        return NotFound();
        }

        _context.Orders.Remove(Order);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
public async Task<ActionResult<Order>> CreateOrder([FromBody] Order newOrder)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    _context.Orders.Add(newOrder);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetOrders), new { id = newOrder.OrderId }, newOrder);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateOrder(int id, Order updatedOrder)
{
    if (id != updatedOrder.OrderId)
    {
        return BadRequest("Invalid ID");
    }

    _context.Entry(updatedOrder).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Orders.Any(p => p.OrderId == id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }

    return NoContent();
}
}