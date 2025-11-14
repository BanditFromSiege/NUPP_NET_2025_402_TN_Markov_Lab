using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;
using MilitaryVehicles.REST.Models;

namespace MilitaryVehicles.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelicoptersController : ControllerBase
    {
        private readonly MilitaryVehiclesContext _context;

        public HelicoptersController(MilitaryVehiclesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<HelicopterResponseModel>> GetAll()
        {
            return await _context.Helicopters
                .Include(h => h.CrewMembers)
                .Select(h => new HelicopterResponseModel
                {
                    Id = h.Id,
                    Model = h.Model,
                    Speed = h.Speed,
                    ArmyId = h.ArmyId,
                    CrewMemberIds = h.CrewMembers.Select(c => c.Id).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HelicopterResponseModel>> Get(Guid id)
        {
            var heli = await _context.Helicopters
                .Include(h => h.CrewMembers)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (heli == null) return NotFound();

            return new HelicopterResponseModel
            {
                Id = heli.Id,
                Model = heli.Model,
                Speed = heli.Speed,
                ArmyId = heli.ArmyId,
                CrewMemberIds = heli.CrewMembers.Select(c => c.Id).ToList()
            };
        }

        [HttpPost]
        public async Task<ActionResult> Create(HelicopterCreateModel model)
        {
            var helicopter = new HelicopterModel
            {
                Id = Guid.NewGuid(),
                Model = model.Model,
                Speed = model.Speed,
                ArmyId = model.ArmyId
            };

            if (model.CrewMemberIds.Any())
            {
                var crewMembers = await _context.CrewMembers
                    .Where(c => model.CrewMemberIds.Contains(c.Id))
                    .ToListAsync();

                helicopter.CrewMembers = crewMembers;
            }

            _context.Helicopters.Add(helicopter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = helicopter.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, HelicopterUpdateModel model)
        {
            var heli = await _context.Helicopters
                .Include(h => h.CrewMembers)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (heli == null) return NotFound();

            if (model.Model != null) heli.Model = model.Model;
            if (model.Speed.HasValue) heli.Speed = model.Speed.Value;
            if (model.ArmyId.HasValue) heli.ArmyId = model.ArmyId.Value;

            if (model.CrewMemberIds != null)
            {
                var crew = await _context.CrewMembers
                    .Where(c => model.CrewMemberIds.Contains(c.Id))
                    .ToListAsync();

                heli.CrewMembers = crew;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var heli = await _context.Helicopters.FindAsync(id);
            if (heli == null) return NotFound();

            _context.Helicopters.Remove(heli);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}