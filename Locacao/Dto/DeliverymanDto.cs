namespace Locacao.Dto
{
    public class DeliverymanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string DriversLicenseNumber { get; set; } = string.Empty;
        public int DriversLicenseType { get; set; }
        public string DriversLicenseImageUrl { get; set; } = string.Empty;
        public int  Rentals { get; set; }
    }
}
