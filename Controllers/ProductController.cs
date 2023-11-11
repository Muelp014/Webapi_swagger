using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

namespace Northwind.MySQL.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly NorthwindContext _context;

    public ProductController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _context.Products
            .ToListAsync();
       
        return products;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
    var product = await _context.Products.FindAsync(id);

    if (product == null)
    {
        return NotFound(); // Return 404 Not Found if the product with the specified ID is not found
    }

    return product;
    }
   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
    var product = await _context.Products.FindAsync(id);

    if (product == null)
        {
        return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
public async Task<ActionResult<Product>> CreateProduct([FromBody] Product newProduct)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    _context.Products.Add(newProduct);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetProducts), new { id = newProduct.ProductId }, newProduct);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
{
    if (id != updatedProduct.ProductId)
    {
        return BadRequest("Invalid ID");
    }

    _context.Entry(updatedProduct).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Products.Any(p => p.ProductId == id))
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