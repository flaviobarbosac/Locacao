namespace Locacao.Dto
{
    public class MotorcycleDto
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;        
    }
}