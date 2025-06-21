    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using KaraokeAPI.Data;
    using KaraokeAPI.Models;

    namespace KaraokeAPI.Controllers
    {
        [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbacksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả feedback
        [HttpGet]
        public IActionResult GetAll() =>
            Ok(_context.Feedbacks.Include(f => f.Room).ToList());

        // Lấy feedback theo phòng
        [HttpGet("room/{roomId}")]
        public IActionResult GetByRoom(int roomId)
        {
            var feedbacks = _context.Feedbacks
                .Where(f => f.RoomId == roomId)
                .OrderByDescending(f => f.CreatedAt)
                .ToList();

            return Ok(feedbacks);
        }

        // Gửi đánh giá
        [HttpPost]
        public IActionResult Create([FromBody] Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
            return Ok(feedback);
        }
            [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Feedback updated)
    {
        var feedback = _context.Feedbacks.Find(id);
        if (feedback == null) return NotFound();

        feedback.Name = updated.Name;
        feedback.Rating = updated.Rating;
        feedback.Comment = updated.Comment;
        feedback.RoomId = updated.RoomId;
        feedback.CreatedAt = DateTime.Now;

        _context.SaveChanges();
        return Ok(feedback);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var feedback = _context.Feedbacks.Find(id);
        if (feedback == null) return NotFound();

        _context.Feedbacks.Remove(feedback);
        _context.SaveChanges();

        return Ok(new { message = "Đã xóa đánh giá thành công." });
    }

    }

    }