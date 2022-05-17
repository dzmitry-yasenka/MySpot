using Microsoft.AspNetCore.Mvc;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
    private readonly ILogger<ExampleController> _logger;

    public ExampleController(ILogger<ExampleController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IResult> Get()
    {
        await Task.Delay(100);
        _logger.LogInformation("GET method was called at {Timestamp}", DateTime.UtcNow);
        return Results.Ok();
    }
    
    [HttpPost]
    public async Task<IResult> Post()
    {
        await Task.Delay(100);
        _logger.LogInformation("POST method was called at {Timestamp}", DateTime.UtcNow);
        return Results.Ok();
    }
    
    [HttpPut]
    public async Task<IResult> Put()
    {
        await Task.Delay(100);
        _logger.LogInformation("PUT method was called at {Timestamp}", DateTime.UtcNow);
        return Results.Ok();
    }
    
    [HttpDelete]
    public async Task<IResult> Delete()
    {
        await Task.Delay(100);
        _logger.LogInformation("DELETE method was called at {Timestamp}", DateTime.UtcNow);
        return Results.Ok();
    }
}