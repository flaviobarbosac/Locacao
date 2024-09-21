using Microsoft.AspNetCore.Mvc;
using Locacao.Domain.Model;
using AutoMapper;
using Locacao.Dto;

namespace Locacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcycleRegistrationEventController : ControllerBase
    {
        private readonly Services.Inteface.IBaseServices<MotorcycleRegistrationEvent> services;
        private readonly IMapper _mapper;

        public MotorcycleRegistrationEventController(Services.Inteface.IBaseServices<MotorcycleRegistrationEvent> _services, IMapper mapper)
        {
            services = _services;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MotorcycleRegistrationEventDto>> Post([FromBody] MotorcycleRegistrationEventDto dados)
        {
            var MotorcycleRegistrationEvent = _mapper.Map<MotorcycleRegistrationEvent>(dados);

            await services.AddAsync(MotorcycleRegistrationEvent);
            var MotorcycleRegistrationEventCreatedDto = _mapper.Map<MotorcycleRegistrationEventDto>(MotorcycleRegistrationEvent);
            return CreatedAtAction(nameof(Get), new { id = MotorcycleRegistrationEventCreatedDto.Id }, MotorcycleRegistrationEventCreatedDto);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resultado = services.GetAllAsync().Result;
            var motorcycleRegistrationEventDto = _mapper.Map<IEnumerable<MotorcycleRegistrationEventDto>>(resultado);
            return Ok(motorcycleRegistrationEventDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<MotorcycleRegistrationEventDto>> Get(Guid id)
        {
            var resultado = services.GetByIdAsync(id).Result;
            var motorcycleRegistrationEvent = _mapper.Map<MotorcycleRegistrationEventDto>(resultado);
            return Ok(motorcycleRegistrationEvent);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] MotorcycleRegistrationEventDto dados)
        {
            var MotorcycleRegistrationEvent = _mapper.Map<MotorcycleRegistrationEvent>(dados);
            await services.UpdateAsync(MotorcycleRegistrationEvent);
            var MotorcycleRegistrationEventCreatedDto = _mapper.Map<MotorcycleRegistrationEventDto>(MotorcycleRegistrationEvent);

            return Ok(MotorcycleRegistrationEventCreatedDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await services.DeleteAsync(id);
            return NoContent();
        }
    }
}