using Microsoft.AspNetCore.Mvc;
using KaraokeAPI.Data;

namespace KaraokeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Statistics/summary
        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            var today = DateTime.Today;

            var totalRevenueToday = _context.Bills
                .Where(b => b.CreatedAt.Date == today)
                .Sum(b => (decimal?)b.TotalAmount) ?? 0;

            var totalRooms = _context.Rooms.Count();

            var totalBookingsToday = _context.Bookings
                .Count(b => b.BookingTime.Date == today);

            return Ok(new
            {
                totalRevenueToday,
                totalRooms,
                totalBookingsToday
            });
        }
    }
}
