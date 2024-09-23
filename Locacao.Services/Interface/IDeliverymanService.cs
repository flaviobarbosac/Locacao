using Locacao.Domain.Model;
using Locacao.Services.Inteface;

namespace Locacao.Services.Interface
{
    public interface IDeliverymanService : IBaseServices<Deliveryman>
    {
        bool IsEligibleForRental(Deliveryman deliveryman);        
    }
}
