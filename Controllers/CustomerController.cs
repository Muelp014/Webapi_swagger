using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

namespace Northwind.MySQL.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly NorthwindContext _context;

    public CustomerController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var Customers = await _context.Customers
            .ToListAsync();
       
        return Customers;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
    var Customer = await _context.Customers.FindAsync(id);

    if (Customer == null)
    {
        return NotFound(); // Return 404 Not Found if the Customer with the specified ID is not found
    }

    return Customer;
    }
   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
    var Customer = await _context.Customers.FindAsync(id);

    if (Customer == null)
        {
        return NotFound();
        }

        _context.Customers.Remove(Customer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
public async Task<ActionResult<Customer>> CreateCustomer([FromBody] Customer newCustomer)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    _context.Customers.Add(newCustomer);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetCustomers), new { id = newCustomer.CustomerId }, newCustomer);
}



}