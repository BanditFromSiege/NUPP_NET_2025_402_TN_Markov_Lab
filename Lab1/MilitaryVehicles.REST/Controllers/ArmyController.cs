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
    public class ArmyController : ControllerBase
    {
        private readonly ICrudServiceAsync<ArmyModel> _armyService;

        public ArmyController(ICrudServiceAsync<ArmyModel> armyService)
        {
            _armyService = armyService;
        }

        [HttpGet]
        public async Task<IEnumerable<ArmyResponseModel>> GetAll()
        {
            var armies = await _armyService.ReadAllAsync();
            return armies.Select(a => new ArmyResponseModel
            {
                Id = a.Id,
                Name = a.Name
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArmyResponseModel>> Get(Guid id)
        {
            try
            {
                var a = await _armyService.ReadAsync(id);
                return new ArmyResponseModel
                {
                    Id = a.Id,
                    Name = a.Name
                };
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ArmyCreateModel model)
        {
            var army = new ArmyModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name
            };

            var created = await _armyService.CreateAsync(army);
            if (!created) return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = army.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, ArmyUpdateModel model)
        {
            try
            {
                var army = await _armyService.ReadAsync(id);

                if (!string.IsNullOrEmpty(model.Name))
                    army.Name = model.Name;

                var updated = await _armyService.UpdateAsync(army);
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
                var army = await _armyService.ReadAsync(id);
                var deleted = await _armyService.RemoveAsync(army);
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