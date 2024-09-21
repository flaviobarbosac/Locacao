namespace Locacao.Domain.Model
{
    public class MotorcycleRegistrationEvent : Base.ModelBase
    {        
        public Guid MotorcycleId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

