namespace Locacao.Dto
{
    public class MotorcycleRegistrationEventDto
    {
        public Guid Id { get; set; }
        public Guid MotorcycleId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
