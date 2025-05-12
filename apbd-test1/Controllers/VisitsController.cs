using apbd_test1.Models.DTOs;
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
        try
        {
            var visit = await _dbService.GetVisit(id);
            return Ok(visit);
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    /*public async Task<IActionResult> NewVisit([FromBody] VisitDTO visit)
    {
        
    }*/
}