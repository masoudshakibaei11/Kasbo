using Kasbo.Models;
using Kasbo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChartController : ControllerBase
{

    private readonly ILogger<ChartController> _logger;
    private readonly IChartService _chartService;

    public ChartController(IChartService chartService, ILogger<ChartController> logger)
    {
        _chartService = chartService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("Add")]
    public async Task<IActionResult> Add(AddChartDTO addChartDTO)
    {
        try
        {
            await _chartService.Add(addChartDTO);
            _logger.Log(LogLevel.Information,"AddChart Ok From User");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error,"AddChart Error From User   "+ex.Message);
            return BadRequest(ex.Message);
        }
    }

}