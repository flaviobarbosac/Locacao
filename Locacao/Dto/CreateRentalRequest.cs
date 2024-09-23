using Locacao.Domain.Enum;

namespace Locacao.Dto
{
    public class CreateRentalRequest
    {
        public Guid MotorcycleId { get; set; }
        public Guid DeliverymanId { get; set; }
        public RentalPlan RentalPlan { get; set; }
    }
}
