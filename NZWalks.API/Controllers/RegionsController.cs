using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        //private NZWalksDbContext dbContext;
        private IRegionRepository regionRepository;
        private IMapper mapper;
        private ILogger<RegionsController> logger;

        public RegionsController( IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            //this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("DEEPZ Log: Getting all Regions!");
            
            var regions = await regionRepository.GetAllAsync();
            var regionsDto = mapper.Map<List<RegionDto>>(regions);
            return Ok(regionsDto);
        }

        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var region = await regionRepository.GetByIdAsync(id);

            if (region is null)
                return NotFound();

            var regionDto = mapper.Map<RegionDto>(region);
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var region = mapper.Map<Region>(addRegionRequestDto);
            region=await regionRepository.CreateAsync(region);
            

            //Map model back to dto
            //var regionDto = new RegionDto { Id = region.Id, Name = region.Name, Code = region.Code, RegionImageUrl = region.RegionImageUrl };
            var regionDto = mapper.Map<RegionDto>(region);

            return CreatedAtAction(nameof(GetById), new {id=region.Id}, regionDto);
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Map dto to domain model
            var region = mapper.Map<Region>(updateRegionRequestDto);

            region = await regionRepository.UpdateAsync(id, region);

            if(region is null)
                return NotFound();

            //Convert domain model to dto
            var regionDto = mapper.Map<RegionDto>(region);

            return Ok(regionDto);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);

            if(region is null)
                return NotFound();

            return Ok();
        }
    }
}
