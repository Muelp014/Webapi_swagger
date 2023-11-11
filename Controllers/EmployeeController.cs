using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;

namespace Northwind.MySQL.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly NorthwindContext _context;

    public EmployeeController(NorthwindContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        var Employees = await _context.Employees
            .ToListAsync();
       
        return Employees;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
    var Employee = await _context.Employees.FindAsync(id);

    if (Employee == null)
    {
        return NotFound(); // Return 404 Not Found if the Employee with the specified ID is not found
    }

    return Employee;
    }
   
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
    var Employee = await _context.Employees.FindAsync(id);

    if (Employee == null)
        {
        return NotFound();
        }

        _context.Employees.Remove(Employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee newEmployee)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    _context.Employees.Add(newEmployee);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetEmployees), new { id = newEmployee.EmployeeId }, newEmployee);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateEmployee(int id, Employee updatedEmployee)
{
    if (id != updatedEmployee.EmployeeId)
    {
        return BadRequest("Invalid ID");
    }

    _context.Entry(updatedEmployee).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Employees.Any(p => p.EmployeeId == id))
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