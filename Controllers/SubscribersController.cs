using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midterm.Data;
using Midterm.Models.Dtos;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SubscribersController : ControllerBase
{
    private readonly MidtermDbContext _context;

    public SubscribersController(MidtermDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubscriberDto>>> GetSubscribers()
    {
        var list = await _context.Subscribers
            .Select(s => new SubscriberDto
            {
                SubscriberNo = s.SubscriberNo,
                Name = s.Name
            }).ToListAsync();

        return Ok(list);
    }
}
