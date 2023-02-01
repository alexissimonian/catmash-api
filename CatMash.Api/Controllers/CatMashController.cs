using CatMash.Api.Models;
using CatMash.Domain.Interfaces;
using CatMash.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatMash.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatMashController : ControllerBase
{
    private readonly ICatMashService _service;

    public CatMashController(ICatMashService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Cat>>> GetTwoDifferentCatsAsync()
    {
        var cats = await _service.GetTwoRandomCatsAsync();
        var response = cats.Select(GetCatApiFromCat);
        return Ok(response);
    }
    
    [HttpGet("worldwide-scores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CatScoreApi>>> GetAllCatsScoreAsync()
    {
        var catScores = await _service.GetAllCatsScoreAsync();
        var response = catScores.Select(c => new CatScoreApi(c.Key.Id, c.Key.Url, c.Value));
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveCatScoresAsync(CatScoreRequest[] request)
    {
        var convertedRequest = request.ToDictionary(r => r.Id, r => r.Score);
        await _service.SaveCatScoresAsync(convertedRequest);
        return NoContent();
    }

    private CatApi GetCatApiFromCat(Cat cat)
    {
        return new CatApi(cat.Id, cat.Url);
    }
}