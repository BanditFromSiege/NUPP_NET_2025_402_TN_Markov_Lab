using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;
using MilitaryVehicles.REST.Models;

namespace MilitaryVehicles.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DestroyersController : ControllerBase
    {
        private readonly MilitaryVehiclesContext _context;

        public DestroyersController(MilitaryVehiclesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<DestroyerResponseModel>> GetAll()
        {
            return await _context.Destroyers
                .Include(d => d.CrewMembers)
                .Select(d => new DestroyerResponseModel
                {
                    Id = d.Id,
                    Model = d.Model,
                    Torpedoes = d.Torpedoes,
                    ArmyId = d.ArmyId,
                    CrewMemberIds = d.CrewMembers.Select(c => c.Id).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DestroyerResponseModel>> Get(Guid id)
        {
            var d = await _context.Destroyers
                .Include(d => d.CrewMembers)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (d == null) return NotFound();

            return new DestroyerResponseModel
            {
                Id = d.Id,
                Model = d.Model,
                Torpedoes = d.Torpedoes,
                ArmyId = d.ArmyId,
                CrewMemberIds = d.CrewMembers.Select(c => c.Id).ToList()
            };
        }

        [HttpPost]
        public async Task<ActionResult> Create(DestroyerCreateModel model)
        {
            var destroyer = new DestroyerModel
            {
                Id = Guid.NewGuid(),
                Model = model.Model,
                Torpedoes = model.Torpedoes,
                ArmyId = model.ArmyId
            };

            _context.Destroyers.Add(destroyer);

            if (model.CrewMemberIds.Any())
            {
                var crew = await _context.CrewMembers
                    .Where(c => model.CrewMemberIds.Contains(c.Id))
                    .ToListAsync();
                destroyer.CrewMembers = crew;
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = destroyer.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, DestroyerUpdateModel model)
        {
            var d = await _context.Destroyers
                .Include(d => d.CrewMembers)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (d == null) return NotFound();

            if (model.Model != null) d.Model = model.Model;
            if (model.Torpedoes.HasValue) d.Torpedoes = model.Torpedoes.Value;
            if (model.ArmyId.HasValue) d.ArmyId = model.ArmyId.Value;

            if (model.CrewMemberIds != null)
            {
                var crew = await _context.CrewMembers
                    .Where(c => model.CrewMemberIds.Contains(c.Id))
                    .ToListAsync();

                d.CrewMembers = crew;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var d = await _context.Destroyers.FindAsync(id);
            if (d == null) return NotFound();

            _context.Destroyers.Remove(d);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}