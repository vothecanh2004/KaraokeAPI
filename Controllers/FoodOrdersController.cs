using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KaraokeAPI.Data;
using KaraokeAPI.Models;

namespace KaraokeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodOrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() =>
            Ok(_context.FoodOrders.Include(f => f.Food).Include(r => r.Room).ToList());

        [HttpPost]
        public IActionResult Order([FromBody] FoodOrder order)
        {
         _context.FoodOrders.Add(order);
            _context.SaveChanges();
            return Ok(order);
        }

        [HttpGet("room/{roomId}")]
        public IActionResult GetByRoom(int roomId)
        {
            var orders = _context.FoodOrders
                .Where(o => o.RoomId == roomId)
                .Include(f => f.Food)
                .ToList();
            return Ok(orders);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FoodOrder updatedOrder)
    {
        var existingOrder = _context.FoodOrders.Find(id);
        if (existingOrder == null) return NotFound();

        existingOrder.FoodId = updatedOrder.FoodId;
        existingOrder.RoomId = updatedOrder.RoomId;
        existingOrder.Quantity = updatedOrder.Quantity;
        existingOrder.OrderedAt = updatedOrder.OrderedAt;

         _context.SaveChanges();
    return Ok(existingOrder);
    }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
    {
        var order = _context.FoodOrders.Find(id);
        if (order == null) return NotFound();

        _context.FoodOrders.Remove(order);
        _context.SaveChanges();
        return Ok(new { message = "Đã xoá đơn gọi món thành công." });
    }

    }
}
