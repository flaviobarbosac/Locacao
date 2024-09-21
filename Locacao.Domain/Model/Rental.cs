using Locacao.Domain.Enum;

namespace Locacao.Domain.Model
{
    public class Rental : Base.ModelBase
    {        
        public Guid MotorcycleId { get; set; }
        public Motorcycle Motorcycle { get; set; }
        public Guid DeliverymanId { get; set; }
        public Deliveryman Deliveryman { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public RentalPlan RentalPlan { get; set; }
        public decimal TotalCost { get; set; }
    }
}