using Microsoft.AspNetCore.Mvc;
using Locacao.Domain.Model;
using AutoMapper;
using Locacao.Dto;

namespace Locacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly Services.Inteface.IBaseServices<Rental> services;
        private readonly IMapper _mapper;

        public RentalController(Services.Inteface.IBaseServices<Rental> _services, IMapper mapper)
        {
            services = _services;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<RentalDto>> Post([FromBody] RentalDto dados)
        {
            var Rental = _mapper.Map<Rental>(dados);

            await services.AddAsync(Rental);
            var RentalCreatedDto = _mapper.Map<RentalDto>(Rental);
            return CreatedAtAction(nameof(Get), new { id = RentalCreatedDto.Id }, RentalCreatedDto);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resultado = services.GetAllAsync().Result;
            var rentalDto = _mapper.Map<IEnumerable<RentalDto>>(resultado);
            return Ok(rentalDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDto>> Get(Guid id)
        {
            var resultado = services.GetByIdAsync(id).Result;
            var rentalDto = _mapper.Map<IEnumerable<RentalDto>>(resultado);
            return Ok(rentalDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] RentalDto dados)
        {
            var Rental = _mapper.Map<Rental>(dados);
            await services.UpdateAsync(Rental);
            var RentalCreatedDto = _mapper.Map<RentalDto>(Rental);

            return Ok(RentalCreatedDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await services.DeleteAsync(id);
            return NoContent();
        }
    }
}