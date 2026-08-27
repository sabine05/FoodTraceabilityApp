using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodTraceabilityApp.Data;
using FoodTraceabilityApp.Models;

namespace FoodTraceabilityApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly AppDbContext _context;// The AppDbContext is injected into the controller through its constructor. This allows the controller to interact with the database using Entity Framework Core.

    public SuppliersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet] //swagger endpoint: GET /api/suppliers
    public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
    {
        return await _context.Suppliers.ToListAsync();// returns a list of all suppliers from the database asynchronously. The result is wrapped in an ActionResult to allow for proper HTTP response handling.
    }

    [HttpPost] //swagger endpoint: POST /api/suppliers
    public async Task<ActionResult<Supplier>> CreateSupplier(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, supplier);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Supplier>> GetSupplier(int id)
    {
    var supplier = await _context.Suppliers.FindAsync(id);

    if (supplier == null)
    {
        return NotFound();
    }

    return supplier;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplier(int id, Supplier supplier)
    {
    if (id != supplier.Id)
    {
        return BadRequest();
    }

    _context.Entry(supplier).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
    var supplier = await _context.Suppliers.FindAsync(id);

    if (supplier == null)
    {
        return NotFound();
    }

    _context.Suppliers.Remove(supplier);
    await _context.SaveChangesAsync();

    return NoContent();
    }


}