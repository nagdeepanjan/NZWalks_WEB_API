using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private NZWalksDbContext dbContext;
        private IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();

            //Map domain model to dto
            var regionsDto = new List<RegionDto>();
            foreach (var region in regions)
                regionsDto.Add(new RegionDto{Id = region.Id, Code = region.Code,Name = region.Name, RegionImageUrl = region.RegionImageUrl});

            return Ok(regionsDto);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var region = await regionRepository.GetByIdAsync(id);

            if (region is null)
                return NotFound();

            //Map domain model to dto
            var regionDto = new RegionDto { Id = region.Id, Name = region.Name, Code = region.Code, RegionImageUrl = region.RegionImageUrl };

            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map dto to domain model
            var region = new Region
            {
                Code = addRegionRequestDto.Code, Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            region=await regionRepository.CreateAsync(region);
            

            //Map model back to dto
            var regionDto = new RegionDto
                { Id = region.Id, Name = region.Name, Code = region.Code, RegionImageUrl = region.RegionImageUrl };

            return CreatedAtAction(nameof(GetById), new {id=region.Id}, regionDto);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map dto to domain model
            var region = new Region { Code = updateRegionRequestDto.Code, Name = updateRegionRequestDto.Name, RegionImageUrl = updateRegionRequestDto.RegionImageUrl};


            region = await regionRepository.UpdateAsync(id, region);

            if(region is null)
                return NotFound();

            //COnvert domain model to dto
            var regionDto = new RegionDto { Id = region.Id, Name = region.Name, Code = region.Code, RegionImageUrl = region.RegionImageUrl };

            return Ok(regionDto);
        }


        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);

            if(region is null)
                return NotFound();

            return Ok();
        }
    }
}
