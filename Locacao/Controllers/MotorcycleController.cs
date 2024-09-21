using Microsoft.AspNetCore.Mvc;
using Locacao.Domain.Model;
using AutoMapper;
using Locacao.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace Locacao.Controllers
{
    /// <summary>
    /// Controller for managing motorcycles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class MotorcycleController : ControllerBase
    {
        private readonly Services.Inteface.IBaseServices<Motorcycle> _services;
        private readonly IMapper _mapper;

        public MotorcycleController(Services.Inteface.IBaseServices<Motorcycle> services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new motorcycle.
        /// </summary>
        /// <param name="data">The motorcycle data.</param>
        /// <returns>The newly created motorcycle.</returns>
        /// <response code="201">Returns the newly created motorcycle.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ErrorResponse))]
        public async Task<ActionResult<MotorcycleDto>> Post([FromBody] MotorcycleDto data)
        {
            try
            {
                var motorcycle = _mapper.Map<Motorcycle>(data);
                await _services.AddAsync(motorcycle);
                var motorcycleCreatedDto = _mapper.Map<MotorcycleDto>(motorcycle);
                return CreatedAtAction(nameof(Get), new { id = motorcycleCreatedDto.Id }, motorcycleCreatedDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = $"Invalid data: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves all motorcycles.
        /// </summary>
        /// <returns>A list of all motorcycles.</returns>
        /// <response code="200">Returns the list of motorcycles.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MotorcycleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MotorcycleDto>>> Get()
        {
            try
            {
                var result = await _services.GetAllAsync();
                var motorcycleDtos = _mapper.Map<IEnumerable<MotorcycleDto>>(result);
                return Ok(motorcycleDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves a specific motorcycle by id.
        /// </summary>
        /// <param name="id">The id of the motorcycle to retrieve.</param>
        /// <returns>The requested motorcycle.</returns>
        /// <response code="200">Returns the requested motorcycle.</response>
        /// <response code="404">If the motorcycle is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MotorcycleDto>> Get(Guid id)
        {
            try
            {
                var motorcycle = await _services.GetByIdAsync(id);
                if (motorcycle == null)
                {
                    return NotFound(new ErrorResponse { Message = "Motorcycle not found" });
                }
                var motorcycleDto = _mapper.Map<MotorcycleDto>(motorcycle);
                return Ok(motorcycleDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Updates an existing motorcycle.
        /// </summary>
        /// <param name="id">The id of the motorcycle to update.</param>
        /// <param name="data">The updated motorcycle data.</param>
        /// <returns>The updated motorcycle.</returns>
        /// <response code="200">Returns the updated motorcycle.</response>
        /// <response code="400">If the data is invalid.</response>
        /// <response code="404">If the motorcycle is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MotorcycleDto>> Put(Guid id, [FromBody] MotorcycleDto data)
        {
            try
            {
                var existingMotorcycle = await _services.GetByIdAsync(id);
                if (existingMotorcycle == null)
                {
                    return NotFound(new ErrorResponse { Message = "Motorcycle not found" });
                }

                var motorcycle = _mapper.Map<Motorcycle>(data);
                motorcycle.Id = id;
                await _services.UpdateAsync(motorcycle);
                var updatedMotorcycleDto = _mapper.Map<MotorcycleDto>(motorcycle);
                return Ok(updatedMotorcycleDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Deletes a specific motorcycle.
        /// </summary>
        /// <param name="id">The id of the motorcycle to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the motorcycle was successfully deleted.</response>
        /// <response code="404">If the motorcycle is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var motorcycle = await _services.GetByIdAsync(id);
                if (motorcycle == null)
                {
                    return NotFound(new ErrorResponse { Message = "Motorcycle not found" });
                }

                await _services.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }
    }  
}