namespace Locacao.Domain.Model
{
    public class MotorcycleEvent
    {
        public Guid MotorcycleId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public int MotorcycleYear { get; set; }
    }
}