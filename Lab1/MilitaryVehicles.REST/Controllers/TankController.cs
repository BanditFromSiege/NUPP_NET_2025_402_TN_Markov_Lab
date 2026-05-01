using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilitaryVehicles.common;
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [Authorize(Roles = "Editor,Admin")]
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
        [Authorize(Roles = "Editor,Admin")]
        public async Task<ActionResult> Update(Guid id, TankUpdateModel model)
        {
            try
            {
                var tank = await _tankService.ReadAsync(id);

                if (!string.IsNullOrEmpty(model.Model))
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
        [Authorize(Roles = "Editor,Admin")]
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