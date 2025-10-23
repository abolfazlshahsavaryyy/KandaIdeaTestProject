using Microsoft.AspNetCore.Mvc;
using AgriculturalLandManagement.Models;
using AgriculturalLandManagement.Repositories;
using AgriculturalLandManagement.Dtos;

namespace AgriculturalLandManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LandController : ControllerBase
    {
        private readonly ILandRepository _repository;

        public LandController(ILandRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all lands in the system.
        /// </summary>
        /// <returns>List of all lands</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Land>>> GetAll()
        {
            var lands = await _repository.GetAllAsync();
            return Ok(lands);
        }

        
        /// <summary>
        /// Get a land by its Id.
        /// </summary>
        /// <param name="id">The id of the land</param>
        /// <returns>The land object</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Land>> GetById(int id)
        {
            var land = await _repository.GetByIdAsync(id);
            if (land == null) return NotFound();
            return Ok(land);
        }

        
        /// <summary>
        /// Create a new land.
        /// Computes the area based on two corners. 
        /// You cannot have X1 == X2 or Y1 == Y2, because that would make the land a line.
        /// </summary>
        /// <param name="dto">DTO containing owner name, production, and two corner coordinates</param>
        /// <returns>The created land object with computed area</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        
        public async Task<ActionResult<Land>> Create(CreateLandDto dto)
        {
            // Map DTO to entity
            var land = new Land
            {
                OwnerName = dto.OwnerName,
                Production = dto.Production,
                // X1 = dto.X1,
                // Y1 = dto.Y1,
                // X2 = dto.X2,
                // Y2 = dto.Y2
                // Area is calculated in repository
            };

            var created = await _repository.AddAsync(land);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        /// <summary>
        /// Update an existing land by Id.
        /// Can change owner, production, and corner coordinates.
        /// Area will be recalculated automatically.
        /// X1 == X2 or Y1 == Y2 is not allowed.
        /// </summary>
        /// <param name="id">Id of the land to update</param>
        /// <param name="dto">DTO with updated fields</param>
        /// <returns>The updated land object</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Land>> Update(int id, UpdateLandDto dto)
        {
            // Validate DTO automatically via [ApiController] + IValidatableObject

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Map DTO to entity
            existing.OwnerName = dto.OwnerName;
            existing.Production = dto.Production;
            // existing.X1 = dto.X1;
            // existing.Y1 = dto.Y1;
            // existing.X2 = dto.X2;
            // existing.Y2 = dto.Y2;

            var updated = await _repository.UpdateAsync(existing);

            return Ok(updated);
        }
        
        /// <summary>
        /// Delete a land by Id.
        /// </summary>
        /// <param name="id">The Id of the land to delete</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
