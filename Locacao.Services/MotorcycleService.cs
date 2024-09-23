using Locacao.Domain.Model;
using Locacao.Repository.Interface;
using Locacao.Services.Interface;
using Locacao.Services.Publisher;

namespace Locacao.Services
{
    public class MotorcycleService : BaseServices<Motorcycle>, IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly MotorcycleEventPublisher _eventPublisher;
                
        public MotorcycleService(IMotorcycleRepository motorcycleRepository, MotorcycleEventPublisher eventPublisher)
            : base(motorcycleRepository) 
        {
            _motorcycleRepository = motorcycleRepository;
            _eventPublisher = eventPublisher; 
        }

        public async Task RegisterMotorcycle(Motorcycle motorcycle)
        {
            // Cadastrar a moto no banco de dados
            await _motorcycleRepository.AddAsync(motorcycle);

            // Publicar o evento de moto cadastrada
            _eventPublisher.PublishMotorcycleRegisteredEvent(motorcycle);
        }

        public async Task<IEnumerable<Motorcycle>> GetMotorcyclesAsync(string licensePlate = null)
        {
            return await _motorcycleRepository.GetMotorcyclesAsync(licensePlate);
        }
    }
}