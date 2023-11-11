using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

namespace Northwind.MySQL.Controllers;

[ApiController]
[Route("[controller]")]
public class SupplierController : ControllerBase
{
    private readonly NorthwindContext _context;

    public SupplierController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
    {
        var Suppliers = await _context.Suppliers
            .ToListAsync();
       
        return Suppliers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Supplier>> GetSupplier(int id)
    {
    var Supplier = await _context.Suppliers.FindAsync(id);

    if (Supplier == null)
    {
        return NotFound(); // Return 404 Not Found if the Supplier with the specified ID is not found
    }

    return Supplier;
    }
   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
    var Supplier = await _context.Suppliers.FindAsync(id);

    if (Supplier == null)
        {
        return NotFound();
        }

        _context.Suppliers.Remove(Supplier);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
public async Task<ActionResult<Supplier>> CreateSupplier([FromBody] Supplier newSupplier)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    _context.Suppliers.Add(newSupplier);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetSuppliers), new { id = newSupplier.SupplierId }, newSupplier);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateSupplier(int id, Supplier updatedSupplier)
{
    if (id != updatedSupplier.SupplierId)
    {
        return BadRequest("Invalid ID");
    }

    _context.Entry(updatedSupplier).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Suppliers.Any(p => p.SupplierId == id))
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