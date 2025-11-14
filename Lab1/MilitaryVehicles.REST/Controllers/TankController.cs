using Microsoft.AspNetCore.Mvc;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;
using MilitaryVehicles.REST.Models;
using Microsoft.EntityFrameworkCore;

namespace MilitaryVehicles.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TanksController : ControllerBase
    {
        private readonly MilitaryVehiclesContext _context;

        public TanksController(MilitaryVehiclesContext context)
        {
            _context = context;
        }

        // GET api/tanks
        [HttpGet]
        public async Task<IEnumerable<TankResponseModel>> GetAll()
        {
            return await _context.Tanks
                .Include(t => t.CrewMembers)
                .Select(t => new TankResponseModel
                {
                    Id = t.Id,
                    Model = t.Model,
                    Firepower = t.Firepower,
                    ArmyId = t.ArmyId,
                    CrewMemberIds = t.CrewMembers.Select(c => c.Id).ToList()
                })
                .ToListAsync();
        }

        // GET api/tanks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TankResponseModel>> Get(Guid id)
        {
            var t = await _context.Tanks
                .Include(t => t.CrewMembers)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (t == null) return NotFound();

            return new TankResponseModel
            {
                Id = t.Id,
                Model = t.Model,
                Firepower = t.Firepower,
                ArmyId = t.ArmyId,
                CrewMemberIds = t.CrewMembers.Select(c => c.Id).ToList()
            };
        }

        // POST api/tanks
        [HttpPost]
        public async Task<ActionResult> Create(TankCreateModel model)
        {
            var tank = new TankModel
            {
                Id = Guid.NewGuid(),
                Model = model.Model,
                Firepower = model.Firepower,
                ArmyId = model.ArmyId
            };

            _context.Tanks.Add(tank);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = tank.Id }, null);
        }

        // PUT api/tanks/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, TankUpdateModel model)
        {
            var tank = await _context.Tanks.FindAsync(id);
            if (tank == null) return NotFound();

            tank.Model = model.Model;
            tank.Firepower = model.Firepower;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/tanks/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var tank = await _context.Tanks.FindAsync(id);
            if (tank == null) return NotFound();

            _context.Tanks.Remove(tank);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
