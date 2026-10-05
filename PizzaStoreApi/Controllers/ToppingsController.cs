using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApi.Data;
using PizzaStoreApi.DTOs;
using PizzaStoreApi.Models;

namespace PizzaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToppingsController : ControllerBase
    {
        private readonly PizzaDbContext _context;

        public ToppingsController(PizzaDbContext context)
        {
            _context = context;
        }

        // GET: api/toppings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToppingDto>>> GetToppings()
        {
            return await _context.Toppings
                .Select(t => new ToppingDto { Id = t.Id, Name = t.Name })
                .ToListAsync();
        }

        // GET: api/toppings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToppingDto>> GetTopping(int id)
        {
            var topping = await _context.Toppings.FindAsync(id);

            if (topping == null) return NotFound();

            return new ToppingDto { Id = topping.Id, Name = topping.Name };
        }

        // POST: api/toppings
        [HttpPost]
        public async Task<ActionResult<ToppingDto>> PostTopping(ToppingCreateUpdateDto dto)
        {
            // Prevent duplicate topping names[cite: 1]
            if (await _context.Toppings.AnyAsync(t => t.Name.ToLower() == dto.Name.ToLower()))
            {
                return Conflict(new { message = "A topping with this name already exists." });
            }

            var topping = new Topping { Name = dto.Name };
            _context.Toppings.Add(topping);
            await _context.SaveChangesAsync();

            var resultDto = new ToppingDto { Id = topping.Id, Name = topping.Name };
            
            // Returns 201 Created with the location URI of the new resource[cite: 1]
            return CreatedAtAction(nameof(GetTopping), new { id = topping.Id }, resultDto);
        }

        // PUT: api/toppings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopping(int id, ToppingCreateUpdateDto dto)
        {
            var topping = await _context.Toppings.FindAsync(id);
            if (topping == null) return NotFound();

            // Prevent duplicate topping names when updating[cite: 1]
            if (await _context.Toppings.AnyAsync(t => t.Id != id && t.Name.ToLower() == dto.Name.ToLower()))
            {
                return Conflict(new { message = "Another topping with this name already exists." });
            }

            topping.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/toppings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopping(int id)
        {
            var topping = await _context.Toppings.FindAsync(id);
            if (topping == null) return NotFound();

            _context.Toppings.Remove(topping);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}