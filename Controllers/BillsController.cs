using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midterm.Data;
using Midterm.Models.Dtos;
using Midterm.Models;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BillsController : ControllerBase
{
    private readonly MidtermDbContext _context;

    public BillsController(MidtermDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateBill([FromBody] CalculateBillRequest request)
    {
        var phoneMinutes = await _context.Usage
            .Where(u => u.SubscriberNo == request.SubscriberNo &&
                        u.Year == request.Year &&
                        u.Month == request.Month &&
                        u.UsageType == "Phone")
            .SumAsync(u => u.Amount);

        var internetMB = await _context.Usage
            .Where(u => u.SubscriberNo == request.SubscriberNo &&
                        u.Year == request.Year &&
                        u.Month == request.Month &&
                        u.UsageType == "Internet")
            .SumAsync(u => u.Amount);

        int extraMinutes = Math.Max(0, phoneMinutes - 1000);
        decimal phoneCost = Math.Ceiling(extraMinutes / 1000.0m) * 10;

        decimal internetCost = 50;
        if (internetMB > 20000)
        {
            int extraMB = internetMB - 20000;
            internetCost += Math.Ceiling(extraMB / 10000.0m) * 10;
        }

        decimal total = phoneCost + internetCost;

        var bill = new Bill
        {
            SubscriberNo = request.SubscriberNo,
            Year = request.Year,
            Month = request.Month,
            PhoneMinutesUsed = phoneMinutes,
            InternetUsedMb = internetMB,
            TotalAmount = total,
            IsPaid = false
        };

        _context.Bills.Add(bill);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Status = "Success",
            PhoneCost = phoneCost,
            InternetCost = internetCost,
            Total = total
        });
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetBills([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0)
            return BadRequest("page and pageSize must be greater than 0");

        var totalItems = await _context.Bills.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var bills = await _context.Bills
            .OrderBy(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BillDto
            {
                Id = b.Id,
                SubscriberNo = b.SubscriberNo,
                Year = b.Year,
                Month = b.Month,
                PhoneMinutesUsed = b.PhoneMinutesUsed,
                InternetUsedMb = b.InternetUsedMb,
                TotalAmount = b.TotalAmount,
                IsPaid = b.IsPaid
            })
            .ToListAsync();

        return Ok(new
        {
            TotalItems = totalItems,
            TotalPages = totalPages,
            CurrentPage = page,
            PageSize = pageSize,
            Data = bills
        });
    }

    [AllowAnonymous]
    [HttpGet("query")]
    public async Task<IActionResult> QueryBill([FromQuery] int subscriberNo, [FromQuery] int year, [FromQuery] int month)
    {
        var bill = await _context.Bills
            .Where(b => b.SubscriberNo == subscriberNo && b.Year == year && b.Month == month)
            .FirstOrDefaultAsync();

        if (bill == null)
            return NotFound(new { Status = "Not Found" });

        return Ok(new
        {
            Month = new DateTime(year, month, 1).ToString("MMM yyyy"),
            Total = bill.TotalAmount + "$",
            IsPaid = bill.IsPaid
        });
    }

    [Authorize]
    [HttpGet("query-detailed")]
    public async Task<IActionResult> QueryBillDetailed([FromQuery] int subscriberNo, [FromQuery] int year, [FromQuery] int month)
    {
        var bill = await _context.Bills
            .FirstOrDefaultAsync(b => b.SubscriberNo == subscriberNo && b.Year == year && b.Month == month);

        if (bill == null)
            return NotFound(new { Status = "Not Found" });

        int extraMinutes = Math.Max(0, bill.PhoneMinutesUsed - 1000);
        decimal phoneCost = Math.Ceiling(extraMinutes / 1000.0m) * 10;

        decimal internetCost = 50;
        if (bill.InternetUsedMb > 20000)
        {
            int extraMB = bill.InternetUsedMb - 20000;
            internetCost += Math.Ceiling(extraMB / 10000.0m) * 10;
        }

        return Ok(new
        {
            Month = new DateTime(year, month, 1).ToString("MMM yyyy"),
            Total = bill.TotalAmount,
            IsPaid = bill.IsPaid,
            PhoneMinutes = bill.PhoneMinutesUsed,
            PhoneCost = phoneCost,
            InternetMB = bill.InternetUsedMb,
            InternetCost = internetCost
        });
    }
}