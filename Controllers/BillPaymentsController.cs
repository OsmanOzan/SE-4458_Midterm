using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Midterm.Data;
using Midterm.Models;
using Midterm.Models.Dtos;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BillPaymentsController : ControllerBase
{
    private readonly MidtermDbContext _context;

    public BillPaymentsController(MidtermDbContext context)
    {
        _context = context;
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillPaymentDto>>> GetPayments()
    {
        var payments = await _context.BillPayments
            .Select(p => new BillPaymentDto
            {
                Id = p.Id,
                BillId = p.BillId,
                PaymentDate = p.PaymentDate,
                AmountPaid = p.AmountPaid
            })
            .ToListAsync();

        return Ok(payments);
    }
 
    [HttpPost]
    public async Task<IActionResult> PostPayment([FromBody] AddBillPaymentRequest request)
    {
        var bill = await _context.Bills.FindAsync(request.BillId);

        if (bill == null)
            return NotFound(new { Status = "Error", Message = "Bill not found." });

        var payment = new BillPayment
        {
            BillId = request.BillId,
            AmountPaid = request.AmountPaid
        };

        _context.BillPayments.Add(payment);

        if (request.AmountPaid >= bill.TotalAmount)
        {
            bill.IsPaid = true;
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Status = "Success",
            PaymentId = payment.Id,
            IsBillFullyPaid = bill.IsPaid
        });
    }
}
