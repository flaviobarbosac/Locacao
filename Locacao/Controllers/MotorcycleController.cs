using Microsoft.AspNetCore.Mvc;
using Locacao.Domain.Model;
using AutoMapper;
using Locacao.Dto;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Locacao.Services.Interface;

namespace Locacao.Controllers
{
    /// <summary>
    /// Controller for managing motorcycles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    //[Authorize] // Requer autenticação para todos os endpoints
    public class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleService _services;
        private readonly IMapper _mapper;

        public MotorcycleController(IMotorcycleService services, IMapper mapper)
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
        //[Authorize(Policy = "AdminOnly")] // Apenas admin pode criar
        [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
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
        //[Authorize(Policy = "AdminOrDeliveryMan")] // Admin ou entregador podem listar
        [ProducesResponseType(typeof(IEnumerable<MotorcycleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
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
        //[Authorize(Policy = "AdminOrDeliveryMan")] // Admin ou entregador podem ver detalhes
        [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
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
        //[Authorize(Policy = "AdminOnly")] // Apenas admin pode atualizar
        [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
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
        //[Authorize(Policy = "AdminOnly")] // Apenas admin pode deletar
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
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

        /// <summary>
        /// Retrieves motorcycles, optionally filtered by license plate.
        /// </summary>
        /// <param name="licensePlate">The license plate to filter motorcycles by. If null, returns all motorcycles.</param>
        /// <returns>A list of motorcycles matching the filter criteria.</returns>
        /// <response code="200">Returns the list of motorcycles.</response>
        /// <response code="400">If the license plate format is invalid.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("filterByLicense")]
        //[Authorize(Policy = "AdminOrDeliveryMan")] // Admin ou entregador podem filtrar
        [ProducesResponseType(typeof(IEnumerable<MotorcycleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<MotorcycleDto>>> GetMotorcycles([FromQuery] string licensePlate = null)
        {
            try
            {
                if (licensePlate != null && !IsValidLicensePlate(licensePlate))
                {
                    return BadRequest(new ErrorResponse { Message = "Invalid license plate format" });
                }

                var motorcycles = await _services.GetMotorcyclesAsync(licensePlate);
                return Ok(motorcycles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        private bool IsValidLicensePlate(string licensePlate)
        {            
            return !string.IsNullOrWhiteSpace(licensePlate) && licensePlate.Length <= 20;
        }
    }  
}