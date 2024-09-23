using Locacao.Domain.Model;
using Locacao.Services.Inteface;

namespace Locacao.Services.Interface
{
    public interface IMotorcycleService : IBaseServices<Motorcycle>
    {
        Task RegisterMotorcycle(Motorcycle motorcycle);
        Task<IEnumerable<Motorcycle>> GetMotorcyclesAsync(string licensePlate = null);
    }
}
