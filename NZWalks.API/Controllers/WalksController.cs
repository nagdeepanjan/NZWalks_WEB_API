using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private IMapper mapper;
        private IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Map DTO to Domain Model
            var walk = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walk);

            // Map Domain model to DTO
            var walkDto = mapper.Map<WalkDto>(walk);

            return CreatedAtAction(nameof(GetById), new { id = walk.Id }, walkDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? filterOn, [FromQuery]string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending)
        {
            var walks = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true);

            //Map domain model to dto
            return Ok(mapper.Map<List<WalkDto>>(walks));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walk = await walkRepository.GetByIdAsync(id);

            return (walk is null) ? NotFound() : Ok(mapper.Map<WalkDto>(walk));
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var walk = mapper.Map<Walk>(updateWalkRequestDto);

            walk = await walkRepository.UpdateAsync(id, walk);

            if (walk is null)
                return NotFound();
            return Ok(mapper.Map<WalkDto>(walk));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalk = await walkRepository.DeleteAsync(id);

            if (deletedWalk is null)
                return NotFound();
            return Ok();
        }
    }
}
