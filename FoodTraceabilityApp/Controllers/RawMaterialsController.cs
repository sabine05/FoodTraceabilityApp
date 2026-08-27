using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodTraceabilityApp.Data;
using FoodTraceabilityApp.Models;

namespace FoodTraceabilityApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RawMaterialsController : ControllerBase
{
    private readonly AppDbContext _context;

public RawMaterialsController(AppDbContext context)
{
    _context = context;
}

[HttpGet] //swagger endpoint: GET /api/rawmaterials
public async Task<ActionResult<IEnumerable<RawMaterial>>> GetRawMaterials()
    {
        return await _context.RawMaterials.ToListAsync();// lit toute les matières premières. The result is wrapped in an ActionResult to allow for proper HTTP response handling.
   
    }

[HttpPost] //swagger endpoint: POST /api/rawmaterials
public async Task<ActionResult<RawMaterial>> CreateRawMaterial(RawMaterial rawMaterial)
{

var supplier = await _context.Suppliers.FindAsync(rawMaterial.SupplierId);

if (supplier == null)
{
    return NotFound("Le fournisseur n'existe pas.");
}

    _context.RawMaterials.Add(rawMaterial);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetRawMaterial), new { id = rawMaterial.Id }, rawMaterial);
}

 [HttpGet("{id}")]
    public async Task<ActionResult<RawMaterial>> GetRawMaterial(int id)
    {
    var rawMaterial = await _context.RawMaterials.FindAsync(id);

    if (rawMaterial == null)
    {
        return NotFound();
    }

    return rawMaterial;
    }

 [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRawMaterial(int id, RawMaterial rawMaterial)
    {
    if (id != rawMaterial.Id)
    {
        return BadRequest();
    }

    _context.Entry(rawMaterial).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
    }

[HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRawMaterial(int id)
    {
    var rawMaterial = await _context.RawMaterials.FindAsync(id);

    if (rawMaterial == null)
    {
        return NotFound();
    }

    _context.RawMaterials.Remove(rawMaterial);
    await _context.SaveChangesAsync();

    return NoContent();
    }


}