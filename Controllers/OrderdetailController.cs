using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

namespace Northwind.MySQL.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderdetailController : ControllerBase
{
    private readonly NorthwindContext _context;

    public OrderdetailController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Orderdetail>>> GetOrderdetails()
    {
        var Orderdetails = await _context.Orderdetails
            .ToListAsync();
       
        return Orderdetails;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Orderdetail>> GetOrderdetail(int id)
    {
    var Orderdetail = await _context.Orderdetails.FindAsync(id);

    if (Orderdetail == null)
    {
        return NotFound(); // Return 404 Not Found if the Orderdetail with the specified ID is not found
    }

    return Orderdetail;
    }
   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderdetail(int id)
    {
    var Orderdetail = await _context.Orderdetails.FindAsync(id);

    if (Orderdetail == null)
        {
        return NotFound();
        }

        _context.Orderdetails.Remove(Orderdetail);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
public async Task<ActionResult<Orderdetail>> CreateOrderdetail([FromBody] Orderdetail newOrderdetail)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    _context.Orderdetails.Add(newOrderdetail);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetOrderdetails), new { id = newOrderdetail.OrderDetailsId }, newOrderdetail);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateOrderdetail(int id, Orderdetail updatedOrderdetail)
{
    if (id != updatedOrderdetail.OrderDetailsId)
    {
        return BadRequest("Invalid ID");
    }

    _context.Entry(updatedOrderdetail).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Orderdetails.Any(p => p.OrderDetailsId == id))
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