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
    public class CrewMembersController : ControllerBase
    {
        private readonly ICrudServiceAsync<CrewMemberModel> _crewService;

        public CrewMembersController(ICrudServiceAsync<CrewMemberModel> crewService)
        {
            _crewService = crewService;
        }

        [HttpGet]
        public async Task<IEnumerable<CrewMemberResponseModel>> GetAll()
        {
            var crewMembers = await _crewService.ReadAllAsync();
            return crewMembers.Select(c => new CrewMemberResponseModel
            {
                Id = c.Id,
                Name = c.Name,
                Rank = c.Rank
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CrewMemberResponseModel>> Get(Guid id)
        {
            try
            {
                var c = await _crewService.ReadAsync(id);
                return new CrewMemberResponseModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Rank = c.Rank
                };
            }
            catch
            {
                return NotFound();
            }
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

            var created = await _crewService.CreateAsync(crew);
            if (!created) return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = crew.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, CrewMemberUpdateModel model)
        {
            try
            {
                var crew = await _crewService.ReadAsync(id);

                if (!string.IsNullOrEmpty(model.Name))
                    crew.Name = model.Name;

                if (!string.IsNullOrEmpty(model.Rank))
                    crew.Rank = model.Rank;

                var updated = await _crewService.UpdateAsync(crew);
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
                var crew = await _crewService.ReadAsync(id);
                var deleted = await _crewService.RemoveAsync(crew);
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