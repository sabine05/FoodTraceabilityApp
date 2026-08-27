using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodTraceabilityApp.Controllers;
using FoodTraceabilityApp.Data;
using FoodTraceabilityApp.Models;

namespace FoodTraceabilityApp.Tests;

public class UnitTest1
{
    [Fact]
     public async Task CreateTraceabilityLink_ReturnsBadRequest_WhenRawMaterialDoesNotExist()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        using var context = new AppDbContext(options);

        context.FinishedProducts.Add(new FinishedProduct
        {
            Id = 1,
            Name = "Salade composée",
            InternalBatchNumber = "PF-001",
            ProductionDate = DateTime.Now,
            Quantity = 100
        });
        await context.SaveChangesAsync();

        var controller = new TraceabilityLinksController(context);

        var traceabilityLink = new TraceabilityLink
        {
            FinishedProductId = 1,
            RawMaterialId = 999,
            QuantityUsed = 20
        };

        var result = await controller.CreateTraceabilityLink(traceabilityLink);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("La matière première n'existe pas.", badRequest.Value);
    }

   [Fact]
public async Task CreateTraceabilityLink_ReturnsBadRequest_WhenFinishedProductDoesNotExist()
{
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDatabase2")
        .Options;
    using var context = new AppDbContext(options);

    context.RawMaterials.Add(new RawMaterial
    {
        Id = 1,
        Name = "Tomate",
        BatchNumber = "MP-001",
        ReceptionDate = DateTime.Now,
        ExpiryDate = DateTime.Now.AddDays(10),
        Status = "Disponible",
        SupplierId = 1
    });
    await context.SaveChangesAsync();

    var controller = new TraceabilityLinksController(context);

    var traceabilityLink = new TraceabilityLink
    {
        FinishedProductId = 999,
        RawMaterialId = 1,
        QuantityUsed = 20
    };

    var result = await controller.CreateTraceabilityLink(traceabilityLink);

    var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
    Assert.Equal("Le produit fini n'existe pas.", badRequest.Value);
}

[Fact]
public async Task CreateTraceabilityLink_ReturnsOk_WhenBothExist()
{
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDatabase3")
        .Options;
    using var context = new AppDbContext(options);

    context.FinishedProducts.Add(new FinishedProduct
    {
        Id = 1,
        Name = "Salade composée",
        InternalBatchNumber = "PF-001",
        ProductionDate = DateTime.Now,
        Quantity = 100
    });

    context.RawMaterials.Add(new RawMaterial
    {
        Id = 1,
        Name = "Tomate",
        BatchNumber = "MP-001",
        ReceptionDate = DateTime.Now,
        ExpiryDate = DateTime.Now.AddDays(10),
        Status = "Disponible",
        SupplierId = 1
    });

    await context.SaveChangesAsync();

    var controller = new TraceabilityLinksController(context);

    var traceabilityLink = new TraceabilityLink
    {
        FinishedProductId = 1,
        RawMaterialId = 1,
        QuantityUsed = 20
    };

    var result = await controller.CreateTraceabilityLink(traceabilityLink);

    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var returnedLink = Assert.IsType<TraceabilityLink>(okResult.Value);
    Assert.Equal(20, returnedLink.QuantityUsed);
}

}
    
