using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodTraceabilityApp.Data;
using FoodTraceabilityApp.Models;

namespace FoodTraceabilityApp.Controllers;

[ApiController]//swagger endpoint: GET /api/TraceabilityLinks
[Route("api/[controller]")]
public class TraceabilityLinksController : ControllerBase // Déclaration de la classe TraceabilityLinksController qui hérite de ControllerBase pour gérer les requêtes HTTP liées aux liens de traçabilité.
{
    private readonly AppDbContext _context; // Déclaration d'une variable privée _context de type AppDbContext pour accéder à la base de données.

    public TraceabilityLinksController(AppDbContext context) // Constructeur de la classe TraceabilityLinksController qui prend en paramètre un objet AppDbContext pour initialiser la variable _context.
    {
        _context = context;  // Initialisation de la variable _context avec l'objet context passé en paramètre.
    }


[HttpPost] //swagger endpoint: POST /api/TraceabilityLinks
public async Task<ActionResult<TraceabilityLink>> CreateTraceabilityLink(TraceabilityLink traceabilityLink)// Déclaration de la méthode CreateTraceabilityLink qui prend en paramètre un objet TraceabilityLink et retourne un ActionResult de type TraceabilityLink.
{
    var rawMaterial = await _context.RawMaterials
        .FindAsync(traceabilityLink.RawMaterialId);

    var finishedProduct = await _context.FinishedProducts
        .FindAsync(traceabilityLink.FinishedProductId);

    if (rawMaterial == null)
    {
        return BadRequest("La matière première n'existe pas.");
    }

    if (finishedProduct == null)
    {
        return BadRequest("Le produit fini n'existe pas.");
    }

    if (traceabilityLink.QuantityUsed <= 0)
    {
        return BadRequest("La quantité utilisée doit être supérieure à 0.");
    }

    var linkExists = await _context.TraceabilityLinks.AnyAsync(link =>
        link.FinishedProductId == traceabilityLink.FinishedProductId &&
        link.RawMaterialId == traceabilityLink.RawMaterialId);

    if (linkExists)
    {
        return BadRequest("Ce lien de traçabilité existe déjà.");
    }

    _context.TraceabilityLinks.Add(traceabilityLink);
    await _context.SaveChangesAsync();

    return Ok(traceabilityLink);
}

[HttpGet]//swagger endpoint: GET /api/TraceabilityLinks
public async Task<ActionResult<IEnumerable<TraceabilityLink>>> GetTraceabilityLinks()
{
    return await _context.TraceabilityLinks.ToListAsync();
}

[HttpGet("product/{finishedProductId}")] // on retourne les donnée en ayant entrer un id d'un produit fini
public async Task<ActionResult> GetByFinishedProduct(int finishedProductId)
{
    var result = await _context.TraceabilityLinks
        .Where(link => link.FinishedProductId == finishedProductId)
        .Join(
            _context.RawMaterials,
            link => link.RawMaterialId,
            rawMaterial => rawMaterial.Id,
            (link, rawMaterial) => new
            {
                link.Id,
                link.FinishedProductId,
                RawMaterialId = rawMaterial.Id,
                RawMaterialName = rawMaterial.Name,
                BatchNumber = rawMaterial.BatchNumber,
                QuantityUsed = link.QuantityUsed
            }
        )
        .ToListAsync();

    if (result.Count == 0)
    {
        return NotFound("Aucune matière première trouvée pour ce produit fini.");
    }

    return Ok(result);
}

[HttpGet("rawmaterial/{rawMaterialId}")] // on retourne les donnée en ayant entrer un id d'une matière première
public async Task<ActionResult> GetByRawMaterial(int rawMaterialId)
{
    var result = await _context.TraceabilityLinks
        .Where(link => link.RawMaterialId == rawMaterialId)
        .Join(
            _context.FinishedProducts,
            link => link.FinishedProductId,
            finishedProduct => finishedProduct.Id,
            (link, finishedProduct) => new
            {
                link.Id,
                RawMaterialId = link.RawMaterialId,
                FinishedProductId = finishedProduct.Id,
                FinishedProductName = finishedProduct.Name,
                InternalBatchNumber = finishedProduct.InternalBatchNumber,
                QuantityUsed = link.QuantityUsed
            }
        )
        .ToListAsync();

    if (result.Count == 0)
    {
        return NotFound("Aucun produit fini trouvé pour cette matière première.");
    }

    return Ok(result);
}

[HttpPut("{id}")] //swagger endpoint: PUT /api/TraceabilityLinks/{id}
    public async Task<IActionResult> UpdateTraceabilityLink(int id, TraceabilityLink traceabilityLink)
    {
    if (id != traceabilityLink.Id)
    {
        return BadRequest();
    }

    _context.Entry(traceabilityLink).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
    }

 [HttpDelete("{id}")] //swagger endpoint: DELETE /api/TraceabilityLinks/{id}
    public async Task<IActionResult> DeleteTraceabilityLink(int id)
    {
    var traceabilityLink = await _context.TraceabilityLinks.FindAsync(id);

    if (traceabilityLink == null)
    {
        return NotFound();
    }

    _context.TraceabilityLinks.Remove(traceabilityLink);
    await _context.SaveChangesAsync();

    return NoContent();
    }

}