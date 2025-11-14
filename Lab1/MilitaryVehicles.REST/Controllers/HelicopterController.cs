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
    public class HelicopterController : ControllerBase
    {
        private readonly ICrudServiceAsync<HelicopterModel> _helicopterService;

        public HelicopterController(ICrudServiceAsync<HelicopterModel> helicopterService)
        {
            _helicopterService = helicopterService;
        }

        [HttpGet]
        public async Task<IEnumerable<HelicopterResponseModel>> GetAll()
        {
            var helicopters = await _helicopterService.ReadAllAsync();
            return helicopters.Select(h => new HelicopterResponseModel
            {
                Id = h.Id,
                Model = h.Model,
                Speed = h.Speed,
                ArmyId = h.ArmyId,
                CrewMemberIds = h.CrewMembers.Select(c => c.Id).ToList()
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HelicopterResponseModel>> Get(Guid id)
        {
            try
            {
                var h = await _helicopterService.ReadAsync(id);
                return new HelicopterResponseModel
                {
                    Id = h.Id,
                    Model = h.Model,
                    Speed = h.Speed,
                    ArmyId = h.ArmyId,
                    CrewMemberIds = h.CrewMembers.Select(c => c.Id).ToList()
                };
            }
            catch
            {
                return NotFound();
            }
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

            var created = await _helicopterService.CreateAsync(helicopter);
            if (!created) return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = helicopter.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, HelicopterUpdateModel model)
        {
            try
            {
                var helicopter = await _helicopterService.ReadAsync(id);

                if (!string.IsNullOrEmpty(model.Model))
                    helicopter.Model = model.Model;

                if (model.Speed.HasValue)
                    helicopter.Speed = model.Speed.Value;

                if (model.ArmyId.HasValue)
                    helicopter.ArmyId = model.ArmyId.Value;

                var updated = await _helicopterService.UpdateAsync(helicopter);
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
                var helicopter = await _helicopterService.ReadAsync(id);
                var deleted = await _helicopterService.RemoveAsync(helicopter);
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