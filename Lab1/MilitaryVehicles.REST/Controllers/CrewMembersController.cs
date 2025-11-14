using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;
using MilitaryVehicles.REST.Models;

namespace MilitaryVehicles.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrewMembersController : ControllerBase
    {
        private readonly MilitaryVehiclesContext _context;

        public CrewMembersController(MilitaryVehiclesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<CrewMemberResponseModel>> GetAll()
        {
            return await _context.CrewMembers
                .Include(c => c.Vehicles)
                .Select(c => new CrewMemberResponseModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Rank = c.Rank,
                    VehicleIds = c.Vehicles.Select(v => v.Id).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CrewMemberResponseModel>> Get(Guid id)
        {
            var c = await _context.CrewMembers
                .Include(cm => cm.Vehicles)
                .FirstOrDefaultAsync(cm => cm.Id == id);

            if (c == null) return NotFound();

            return new CrewMemberResponseModel
            {
                Id = c.Id,
                Name = c.Name,
                Rank = c.Rank,
                VehicleIds = c.Vehicles.Select(v => v.Id).ToList()
            };
        }

        [HttpPost]
        public async Task<ActionResult> Create(CrewMemberCreateModel model)
        {
            var crew = new CrewMemberModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Rank = model.Rank
            };

            _context.CrewMembers.Add(crew);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = crew.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, CrewMemberUpdateModel model)
        {
            var crew = await _context.CrewMembers.FindAsync(id);
            if (crew == null) return NotFound();

            if (model.Name != null) crew.Name = model.Name;
            if (model.Rank != null) crew.Rank = model.Rank;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var crew = await _context.CrewMembers.FindAsync(id);
            if (crew == null) return NotFound();

            _context.CrewMembers.Remove(crew);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}