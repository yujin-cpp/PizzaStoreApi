using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApi.Data;
using PizzaStoreApi.DTOs;
using PizzaStoreApi.Models;

namespace PizzaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private readonly PizzaDbContext _context;

        public PizzasController(PizzaDbContext context)
        {
            _context = context;
        }

        // GET: api/pizzas
        // Lists all pizzas with their toppings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PizzaDto>>> GetPizzas()
        {
            var pizzas = await _context.Pizzas
                .Include(p => p.Toppings)
                .ToListAsync();

            return pizzas.Select(p => new PizzaDto
            {
                Id = p.Id,
                Name = p.Name,
                Toppings = p.Toppings.Select(t => new ToppingDto { Id = t.Id, Name = t.Name }).ToList()
            }).ToList();
        }

        // GET: api/pizzas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PizzaDto>> GetPizza(int id)
        {
            var pizza = await _context.Pizzas
                .Include(p => p.Toppings)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pizza == null) return NotFound();

            return new PizzaDto
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Toppings = pizza.Toppings.Select(t => new ToppingDto { Id = t.Id, Name = t.Name }).ToList()
            };
        }

        // POST: api/pizzas
        // Adds a new pizza with toppings[cite: 1]
        [HttpPost]
        public async Task<ActionResult<PizzaDto>> PostPizza(PizzaCreateUpdateDto dto)
        {
            // Prevent duplicate pizza names[cite: 1]
            if (await _context.Pizzas.AnyAsync(p => p.Name.ToLower() == dto.Name.ToLower()))
            {
                return Conflict(new { message = "A pizza with this name already exists." });
            }

            var pizza = new Pizza { Name = dto.Name };

            // Map provided Topping IDs to actual Topping entities
            if (dto.ToppingIds != null && dto.ToppingIds.Any())
            {
                var toppings = await _context.Toppings
                    .Where(t => dto.ToppingIds.Contains(t.Id))
                    .ToListAsync();
                pizza.Toppings = toppings;
            }

            _context.Pizzas.Add(pizza);
            await _context.SaveChangesAsync();

            var resultDto = new PizzaDto
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Toppings = pizza.Toppings.Select(t => new ToppingDto { Id = t.Id, Name = t.Name }).ToList()
            };

            return CreatedAtAction(nameof(GetPizza), new { id = pizza.Id }, resultDto);
        }

        // PUT: api/pizzas/5
        // Updates pizza details and its associated toppings[cite: 1]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPizza(int id, PizzaCreateUpdateDto dto)
        {
            var pizza = await _context.Pizzas
                .Include(p => p.Toppings)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pizza == null) return NotFound();

            // Prevent duplicate pizza names when updating[cite: 1]
            if (await _context.Pizzas.AnyAsync(p => p.Id != id && p.Name.ToLower() == dto.Name.ToLower()))
            {
                return Conflict(new { message = "Another pizza with this name already exists." });
            }

            pizza.Name = dto.Name;

            // Clear existing toppings and replace with the newly provided set
            pizza.Toppings.Clear();
            if (dto.ToppingIds != null && dto.ToppingIds.Any())
            {
                var toppings = await _context.Toppings
                    .Where(t => dto.ToppingIds.Contains(t.Id))
                    .ToListAsync();
                
                foreach (var topping in toppings)
                {
                    pizza.Toppings.Add(topping);
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/pizzas/5
        // Deletes a pizza[cite: 1]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizza(int id)
        {
            var pizza = await _context.Pizzas.FindAsync(id);
            if (pizza == null) return NotFound();

            _context.Pizzas.Remove(pizza);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}