using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midterm.Data;
using Midterm.Models;
using Midterm.Models.Dtos;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsageController : ControllerBase
{
    private readonly MidtermDbContext _context;

    public UsageController(MidtermDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsageDto>>> GetUsage()
    {
        var usageList = await _context.Usage
            .Select(u => new UsageDto
            {
                Id = u.Id,
                SubscriberNo = u.SubscriberNo,
                Year = u.Year,
                Month = u.Month,
                UsageType = u.UsageType,
                Amount = u.Amount
            })
            .ToListAsync();

        return Ok(usageList);
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddUsage([FromBody] AddUsageRequest request)
    {
        if (request.UsageType is not ("Phone" or "Internet"))
            return BadRequest(new { Status = "Invalid usage type" });

        var usage = new Usage
        {
            SubscriberNo = request.SubscriberNo,
            Year = DateTime.Now.Year,
            Month = request.Month,
            UsageType = request.UsageType,
            Amount = request.UsageType == "Phone" ? 10 : 1
        };

        _context.Usage.Add(usage);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Status = "Success",
            usage.SubscriberNo,
            usage.UsageType,
            AddedAmount = usage.Amount
        });
    }
}
