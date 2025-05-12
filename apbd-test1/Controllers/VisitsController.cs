using apbd_test1.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_test1.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class VisitsController : ControllerBase
{
    private readonly IDbService _dbService;

    public VisitsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVisit(int id)
    {
        return Ok();
    }
}