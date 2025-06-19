using Microsoft.AspNetCore.Mvc;
using KaraokeAPI.Data;
using KaraokeAPI.Models;

namespace KaraokeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PromotionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Promotions
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.Promotions.ToList();
            return Ok(list);
        }

        // GET: api/Promotions/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var promo = _context.Promotions.Find(id);
            if (promo == null) return NotFound();
            return Ok(promo);
        }

        // POST: api/Promotions
        [HttpPost]
        public IActionResult Create([FromBody] Promotion promo)
        {
            _context.Promotions.Add(promo);
            _context.SaveChanges();
            return Ok(promo);
        }

        // PUT: api/Promotions/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Promotion updated)
        {
            var promo = _context.Promotions.Find(id);
            if (promo == null) return NotFound();

            promo.Name = updated.Name;
            promo.Description = updated.Description;
            promo.DiscountPercent = updated.DiscountPercent;
            promo.StartDate = updated.StartDate;
            promo.EndDate = updated.EndDate;

            _context.SaveChanges();
            return Ok(promo);
        }

        // DELETE: api/Promotions/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var promo = _context.Promotions.Find(id);
            if (promo == null) return NotFound();

            _context.Promotions.Remove(promo);
            _context.SaveChanges();
            return Ok(new { message = "Đã xoá chương trình khuyến mãi." });
        }
    }
}
