using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodTraceabilityApp.Data;
using FoodTraceabilityApp.Models;

namespace FoodTraceabilityApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinishedProductsController : ControllerBase
{
    private readonly AppDbContext _context;

public FinishedProductsController(AppDbContext context)
{
    _context = context;
}

[HttpGet] //swagger endpoint: GET /api/finishedproducts
public async Task<ActionResult<IEnumerable<FinishedProduct>>> GetFinishedProducts()
    {
        return await _context.FinishedProducts.ToListAsync();//Lit tous les produits finis enregistrés dans la base de données.
   
    }

[HttpGet("{id}")]
    public async Task<ActionResult<FinishedProduct>> GetFinishedProduct(int id)
    {
    var finishedProduct = await _context.FinishedProducts.FindAsync(id);

    if (finishedProduct == null)
    {
        return NotFound();
    }

    return finishedProduct;
    }

[HttpPost] //swagger endpoint: POST /api/finishedproducts
public async Task<ActionResult<FinishedProduct>> CreateFinishedProduct(FinishedProduct finishedProduct)
{

var rawMaterialsExist = await _context.RawMaterials.AnyAsync();

if (!rawMaterialsExist)
    {
        return BadRequest("Aucune matière première n'existe dans la table.");
    }

    _context.FinishedProducts.Add(finishedProduct);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetFinishedProduct), new { id = finishedProduct.Id }, finishedProduct);
}

 
 [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFinishedProduct(int id, FinishedProduct finishedProduct)
    {
    if (id != finishedProduct.Id)
    {
        return BadRequest();
    }

    _context.Entry(finishedProduct).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
    }

[HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFinishedProduct(int id)
    {
    var finishedProduct = await _context.FinishedProducts.FindAsync(id);

    if (finishedProduct == null)
    {
        return NotFound();
    }

    _context.FinishedProducts.Remove(finishedProduct);
    await _context.SaveChangesAsync();

    return NoContent();
    }


}