using Locacao.Domain.Model;

namespace Locacao.Repository.Interface
{
    public interface IMotorcycleRepository : IBaseRepository<Motorcycle>
    {
        Task AddAsync(Motorcycle motorcycle);
        Task SaveMotorcycleEventAsync(Motorcycle motorcycle);
        Task<IEnumerable<Motorcycle>> GetMotorcyclesAsync(string licensePlate = null);
        Task<bool> LicensePlateExistsAsync(string licensePlate);
    }
}
