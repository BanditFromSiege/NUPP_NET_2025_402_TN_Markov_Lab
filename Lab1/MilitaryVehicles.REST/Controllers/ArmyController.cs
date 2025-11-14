using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;
using MilitaryVehicles.REST.Models;

namespace MilitaryVehicles.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArmiesController : ControllerBase
    {
        private readonly MilitaryVehiclesContext _context;

        public ArmiesController(MilitaryVehiclesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<ArmyResponseModel>> GetAll()
        {
            return await _context.Armies
                .Include(a => a.Vehicles)
                .Select(a => new ArmyResponseModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    VehicleIds = a.Vehicles.Select(v => v.Id).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArmyResponseModel>> Get(Guid id)
        {
            var army = await _context.Armies
                .Include(a => a.Vehicles)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (army == null) return NotFound();

            return new ArmyResponseModel
            {
                Id = army.Id,
                Name = army.Name,
                VehicleIds = army.Vehicles.Select(v => v.Id).ToList()
            };
        }

        [HttpPost]
        public async Task<ActionResult> Create(ArmyCreateModel model)
        {
            var army = new ArmyModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name
            };

            _context.Armies.Add(army);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = army.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, ArmyUpdateModel model)
        {
            var army = await _context.Armies.FindAsync(id);
            if (army == null) return NotFound();

            army.Name = model.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var army = await _context.Armies.FindAsync(id);
            if (army == null) return NotFound();

            _context.Armies.Remove(army);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}