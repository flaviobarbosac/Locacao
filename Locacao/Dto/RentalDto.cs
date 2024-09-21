namespace Locacao.Dto
{
    public class RentalDto
    {
        public Guid Id { get; set; }   
        public Guid MotorcycleId { get; set; }
        public int Motorcycle { get; set; }
        public Guid DeliverymanId { get; set; }
        public int Deliveryman { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int RentalPlan { get; set; }
        public decimal TotalCost { get; set; }
    }
}
