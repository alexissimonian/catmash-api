using CatMash.Domain.Interfaces;
using CatMash.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatMash.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CatMashController : ControllerBase
{
    private readonly ICatMashService _service;

    public CatMashController(ICatMashService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Cat>>> GetTwoDifferentCats()
    {
        var cats = await _service.GetTwoRandomCatsAsync();
        return Ok(cats);
    }
}