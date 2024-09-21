using Microsoft.AspNetCore.Mvc;
using Locacao.Domain.Model;
using AutoMapper;
using Locacao.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Locacao.Controllers
{
    /// <summary>
    /// Controller for managing motorcycle registration events.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class MotorcycleRegistrationEventController : ControllerBase
    {
        private readonly Services.Inteface.IBaseServices<MotorcycleRegistrationEvent> _services;
        private readonly IMapper _mapper;

        public MotorcycleRegistrationEventController(Services.Inteface.IBaseServices<MotorcycleRegistrationEvent> services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new motorcycle registration event.
        /// </summary>
        /// <param name="data">The motorcycle registration event data.</param>
        /// <returns>The newly created motorcycle registration event.</returns>
        /// <response code="201">Returns the newly created motorcycle registration event.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(MotorcycleRegistrationEventDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ErrorResponse))]
        public async Task<ActionResult<MotorcycleRegistrationEventDto>> Post([FromBody] MotorcycleRegistrationEventDto data)
        {
            try
            {
                var motorcycleRegistrationEvent = _mapper.Map<MotorcycleRegistrationEvent>(data);
                await _services.AddAsync(motorcycleRegistrationEvent);
                var createdDto = _mapper.Map<MotorcycleRegistrationEventDto>(motorcycleRegistrationEvent);
                return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = $"Invalid data: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves all motorcycle registration events.
        /// </summary>
        /// <returns>A list of all motorcycle registration events.</returns>
        /// <response code="200">Returns the list of motorcycle registration events.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MotorcycleRegistrationEventDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MotorcycleRegistrationEventDto>>> Get()
        {
            try
            {
                var result = await _services.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<MotorcycleRegistrationEventDto>>(result);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves a specific motorcycle registration event by id.
        /// </summary>
        /// <param name="id">The id of the motorcycle registration event to retrieve.</param>
        /// <returns>The requested motorcycle registration event.</returns>
        /// <response code="200">Returns the requested motorcycle registration event.</response>
        /// <response code="404">If the motorcycle registration event is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MotorcycleRegistrationEventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MotorcycleRegistrationEventDto>> Get(Guid id)
        {
            try
            {
                var motorcycleRegistrationEvent = await _services.GetByIdAsync(id);
                if (motorcycleRegistrationEvent == null)
                {
                    return NotFound(new ErrorResponse { Message = "Motorcycle registration event not found" });
                }
                var dto = _mapper.Map<MotorcycleRegistrationEventDto>(motorcycleRegistrationEvent);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Updates an existing motorcycle registration event.
        /// </summary>
        /// <param name="id">The id of the motorcycle registration event to update.</param>
        /// <param name="data">The updated motorcycle registration event data.</param>
        /// <returns>The updated motorcycle registration event.</returns>
        /// <response code="200">Returns the updated motorcycle registration event.</response>
        /// <response code="400">If the data is invalid.</response>
        /// <response code="404">If the motorcycle registration event is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MotorcycleRegistrationEventDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MotorcycleRegistrationEventDto>> Put(Guid id, [FromBody] MotorcycleRegistrationEventDto data)
        {
            try
            {
                var existingEvent = await _services.GetByIdAsync(id);
                if (existingEvent == null)
                {
                    return NotFound(new ErrorResponse { Message = "Motorcycle registration event not found" });
                }

                var motorcycleRegistrationEvent = _mapper.Map<MotorcycleRegistrationEvent>(data);
                motorcycleRegistrationEvent.Id = id;
                await _services.UpdateAsync(motorcycleRegistrationEvent);
                var updatedDto = _mapper.Map<MotorcycleRegistrationEventDto>(motorcycleRegistrationEvent);
                return Ok(updatedDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Deletes a specific motorcycle registration event.
        /// </summary>
        /// <param name="id">The id of the motorcycle registration event to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the motorcycle registration event was successfully deleted.</response>
        /// <response code="404">If the motorcycle registration event is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var motorcycleRegistrationEvent = await _services.GetByIdAsync(id);
                if (motorcycleRegistrationEvent == null)
                {
                    return NotFound(new ErrorResponse { Message = "Motorcycle registration event not found" });
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