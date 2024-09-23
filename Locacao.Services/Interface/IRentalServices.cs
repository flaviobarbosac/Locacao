using Locacao.Domain.Enum;
using Locacao.Domain.Model;
using Locacao.Services.Inteface;

namespace Locacao.Services.Interface
{
    public interface IRentalService : IBaseServices<Rental>
    {
        decimal CalculateTotalCost(Rental rental, DateTime actualReturnDate);
        Rental CreateRental(Guid motorcycleId, Guid deliverymanId, RentalPlan rentalPlan);
    }
}
