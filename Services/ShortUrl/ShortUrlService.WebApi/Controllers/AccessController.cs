using ShortUrlService.WebApi.Models;
using ShortUrlService.WebApi.Services;

namespace ShortUrlService.WebApi.Controllers;

[Route("/")]
[ApiController]
public class AccessController(IShortUrlService service) : ControllerBase
{
    [HttpGet("{shortKey}")]
    public Task<IActionResult> Get(string shortKey)
    {
        return service.AccessAsync(new AccessRequest(shortKey));
    }
}