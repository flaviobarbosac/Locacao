using Microsoft.AspNetCore.Mvc;
using Locacao.Dto;
using AutoMapper;
using Locacao.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Locacao.Controllers
{
    /// <summary>
    /// Controller for managing deliverymen.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class DeliverymanController : ControllerBase
    {
        private readonly Services.Inteface.IBaseServices<Deliveryman> _services;
        private readonly IMapper _mapper;

        public DeliverymanController(Services.Inteface.IBaseServices<Deliveryman> services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new deliveryman.
        /// </summary>
        /// <param name="data">The deliveryman data.</param>
        /// <returns>The newly created deliveryman.</returns>
        /// <response code="201">Returns the newly created deliveryman.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(DeliverymanDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data", typeof(ErrorResponse))]
        public async Task<ActionResult<DeliverymanDto>> Post([FromBody] DeliverymanDto data)
        {
            try
            {
                var deliveryman = _mapper.Map<Deliveryman>(data);
                await _services.AddAsync(deliveryman);
                var deliverymanCreatedDto = _mapper.Map<DeliverymanDto>(deliveryman);
                return CreatedAtAction(nameof(Get), new { id = deliverymanCreatedDto.Id }, deliverymanCreatedDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = $"Invalid data: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves all deliverymen.
        /// </summary>
        /// <returns>A list of all deliverymen.</returns>
        /// <response code="200">Returns the list of deliverymen.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliverymanDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DeliverymanDto>>> Get()
        {
            try
            {
                var result = await _services.GetAllAsync();
                var deliverymanDtos = _mapper.Map<IEnumerable<DeliverymanDto>>(result);
                return Ok(deliverymanDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves a specific deliveryman by id.
        /// </summary>
        /// <param name="id">The id of the deliveryman to retrieve.</param>
        /// <returns>The requested deliveryman.</returns>
        /// <response code="200">Returns the requested deliveryman.</response>
        /// <response code="404">If the deliveryman is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DeliverymanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeliverymanDto>> Get(Guid id)
        {
            try
            {
                var deliveryman = await _services.GetByIdAsync(id);
                if (deliveryman == null)
                {
                    return NotFound(new ErrorResponse { Message = "Deliveryman not found" });
                }
                var deliverymanDto = _mapper.Map<DeliverymanDto>(deliveryman);
                return Ok(deliverymanDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Updates an existing deliveryman.
        /// </summary>
        /// <param name="id">The id of the deliveryman to update.</param>
        /// <param name="data">The updated deliveryman data.</param>
        /// <returns>The updated deliveryman.</returns>
        /// <response code="200">Returns the updated deliveryman.</response>
        /// <response code="400">If the data is invalid.</response>
        /// <response code="404">If the deliveryman is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DeliverymanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeliverymanDto>> Put(Guid id, [FromBody] DeliverymanDto data)
        {
            try
            {
                var existingDeliveryman = await _services.GetByIdAsync(id);
                if (existingDeliveryman == null)
                {
                    return NotFound(new ErrorResponse { Message = "Deliveryman not found" });
                }

                var deliveryman = _mapper.Map<Deliveryman>(data);
                deliveryman.Id = id;
                await _services.UpdateAsync(deliveryman);
                var updatedDeliverymanDto = _mapper.Map<DeliverymanDto>(deliveryman);
                return Ok(updatedDeliverymanDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse { Message = $"An error occurred while processing your request: {ex.Message}" });
            }
        }

        /// <summary>
        /// Deletes a specific deliveryman.
        /// </summary>
        /// <param name="id">The id of the deliveryman to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the deliveryman was successfully deleted.</response>
        /// <response code="404">If the deliveryman is not found.</response>
        /// <response code="500">If there was an internal server error.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deliveryman = await _services.GetByIdAsync(id);
                if (deliveryman == null)
                {
                    return NotFound(new ErrorResponse { Message = "Deliveryman not found" });
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