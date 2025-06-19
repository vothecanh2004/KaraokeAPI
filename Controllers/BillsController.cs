using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KaraokeAPI.Data;
using KaraokeAPI.Models;

namespace KaraokeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Bills
        [HttpGet]
        public IActionResult GetAll()
        {
            var bills = _context.Bills
                .Include(b => b.Room)
                .Include(b => b.Booking)
                .ToList();

            return Ok(bills);
        }

        // GET: api/Bills/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var bill = _context.Bills
                .Include(b => b.Room)
                .Include(b => b.Booking)
                .FirstOrDefault(b => b.Id == id);

            if (bill == null)
                return NotFound();

            return Ok(bill);
        }

        // POST: api/Bills
        [HttpPost]
        public IActionResult Create([FromBody] Bill bill)
        {
            var room = _context.Rooms.Find(bill.RoomId);
            var booking = _context.Bookings.Find(bill.BookingId);

            if (room == null || booking == null)
                return BadRequest("Room hoặc Booking không tồn tại.");

            bill.CreatedAt = DateTime.Now;

            _context.Bills.Add(bill);
            _context.SaveChanges();

            return Ok(bill);
        }
        // PUT: Cập nhật hóa đơn theo ID
[HttpPut("{id}")]
public IActionResult Update(int id, [FromBody] Bill updatedBill)
{
    var existingBill = _context.Bills.Find(id);
    if (existingBill == null) return NotFound();

    existingBill.RoomId = updatedBill.RoomId;
    existingBill.BookingId = updatedBill.BookingId;
    existingBill.TotalAmount = updatedBill.TotalAmount;
    existingBill.CreatedAt = updatedBill.CreatedAt;

    _context.SaveChanges();
    return Ok(existingBill);
}

// DELETE: Xóa hóa đơn theo ID
[HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    var bill = _context.Bills.Find(id);
    if (bill == null) return NotFound();

    _context.Bills.Remove(bill);
    _context.SaveChanges();
    return Ok(new { message = "Đã xóa hóa đơn thành công." });
}

        // GET: api/Bills/total
        [HttpGet("total")]
        public IActionResult GetTotalAmount()
        {
            var total = _context.Bills.Sum(b => b.TotalAmount);
            return Ok(new { TotalAmount = total });
        }

        // GET: api/Bills/room/{roomId}
        [HttpGet("room/{roomId}")]
        public IActionResult GetByRoom(int roomId)
        {
            var bills = _context.Bills
                .Where(b => b.RoomId == roomId)
                .Include(b => b.Room)
                .Include(b => b.Booking)
                .ToList();

            return Ok(bills);
        }
    }
}
