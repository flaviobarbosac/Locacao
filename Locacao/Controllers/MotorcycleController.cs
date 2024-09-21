using Microsoft.AspNetCore.Mvc;
using Locacao.Domain.Model;
using AutoMapper;
using Locacao.Dto;

namespace Locacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]    
    public class MotorcycleController : ControllerBase
    {
        private readonly Services.Inteface.IBaseServices<Motorcycle> services;
        private readonly IMapper _mapper;

        public MotorcycleController(Services.Inteface.IBaseServices<Motorcycle> _services, IMapper mapper)
        {
            services = _services;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MotorcycleDto>> Post([FromBody] MotorcycleDto dados)
        {
            var Motorcycle = _mapper.Map<Motorcycle>(dados);

            await services.AddAsync(Motorcycle);
            var MotorcycleCreatedDto = _mapper.Map<MotorcycleDto>(Motorcycle);
            return CreatedAtAction(nameof(Get), new { id = MotorcycleCreatedDto.Id }, MotorcycleCreatedDto);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resultado = services.GetAllAsync().Result;
            var MotorcycleDto = _mapper.Map<IEnumerable<MotorcycleDto>>(resultado);
            return Ok(MotorcycleDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<MotorcycleDto>> Get(Guid id)
        {
            var Motorcycle = services.GetByIdAsync(id).Result;
            var list = _mapper.Map<MotorcycleDto>(Motorcycle);
            return Ok(list);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] MotorcycleDto dados)
        {
            var Motorcycle = _mapper.Map<Motorcycle>(dados);
            await services.UpdateAsync(Motorcycle);
            var MotorcycleCreatedDto = _mapper.Map<MotorcycleDto>(Motorcycle);

            return Ok(MotorcycleCreatedDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await services.DeleteAsync(id);
            return NoContent();
        }
    }
}