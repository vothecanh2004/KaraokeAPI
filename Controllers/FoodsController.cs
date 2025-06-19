using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KaraokeAPI.Data;
using KaraokeAPI.Models;

namespace KaraokeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Foods.ToList());

        [HttpPost]
        public IActionResult Create([FromBody] Food food)
        {
            _context.Foods.Add(food);
            _context.SaveChanges();
            return Ok(food);
        }
        [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Food updatedFood)
    {
        var food = _context.Foods.Find(id);
        if (food == null) return NotFound();

        food.Name = updatedFood.Name;
        food.Price = updatedFood.Price;
        food.ImageUrl = updatedFood.ImageUrl;

         _context.SaveChanges();
        return Ok(food);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var food = _context.Foods.Find(id);
        if (food == null) return NotFound();

    _context.Foods.Remove(food);
    _context.SaveChanges();
    return Ok(new { message = "Xoá món ăn thành công." });
    }

    } 
}
