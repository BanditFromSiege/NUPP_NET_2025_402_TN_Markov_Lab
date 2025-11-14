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
    public class DestroyerController : ControllerBase
    {
        private readonly ICrudServiceAsync<DestroyerModel> _destroyerService;

        public DestroyerController(ICrudServiceAsync<DestroyerModel> destroyerService)
        {
            _destroyerService = destroyerService;
        }

        [HttpGet]
        public async Task<IEnumerable<DestroyerResponseModel>> GetAll()
        {
            var destroyers = await _destroyerService.ReadAllAsync();
            return destroyers.Select(d => new DestroyerResponseModel
            {
                Id = d.Id,
                Model = d.Model,
                Torpedoes = d.Torpedoes,
                ArmyId = d.ArmyId,
                CrewMemberIds = d.CrewMembers.Select(c => c.Id).ToList()
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DestroyerResponseModel>> Get(Guid id)
        {
            try
            {
                var d = await _destroyerService.ReadAsync(id);
                return new DestroyerResponseModel
                {
                    Id = d.Id,
                    Model = d.Model,
                    Torpedoes = d.Torpedoes,
                    ArmyId = d.ArmyId,
                    CrewMemberIds = d.CrewMembers.Select(c => c.Id).ToList()
                };
            }
            catch
            {
                return NotFound();
            }
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

            var created = await _destroyerService.CreateAsync(destroyer);
            if (!created) return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = destroyer.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, DestroyerUpdateModel model)
        {
            try
            {
                var destroyer = await _destroyerService.ReadAsync(id);

                if (!string.IsNullOrEmpty(model.Model))
                    destroyer.Model = model.Model;

                if (model.Torpedoes.HasValue)
                    destroyer.Torpedoes = model.Torpedoes.Value;

                if (model.ArmyId.HasValue)
                    destroyer.ArmyId = model.ArmyId.Value;

                var updated = await _destroyerService.UpdateAsync(destroyer);
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
                var destroyer = await _destroyerService.ReadAsync(id);
                var deleted = await _destroyerService.RemoveAsync(destroyer);
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