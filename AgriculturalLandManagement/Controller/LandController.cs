using AgriculturalLandManagement.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LandController : ControllerBase
{
    private readonly ILandService _landService;

    public LandController(ILandService landService)
    {
        _landService = landService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LandReadDto>>> GetAll()
    {
        var lands = await _landService.GetAllAsync();
        var dtos = lands.Select(l => new LandReadDto
        {
            Id = l.Id,
            OwnerName = l.OwnerName,
            Production = l.Production,
            Area = l.Area
        });

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LandReadDto>> GetById(int id)
    {
        var land = await _landService.GetByIdAsync(id);
        if (land == null)
            return NotFound();

        var dto = new LandReadDto
        {
            Id = land.Id,
            OwnerName = land.OwnerName,
            Production = land.Production,
            Area = land.Area
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<LandReadDto>> Create([FromBody] LandCreateDto dto)
    {
        var land = new Land
        {
            OwnerName = dto.OwnerName,
            Production = dto.Production,
            Area = 0
        };

        var created = await _landService.CreateAsync(land);

        var resultDto = new LandReadDto
        {
            Id = created.Id,
            OwnerName = created.OwnerName,
            Production = created.Production,
            Area = created.Area
        };

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LandReadDto>> Update(int id, [FromBody] LandUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var land = new Land
        {
            Id = dto.Id,
            OwnerName = dto.OwnerName,
            Production = dto.Production
        };

        var updated = await _landService.UpdateAsync(land);
        if (updated == null)
            return NotFound();

        var resultDto = new LandReadDto
        {
            Id = updated.Id,
            OwnerName = updated.OwnerName,
            Production = updated.Production,
            Area = updated.Area
        };

        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _landService.DeleteAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
