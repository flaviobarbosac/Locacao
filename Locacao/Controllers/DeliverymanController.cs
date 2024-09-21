using Microsoft.AspNetCore.Mvc;
using Locacao.Dto;
using AutoMapper;
using Locacao.Domain.Model;

namespace Locacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliverymanController : ControllerBase
    {        
        private readonly Services.Inteface.IBaseServices<Deliveryman> services;
        private readonly IMapper _mapper;

        public DeliverymanController(Services.Inteface.IBaseServices<Deliveryman> _services, IMapper mapper)
        {
            services = _services;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<DeliverymanDto>> Post([FromBody] DeliverymanDto dados)
        {
            var deliveryMan = _mapper.Map<Deliveryman>(dados);

            await services.AddAsync(deliveryMan);
            var DeliveryManCreatedDto = _mapper.Map<DeliverymanDto>(deliveryMan);
            return CreatedAtAction(nameof(Get), new { id = DeliveryManCreatedDto.Id }, DeliveryManCreatedDto);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resultado = services.GetAllAsync().Result;
            var DeliveryManDtos = _mapper.Map<IEnumerable<DeliverymanDto>>(resultado);
            return Ok(DeliveryManDtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DeliverymanDto>> Get(Guid id)
        {
            var resultado = services.GetByIdAsync(id).Result;
            var devliveryMan = _mapper.Map<DeliverymanDto>(resultado);
            return Ok(devliveryMan);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] DeliverymanDto dados)
        {
            var deliveryMan = _mapper.Map<Deliveryman>(dados);
            await services.UpdateAsync(deliveryMan);
            var deliveryManCreatedDto = _mapper.Map<DeliverymanDto>(deliveryMan);

            return Ok(deliveryManCreatedDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await services.DeleteAsync(id);
            return NoContent();
        }        
    }
}