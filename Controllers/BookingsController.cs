using Microsoft.AspNetCore.Mvc;
using KaraokeAPI.Data;
using KaraokeAPI.Models;

namespace KaraokeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            // Thêm booking vào database
            _context.Bookings.Add(booking);
            _context.SaveChanges(); // Lúc này booking.Id đã có

            // Tạo hóa đơn tương ứng
            var bill = new Bill
            {
                BookingId = booking.Id,
                RoomId = booking.RoomId, // lấy từ booking
                TotalAmount = 0,
                CreatedAt = DateTime.Now,
                Status = "Chưa thanh toán"
            };

            _context.Bills.Add(bill);
            _context.SaveChanges();

            return Ok(new
            {
                Booking = booking,
                Bill = bill
            });
        }

        [HttpGet]
        public IActionResult GetBookings()
        {
            var bookings = _context.Bookings.ToList();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookingById(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return NotFound(new { message = "Không tìm thấy đặt phòng với ID này." });
            }
            return Ok(booking);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return NotFound(new { message = "Không tìm thấy đặt phòng với ID này." });
            }

            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return Ok(new { message = "Đã hủy đặt phòng thành công." });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] Booking updatedBooking)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return NotFound(new { message = "Không tìm thấy đặt phòng với ID này." });
            }

            booking.Name = updatedBooking.Name;
            booking.PhoneNumber = updatedBooking.PhoneNumber;
            booking.Email = updatedBooking.Email;
            booking.BookingTime = updatedBooking.BookingTime;
            booking.DurationHours = updatedBooking.DurationHours;
            booking.RoomId = updatedBooking.RoomId;

            _context.SaveChanges();

            return Ok(new { message = "Cập nhật đặt phòng thành công.", data = booking });
        }

        [HttpGet("search")]
        public IActionResult SearchBookings([FromQuery] string name, [FromQuery] string phoneNumber)
        {
            var query = _context.Bookings.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(b => b.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                query = query.Where(b => b.PhoneNumber.Contains(phoneNumber));
            }

            var bookings = query.ToList();
            return Ok(bookings);
        }
    }
}
