using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

namespace Northwind.MySQL.Controllers;

[ApiController]
[Route("[controller]")]
public class ShipperController : ControllerBase
{
    private readonly NorthwindContext _context;

    public ShipperController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shipper>>> GetShippers()
    {
        var Shippers = await _context.Shippers
            .ToListAsync();
       
        return Shippers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Shipper>> GetShipper(int id)
    {
    var Shipper = await _context.Shippers.FindAsync(id);

    if (Shipper == null)
    {
        return NotFound(); // Return 404 Not Found if the Shipper with the specified ID is not found
    }

    return Shipper;
    }
   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShipper(int id)
    {
    var Shipper = await _context.Shippers.FindAsync(id);

    if (Shipper == null)
        {
        return NotFound();
        }

        _context.Shippers.Remove(Shipper);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
public async Task<ActionResult<Shipper>> CreateShipper([FromBody] Shipper newShipper)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    _context.Shippers.Add(newShipper);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetShippers), new { id = newShipper.ShipperId }, newShipper);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateShipper(int id, Shipper updatedShipper)
{
    if (id != updatedShipper.ShipperId)
    {
        return BadRequest("Invalid ID");
    }

    _context.Entry(updatedShipper).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Shippers.Any(p => p.ShipperId == id))
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