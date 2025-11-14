using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.common;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;
using MilitaryVehicles.REST.Models;

namespace MilitaryVehicles.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TankController : ControllerBase
    {
        private readonly ICrudServiceAsync<TankModel> _tankService;

        public TankController(ICrudServiceAsync<TankModel> tankService)
        {
            _tankService = tankService;
        }

        [HttpGet]
        public async Task<IEnumerable<TankResponseModel>> GetAll()
        {
            var tanks = await _tankService.ReadAllAsync();
            return tanks.Select(t => new TankResponseModel
            {
                Id = t.Id,
                Model = t.Model,
                Firepower = t.Firepower,
                ArmyId = t.ArmyId,
                CrewMemberIds = t.CrewMembers.Select(c => c.Id).ToList()
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TankResponseModel>> Get(Guid id)
        {
            try
            {
                var t = await _tankService.ReadAsync(id);
                return new TankResponseModel
                {
                    Id = t.Id,
                    Model = t.Model,
                    Firepower = t.Firepower,
                    ArmyId = t.ArmyId,
                    CrewMemberIds = t.CrewMembers.Select(c => c.Id).ToList()
                };
            }
            catch
            {
                return NotFound();
            }
        }

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

            var created = await _tankService.CreateAsync(tank);
            if (!created) return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = tank.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, TankUpdateModel model)
        {
            try
            {
                var tank = await _tankService.ReadAsync(id);
                tank.Model = model.Model;
                tank.Firepower = model.Firepower;

                var updated = await _tankService.UpdateAsync(tank);
                if (!updated) return BadRequest();

                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var tank = await _tankService.ReadAsync(id);
                var deleted = await _tankService.RemoveAsync(tank);
                if (!deleted) return BadRequest();

                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
