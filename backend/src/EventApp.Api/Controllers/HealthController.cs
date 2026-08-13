using Microsoft.AspNetCore.Mvc;

namespace EventApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet(Name = "Health")]
    public bool Get()
    {
        return true;
    }
}
