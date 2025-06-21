using Microsoft.AspNetCore.Mvc;
using KaraokeAPI.Data;
using KaraokeAPI.Models;

namespace KaraokeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all rooms
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Rooms.ToList());
        }

        // GET a specific room
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        // POST: Create new room
        [HttpPost]
        public IActionResult Create([FromBody] Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return Ok(room);
            
        }

        // PUT: Update room
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Room updatedRoom)
        {
            var room = _context.Rooms.Find(id);
            if (room == null) return NotFound();

            room.Name = updatedRoom.Name;
            room.Capacity = updatedRoom.Capacity;
            room.PricePerHour = updatedRoom.PricePerHour;
            room.ImageUrl = updatedRoom.ImageUrl;

            _context.SaveChanges();
            return Ok(room);
        }

        // DELETE: Delete room
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null) return NotFound();

            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return Ok(new { message = "Đã xóa phòng thành công." });
        }
        [HttpPost("upload")]
public async Task<IActionResult> UploadImage(IFormFile image)
{
    if (image == null || image.Length == 0)
        return BadRequest("No file uploaded");

    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    Directory.CreateDirectory(uploadsFolder);

    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
    var filePath = Path.Combine(uploadsFolder, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await image.CopyToAsync(stream);
    }

    // Trả về đường dẫn ảnh
    var imageUrl = "/uploads/" + fileName;
    return Ok(new { imageUrl });
}

    }
}
