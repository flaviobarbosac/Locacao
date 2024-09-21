using Locacao.Domain.Enum;

namespace Locacao.Domain.Model
{
    public class Deliveryman : Base.ModelBase
    {       
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public DateTime BirthDate { get; set; }
        public string DriversLicenseNumber { get; set; }
        public DriversLicenseType DriversLicenseType { get; set; }
        public string DriversLicenseImageUrl { get; set; }
        public List<Rental> Rentals { get; set; }
    }
}